using Domain.DBScheme.Oracle;
using System.Collections.Generic;

namespace Service.IService.Scheme.Oracle
{
    public interface IUserTabCommentsService
    {
        List<UserTabCommentsEntity> GetList(string configID);
        UserTabCommentsEntity GetByName(string dataName, string configID);
    }
}


