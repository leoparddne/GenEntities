using Domain.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using System.Collections.Generic;

namespace Domain.IRepository.DBScheme.Oracle
{
    public interface IUserColCommentsRepository : IBaseRepository<UserColCommentsEntity>
    {
        List<UserColCommentsEntity> All();
    }
}
