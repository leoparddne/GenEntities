using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

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
