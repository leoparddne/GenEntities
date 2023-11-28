namespace CommonGenerateClient.Resource.Dto.Out
{
    public class UserTabColumnOutDto
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public string Nullable { get; set; }

        /// <summary>
        /// 是否可以为空值
        /// </summary>
        public bool IsNullable { get; set; }

        public string DataDefault { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool ISPrimaryKey { get; set; }

        /// <summary>
        /// 生成dto时是否需要忽略此字段
        /// </summary>
        public bool NeedSkip { get; set; }
    }
}
