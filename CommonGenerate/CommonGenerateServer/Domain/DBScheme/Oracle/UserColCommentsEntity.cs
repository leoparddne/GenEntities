using SqlSugar;

namespace Domain.DBScheme.Oracle
{
    /// <summary>
    /// 字段注释
    /// </summary>
    [SugarTable("USER_COL_COMMENTS")]
    public class UserColCommentsEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "COLUMN_NAME")]
        public string ColumnName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "COMMENTS")]
        public string Comments { get; set; }
    }
}
