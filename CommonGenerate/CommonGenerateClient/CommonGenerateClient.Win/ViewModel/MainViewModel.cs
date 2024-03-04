using CommonGenerateClient.Resource.Dto.Out;
using CommonGenerateClient.Resource.Enums;
using CommonGenerateClient.Resource.Models.WebAPI;
using CommonGenerateClient.Win.Ex;
using CommonGenerateClient.Win.Helpers;
using CommonGenerateClient.Win.Models;
using CommonGenerateClient.Win.T4s;
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
        /// 原始的所有表
        /// </summary>
        //public ObservableCollection<SelectTypeModel> FilterDataSourceList { get; set; } = new ObservableCollection<SelectTypeModel>();

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


        //private SelectTypeModel selectModel;

        ///// <summary>
        ///// 选中的数据
        ///// </summary>
        //public SelectTypeModel SelectModel
        //{
        //    get { return selectModel; }
        //    set { selectModel = value; LoadParameterMapper(); }
        //}


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

        //private string searchTableText;
        ///// <summary>
        ///// 搜索表名
        ///// </summary>
        //public string SearchTableText
        //{
        //    get { return searchTableText; }
        //    set
        //    {
        //searchTableText = value;

        //FilterTableCale(value);
        //    }
        //}

        //private void FilterTableCale(string text)
        //{
        //    var searchFinalText = text.ToUpper();
        //    if (string.IsNullOrEmpty(searchFinalText))
        //    {
        //        FilterDataSourceList = new ObservableCollection<SelectTypeModel>(DataSourceList.Take(10));
        //    }
        //    else
        //    {
        //        var addData = DataSourceList.Where(f => f.Label.ToUpper().Contains(searchFinalText))?.ToList();

        //        if (!addData.IsNullOrEmpty())
        //        {
        //            FilterDataSourceList = new ObservableCollection<SelectTypeModel>(addData.Take(10));
        //        }
        //    }
        //}


        private async void LoadTable(SelectTypeModel value)
        {
            if (value == null)
            {
                return;
            }
            //获取数据库类型
            try
            {
                var dbTypeValue = await HttpHelper.GetAsync<CommonDto<string>>(BaseURL + $"api/Data/GetDBType?configID={value.Value}", null, null, 10 * 1000);
                DBType = dbTypeValue.Data;
            }
            catch (Exception)
            {
                DBType = "Oracle";
            }

            var ret = await HttpHelper.GetAsync<CommonDto<List<TableModel>>>(BaseURL + $"api/Data/GetList?configID={value.Value}", null, null, 10 * 1000);

            DataSourceList.Clear();
            //FilterDataSourceList.Clear();
            if (ret.Data.IsNullOrEmpty())
            {
                return;
            }
            var sortTableList=ret.Data.OrderBy(f => f.TableName);
            foreach (var item in sortTableList)
            {
                //if (string.IsNullOrEmpty(SearchTableText) || item.TableName.ToUpper().Contains(SearchTableText.ToUpper()))
                //{
                //    FilterDataSourceList.Add(new SelectTypeModel { Value = item.TableType, Label = item.TableName, Name = item.Comments });
                //}
                DataSourceList.Add(new SelectTypeModel { Value = item.TableType, Label = item.TableName, Name = item.Comments });
            }
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

            //TableSelctedCommand = new CommandBase(l =>
            //{
            //    if (l is SelectTypeModel typeModel)
            //    {
            //        SelectModel = DataSourceList.FirstOrDefault(f => f.Label == typeModel.Label);
            //        if (SelectModel != null && !string.IsNullOrWhiteSpace(SelectModel.Value))
            //        {
            //            var split = SelectModel.Label.Split('_');
            //            if (split.Count() > 1)
            //            {
            //                TxtPrefix = split.FirstOrDefault() + "_";
            //            }
            //            else
            //            {
            //                TxtPrefix = string.Empty;
            //            }
            //        }
            //    }
            //});
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
                //fun.SavePath = folderBrowser.SelectedPath;

                //"T_BD_PART"
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

        private async void Load()
        {
            try
            {
                //获取数据库列表
                var dbList = await HttpHelper.GetAsync<CommonDto<ObservableCollection<SelectTypeModel>>>(BaseURL + "api/Data/GetDBSourceSelect", null, null, 10 * 1000);
                if (dbList != null)
                {
                    DBSourceList = dbList.Data;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
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

                var tableData = await HttpHelper.GetAsync<CommonDto<UserTabCommentsOutDto>>(BaseURL + $"api/Data/GetDataInfo?dataName={tableName}&configID={SelectDB.Value}", null, null, 60 * 1000);
                UserTabCommentsOutDto? table = tableData.Data;

                dataSource.TableInfo = table;


                var tableFieldsData = await HttpHelper.GetAsync<CommonDto<List<UserTabColumnOutDto>>>(BaseURL + $"api/Data/GetDataDetail?table={tableName}&configID={SelectDB.Value}", null, null, 60 * 1000);
                List<UserTabColumnOutDto> tableFields = tableFieldsData.Data;

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

            //var infrastructDir = ResourceHelper.GetDir("\\"+GUIDHelper.NewGuid +"-"+ ResourceHelper.TempInfrastructPath);

            //var infrastructDir = "\\" + GUIDHelper.NewGuid + "-" + ResourceHelper.TempInfrastructPath;
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

            ////TODO - tableParameterDic
            ////host.Session.Add("ParameterName", base64Path);
            ////批量生成多个参数信息
            //foreach (var item in generateTemplate.TableParameterName)
            //{
            //    //获取对应的数据
            //    var tableInfo = tableInfoList.FirstOrDefault(f => f.T4ParameterName == item);
            //    if (tableInfo == null)
            //    {
            //        //TODO - 
            //    }
            //    var tableInfoBase64FilePath = GenerateT4TableParameterFilePath(tableInfo, apiNameSpace, savePath);
            //    tableInfoBase64FilePath.Wait();
            //    //tableParameterDic.Add(item, tableInfoFile);
            //    //host.Session.Add("ParameterName", base64Path);
            //    host.Session.Add(item, tableInfoBase64FilePath.Result);
            //}

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

            var fileName = fileSavePath + generateTemplate.TemplatePath + "\\" + fileTemplateFinal + generateTemplate.FileExt;
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