using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;

namespace Domain.Repository.DBScheme.Oracle
{
    public class UserConstraintsRepository : BaseRepository<UserConstraintsEntity>, IUserConstraintsRepository
    {
        public UserConstraintsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
