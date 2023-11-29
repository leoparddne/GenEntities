using CommonGenerateClient.Resource.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Models
{
    [DoNotNotify]
    public class DataSourceModel
    {
        private int type;
        private string data = string.Empty;

        /// <summary>
        /// 数据源类型
        /// </summary>
        public int Type
        {
            get => type; set
            {
                type = value;
                DataSourceType = (DataSourceTypeEnum)value;
            }
        }

        /// <summary>
        /// 数据源类型枚举
        /// </summary>
        public DataSourceTypeEnum DataSourceType { get; set; } = DataSourceTypeEnum.Tabel;

        /// <summary>
        /// 数据源内容,可以是表名，接口，常量数据或存储过程
        /// </summary>
        public string Data
        {
            get => data; set
            {
                data = value;
                if (value != null)
                {
                    if (Type == 0)
                    {
                        int index = value.LastIndexOf('_');
                        TablePrefix = value[..(index + 1)];
                    }
                }
            }
        }


        /// <summary>
        /// 表前缀
        /// </summary>
        public string TablePrefix { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; } = string.Empty;
    }
}
