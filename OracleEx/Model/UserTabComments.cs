using System.ComponentModel.DataAnnotations.Schema;

namespace OracleEx.Model
{
    /// <summary>
    /// 表注释
    /// </summary>
    [Table("USER_TAB_COMMENTS")]
    public class UserTabComments
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("TABLE_TYPE")]
        public string TableType { get; set; }

        [Column("COMMENTS")]
        public string Comments { get; set; }
    }
}
