using ETL.Models;
using Microsoft.AspNetCore.Mvc;
using MigrationService.Helper;
using MigrationService.Models;
using MigrationService.Repository;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationService.Controllers
{
	[ApiController]
	[Route("api/sqltomongo")]
	public class SqlToMongoController : ControllerBase
	{
		private ISqlRepository _sqlRepository;
		private string _postgresConnectionString = "Host=ims-rds.cdmnj4oncsro.us-west-2.rds.amazonaws.com;Username=postgres;Password=IMSHACK1;Database=postgres";
		private string _sqsBackGroundUrl = "https://sqs.us-west-2.amazonaws.com/794344549935/backGround_IMS_Queue.fifo";
		private PostgresRepository _postgresRepository;
		private SqsRepository _sqsRepository;

		public SqlToMongoController(ISqlRepository sqlRepository)
		{
			_sqlRepository = sqlRepository;
			_postgresRepository = new PostgresRepository(_postgresConnectionString);
			_sqsRepository = new SqsRepository(_sqsBackGroundUrl);
		}

		[HttpPost("schema")]
		public async Task<IActionResult> GetSchema([FromBody] SchemaRequest request)
		{
			if (request == null || string.IsNullOrEmpty(request.URL) || string.IsNullOrEmpty(request.DbName))
				return BadRequest();

			var schemaDs = _sqlRepository.GetTablesSchema(request.URL, request.ToSchemaQuery());

			var schema = LoadDbSchema(schemaDs);

			return await Task.FromResult(new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(schema, Formatting.Indented)
			});
		}

		[HttpPost("sample")]
		public async Task<IActionResult> GetSampleDocument([FromBody] SqlToMongoTemplate template)
		{
			if (template == null && template.MainTable == null && template.NestedTables == null && template.Settings == null)
				return BadRequest();

			// Transform table
			var mainTableDataset = _sqlRepository.ExtractSourceData(template.Settings.Sql.Connection, Parser.ToSQLQuery(
				select: template.MainTable.Select,
				conditions: template.MainTable.Conditions,
				tableName: template.MainTable.TableName,
				orderByKey: template.MainTable.Key,
				offset: 0, 
				fetch: template.TransferSize));

			var outputList = Transform(mainTableDataset, template);

			var dotNetObj = outputList.ConvertAll(BsonTypeMapper.MapToDotNetValue);

			return await Task.FromResult(new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(dotNetObj, Formatting.Indented)
			});
		}

		[HttpPost("migrate")]
		public async Task<IActionResult> MigrateData([FromBody] SqlToMongoTemplate template)
		{
			if (template == null)
				return BadRequest();
			try
			{
				var postgresModel = new PostgresModelMongo()
				{
					count = template.TransferSize,
					TargetDatabase = "mongo",
					SourceDatabase = "sql",
					Status = "Request Created",
					sqlToMongoTemplate = template
				};
				var requestId = await _postgresRepository.CreateRequestEntry(postgresModel);

				var sqsMessage = new SqsMessageBodyModel()
				{
					RequestId = requestId,
					TargetDatabase = postgresModel.TargetDatabase,
					Count = postgresModel.count
				};

				var SqsMessageModel = new RequestSqsModel()
				{
					MessageGroupId = Guid.NewGuid().ToString(),
					MessageDeduplicationId = Guid.NewGuid().ToString(),
					Body = JsonConvert.SerializeObject(sqsMessage)
				};

				await _sqsRepository.SendMessagetoBatchSQS(SqsMessageModel);

				return new ContentResult
				{
					ContentType = "application/json",
					Content = JsonConvert.SerializeObject(new MigrationResponse { Message = "Request Created!", RequestId = requestId }, Formatting.Indented)
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new ContentResult();
			}
			
		}

		private List<BsonDocument> Transform(DataSet mainData, SqlToMongoTemplate template)
		{
			// Transform table
			var outputList = LoadTableData(mainData);

			foreach (var item in outputList)
			{
				// Transform nested tables
				foreach (var nest in template.NestedTables)
				{
					var mappingKey = item[nest.TargetKey].ToString();
					var nestedData = _sqlRepository.ExtractSourceData(template.Settings.Sql.Connection, Parser.ToSQLQuery(
						select: nest.Select,
						conditions: nest.Conditions,
						sourceKey: nest.SourceKey,
						tableName: nest.TableName,
						orderByKey: nest.TargetKey,
						isNestedQuery: true, 
						nestedMatchingId: mappingKey));
					var transformedData = LoadTableData(nestedData);
					item.Add(nest.ObjectIdentifier, BsonValue.Create(transformedData));
				}
			}

			return outputList;
		}

		private List<BsonDocument> LoadTableData(DataSet data)
		{
			var outputList = new List<BsonDocument>();

			foreach (DataRow row in data.Tables[0].Rows)
			{
				var pair = new BsonDocument();

				foreach (DataColumn col in data.Tables[0].Columns)
				{
					var fieldValue = row[$"{col.ColumnName}"].ToString();
					pair.Add(col.ColumnName, fieldValue);
				}

				outputList.Add(pair);
			}

			return outputList;
		}

		private List<SchemaResponse> LoadDbSchema(DataSet data)
		{
			var dbSchemaList = new List<SchemaResponse>();

			var dbSchema = from r in data.Tables[0].Rows.OfType<DataRow>()
						   group r by r["TABLE_NAME"] into g
						   select new { Table = g.Key, Data = g.Select(x => x.ItemArray) };

			foreach (var row in dbSchema)
			{
				var schemaResponse = new SchemaResponse
				{
					Table = row.Table.ToString()
				};

				foreach (var r in row.Data)
				{
					schemaResponse.Columns.Add(new Column { Name = r[1].ToString(), DataType = r[2].ToString() });
				}

				dbSchemaList.Add(schemaResponse);
			}

			return dbSchemaList;
		}
	}
}
