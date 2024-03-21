using Npgsql;

namespace ProducerGennericDataMigration.SqlRepository;

public class BatchSqlHelper
{
    private readonly string PostgreConnection; 
    public BatchSqlHelper(string Connection)
    {
        PostgreConnection = Connection;
    }
    
    public async Task<List<BatchSQSModel>> GetPostgresQueryResultAsync(List<BatchSqlModelInput> sqlModels)
    {
        var sqsModelList = new List<BatchSQSModel>();
        await using (var connection = new NpgsqlConnection(PostgreConnection))
        {
            connection.Open();
            foreach (var sqlModel in sqlModels)
            {
                var sqlQuery = $"INSERT INTO public.\"Batch\" (\"RequestId\", \"IsLastBatch\", \"Top\", \"Skip\", \"Status\") VALUES ({sqlModel.RequestId},{sqlModel.IsLastBatch},{sqlModel.top},{sqlModel.Skip},\'{sqlModel.Status}\') returning \"BatchId\";";
                var com = new NpgsqlCommand(sqlQuery, connection);
                var BatchId = com.ExecuteScalar();
                var sqsModel = new BatchSQSModel();
                sqsModel.BatchId = Convert.ToInt32(BatchId);
                sqsModel.RequestId = sqlModel.RequestId;
                sqsModel.top = sqlModel.top;
                sqsModel.Skip = sqlModel.Skip;
                sqsModel.Status = sqlModel.Status;
                
                sqsModelList.Add(sqsModel);
            }
            connection.Close();
        }

        return sqsModelList;
    }
}