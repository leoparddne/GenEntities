using Model;
using OracleEx.DBContext;
using OracleEx.Ex;
using OracleEx.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using T4Ext;

namespace OracleGenerate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SchemeContext context;
        public MainWindow(SchemeContext context)
        {
            InitializeComponent();
            this.context = context;
        }

        #region 基表查询语句
        //获取用户创建的表
        //select table_name from user_tables;

        //获取表字段
        //select* from user_tab_columns where Table_Name = '用户表';
        //获取表注释
        //select* from user_tab_comments user_tab_comments：table_name,table_type,comments

        //获取字段注释
        //select * from user_col_comments
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearMsg();

            string table = txtTableName.Text.ToUpper(), mask = txtMask.Text, path = txtSavePath.Text;
            this.Dispatcher.Invoke(async () =>
            {
                await Task.Run(() =>
                {
                    Generate(table, mask, path);

                });

                txtMsg.Content = "生成成功";
            }, System.Windows.Threading.DispatcherPriority.Background);
        }
        private void ClearMsg()
        {
            txtMsg.Content = "";
        }

        private void Generate(string tableName, string tableMask, string path)
        {
            List<string> searchTables = new List<string>();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                MessageBox.Show("请输入要生成的表名");
                return;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            tableName = tableName.ToUpper();

            string nameSpace = "baseNamespaceName";
            //string tableDesc = "";
            //string tableName =  "t_pd_wo_msl"; //数据库表名称
            string entityFileName = GenName(tableName, tableMask);//实体基本名称
            string entityName = entityFileName + "Entity"; //实体文件名称
            //string saveFile = $@"D:\{entityName}.cs";
            string saveFile = $@"{path + entityName}.cs";

            //TODO 后续扩展成显示所有表后勾选要生成的表
            searchTables.Add(tableName);


            List<OracleEx.Model.UserTables> tables = context.UserTables.Where(f => searchTables.Contains(f.TableName)).ToList();

            //List<OracleEx.Model.UserTables>? tmp = tables.Where(f => f.TableName == "T_SYS_USER").ToList();
            //foreach (OracleEx.Model.UserTables? table in tmp)
            foreach (OracleEx.Model.UserTables table in tables)
            {
                //表字段
                List<OracleEx.Model.UserTabColumn> columns = context.UserTabColumns.Where(f => f.TableName == table.TableName).ToList();
                List<string> columnName = columns.Select(f => f.ColumnName).ToList();

                string tableDesc = string.Empty;
                //表注释
                OracleEx.Model.UserTabComments? tableComment = context.UserTabComments.Where(f => f.TableName == table.TableName).FirstOrDefault();
                if (tableComment != null)
                {
                    tableDesc = tableComment.Comments;
                }

                Dictionary<string, string> columnComments = new();
                if (columns.Any())
                {
                    //字段注释
                    columnComments = context.UserColComments.Where(f => f.TableName == table.TableName && columnName.Contains(f.ColumnName)).ToDictionary(f => f.ColumnName, f => f.Comments);
                }



                //获取主键
                string? primaryKey = context.UserConstraints.Join(context.UserConsColumns, f => f.ConstraintName, f => f.ConstraintName, (cons, columns) => new { columns.ColumnName, cons.ConstraintType, cons.TableName }).Where(f => f.TableName == table.TableName && f.ConstraintType == "P").Select(f => f.ColumnName).FirstOrDefault();

                string? entityFieldStr = GenEntity(table.TableName, tableDesc, primaryKey, columns, columnComments);

                string? entityStartBlock = $@"using MES.Server.Domain.Entity;
                using SqlSugar;
using System;

                namespace  {nameSpace}
                {{
                    /// <summary>
                    /// {tableDesc}
                    /// </summary>
                    [SugarTable(""{table.TableName}"")]
                    public class {entityName} : CommonEntity, IEnabled, ICommonID
                    {{";

                string? entityEndBlock = @"
                        }
                    }";
                File.WriteAllText(saveFile, entityStartBlock);
                File.AppendAllText(saveFile, entityFieldStr);
                File.AppendAllText(saveFile, entityEndBlock);
            }
        }

        class FiledModel : UserTabColumn
        {

        }

        /// <summary>
        /// 驼峰
        /// </summary>
        /// <param name="rawStr"></param>
        /// <returns></returns>
        string GenName(string rawStr, string prevMask = "")
        {
            string? tmp = rawStr;
            //移除前缀
            if (!string.IsNullOrWhiteSpace(prevMask))
            {
                if (tmp.StartsWith(prevMask))
                {
                    tmp = tmp.Substring(prevMask.Length);
                }
            }

            StringBuilder result = new();
            bool start = true;

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == '_')
                {
                    start = true;
                    continue;
                }

                //首字母不变
                if (start)
                {
                    result.Append(tmp[i]);
                    start = false;
                    continue;
                }

                //大小转小写
                if (tmp[i] >= 65 && tmp[i] <= (65 + 25))
                {
                    result.Append((char)(tmp[i] + 32));
                }
                else
                {
                    //其他字符原样输出
                    result.Append(tmp[i]);
                }
            }



            return result.ToString();
        }

        /// <summary>
        /// 生成实体的字段信息
        /// </summary>
        /// <param name="fileds">所有字段</param>
        /// <param name="columnComments">字段注释</param>
        /// <returns></returns>
        string GenEntity(string tableName, string tableDesc, string primaryKey, List<UserTabColumn> fileds, Dictionary<string, string> columnComments, bool withSql = true, List<string> skipList = null)
        {
            StringBuilder all = new();

            foreach (UserTabColumn? field in fileds)
            {
                string name = field.ColumnName;
                string? convertName = GenName(name);

                if (skipList?.Contains(convertName) ?? false)
                {
                    continue;
                }

                //bool start = true;

                StringBuilder finalName = new();

                //for (int i = 0; i < name.Length; i++)
                //{
                //    if (name[i] == '_')
                //    {
                //        start = true;
                //        continue;
                //    }

                //    //首字母不变
                //    if (start)
                //    {
                //        finalName.Append(name[i]);
                //        start = false;
                //        continue;
                //    }

                //    //大小转小写
                //    if (name[i] >= 65 && name[i] <= (65 + 25))
                //    {
                //        finalName.Append((char)(name[i] + 32));
                //    }
                //    else
                //    {
                //        //其他字符原样输出
                //        finalName.Append(name[i]);
                //    }
                //}
                finalName.Append(convertName);

                //获取字段注释
                string commnet = "";//columns[columns.Length - 1];
                if (columnComments != null && columnComments.ContainsKey(field.ColumnName))
                {
                    columnComments.TryGetValue(field.ColumnName, out commnet);
                }

                string type = TypeEx.CalcType(field.DataType);

                StringBuilder resultStr = new();
                resultStr.AppendLine("/// <summary>");
                resultStr.AppendLine("/// " + commnet);
                resultStr.AppendLine("/// </summary>");

                if (withSql)
                {
                    resultStr.Append($"[SugarColumn(  {((primaryKey?.Trim() ?? "") == field.ColumnName.Trim() ? "IsPrimaryKey = true," : "")} ColumnDescription = \"");
                    resultStr.Append(commnet);
                    resultStr.Append("\",  ColumnName = \"");
                    resultStr.Append(name.ToLower());
                    resultStr.Append("\")]");
                }


                all.AppendLine(resultStr.ToString());

                //是否可以为空

                string result = $"public {type}{(field.IsNullable ? "?" : "")} {finalName}" + "{ get;set; }\n";
                all.AppendLine(result);

            }

            Trace.WriteLine(all.ToString());
            return all.ToString();
        }

        private void btnInDto_Click(object sender, RoutedEventArgs e)
        {
            ClearMsg();
            List<string>? skipProperty = new List<string> { "CreateUser", "CreateTime", "UpdateUser", "UpdateTime" };
            string table = txtTableName.Text.ToUpper(), mask = txtMask.Text, path = txtSavePath.Text;
            this.Dispatcher.Invoke(async () =>
            {
                await Task.Run(() =>
                {
                    GenerateInDto(table, mask, path, skipProperty);

                });

                txtMsg.Content = "生成成功";
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        private void GenerateInDto(string tableName, string tableMask, string path, List<string> skipProperty)
        {
            List<string> searchTables = new List<string>();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                MessageBox.Show("请输入要生成的表名");
                return;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            tableName = tableName.ToUpper();

            string nameSpace = "baseNamespaceName";

            string entityFileName = GenName(tableName, tableMask);//实体基本名称
            string entityName = entityFileName + "InDto"; //实体文件名称
            string saveFile = $@"{path + entityName}.cs";

            //TODO 后续扩展成显示所有表后勾选要生成的表
            searchTables.Add(tableName);

            //OPTIMIZE
            List<OracleEx.Model.UserTables> tables = context.UserTables.Where(f => searchTables.Contains(f.TableName)).ToList();

            foreach (OracleEx.Model.UserTables table in tables)
            {
                //表字段
                List<OracleEx.Model.UserTabColumn> columns = context.UserTabColumns.Where(f => f.TableName == table.TableName).ToList();
                List<string> columnName = columns.Select(f => f.ColumnName).ToList();

                string tableDesc = string.Empty;
                //表注释
                OracleEx.Model.UserTabComments? tableComment = context.UserTabComments.Where(f => f.TableName == table.TableName).FirstOrDefault();
                if (tableComment != null)
                {
                    tableDesc = tableComment.Comments;
                }

                Dictionary<string, string> columnComments = new();
                if (columns.Any())
                {
                    //字段注释
                    columnComments = context.UserColComments.Where(f => f.TableName == table.TableName && columnName.Contains(f.ColumnName)).ToDictionary(f => f.ColumnName, f => f.Comments);
                }



                //获取主键
                //var primaryKey = context.UserConstraints.Join(context.UserConsColumns, f => f.ConstraintName, f => f.ConstraintName, (cons, columns) => new { columns.ColumnName, cons.ConstraintType, cons.TableName }).Where(f => f.TableName == table.TableName && f.ConstraintType == "P").Select(f => f.ColumnName).FirstOrDefault();

                string? entityFieldStr = GenEntity(table.TableName, tableDesc, /*primaryKey*/ "", columns, columnComments, false, skipProperty);

                string? entityStartBlock = $@"using MES.Server.Domain.Entity;
                using System;

                namespace  {nameSpace}
                {{
                    /// <summary>
                    /// {tableDesc}
                    /// </summary>
                    public class {entityName} 
                    {{";

                string? entityEndBlock = @"
                        }
                    }";
                File.WriteAllText(saveFile, entityStartBlock);
                File.AppendAllText(saveFile, entityFieldStr);
                File.AppendAllText(saveFile, entityEndBlock);
            }
        }

        private void btnGenInfrastruct_Click(object sender, RoutedEventArgs e)
        {
            InfrastructModel model = new();


            model.Config.BaseNamespaceName = "TMES.Server.Test"; //基础名称空间
            model.Config.EntityNamespaceName = "TDomain.Entity"; //实体名称空间
            model.Config.RepositoryNamespaceName = "TInfrastruct.Repository"; //仓储名称空间
            model.Config.IRepositoryNamespaceName = "TDomain.IRepository"; //仓储接口名称空间
            model.Config.ServiceNamespaceName = "TApplication.Service"; //服务名称空间
            model.Config.IServiceNameSpaceName = "TApplication.IService"; //服务接口名称空间
            model.Config.ControllerNameSpaceName = "TControllers";//控制器名称空间

            model.Config.IsBaseRepository = true; //是否生成基础仓储模型

            model.Config.TableDesc = "Test";
            model.Config.TableName = "Test"; //数据库表名称
            model.Config.EntityFileName = "TestEntity"; //实体基本名称

            model.Config.EntityName = model.Config.EntityFileName + "Entity"; //实体文件名称
            model.Config.RepositoryName = model.Config.EntityFileName + "Repository"; //仓储名称
            model.Config.IRepositoryName = "I" + model.Config.RepositoryName; //仓储基类
            model.Config.ServiceName = model.Config.EntityFileName + "Service"; //服务名称
            model.Config.IServiceName = "I" + model.Config.ServiceName; //服务接口名称
            model.Config.ControllerName = model.Config.EntityFileName + "Controller";//控制器名称
            model.Config.InDtoName = model.Config.EntityFileName + "InDto";

            T4Command command = new(@"D:\VS2022\Enterprise\Common7\IDE\TextTransform.exe");

            command.SetT4Path(@"D:\tool\GenEntity\OracleGenerate\Infrastruct.tt");
            command.SetOutFilePath(@" C:\Users\ives\Desktop\out\out.txt");
            command.Parameters.Add("ParameterName", model);

            command.Generate();
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            var d = context.PGTables.ToList();

            var path = @"D:\allTables.txt";
            Dictionary<string, string> dir = new();
            List<string> allTables = context.UserTables.Select(f => f.TableName).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            var tableComment = context.UserTabComments.Where(f => allTables.Contains(f.TableName));

            if (tableComment != null)
            {
                foreach (var item in tableComment)
                {
                    //dir.Add(tableComment.TableName, tableComment.Comments);
                    stringBuilder.AppendLine(@$"{item.TableName} {item.Comments}");
                }
            }
            File.WriteAllText(path, stringBuilder.ToString());

            txtMsg.Content = "生成成功";
        }
    }
}
