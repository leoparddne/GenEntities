using CommonGenerateClient.Resource.Dto.Out;
using SqlSugar;
using System.Collections.Generic;

namespace DBConnecter
{
    public class DBHelper
    {
        SqlSugarClient db;
        public DBHelper(DbType dbType, string connectionString)
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true
            });
        }

        public IList<UserTabColumnOutDto> GetColumnInfo(string table)
        {
            var fieldList = GetFieldInfoList(table);
            if (fieldList == null || fieldList.Count == 0)
            {
                return null;
            }

            var result = new List<UserTabColumnOutDto>();
            foreach (var field in fieldList)
            {
                result.Add(new UserTabColumnOutDto
                {
                    ColumnName = field.DbColumnName,
                    Comment = field.ColumnDescription,
                    DataDefault = field.DefaultValue,
                    DataType = field.DataType.ToString(),
                    ISPrimaryKey = field.IsPrimarykey,
                    Nullable = field.IsNullable ? "Y" : "N"
                });
            }

            return result;
        }

        /// <summary>
        /// 根据表名获取表信息
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public UserTabCommentsOutDto GetTableByName(string dataName)
        {
            var allTableList = GetTableList();
            if (allTableList == null || allTableList.Count == 0)
            {
                return null;
            }

            foreach (var table in allTableList)
            {
                if (table.TableName == dataName)
                {
                    return table;
                }
            }

            return null;
        }

        public List<UserTabCommentsOutDto> GetTableList()
        {
            var tableList = db.DbMaintenance.GetTableInfoList(false);

            if (tableList == null || tableList.Count == 0)
            {
                return null;
            }

            var result = new List<UserTabCommentsOutDto>();
            foreach (var table in tableList)
            {
                result.Add(new UserTabCommentsOutDto
                {
                    Comments = table.Description,
                    TableName = table.Name,
                    TableType = "TABLE"
                });
            }

            var viewList = db.DbMaintenance.GetViewInfoList(false);
            if (viewList == null || viewList.Count == 0)
            {
                foreach (var view in viewList)
                {
                    result.Add(new UserTabCommentsOutDto
                    {
                        Comments = view.Description,
                        TableName = view.Name,
                        TableType = "VIEW"
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<DbColumnInfo> GetFieldInfoList(string tableName)
        {
            List<DbColumnInfo> data = db.DbMaintenance.GetColumnInfosByTableName(tableName, false);
            return data;
        }
    }
}
