using Domain.DBScheme.Postgre;
using Domain.IRepository.DBScheme.Postgre;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;

namespace Domain.Repository.DBScheme.Postgre
{
    /// <summary>
    /// 表
    /// </summary>
    public class PostgreTableRepository : BaseRepository<PostgreTableEntity>, IPostgreTableRepository
    {
        public PostgreTableRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
