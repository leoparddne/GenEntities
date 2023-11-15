using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;

namespace Domain.Repository.DBScheme.Oracle
{
    public class UserTablesRepository : BaseRepository<UserTablesEntity>, IUserTablesRepository
    {
        public UserTablesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
