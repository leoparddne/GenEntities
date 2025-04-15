using SqlSugar;

namespace Domain.DBScheme.Oracle
{
    [SugarTable("USER_CONSTRAINTS")]
    public class UserConstraintsEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]

        public string TableName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "CONSTRAINT_NAME")]
        public string ConstraintName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "CONSTRAINT_TYPE")]
        public string ConstraintType { get; set; }
    }
}
