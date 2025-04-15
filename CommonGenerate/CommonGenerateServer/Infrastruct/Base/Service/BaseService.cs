using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;
using System;

namespace Infrastruct.Base.Service
{
    public class BaseService : BaseRepositoryExtension, IBaseService, IBaseRepositoryExtension, IDisposable
    {
        public BaseService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
