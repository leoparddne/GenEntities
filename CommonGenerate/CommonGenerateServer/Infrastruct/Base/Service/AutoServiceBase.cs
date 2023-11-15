using Infrastruct.Base.Repository;

namespace Infrastruct.Base.Service
{
    public class AutoServiceBase<T> : IAutoServiceBase<T> where T : class
    {
        public IBaseRepository<T> Repository { get; set; }

        public AutoServiceBase(IBaseRepository<T> repository)
        {
            Repository = repository;
        }
    }
}
