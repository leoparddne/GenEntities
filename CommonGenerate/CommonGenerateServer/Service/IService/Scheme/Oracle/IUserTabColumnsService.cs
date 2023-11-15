using Domain.DBScheme.Oracle;
using System.Collections.Generic;

namespace Service.IService.Scheme.Oracle
{
    public interface IUserTabColumnsService
    {
        List<UserTabColumnEntity> GetDataDetail(string dataName, string configID);
    }
}


