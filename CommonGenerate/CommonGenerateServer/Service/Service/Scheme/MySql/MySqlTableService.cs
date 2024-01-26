using Domain.DBScheme.MySql;
using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Infrastruct.Ex;
using Service.IService.Scheme.MySql;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service.Scheme.MySql
{
    public class MySqlTableService : BaseService, IMysqlTableService
    {
        public MySqlTableService(IUnitOfWork unitOfWork) : base(unitOfWork)
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
    }
}
