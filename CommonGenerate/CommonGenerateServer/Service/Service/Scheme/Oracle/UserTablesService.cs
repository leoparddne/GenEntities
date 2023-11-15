using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.UOF;
using Service.IService.Scheme.Oracle;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserTablesService : IUserTablesService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserTablesRepository UserTablesRepository;

        public UserTablesService(IUnitOfWork unitOfWork, IUserTablesRepository UserTablesRepository)
        {
            this.unitOfWork = unitOfWork;

            this.UserTablesRepository = UserTablesRepository;
        }

        public List<UserTablesEntity> GetList()
        {
            return UserTablesRepository.QueryAllList();
        }
    }
}

