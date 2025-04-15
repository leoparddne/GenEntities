using Infrastruct.Base.Repository;
using System;

namespace Infrastruct.Base.Service
{
    public interface IBaseService : IBaseRepositoryExtension, IDisposable
    {
    }
}
