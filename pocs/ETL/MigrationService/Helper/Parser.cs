using ETL.Models;
using MigrationService.Models;

namespace MigrationService.Helper
{
	public static class Parser
	{
		public static string ToSQLQuery(
			string[] select,
			string conditions,
			string tableName,
			string orderByKey = "",
			bool isNestedQuery = false,
			string sourceKey = "",
			string nestedMatchingId = "", 
			int offset = 0, 
			int fetch = 0)
		{
			var selectQuery = string.Join(",", select);

			var conditionsQuery = string.Empty;
			if (!string.IsNullOrEmpty(conditions))
				conditionsQuery += $"WHERE {conditions} ";

			if(isNestedQuery && !string.IsNullOrEmpty(sourceKey) && !string.IsNullOrEmpty(nestedMatchingId))
			{
				var nestedCondition = $"{sourceKey} = '{nestedMatchingId}'";

				if (string.IsNullOrEmpty(conditionsQuery))
					nestedCondition = $"WHERE {nestedCondition}";

				if (!string.IsNullOrEmpty(conditionsQuery))
					conditionsQuery += $"AND {nestedCondition}'";
				else
					conditionsQuery += nestedCondition;
			}

			var limitQuery = string.Empty;
			if (fetch > 0)
				limitQuery = $"OFFSET {offset} ROWS FETCH NEXT {fetch} ROW ONLY";

			var oderByQuery = string.Empty;
			if (!string.IsNullOrEmpty(orderByKey))
				oderByQuery = $"ORDER BY {orderByKey}";

			return $"SELECT {selectQuery} " +
				   $"FROM {tableName} " +
				   $"{conditionsQuery} " +
				   $"{oderByQuery} " +
				   $"{limitQuery}";
		}

		public static string ToSchemaQuery(this SchemaRequest schemaRequest)
		{
			return @$"USE [{schemaRequest.DbName}]
					SELECT CONCAT(isc.TABLE_SCHEMA, '.', isc.TABLE_NAME) TABLE_NAME, isc.COLUMN_NAME, isc.DATA_TYPE
					FROM sys.tables st INNER JOIN INFORMATION_SCHEMA.COLUMNS isc
					ON st.name = isc.TABLE_NAME
					WHERE isc.TABLE_NAME LIKE '%{schemaRequest.Filter}%' AND st.is_ms_shipped = 0";
		}
	}
}
