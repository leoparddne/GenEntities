using Infrastruct.Base.Repository;

namespace Infrastruct.Base.Service
{
    public interface IAutoServiceBase<T> where T : class
    {
        IBaseRepository<T> Repository { get; set; }
    }
}
