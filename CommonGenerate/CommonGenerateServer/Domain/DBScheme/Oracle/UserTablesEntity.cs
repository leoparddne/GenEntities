using SqlSugar;

namespace Domain.DBScheme.Oracle
{
    [SugarTable("USER_TABLES")]
    public class UserTablesEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }
    }
}
