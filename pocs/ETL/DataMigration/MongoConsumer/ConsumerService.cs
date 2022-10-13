using MongoDB.Bson.Serialization;
using SqsService;
using SqlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlService.Helper;
using MongoConsumer.Models;
using MongoDB.Bson;
using System.Data;
using MongoConsumer.MongoService;
using PostgresService;
using System.Text.Json;

namespace MongoConsumer
{
    public class ConsumerService
    {
        private readonly PollMessage _pollMessage;
        private readonly SqlClientService _sqlService;
        private readonly PostgresClientService _postgresService;
        private readonly MongoRepository _mongoRepository;
        public ConsumerService(PollMessage pollMessage, SqlClientService sqlService, MongoRepository mongoRepository, PostgresClientService postgresClientService)
        {
            _pollMessage = pollMessage;
            _sqlService = sqlService;
            _mongoRepository = mongoRepository;
            _postgresService = postgresClientService;
        }

        public async Task ConsumeMessageAsync()
        {
            List<MessageInfo> receivedMessages = await _pollMessage.FetchMessageFromSqsAsync("Mongo_IMS_Queue.fifo");
            foreach (MessageInfo message in receivedMessages)
            {
                var messageBody = JsonSerializer.Deserialize<MessageBody>(message.Body);
                //var messageBody = BsonSerializer.Deserialize<MessageBody>(message.Body);
                

                try
                {
                    var requestId = messageBody.RequestId;
                    var batchId = messageBody.BatchId;

                    var templateResult = _postgresService.GetPostgresQueryResultAsync($"SELECT \"Template\" FROM public.\"Request\" where public.\"Request\".\"RequestId\" = {requestId};");
                    var template = BsonSerializer.Deserialize<SqlToMongoTemplate>(templateResult.Result);

                    var sourceData = _sqlService.ExtractSourceData(Parser.ToSQLQuery(
                        select: template.MainTable.select,
                        conditions: template.MainTable.conditions,
                        tableName: template.MainTable.tableName,
                        orderByKey: template.MainTable.key,
                        offset: messageBody.Skip,
                        fetch: messageBody.top));

                    var outputList = Transform(sourceData, template);

                    await _mongoRepository.Save(template.Settings.Mongo, outputList);

                    _postgresService.Save($"Update public.\"Batch\" Set \"Status\" = 'Completed' Where public.\"Batch\".\"BatchId\" = {batchId};");

                    if (messageBody.IsLastBatch)
                    {
                        _postgresService.Save($"Update public.\"Request\" Set \"Status\" = 'Completed' where public.\"Request\".\"RequestId\" = {requestId};");
                    }

                    await _pollMessage.DeleteMessageAsync(message.Id);
                }
                catch(Exception ex)
                {
                    throw;
                }
                
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
                    var mappingKey = item[nest.tkey].ToString();
                    var nestedData = _sqlService.ExtractSourceData(Parser.ToSQLQuery(
                        select: nest.select,
                        conditions: nest.conditions,
                        sourceKey: nest.skey,
                        tableName: nest.tableName,
                        orderByKey: nest.tkey,
                        isNestedQuery: true,
                        nestedMatchingId: mappingKey));
                    var transformedData = LoadTableData(nestedData);
                    item.Add(nest.objectIdentifier, BsonValue.Create(transformedData));
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
    }
}
