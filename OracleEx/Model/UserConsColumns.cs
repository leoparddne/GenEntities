using System.ComponentModel.DataAnnotations.Schema;

namespace OracleEx.Model
{
    [Table("USER_CONS_COLUMNS")]

    public class UserConsColumns
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("CONSTRAINT_NAME")]
        public string ConstraintName { get; set; }

        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }
    }
}
