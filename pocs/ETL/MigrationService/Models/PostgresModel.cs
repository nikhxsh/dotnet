using ETL.Models;

namespace MigrationService.Models
{
	public class PostgresModelMongo
	{
		public string TargetDatabase { get; set; }
		public string SourceDatabase { get; set; }
		public int count { get; set; }
		public SqlToMongoTemplate sqlToMongoTemplate { get; set; }
		public string Status { get; set; }

	}

	public class PostgresModelPostgres
	{
		public string TargetDatabase { get; set; }
		public string SourceDatabase { get; set; }
		public int count { get; set; }
		public SqlToPostresTemplate sqlToMongoTemplate { get; set; }
		public string Status { get; set; }

	}

	public class PostgresRequestModel
	{
		public int RequestId { get; set; }
		public int Count { get; set; }
		public string Status { get; set; }
	}
}
