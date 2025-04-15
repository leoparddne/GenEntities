using SqlSugar;

namespace Domain.DBScheme.Oracle
{
    /// <summary>
    /// 表注释
    /// </summary>
    [SugarTable("USER_TAB_COMMENTS")]
    public class UserTabCommentsEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_TYPE")]
        public string TableType { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "COMMENTS")]
        public string Comments { get; set; }
    }
}
