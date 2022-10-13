using Npgsql;
using System;
using System.Threading.Tasks;

namespace PostgresService
{
    public class PostgresClientService
    {
        public string PostgresConnectionString { get; set; }

        public PostgresClientService()
        {
            PostgresConnectionString = "Host=ims-rds.cdmnj4oncsro.us-west-2.rds.amazonaws.com;Username=postgres;Password=IMSHACK1;Database=postgres";
        }

        public async Task<string> GetPostgresQueryResultAsync(string sqlQuery)
        {
            var json = "";

            await using (var connection = new NpgsqlConnection(PostgresConnectionString))
            {
                connection.Open();
                var com = new NpgsqlCommand(sqlQuery, connection);
                using (NpgsqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        json += reader[0];
                    }
                }
            }

            return json;
        }

        
        public void Save(string query)
        {
            //var connString = "Host=ims-rds.cdmnj4oncsro.us-west-2.rds.amazonaws.com;Username=postgres;Password=IMSHACK1;Database=postgres";

            using var conn = new NpgsqlConnection(PostgresConnectionString);
            conn.Open();


            using (var cmd = new NpgsqlCommand(query, conn))
            {
                //cmd.Parameters.AddWithValue("Hello world");
                cmd.ExecuteNonQuery();
            }

            // Insert some data
            //using (var cmd = new NpgsqlCommand("INSERT INTO public.samplemigration (id) VALUES (1)", conn))
            //{
            //    //cmd.Parameters.AddWithValue("Hello world");
            //    cmd.ExecuteNonQuery();
            //}

            // Retrieve all rows
            //using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
            //using (var reader = cmd.ExecuteReaderAsync())
            //{
            //    while (await reader.ReadAsync())
            //    {
            //        Console.WriteLine(reader.GetString(0));
            //    }
            //}
        }

        public void createSchema(string createScript)
        {
            using var conn = new NpgsqlConnection(PostgresConnectionString);
            conn.Open();


            using (var cmd = new NpgsqlCommand(createScript, conn))
            {
                cmd.ExecuteNonQuery();
            }

        }

    }
}
