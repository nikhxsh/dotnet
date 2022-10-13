using Npgsql;

namespace ProducerGennericDataMigration.SqlRepository;

public class RequestSqlHelper
{
    private readonly string PostgreConnection; 
    public RequestSqlHelper(string Connection)
    {
        PostgreConnection = Connection;
    }

    public async Task UpdateRequestAsync(int requestId)
    {
        using (var connection = new NpgsqlConnection(PostgreConnection))
        {
            connection.Open();
            var sqlQuery = $"Update public.\"Request\" Set \"Status\" = 'Batches Created' where \"RequestId\" = {requestId}";
            var com = new NpgsqlCommand(sqlQuery, connection);
            await com.ExecuteNonQueryAsync();
            connection.Close();
        }
    }
}