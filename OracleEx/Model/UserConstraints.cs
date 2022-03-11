using System.ComponentModel.DataAnnotations.Schema;

namespace OracleEx.Model
{
    [Table("USER_CONSTRAINTS")]
    public class UserConstraints
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("CONSTRAINT_NAME")]
        public string ConstraintName { get; set; }

        [Column("CONSTRAINT_TYPE")]
        public string ConstraintType { get; set; }
    }
}
