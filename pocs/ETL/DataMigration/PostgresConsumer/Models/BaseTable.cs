using System;
using System.Collections.Generic;
using System.Text;

namespace PostgresConsumer
{
	public class BaseTable
	{
		public string key { get; set; }
		public string tableName { get; set; }
		public string[] select { get; set; }
		public string conditions { get; set; }
	}
}
