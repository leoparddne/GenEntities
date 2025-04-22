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

        public IList<UserTabColumnOutDto> GetTableFieldList(string table, string configID)
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

            return tableService.GetColumnInfo(table, configID);
        }


        public List<UserTabCommentsEntity> GetTableList(string configID)
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

            return tableService.GetTableList(configID);
        }
    }
}
