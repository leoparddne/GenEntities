using Domain.DBScheme.Oracle;
using System.Collections.Generic;

namespace Service.IService.Scheme.Postgre
{
    public interface IPostgreTableService
    {
        List<UserTabCommentsEntity> GetList(string configID);
        UserTabCommentsEntity GetByName(string dataName, string configID);
    }
}


