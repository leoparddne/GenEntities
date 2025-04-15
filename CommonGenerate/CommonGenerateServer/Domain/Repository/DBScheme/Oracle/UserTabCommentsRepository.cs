using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;

namespace Domain.Repository.DBScheme.Oracle
{
    /// <summary>
    /// 表注释
    /// </summary>
    public class UserTabCommentsRepository : BaseRepository<UserTabCommentsEntity>, IUserTabCommentsRepository
    {
        public UserTabCommentsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
