namespace Domain.DBScheme.MySql
{
    /// <summary>
    /// 表字段信息
    /// </summary>
    public class MySqlTableFieldEntity
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否可为null
        /// </summary>
        public string Null { get; set; }

        /// <summary>
        /// 关键字类型(PRI-主键  UNI-唯一键)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }
        //public string Extra { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }
    }
}
