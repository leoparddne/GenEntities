using System.ComponentModel.DataAnnotations.Schema;

namespace OracleEx.Model
{
    /// <summary>
    /// 字段注释
    /// </summary>
    [Table("USER_COL_COMMENTS")]
    public class UserColComments
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }

        [Column("COMMENTS")]
        public string Comments { get; set; }
    }
}
