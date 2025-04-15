using Domain.DBScheme.MySql;
using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Infrastruct.Ex;
using Service.IService.Scheme;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service.Scheme
{
    public class TableService : BaseService, ITableService
    {
        public TableService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserTabCommentsEntity GetByName(string dataName, string configID)
        {
            ChangeDB(configID);
            var fieldList = SqlQuery<MySqlTableEntity>($"show table status where Name= '{dataName}';");
            if (fieldList.IsNullOrEmpty())
            {
                return null;
            }
            var data = fieldList.First();

            return new UserTabCommentsEntity
            {
                Comments = data.Comment,
                TableName = data.Name,
                TableType = string.Empty
            };
        }

        public IList<UserTabColumnOutDto> GetDataDetail(string table, string configID)
        {
            ChangeDB(configID);
            var fieldList = SqlQuery<MySqlTableFieldEntity>($"SHOW FULL COLUMNS FROM {table};");
            if (fieldList.IsNullOrEmpty())
            {
                return null;
            }

            var result = new List<UserTabColumnOutDto>();
            foreach (var field in fieldList)
            {
                result.Add(new UserTabColumnOutDto
                {
                    ColumnName = field.Field,
                    Comment = field.Comment,
                    DataDefault = field.Default,
                    DataType = field.Type,
                    ISPrimaryKey = field.Key == "PRI",
                    Nullable = field.Null == "NO" ? "N" : "Y",
                });
            }

            return result;
        }

        public List<UserTabCommentsEntity> GetList(string configID)
        {
            ChangeDB(configID);
            var tableList = SqlQuery<MySqlTableEntity>("show table status;");

            if (tableList.IsNullOrEmpty())
            {
                return null;
            }

            var result = new List<UserTabCommentsEntity>();
            foreach (var table in tableList)
            {
                result.Add(new UserTabCommentsEntity
                {
                    Comments = table.Comment,
                    TableName = table.Name,
                    TableType = string.Empty
                });
            }

            return result;
        }


        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <returns></returns>
        private List<DbTableInfo> GetTableList(string modelID)
        {
            List<DbTableInfo> data = sqlSugarClient.DbMaintenance.GetTableInfoList(false);
            if (string.IsNullOrEmpty(modelID))
            {
                return data;
            }

            var result = data.Where(f => f.Name.Contains(modelID)).ToList();
            return result;
        }

        /// <summary>
        /// 获取视图信息
        /// </summary>
        /// <returns></returns>
        private List<DbTableInfo> GetViewInfoList(string modelID)
        {
            var data = sqlSugarClient.DbMaintenance.GetViewInfoList(false);
            if (string.IsNullOrEmpty(modelID))
            {
                return data;
            }

            var result = data.Where(f => f.Name.Contains(modelID)).ToList();
            return result;
        }

        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<DbColumnInfo> GetFieldInfoList(string tableName)
        {
            List<DbColumnInfo> data = sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(tableName, false);
            return data;
        }
    }
}
