using CommonGenerateClient.Resource.Dto.Out;
using CommonGenerateClient.Resource.Enums;
using CommonGenerateClient.Resource.Models.WebAPI;
using CommonGenerateClient.Win.Ex;
using CommonGenerateClient.Win.Helpers;
using CommonGenerateClient.Win.Models;
using CommonGenerateClient.Win.T4s;
using DBConnecter;
using HandyControl.Controls;
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility.Common;
using MessageBox = System.Windows.MessageBox;

namespace CommonGenerateClient.Win.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// 页面绑定的表
        /// </summary>
        public ObservableCollection<SelectTypeModel> DataSourceList { get; set; } = new ObservableCollection<SelectTypeModel>();

        /// <summary>
        /// 数据库
        /// </summary>
        public ObservableCollection<SelectTypeModel> DBSourceList { get; set; } = new ObservableCollection<SelectTypeModel>();

        public ObservableCollection<TemplateConfig> TemplateList { get; set; } = new();

        /// <summary>
        /// 多表批量生成模板
        /// </summary>
        public ObservableCollection<BatchTemplate> BatchTemplates { get; set; } = new();

        /// <summary>
        /// 选择的多表批量生成模板
        /// </summary>
        private BatchTemplate selectBatchTemplate;

        public BatchTemplate SelectBatchTemplate
        {
            get { return selectBatchTemplate; }
            set { selectBatchTemplate = value; LoadParameterMapper(); }
        }

        /// <summary>
        /// 过滤模板标签
        /// </summary>
        public string SelectTemplateTag { get; set; }


        private TemplateConfig selectTemplate;
        /// <summary>
        /// 选择的独立运行模板
        /// </summary>
        public TemplateConfig SelectTemplate
        {
            get { return selectTemplate; }
            set { selectTemplate = value; LoadParameterMapper(); }
        }


        private SelectTypeModel selectDB;

        public SelectTypeModel SelectDB
        {
            get { return selectDB; }
            set
            {
                selectDB = value;

                LoadTable(value);
            }
        }



        private bool isBatchGenerateMode = true;

        /// <summary>
        /// 批量生成模式 - tag生成模式
        /// </summary>
        public bool ISBatchGenerateMode
        {
            get { return isBatchGenerateMode; }
            set { isBatchGenerateMode = value; LoadParameterMapper(); }
        }

        /// <summary>
        /// 选择的所有表 - 多表逻辑下使用
        /// </summary>
        public ObservableCollection<SelectTypeModel> SelectTableList { get; set; } = new();


        /// <summary>
        /// 参数与表绑定关系
        /// </summary>
        public ObservableCollection<ParameterMapper> ParameterMapperList { get; set; } = new();


        public ICommand TableSelctedCommand { get; set; }
        public ICommand GenerateCommand { get; set; }

        ///// <summary>
        ///// 前缀
        ///// </summary>
        //public string TxtPrefix { get; set; }
        public string BaseURL { get; }


        //打开的数据库
        private DBHelper dbHelper;


        private async void LoadTable(SelectTypeModel value)
        {
            if (value == null)
            {
                return;
            }

            var selectDB = DBConfigSingleton.Instance.DBConfig.FirstOrDefault(f => f.ConfigID == SelectDB.Value);
            if (selectDB == null)
            {
                HandyControl.Controls.Growl.Warning("请选择数据库.");
                return;
            }
            dbHelper = new DBHelper(selectDB.ConnectionType, selectDB.ConnectionString);

            //获取数据库类型
            DBType = selectDB.ConnectionType.ToString();

            //获取数据库下的所有表
            var rawTableList = dbHelper.GetTableList();

            if (rawTableList == null || rawTableList.Count == 0)
            {
                return;
            }

            var tableList = new List<SelectTypeModel>();
            foreach (var item in rawTableList)
            {
                tableList.Add(new SelectTypeModel { Value = item.TableType, Label = item.TableName, Name = item.Comments });
            }

            DataSourceList = new(tableList);
        }

        public MainViewModel()
        {
            if (AppSettingSingleton.Instance.Template != null && AppSettingSingleton.Instance.Template.Count > 0)
            {
                foreach (var item in AppSettingSingleton.Instance.Template)
                {
                    if (item != null && !item.Template.IsNullOrEmpty())
                    {
                        foreach (var itemTemplate in item.Template)
                        {
                            TemplateList.Add(itemTemplate);
                        }
                    }

                    BatchTemplates.Add(item);
                }

                if (BatchTemplates != null && BatchTemplates.Count > 0)
                {
                    SelectBatchTemplate = BatchTemplates.First();
                }

                SelectTemplate = TemplateList.First();
            }

            var apiBaseURL = AppSettingSingleton.Instance.API;
            if (apiBaseURL == null)
            {
                MessageBox.Show("config can not fetch");
                return;
            }
            BaseURL = apiBaseURL;// "http://192.168.2.49:7013/";

            Load();

            GenerateCommand = new CommandBase(l =>
            {
                if (SelectDB == null)
                {
                    HandyControl.Controls.Growl.Error("请选择数据库.");
                    return;
                }

                //检测是否存在未选择的表
                if (ParameterMapperList != null && ParameterMapperList.Count > 0)
                {
                    if (ParameterMapperList.Any(f => f.SelectTable == null))
                    {
                        HandyControl.Controls.Growl.Error("请选择表.");
                        return;
                    }
                }



                if (ISBatchGenerateMode)
                {
                    if (SelectBatchTemplate == null)
                    {
                        HandyControl.Controls.Growl.Info("请选择tag.");
                        return;
                    }
                }
                else
                {
                    if (SelectTemplate == null)
                    {
                        HandyControl.Controls.Growl.Error("请选择模板.");
                        return;
                    }
                }


                System.Windows.Forms.FolderBrowserDialog folderBrowser = new();
                if (folderBrowser.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                Task.Run(async () =>
                {
                    var tableInfoListTask = GetTemplateTableInfo("Generate");
                    tableInfoListTask.Wait();
                    var tableInfoList = tableInfoListTask.Result;
                    if (tableInfoList == null || tableInfoList.Count == 0)
                    {
                        HandyControl.Controls.Growl.Info("未获取道任何表信息.");
                        return;
                    }
                    if (ISBatchGenerateMode)
                    {

                        if (SelectBatchTemplate.Template.IsNullOrEmpty())
                        {
                            HandyControl.Controls.Growl.Info("此标签下未配置模板.");
                            return;
                        }
                        foreach (var item in SelectBatchTemplate.Template)
                        {
                            GenerateAPI(item, tableInfoList, "Generate", folderBrowser.SelectedPath).Wait();
                        }

                        HandyControl.Controls.Growl.Info("批量生成完成.");
                    }
                    else
                    {
                        await GenerateAPI(SelectTemplate, tableInfoList, "Generate", folderBrowser.SelectedPath);
                    }
                });

            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 加载数据库列表
        /// </summary>
        private async void Load()
        {
            try
            {
                //获取数据库列表
                var rawTableList = DBConfigSingleton.Instance.DBConfig;

                if (rawTableList == null || rawTableList.Count == 0)
                {
                    return;
                }

                var tableList = new List<SelectTypeModel>();
                foreach (var item in rawTableList)
                {
                    tableList.Add(new SelectTypeModel { Value = item.ConfigID, Label = item.ConnectionString, Name = item.ConnectionString });
                }

                DBSourceList = new(tableList);
            }
            catch (Exception e)
            {
                HandyControl.Controls.Growl.Error(e.Message);
            }
        }

        /// <summary>
        /// 获取页面上所有表对应的配置
        /// </summary>
        /// <param name="apiNameSpace"></param>
        /// <returns></returns>
        private async Task<List<DataSourceGenerateModel>> GetTemplateTableInfo(string apiNameSpace)
        {
            var dataSourceList = new List<DataSourceGenerateModel>();

            if (ParameterMapperList.IsNullOrEmpty())
            {
                return dataSourceList;
            }


            foreach (var item in ParameterMapperList)
            {
                if (item.SelectTable == null)
                {
                    HandyControl.Controls.Growl.Error("请配置模板所需的表");
                    return dataSourceList;
                }
                var dataSource = new DataSourceGenerateModel { Data = item.SelectTable.Label, DataSourceType = DataSourceTypeEnum.Tabel, TablePrefix = item.TablePreFix ?? "", Type = 0 };
                dataSourceList.Add(dataSource);

                dataSource.T4ParameterName = item.Parameter;

                // TODO 
                // dataSource.Type
                string preMask = dataSource.TablePrefix;
                string tableName = dataSource.Data;
                string namespaceName = apiNameSpace;
                dataSource.NameSpaceName = namespaceName;

                var formatTableName = tableName;
                if (!string.IsNullOrWhiteSpace(preMask))
                {
                    formatTableName = tableName.Replace(preMask, "");
                }
                string entityName = CamelCaseUtility.Convert2Camel(formatTableName);
                dataSource.EntityName = entityName;

                UserTabCommentsOutDto? table = dbHelper.GetTableByName(tableName);

                dataSource.TableInfo = table;

                List<UserTabColumnOutDto> tableFields = dbHelper.GetColumnInfo(tableName)?.ToList();

                dataSource.TableFields = tableFields;
            }

            return dataSourceList;
        }

        /// <summary>
        /// 生成模板所需要的表参数
        /// </summary>
        /// <param name="generateTemplate"></param>
        /// <param name="apiNameSpace"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private async Task<string> GenerateT4TableParameterFilePath(List<DataSourceGenerateModel> allDataSource, string apiNameSpace, string savePath)
        {
            Dictionary<string, InfrastructModel> allParameterModel = new();
            foreach (var dataSource in allDataSource)
            {
                string preMask = dataSource.TablePrefix;
                string tableName = dataSource.Data;
                string namespaceName = apiNameSpace;

                var formatTableName = tableName;
                if (!string.IsNullOrWhiteSpace(preMask))
                {
                    formatTableName = tableName.Replace(preMask, "");
                }
                string entityName = CamelCaseUtility.Convert2Camel(formatTableName);

                UserTabCommentsOutDto? table = dataSource.TableInfo;

                List<UserTabColumnOutDto> tableFields = dataSource.TableFields;

                //忽略字段
                var allColumns = tableFields.Select(f => f.ColumnName);

                List<string> skipProperty = new List<string> { "CREATE_USER", "CREATE_TIME", "UPDATE_USER", "UPDATE_TIME" };

                //忽略未选择的字段、固定的更新字段
                var skipColumns = tableFields.Where(f => skipProperty.Contains(f.ColumnName.ToUpper()));//.Select();
                foreach (var item in skipColumns)
                {
                    item.NeedSkip = true;
                }
                InfrastructModel infrastructModel = new InfrastructModel()
                {
                    Config = new InfrastructRunningConfig
                    {
                        BaseNamespaceName = namespaceName,
                        TableDesc = table.Comments,
                        TableName = table.TableName,
                        Columns = tableFields,
                        EntityName = entityName,
                        SavePath = savePath,
                        DBType = DBType
                    }
                };
                allParameterModel.Add(dataSource.T4ParameterName, infrastructModel);
            }

            var infrastructDir = "\\" + ResourceHelper.TempInfrastructPath;

            ResourceHelper.SaveInfrastructDataSource(infrastructDir, allParameterModel);

            var fullInfrastructDir = ResourceHelper.GetDir(infrastructDir);
            //var encodePath=System.Web.HttpUtility.UrlEncode(infrastructDir);
            //var jsonData = JsonConvert.SerializeObject(infrastructDir);
            var pathBytes = Encoding.UTF8.GetBytes(fullInfrastructDir);
            var base64Path = Convert.ToBase64String(pathBytes);

            return base64Path;
        }

        /// <summary>
        /// 生成模板所需要的表参数
        /// </summary>
        /// <param name="generateTemplate"></param>
        /// <param name="apiNameSpace"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private async Task<string> GenerateT4TableParameterFilePath(DataSourceGenerateModel dataSource, string apiNameSpace, string savePath)
        {
            string preMask = dataSource.TablePrefix;
            string tableName = dataSource.Data;
            string namespaceName = apiNameSpace;

            var formatTableName = tableName;
            if (!string.IsNullOrWhiteSpace(preMask))
            {
                formatTableName = tableName.Replace(preMask, "");
            }
            string entityName = CamelCaseUtility.Convert2Camel(formatTableName);

            UserTabCommentsOutDto? table = dataSource.TableInfo;

            List<UserTabColumnOutDto> tableFields = dataSource.TableFields;

            //忽略字段
            var allColumns = tableFields.Select(f => f.ColumnName);

            //TODO 后续添加配置可以动态控制需要跳过的表字段
            //List<string> skipProperty = new List<string> { "CREATE_USER", "CREATE_TIME", "UPDATE_USER", "UPDATE_TIME" };
            List<string> skipProperty = new List<string>();

            //忽略未选择的字段、固定的更新字段
            var skipColumns = tableFields.Where(f => skipProperty.Contains(f.ColumnName.ToUpper()));//.Select();
            foreach (var item in skipColumns)
            {
                item.NeedSkip = true;
            }
            InfrastructModel infrastructModel = new InfrastructModel()
            {
                Config = new InfrastructRunningConfig
                {
                    BaseNamespaceName = namespaceName,
                    TableDesc = table.Comments,
                    TableName = table.TableName,
                    Columns = tableFields,
                    EntityName = entityName,
                    SavePath = savePath,
                    DBType = DBType
                }
            };

            //var infrastructDir = ResourceHelper.GetDir("\\"+GUIDHelper.NewGuid +"-"+ ResourceHelper.TempInfrastructPath);

            var infrastructDir = "\\" + GUIDHelper.NewGuid + "-" + ResourceHelper.TempInfrastructPath;

            ResourceHelper.SaveInfrastructDataSource(infrastructDir, infrastructModel);

            var fullInfrastructDir = ResourceHelper.GetDir(infrastructDir);
            //var encodePath=System.Web.HttpUtility.UrlEncode(infrastructDir);
            //var jsonData = JsonConvert.SerializeObject(infrastructDir);
            var pathBytes = Encoding.UTF8.GetBytes(fullInfrastructDir);
            var base64Path = Convert.ToBase64String(pathBytes);

            return base64Path;
        }

        /// <summary>
        /// 根据模板生成
        /// </summary>
        /// <param name="generateTemplate"></param>
        /// <param name="apiNameSpace"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private async Task GenerateAPI(TemplateConfig generateTemplate, List<DataSourceGenerateModel> tableInfoList, string apiNameSpace, string savePath)
        {
            if (generateTemplate == null)
            {
                HandyControl.Controls.Growl.Error("请选择模板");
                return;
            }


            CustomCmdLineHost host = new CustomCmdLineHost();
            Engine engine = new Engine();
            host.TemplateFileValue = $@"{ResourceHelper.TargetDir}{generateTemplate.Path}";

            //参数传递
            host.Session = new TextTemplatingSession();

            //TODO - 使用批量逻辑优化
            var tableInfoBase64FilePath = GenerateT4TableParameterFilePath(tableInfoList, apiNameSpace, savePath);
            host.Session.Add("ParameterName", tableInfoBase64FilePath.Result);



            //Read the text template.
            string input = File.ReadAllText($@"{ResourceHelper.TargetDir}{generateTemplate.Path}");

            host.StandardAssemblyReferences.Add("Mono.TextTemplating.dll");
            host.StandardAssemblyReferences.Add("CommonGenerateClient.Win.dll");

            host.AddStandardAssemblyReferences(ResourceHelper.TargetDir + "CommonGenerateClient.Resource.dll");

            //host.AddStandardAssemblyReferences(ResourceHelper.TargetDir + "netstandard.dll");


            host.AddStandardAssemblyReferences(ResourceHelper.TargetDir + "Utility.dll");

            host.AddStandardAssemblyReferences(ResourceHelper.TargetDir + "Newtonsoft.Json.dll");


            //host.StandardAssemblyReferences.Add("System.Core.dll");
            host.StandardImports.Add("CommonGenerateClient.Win.Models");

            string output = engine.ProcessTemplate(input, host);


            //string fileSavePath = infrastructModel.Config.SavePath;
            string fileSavePath = savePath;

            //获取此模板所需的主实体名称
            string mainEntityName = string.Empty;
            if (generateTemplate.TableParameterName != null && generateTemplate.TableParameterName.Count != 0)
            {
                foreach (var item in tableInfoList)
                {
                    //匹配第一个主表
                    if (item.T4ParameterName == generateTemplate.MainEntityParameterName)
                    {
                        mainEntityName = item.EntityName;
                        break;
                    }
                }
            }


            if (string.IsNullOrEmpty(mainEntityName))
            {
                mainEntityName = tableInfoList.First().EntityName;
            }

            //根据配置调整输出文件名
            //var fileTemplateFinal = string.Format(generateTemplate.FileNameTemplate, entityName);
            var fileTemplateFinal = string.Format(generateTemplate.FileNameTemplate, mainEntityName);

            //根据AutoDir生成表同名的子目录
            var fileName = fileSavePath + generateTemplate.TemplatePath + (generateTemplate.AutoDir ? ("\\" + mainEntityName) : string.Empty) + "\\" + fileTemplateFinal + generateTemplate.FileExt;
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            File.WriteAllText(fileName, output);


            if (host.Errors.IsNullOrEmpty())
            {
                HandyControl.Controls.Growl.Info($"{generateTemplate.Name} 生成成功.");
                return;
            }

            foreach (CompilerError error in host.Errors)
            {
                //LogHelper.WriteLogForCustom(error.ToString());
                Growl.Error(error.ToString());
            }
        }

        public void LoadParameterMapper()
        {
            ParameterMapperList.Clear();

            List<string> parameterList = null;
            //批量生产
            if (ISBatchGenerateMode)
            {
                //SelectBatchTemplate
                if ((SelectBatchTemplate?.TableParameterName) == null)
                {
                    ParameterMapperList.Clear();
                    return;
                }
                parameterList = SelectBatchTemplate.TableParameterName;
            }
            else
            {
                //SelectTemplate
                if ((SelectTemplate) == null)
                {
                    ParameterMapperList.Clear();
                    return;
                }
                parameterList = SelectTemplate.TableParameterName;
            }

            if (parameterList == null)
            {
                ParameterMapperList.Clear();
                return;
            }

            foreach (var item in parameterList)
            {
                ParameterMapperList.Add(new ParameterMapper
                {
                    Parameter = item,
                });
            }
        }
    }
}
