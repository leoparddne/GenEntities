using Infrastruct.Base.Repository;
using Infrastruct.Base.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
