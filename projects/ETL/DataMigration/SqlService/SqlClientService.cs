using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlService
{
    public class SqlClientService
    {
        public string SqlConnectionString { get; set; }

        public SqlClientService()
        {
            SqlConnectionString = "data source=uw2cqasql.awscqa.avalara.net;initial catalog=AvaTaxAccount;User ID=AvaService;Password=S^)@1Kr9cBNZH;MultipleActiveResultSets=True;";
        }

        public async Task<string> GetSqlQueryResultAsync(string sqlQuery)
        {
            var json = "";

            await using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(sqlQuery, connection);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        json += reader[0];
                    }
                }
            }

            return json;
        }

        public DataSet ExtractSourceData(string query)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = conn;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return ds;
        }
    }
}
