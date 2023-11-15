using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;
using System.Collections.Generic;

namespace Domain.Repository.DBScheme.Oracle
{
    public class UserColCommentsRepository : BaseRepository<UserColCommentsEntity>, IUserColCommentsRepository
    {
        public UserColCommentsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<UserColCommentsEntity> All()
        {
            return QueryListByCondition(f => f.TableName == "T_BC_ENCODE_RULE_TYPE");
            //return null;
        }
    }
}
