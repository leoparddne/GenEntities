using Infrastruct.Base.UOF;
using Infrastruct.Config;
using Infrastruct.Ex;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Helper;

namespace Infrastruct.Base.Repository
{
    public class BaseRepositoryExtension : IBaseRepositoryExtension, IDisposable
    {
        private IUnitOfWork unitOfWork;

        public ISqlSugarClient sqlSugarClient { get; set; }

        public BaseRepositoryExtension(IUnitOfWork unitOfWork)
        {
            sqlSugarClient = unitOfWork.GetInstance();
            this.unitOfWork = unitOfWork;
        }

        public ISqlSugarClient GetDB(string dbID)
        {
            SqlSugarScopeProvider connectionScope = unitOfWork.GetInstance().GetConnectionScope(dbID);
            if (connectionScope != null)
            {
                ISqlSugarClient sqlSugarClient = connectionScope;
                if (sqlSugarClient != null)
                {
                    return sqlSugarClient;
                }
            }

            return null;
        }

        public void ChangeDB(string dbID)
        {
            ISqlSugarClient dB = GetDB(dbID);
            ExceptionHelper.CheckException(dB == null, "dbid:" + dbID + " can not fetch");
            sqlSugarClient = dB;
        }

        //
        // 摘要:
        //     切换默认数据库 - 不推荐使用
        //
        // 参数:
        //   dbID:
        [Obsolete]
        public void ChangeDefaultDB(string dbID)
        {
            unitOfWork.GetInstance().ChangeDatabase(dbID);
        }

        //
        // 摘要:
        //     转换成小写
        //
        // 参数:
        //   sugarParameterList:
        private List<SugarParameter> Copy2LowerList(List<SugarParameter> sugarParameterList)
        {
            if (sugarParameterList.IsNullOrEmpty() || !DBConfigSingleton.GetConfig().SqlAutoToLower)
            {
                return sugarParameterList;
            }

            List<SugarParameter> list = new List<SugarParameter>();
            foreach (SugarParameter sugarParameter2 in sugarParameterList)
            {
                SugarParameter sugarParameter = new SugarParameter(sugarParameter2.ParameterName.ToLower(), sugarParameter2.Value, sugarParameter2.Direction == ParameterDirection.Output);
                sugarParameter.DbType = sugarParameter2.DbType;
                list.Add(sugarParameter);
            }

            return list;
        }

        //
        // 摘要:
        //     还原参数
        //
        // 参数:
        //   origin:
        //
        //   returnParameter:
        private List<SugarParameter> ReverseParameter(List<SugarParameter> origin, List<SugarParameter> returnParameter)
        {
            if (origin.IsNullOrEmpty() || !DBConfigSingleton.GetConfig().SqlAutoToLower)
            {
                return origin;
            }

            foreach (SugarParameter item in origin)
            {
                SugarParameter sugarParameter = returnParameter.FirstOrDefault((SugarParameter f) => f.ParameterName.ToUpper() == item.ParameterName.ToUpper());
                if (sugarParameter != null)
                {
                    item.Value = sugarParameter.Value;
                }
            }

            return origin;
        }

        public DataTable ExecuteProcedure(string procedureName, params SugarParameter[] parameters)
        {
            List<SugarParameter> list = parameters.ToList();
            List<SugarParameter> list2 = Copy2LowerList(list);
            DataTable dataTable = sqlSugarClient.Ado.UseStoredProcedure().GetDataTable(procedureName, list2);
            List<SugarParameter> source = ReverseParameter(list, list2);
            SugarParameter sugarParameter = source.FirstOrDefault((SugarParameter f) => f.ParameterName.ToUpper() == "O_RES");
            if (sugarParameter != null && !(sugarParameter.Value.ToString() == "OK"))
            {
                throw new Exception($"{procedureName}:{sugarParameter.Value}");
            }

            return dataTable;
        }

        public bool ExecuteProcedure(string procedureName, List<SugarParameter> sugarParameterList, out dynamic model, out string returnMessage)
        {
            List<SugarParameter> list = Copy2LowerList(sugarParameterList);
            model = null;
            bool flag = false;
            returnMessage = string.Empty;
            DataTable dataTable = sqlSugarClient.Ado.UseStoredProcedure().GetDataTable(procedureName, list);
            List<SugarParameter> list2 = ReverseParameter(sugarParameterList, list);
            if (list2 != null && list2.Count > 0)
            {
                List<SugarParameter> list3 = list2.Where((SugarParameter t) => t.Direction == ParameterDirection.Output).ToList();
                if (list3 != null && list3.Count > 0)
                {
                    model = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)model;
                    {
                        foreach (SugarParameter item in list3)
                        {
                            object obj = ((item.Value.GetType() == typeof(DBNull)) ? null : item.Value);
                            string text = AssemblyAttributeName(item.ParameterName);
                            if (text.Equals("Res"))
                            {
                                flag = item.Value.ToString() == "OK";
                                if (!flag)
                                {
                                    returnMessage = $"{procedureName}:{obj}";
                                    throw new Exception(returnMessage);
                                }
                            }
                            else
                            {
                                dictionary[text] = obj;
                            }
                        }

                        return flag;
                    }
                }
            }

            return flag;
        }

        private string AssemblyAttributeName(string columnName)
        {
            string text = string.Empty;
            if (columnName.Contains("_"))
            {
                string[] array = columnName.Split("_");
                for (int i = 1; i < array.Length; i++)
                {
                    string text2 = array[i].ToLower();
                    text = ((!text2.Equals("upn")) ? (text + text2.Substring(0, 1).ToUpper() + text2.Substring(1)) : "UPN");
                }

                return text;
            }

            return columnName;
        }

        //
        // 摘要:
        //     执行sql语句
        //
        // 参数:
        //   sql:
        //
        //   sugarParameterList:
        public List<T> SqlQuery<T>(string sql, List<SugarParameter> sugarParameterList = null)
        {
            return sqlSugarClient.Ado.SqlQuery<T>(sql, sugarParameterList);
        }

        //
        // 摘要:
        //     执行sql语句,返回单一数据
        //
        // 参数:
        //   sql:
        //
        //   sugarParameterList:
        public string SqlQuerySingle(string sql, List<SugarParameter> sugarParameterList = null)
        {
            return sqlSugarClient.Ado.SqlQuerySingle<string>(sql, sugarParameterList);
        }

        public void Dispose()
        {
            sqlSugarClient.Dispose();
        }

        //
        // 摘要:
        //     创建数据库
        //
        // 参数:
        //   databaseDirectory:
        public bool CreateDatabase(string databaseDirectory = null)
        {
            return sqlSugarClient.DbMaintenance.CreateDatabase();
        }
    }
}
