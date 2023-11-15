using Domain.DBScheme.Oracle;
using System.Collections.Generic;

namespace Service.IService.Scheme.Oracle
{
    public interface IUserColCommentsService
    {
        List<UserColCommentsEntity> GetList(string table, IList<string> columnsName, string configID);
    }
}


