using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class InfrastructRunningConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string BaseNamespaceName { get; set; }
        public string EntityNamespaceName { get; set; }
        public string RepositoryNamespaceName { get; set; }
        public string IRepositoryNamespaceName { get; set; }
        public string ServiceNamespaceName { get; set; }
        public string IServiceNameSpaceName { get; set; }
        public string ControllerNameSpaceName { get; set; }
        public bool IsBaseRepository { get; set; }
        public string TableDesc { get; set; }
        public string TableName { get; set; }
        public string EntityFileName { get; set; }
        public string RepositoryName { get; set; }
        public string EntityName { get; set; }
        public string IRepositoryName { get; set; }
        public string ServiceName { get; set; }
        public string IServiceName { get; set; }
        public string ControllerName { get; set; }
        public string InDtoName { get; set; }
    }
}
