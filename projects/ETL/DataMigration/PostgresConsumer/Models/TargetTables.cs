using System;
using System.Collections.Generic;
using System.Text;

namespace PostgresConsumer
{
    public class TargetTable
    {
        public string tableName { get; set; }
        public ColumnMapping[] columnMappings { get; set; }
    }
}
