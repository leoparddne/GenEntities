using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository.DBScheme.Oracle
{
    public class UserConstraintsRepository : BaseRepository<UserConstraintsEntity>, IUserConstraintsRepository
    {
        public UserConstraintsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
