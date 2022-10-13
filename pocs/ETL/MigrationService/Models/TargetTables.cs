using System;
using System.Collections.Generic;
using System.Text;

namespace MigrationService.Models
{
    public class TargetTable
    {
        public string tableName { get; set; }
        public ColumnMapping[] columnMappings { get; set; }
    }
}
