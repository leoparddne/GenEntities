using CommonGenerateClient.Resource.Dto.Out;
using CommonGenerateClient.Resource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Models
{
    /// <summary>
    /// 生成代码所需的额外数据
    /// </summary>
    public class DataSourceGenerateModel : DataSourceModel
    {
        /// <summary>
        /// 表相关数据 - 表名、注释
        /// </summary>
        public UserTabCommentsOutDto TableInfo { get; set; }

        /// <summary>
        /// 表字段
        /// </summary>
        public List<UserTabColumnOutDto> TableFields { get; set; }

        /// <summary>
        /// 生成的实体名
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 生成的名称空间
        /// </summary>
        public string NameSpaceName { get; set; }

        /// <summary>
        /// T4所需要的参数名
        /// </summary>
        public string T4ParameterName { get; set; }
    }
}
