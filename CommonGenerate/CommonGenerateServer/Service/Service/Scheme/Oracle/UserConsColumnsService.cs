using Service.IService.Scheme.Oracle;

namespace Service.Service.Scheme.Oracle
{
    public class UserConsColumnsService : IUserConsColumnsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserConsColumnsRepository UserConsColumnsRepository;

        public UserConsColumnsService(IUnitOfWork unitOfWork, IUserConsColumnsRepository UserConsColumnsRepository)

        {
            this.unitOfWork = unitOfWork;

            this.UserConsColumnsRepository = UserConsColumnsRepository;
        }
    }
}

