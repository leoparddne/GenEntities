using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Config;
using Infrastruct.Ex;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Collections.Generic;
using System.Linq;

namespace Server.WebAPI.Controllers
{/// <summary>
 /// 数据信息
 /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[ServiceFilter(typeof(AuthorizeFilter))]
    public class DataController : BaseController
    {
        IDataService dataService;

        public DataController(
            IDataService dataService)
        {
            this.dataService = dataService;
        }



        /// <summary>
        /// 获取数据源下的数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //TODO 后续需要扩展根据数据源获取数据源下的所有数据
        public List<UserTabCommentsEntity> GetList(string configID)
        {
            return dataService.GetList(configID);
        }

        /// <summary>
        /// 获取数据源信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //TODO 后续需要扩展根据数据源获取数据源下的所有数据
        public UserTabCommentsEntity GetDataInfo(string dataName, string configID)
        {
            return dataService.GetByName(dataName, configID);
        }


        /// <summary>
        /// 获取详情 后续扩展为根据id获取其他唯一字段获取
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns></returns>
        [HttpGet]
        public IList<UserTabColumnOutDto> GetDataDetail(string table, string configID)
        {
            return dataService.GetDataDetail(table, configID);
        }

        //[AllowAnonymous]
        [HttpGet]
        public string GetDBType(string configID)
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (configList.IsNullOrEmpty())
            {
                return "Oracle";
            }

            var config = configList.FirstOrDefault(f => f.ConfigID == configID);
            if (config == null)
            {
                return "Oracle";
            }

            return config.ConnectionType;

            ////switch (DBTypeService.GetDBType())
            //var dbType = SqlSugarCollectionExtension.GetDBType(config);


            //var setting = AppSettingsHelper.GetSetting("ConnectionType");
            //if (string.IsNullOrEmpty(setting))
            //{
            //    return "Oracle";
            //}
            //return setting;
        }

        /// <summary>
        /// 获取数据库列表下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SelectOutDto> GetDBSourceSelect()
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (configList.IsNullOrEmpty())
            {
                return null;
            }

            return configList.Select(f => new SelectOutDto
            {
                Label = f.ConnectionString,
                Value = f.ConfigID
            });
        }
    }
}