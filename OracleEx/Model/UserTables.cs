using System.ComponentModel.DataAnnotations.Schema;

namespace OracleEx.Model
{
    [Table("USER_TABLES")]
    public class UserTables
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }
    }
}
