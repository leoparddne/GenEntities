using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Models
{
    public class BatchTemplate
    {
        /// <summary>
        /// 生成模板名称
        /// </summary>
        public string GenerateName { get; set; }

        /// <summary>
        /// 批量生产需要生成的表参数名 - 自动获取所有模板所需要的表参数名称并去重 - 多个别名将在页面要求选择对应数量的表
        /// </summary>
        public List<string> TableParameterName
        {
            get
            {
                if (Template == null || Template.Count == 0)
                {
                    return null;
                }

                var result = new List<string>();
                foreach (var item in Template)
                {
                    if (item.TableParameterName == null || item.TableParameterName.Count == 0)
                    {
                        continue;
                    }
                    result.AddRange(item.TableParameterName);
                }
                if (result.Count > 0)
                {
                    result = result.Distinct().ToList();
                }
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TemplateConfig> Template { get; set; }
    }
    public class TemplateConfig
    {
        /// <summary>
        /// 模板显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板相对主程序路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 自动生成表同名的目录名
        /// </summary>
        public bool AutoDir { get; set; }

        /// <summary>
        /// 模板生成后路径固定串
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// 文件名规则
        /// </summary>
        public string FileNameTemplate { get; set; }

        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 需要生成的表参数名 - 模板内部使用 - 多个别名将在页面要求选择对应数量的表
        /// </summary>
        public List<string> TableParameterName { get; set; }

        /// <summary>
        /// 生成文件时候对应文件名生成的依据 - 从TableParameterName配置中获取对应选择的表
        /// </summary>
        public string MainEntityParameterName { get; set; }
    }
    public class AppSettingSingleton
    {
        private static readonly object _lock = new object();
        private static AppSettingSingleton Ins { get; set; }
        private AppSettingSingleton()
        {

        }

        #region 
        //public string TextTransformPath { get; set; }
        public string API { get; set; }
        public List<BatchTemplate> Template { get; set; }
        #endregion

        public static AppSettingSingleton Instance
        {
            get
            {
                if (Ins != null)
                {
                    return Ins;
                }

                lock (_lock)
                {
                    {
                        if (Ins != null)
                        {
                            return Ins;
                        }

                        var appsettingPath = AppDomain.CurrentDomain.BaseDirectory + "appsetting.json";

                        if (!File.Exists(appsettingPath))
                        {
                            throw new Exception("setting err");
                        }

                        var appsettingContent = File.ReadAllText(appsettingPath);
                        if (string.IsNullOrEmpty(appsettingContent))
                        {
                            throw new Exception("setting cannot parse");
                        }

                        try
                        {
                            Ins = JsonConvert.DeserializeObject<AppSettingSingleton>(appsettingContent);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("setting cannot parse," + e.Message);
                        }
                    }
                }


                return Ins;
            }
        }
    }
}
