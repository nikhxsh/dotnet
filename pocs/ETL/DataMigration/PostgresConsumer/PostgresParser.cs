using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PostgresConsumer
{
    public class PostgresParser
    {

        public static string ToCreateTableScript(string tableName, ColumnMapping[] columnMappings)
        {
            Dictionary<string, string> mapdataTypeMapping = new Dictionary<string, string>();
            mapdataTypeMapping.Add("bigint", "bigint");
            mapdataTypeMapping.Add("varchar", "text");
            mapdataTypeMapping.Add("int", "integer");
            mapdataTypeMapping.Add("nvarchar", "text");
            mapdataTypeMapping.Add("datetime", "timestamp");
            mapdataTypeMapping.Add("uniqueidentifier", "uuid");
            StringBuilder createScript = new StringBuilder($"CREATE TABLE IF NOT EXISTS {tableName}(");

            foreach (var mapping in columnMappings)
            {
                var dataTypeConverted = mapdataTypeMapping.ContainsKey(mapping.dataType)? mapdataTypeMapping[mapping.dataType] : "text";
                createScript.Append(mapping.target).Append(" ").Append(dataTypeConverted).Append(",");

            }
            createScript.Remove(createScript.Length - 1, 1);
            createScript.Append(")");

            return createScript.ToString();
        }


        public static string ToSQLInsertQuery(
            string tableName,
            ColumnMapping[] columnMappings,
            DataSet dataset
            )
        {


            StringBuilder sqlInsert = new StringBuilder($"INSERT INTO {tableName} (");
            foreach (var targetColumnName in columnMappings)
            {
                sqlInsert.Append(targetColumnName.target).Append(",");
            }

            sqlInsert.Remove(sqlInsert.Length - 1, 1);
            sqlInsert.Append(") VALUES ");

            for (int rowNum = 0; rowNum < dataset.Tables[0].Rows.Count; rowNum++)
            {
                sqlInsert.Append("(");
                foreach (var targetColumnName in columnMappings)
                {

                    if (targetColumnName.dataType == "integer")
                    {
                        sqlInsert.Append(dataset.Tables[0].Rows[rowNum][targetColumnName.source]).Append(",");
                    }

                    if (targetColumnName.dataType == "text")
                    {
                        sqlInsert.Append($"'{dataset.Tables[0].Rows[rowNum][targetColumnName.source]}'").Append(",");
                    }

                }
                sqlInsert.Remove(sqlInsert.Length - 1, 1);
                sqlInsert.Append("),");
            }
            sqlInsert.Remove(sqlInsert.Length - 1, 1);
            sqlInsert.Append(";");
         

            return sqlInsert.ToString();
        }
       
    }
}
