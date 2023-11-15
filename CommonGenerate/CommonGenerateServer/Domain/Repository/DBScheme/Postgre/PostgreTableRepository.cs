using Domain.DBScheme.Postgre;
using Domain.IRepository.DBScheme.Postgre;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
