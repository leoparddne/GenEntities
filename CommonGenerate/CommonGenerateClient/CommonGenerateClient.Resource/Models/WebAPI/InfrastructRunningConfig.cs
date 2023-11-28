using CommonGenerateClient.Resource.Dto.Out;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonGenerateClient.Resource.Models.WebAPI
{
    public class InfrastructRunningConfig
    {
        /// <summary>
        /// 基础名称空间:可以传递
        /// </summary>
        public string BaseNamespaceName { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public string TableDesc { get; set; }

        /// <summary>
        /// 表明
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段信息
        /// </summary>
        public List<UserTabColumnOutDto> Columns { get; set; }
        public string EntityName { get; set; }
        public string SavePath { get; set; }
        public string DBType { get; set; }
    }
}
