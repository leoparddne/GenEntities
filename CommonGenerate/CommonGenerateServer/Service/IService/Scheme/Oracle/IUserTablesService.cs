using Domain.DBScheme.Oracle;
using System.Collections.Generic;

namespace Service.IService.Scheme.Oracle
{
    public interface IUserTablesService
    {
        List<UserTablesEntity> GetList();
    }
}


