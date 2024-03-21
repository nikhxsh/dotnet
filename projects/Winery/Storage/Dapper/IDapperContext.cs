using System.Data;

namespace WineryAPI.Storage.Dapper
{
	public interface IDapperContext
	{
		IDbConnection CreateConnection();
	}
}
