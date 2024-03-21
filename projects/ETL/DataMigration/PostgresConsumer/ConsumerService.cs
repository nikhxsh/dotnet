using MongoDB.Bson.Serialization;
using SqsService;
using SqlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlService.Helper;
using MongoDB.Bson;
using System.Data;
using PostgresService;
using System.Text.Json;
using Repository;

namespace PostgresConsumer
{
    public class ConsumerService
    {
        private readonly PollMessage _pollMessage;
        private readonly SqlClientService _sqlService;
        private readonly PostgresClientService _postgresService;
      
        public ConsumerService(PollMessage pollMessage, SqlClientService sqlService, PostgresClientService postgresClientService)
        {
            _pollMessage = pollMessage;
            _sqlService = sqlService;
            _postgresService = postgresClientService;
        }

        public async Task ConsumeMessageAsync()
        {
            List<MessageInfo> receivedMessages = await _pollMessage.FetchMessageFromSqsAsync("Postgres_IMS_Queue.fifo");
            foreach (MessageInfo message in receivedMessages)
            {
                var messageBody = JsonSerializer.Deserialize<MessageBody>(message.Body);
               
                try
                {
                    var requestId = messageBody.RequestId;
                    var batchId = messageBody.BatchId;

                    var templateResult = _postgresService.GetPostgresQueryResultAsync($"SELECT \"Template\" FROM public.\"Request\" where public.\"Request\".\"RequestId\" = {requestId};");
                    var template = BsonSerializer.Deserialize<SqlToPostresTemplate>(templateResult.Result);

                    var sourceData = _sqlService.ExtractSourceData(Parser.ToSQLQuery(
                        select: template.MainTable.select,
                        conditions: template.MainTable.conditions,
                        tableName: template.MainTable.tableName,
                        orderByKey: template.MainTable.key,
                        offset: messageBody.Skip,
                        fetch: messageBody.top));

                    foreach (var table in template.targetTables)
                    {

                        var createScript = PostgresParser.ToCreateTableScript(table.tableName, table.columnMappings);

                        _postgresService.createSchema(createScript);
                    }

                    //
                    foreach (var table in template.targetTables)
                    {
                        var InsertScript = PostgresParser.ToSQLInsertQuery(table.tableName, table.columnMappings, sourceData);
                        _postgresService.Save(InsertScript);
                    }

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
