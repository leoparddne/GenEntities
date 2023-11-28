using CommonGenerateClient.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonGenerateClient.Resource.Models
{
    public class DataSourceModel
    {
        public string ID { get; set; } = string.Empty;
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 数据源类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 数据源类型枚举
        /// </summary>
        public DataSourceTypeEnum DataSourceType { get { return (DataSourceTypeEnum)Type; } }
    }
}
