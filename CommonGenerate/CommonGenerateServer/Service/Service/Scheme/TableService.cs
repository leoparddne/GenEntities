using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Infrastruct.Ex;
using Service.IService.Scheme;
using SqlSugar;
using System.Collections.Generic;

namespace Service.Service.Scheme
{
    public class TableService : BaseService, ITableService
    {
        public TableService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IList<UserTabColumnOutDto> GetColumnInfo(string table, string configID)
        {
            ChangeDB(configID);
            var fieldList = GetFieldInfoList(table);
            if (fieldList.IsNullOrEmpty())
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

        public List<UserTabCommentsEntity> GetTableList(string configID)
        {
            ChangeDB(configID);
            var tableList = sqlSugarClient.DbMaintenance.GetTableInfoList(false);

            if (tableList.IsNullOrEmpty())
            {
                return null;
            }

            var result = new List<UserTabCommentsEntity>();
            foreach (var table in tableList)
            {
                result.Add(new UserTabCommentsEntity
                {
                    Comments = table.Description,
                    TableName = table.Name,
                    TableType = "TABLE"
                });
            }

            var viewList = sqlSugarClient.DbMaintenance.GetViewInfoList(false);
            if (!viewList.IsNullOrEmpty())
            {
                foreach (var view in viewList)
                {
                    result.Add(new UserTabCommentsEntity
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
            List<DbColumnInfo> data = sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(tableName, false);
            return data;
        }
    }
}
