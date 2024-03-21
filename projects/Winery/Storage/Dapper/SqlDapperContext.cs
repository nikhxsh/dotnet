using System.Data;
using System.Data.SqlClient;

namespace WineryAPI.Storage.Dapper
{
	public class SqlDapperContext : IDapperContext
	{
		private readonly IConfiguration _configuration;

		public SqlDapperContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
		}
	}
}
