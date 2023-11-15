using System.Collections.Generic;

namespace Service.IService.Scheme.Oracle
{
    public interface IUserConstraintsService
    {
        /// <summary>
        /// 获取指定表主键字段
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        List<string> GetPK(string table, string configID);
    }
}


