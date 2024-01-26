using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Config;
using Infrastruct.Ex;
using Service.IService;
using Service.IService.Scheme.MySql;
using Service.IService.Scheme.Oracle;
using Service.IService.Scheme.Postgre;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using Toolkit.Helper;

namespace Service.Service
{
    public class DataService : IDataService
    {
        IUserTabCommentsService userTabCommentsService;
        IPostgreTableService postgreTableService;
        IPostgreTableColumnService postgreTableColumnService;
        IUserTabColumnsService userTabColumnsService;
        IUserConstraintsService userConstraintsService;
        IUserColCommentsService userColCommentsService;


        IMysqlTableService mysqlTableService;


        public DataService(IUserTabCommentsService userTabCommentsService, IPostgreTableService postgreTableService, IUserTabColumnsService userTabColumnsService, IUserConstraintsService userConstraintsService, IUserColCommentsService userColCommentsService, IPostgreTableColumnService postgreTableColumnService, IMysqlTableService mysqlTableService)
        {
            this.userTabCommentsService = userTabCommentsService;
            this.postgreTableService = postgreTableService;
            this.userTabColumnsService = userTabColumnsService;
            this.userConstraintsService = userConstraintsService;
            this.userColCommentsService = userColCommentsService;
            this.postgreTableColumnService = postgreTableColumnService;
            this.mysqlTableService = mysqlTableService;
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
                    return mysqlTableService.GetByName(dataName, configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    return userTabCommentsService.GetByName(dataName, configID);
                    break;
                case DbType.PostgreSQL:
                    return postgreTableService.GetByName(dataName, configID);
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
                    return mysqlTableService.GetDataDetail(table, configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    return GetOracleTableColumn(table, configID);
                    break;
                case DbType.PostgreSQL:
                    return GetPostgreTableColumn(table, configID);
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

        private IList<UserTabColumnOutDto> GetPostgreTableColumn(string table, string configID)
        {
            var data = postgreTableColumnService.GetDataDetail(table, configID);
            if (data.IsNullOrEmpty())
            {
                return null;
            }

            var constraints = postgreTableColumnService.GetPK(table, configID);


            //主键
            foreach (var item in data)
            {
                if (constraints.Contains(item.ColumnName))
                {
                    item.ISPrimaryKey = true;
                }
            }

            return data;
        }

        private IList<UserTabColumnOutDto> GetOracleTableColumn(string table, string configID)
        {
            var entity = userTabColumnsService.GetDataDetail(table, configID);
            if (entity.IsNullOrEmpty())
            {
                return null;
            }

            var data = entity.AutoMap<UserTabColumnEntity, UserTabColumnOutDto>();

            var columns = entity.Select(f => f.ColumnName).ToList();
            var constraints = userConstraintsService.GetPK(table, configID);

            var comments = userColCommentsService.GetList(table, columns, configID).ToDictionary(f => f.ColumnName, f => f.Comments);

            //注释
            foreach (var item in data)
            {
                if (comments.ContainsKey(item.ColumnName))
                {
                    item.Comment = comments[item.ColumnName];
                }
                if (constraints.Contains(item.ColumnName))
                {
                    item.ISPrimaryKey = true;
                }
            }

            return data;
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
                    return mysqlTableService.GetList(configID);
                    break;
                case DbType.SqlServer:
                    break;
                case DbType.Sqlite:
                    break;
                case DbType.Oracle:
                    return userTabCommentsService.GetList(configID);
                    break;
                case DbType.PostgreSQL:
                    return postgreTableService.GetList(configID);
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
