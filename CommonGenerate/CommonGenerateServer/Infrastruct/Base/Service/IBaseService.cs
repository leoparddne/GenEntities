using Infrastruct.Base.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruct.Base.Service
{
    public interface IBaseService : IBaseRepositoryExtension, IDisposable
    {
    }
}
