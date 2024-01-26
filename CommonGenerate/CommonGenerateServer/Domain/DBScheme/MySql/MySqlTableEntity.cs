namespace Domain.DBScheme.MySql
{
    /// <summary>
    /// 表基本信息
    /// </summary>
    public class MySqlTableEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }
        //public string Engine { get; set; }
        //public string Version { get; set; }
        //public string Row_format { get; set; }

        ///// <summary>
        ///// 表数据行
        ///// </summary>
        //public string Rows { get; set; }
        //public string Avg_row_length { get; set; }
        //public string Data_length { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public string Comment { get; set; }
    }
}
