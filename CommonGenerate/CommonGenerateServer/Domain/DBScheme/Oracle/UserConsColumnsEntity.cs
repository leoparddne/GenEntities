using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DBScheme.Oracle
{
    [SugarTable("USER_CONS_COLUMNS")]
    public class UserConsColumnsEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "CONSTRAINT_NAME")]
        public string ConstraintName { get; set; }

        [Column("COLUMN_NAME")]
        [SugarColumn(ColumnDescription = "", ColumnName = "COLUMN_NAME")]
        public string ColumnName { get; set; }
    }
}
