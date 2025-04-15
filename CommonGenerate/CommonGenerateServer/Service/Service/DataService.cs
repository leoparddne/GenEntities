using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Config;
using Infrastruct.Ex;
using Service.IService;
using Service.IService.Scheme;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service
{
    public class DataService : IDataService
    {

        ITableService tableService;


        public DataService(ITableService mysqlTableService)
        {
            this.tableService = mysqlTableService;
        }

        public UserTabCommentsEntity GetByName(string dataName, string configID)
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (configList.IsNullOrEmpty())
            {
                return null;
            }

            var config = configList.FirstOrDefault(f => f.ConfigID == configID);
            if (config == null)
            {
                return null;
            }

            //switch (DBTypeService.GetDBType())
            var dbType = SqlSugarCollectionExtension.GetDBType(config);

            switch (dbType)
            {
                case DbType.MySql:
                    return tableService.GetTableByName(dataName, configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    break;
                case DbType.PostgreSQL:
                    break;
                case DbType.Dm:
                    break;
                case DbType.Kdbndp:
                    break;
                case DbType.Oscar:
                    break;
                default:
                    break;
            }

            return null;
        }

        public IList<UserTabColumnOutDto> GetDataDetail(string table, string configID)
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (configList.IsNullOrEmpty())
            {
                return null;
            }

            var config = configList.FirstOrDefault(f => f.ConfigID == configID);
            if (config == null)
            {
                return null;
            }

            //switch (DBTypeService.GetDBType())
            var dbType = SqlSugarCollectionExtension.GetDBType(config);

            switch (dbType)
            {
                case DbType.MySql:
                    return tableService.GetColumnInfo(table, configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    break;
                case DbType.PostgreSQL:
                    break;
                case DbType.Dm:
                    break;
                case DbType.Kdbndp:
                    break;
                case DbType.Oscar:
                    break;
            }

            return null;
        }


        public List<UserTabCommentsEntity> GetList(string configID)
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (configList.IsNullOrEmpty())
            {
                return new List<UserTabCommentsEntity>();
            }

            var config = configList.FirstOrDefault(f => f.ConfigID == configID);
            if (config == null)
            {
                return new List<UserTabCommentsEntity>();
            }



            //switch (DBTypeService.GetDBType())
            var dbType = SqlSugarCollectionExtension.GetDBType(config);
            switch (dbType)
            {
                case DbType.MySql:
                    return tableService.GetTableList(configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    break;
                case DbType.PostgreSQL:
                    break;
                case DbType.Dm:
                    break;
                case DbType.Kdbndp:
                    break;
                case DbType.Oscar:
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
