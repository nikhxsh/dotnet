using System.Collections.Generic;

namespace MigrationService.Models
{
	public class SchemaRequest
	{
		public string URL { get; set; }
		public string DbName { get; set; }
		public string Filter { get; set; }
	}

	public class SchemaResponse
	{
		public string Table { get; set; }
		public List<Column> Columns { get; set; } = new List<Column>();
	}

	public class Column
	{
		public string Name { get; set; }
		public string DataType { get; set; }
	}
}
