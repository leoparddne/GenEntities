using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Infrastruct.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastruct.Ex
{
    public static class SqlSugarCollectionExtension
    {
        private static SqlSugarScope sqlSugarClient;

        private static ConnectionConfig InjectConnectionStr(DBConfigBase config)
        {
            DbType databaseType = GetDBType(config);
            List<SqlFuncExternal> list = new List<SqlFuncExternal>();
            list.Add(new SqlFuncExternal
            {
                UniqueMethodName = config.ConnectionType + "NVLNull",
                MethodValue = delegate (MethodCallExpressionModel expInfo, DbType dbType, ExpressionContext expContext)
                {
                    if (dbType == databaseType)
                    {
                        return $"nvl({expInfo.Args[0].MemberName},' ')";
                    }

                    throw new Exception("Unrealized");
                }
            });
            list.Add(new SqlFuncExternal
            {
                UniqueMethodName = config.ConnectionType + "Decode",
                MethodValue = delegate (MethodCallExpressionModel expInfo, DbType dbType, ExpressionContext expContext)
                {
                    if (dbType == databaseType)
                    {
                        return $"decode({expInfo.Args[0].MemberName},null,{expInfo.Args[1].MemberName},{expInfo.Args[0].MemberName})";
                    }

                    throw new Exception("Unrealized");
                }
            });
            return InitSugarConnection(config, databaseType, list);
        }

        public static ConnectionConfig InitSugarConnection(DBConfigBase config, DbType databaseType, List<SqlFuncExternal> sqlFuncList)
        {
            return new ConnectionConfig
            {
                ConfigId = config.ConfigID,
                ConnectionString = config.ConnectionString,
                DbType = databaseType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    SqlFuncServices = sqlFuncList
                },
                MoreSettings = new ConnMoreSettings()
            };
        }

        public static void AddSqlSugar(this IServiceCollection services, Action<string> logExecuting = null, Action<string> logExecuted = null)
        {
            DBConfigSingleton config = DBConfigSingleton.GetConfig();
            ConnectionConfig config2 = InjectConnectionStr(config);
            sqlSugarClient = new SqlSugarScope(config2);
            InjectSugar(services, logExecuting, logExecuted);
        }

        //
        // 摘要:
        //     多数据库支持
        //
        // 参数:
        //   services:
        //
        //   logExecuting:
        //
        //   logExecuted:
        public static void AddSqlSugarMuti(this IServiceCollection services, Action<string> logExecuting = null, Action<string> logExecuted = null)
        {
            List<DBConfigBase> config = DBConfigMutiSingleton.GetConfig();
            if (!config.IsNullOrEmpty())
            {
                List<ConnectionConfig> list = new List<ConnectionConfig>();
                foreach (DBConfigBase item in config)
                {
                    ConnectionConfig connectionConfig = InjectConnectionStr(item);
                    if (connectionConfig != null)
                    {
                        list.Add(connectionConfig);
                    }
                }

                sqlSugarClient = new SqlSugarScope(list);
            }

            InjectSugar(services, logExecuting, logExecuted);
        }

        private static void InjectSugar(IServiceCollection services, Action<string> logExecuting, Action<string> logExecuted)
        {
            services.AddSingleton((Func<IServiceProvider, ISqlSugarClient>)delegate
            {
                sqlSugarClient.Aop.OnLogExecuting = delegate (string sql, SugarParameter[] pars)
                {
                    logExecuting?.Invoke(sql);
                };
                sqlSugarClient.Aop.OnLogExecuted = delegate (string sql, SugarParameter[] pars)
                {
                    logExecuted?.Invoke(sql);
                    if (sqlSugarClient.Ado.SqlExecutionTime.TotalSeconds > 1.0 && sqlSugarClient.Ado.SqlStackTrace != null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (StackTraceInfoItem myStackTrace in sqlSugarClient.Ado.SqlStackTrace.MyStackTraceList)
                        {
                            stringBuilder.Append($"{myStackTrace.FileName}-{myStackTrace.Line}-{myStackTrace.MethodName}");
                        }

                        //LogHelper.WriteLog("SlowLog.txt", new string[2]
                        //{
                        //    "执行时长{sqlSugarClient.Ado.SqlExecutionTime.TotalSeconds}:{sql}",
                        //    stringBuilder.ToString()
                        //});
                    }
                };
                return sqlSugarClient;
            });
        }

        //
        // 摘要:
        //     参数为null时通过单例配置获取，多数据库时需要手动传入配置
        //
        // 参数:
        //   config:
        public static DbType GetDBType(DBConfigBase config = null)
        {
            if (config == null)
            {
                config = DBConfigSingleton.GetConfig();
            }

            return (DbType)Enum.Parse(typeof(DbType), config.ConnectionType);
        }
    }
}
