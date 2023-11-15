using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DBScheme.Postgre
{
    /// <summary>
    /// 表
    /// </summary>
    [SugarTable("pg_tables")]
    public class PostgreTableEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "tablename")]
        public string TableName { get; set; }


        [SugarColumn(ColumnDescription = "", ColumnName = "schemaname")]
        public string SchemaName { get; set; }
    }
}
