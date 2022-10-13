using System;
using System.Collections.Generic;
using System.Text;

namespace MigrationService.Models
{
	public class SqlToPostresTemplate
	{
		public Settings Settings { get; set; }
		public BaseTable MainTable { get; set; }
		public TargetTable[] targetTables { get; set; }
		public int TransferSize { get; set; }
	}

	public class Settings
	{
		public DbSettings Sql { get; set; }
		public DbSettings Postgres { get; set; }
		public DbSettings Mongo { get; set; }
	}
}
