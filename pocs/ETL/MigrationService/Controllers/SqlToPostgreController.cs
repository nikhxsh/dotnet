using Microsoft.AspNetCore.Mvc;
using MigrationService.Helper;
using MigrationService.Models;
using MigrationService.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationService.Controllers
{
	[ApiController]
	[Route("api/sqltopostgre")]
	public class SqlToPostgreController : ControllerBase
	{
		private ISqlRepository _sqlRepository;
		private string _postgresConnectionString = "Host=ims-rds.cdmnj4oncsro.us-west-2.rds.amazonaws.com;Username=postgres;Password=IMSHACK1;Database=postgres";
		private string _sqsBackGroundUrl = "https://sqs.us-west-2.amazonaws.com/794344549935/backGround_IMS_Queue.fifo";
		private PostgresRepository _postgresRepository;
		private SqsRepository _sqsRepository;

		public SqlToPostgreController(ISqlRepository sqlRepository, IMongoRepository mongoRepository)
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

		[HttpPost("migrate")]
		public async Task<IActionResult> MigrateData([FromBody] SqlToPostresTemplate template)
		{
			if (template == null)
				return BadRequest();

			try
			{
				var postgresModel = new PostgresModelPostgres()
				{
					count = template.TransferSize,
					TargetDatabase = "postgres",
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
					Body = System.Text.Json.JsonSerializer.Serialize(sqsMessage)
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

		[HttpGet("status")]
		public async Task<IActionResult> GetStatus([FromQuery] int requestId)
		{
			if (requestId == 0)
				return BadRequest();

			var status = await _postgresRepository.GetRequestEntry(requestId);

			return await Task.FromResult(new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(status, Formatting.Indented)
			});
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
