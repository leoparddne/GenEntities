using Domain.DBScheme.Oracle;
using Service.IService.Scheme.Oracle;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserTabColumnsService : IUserTabColumnsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserTabColumnsRepository userTabColumnsRepository;

        public UserTabColumnsService(IUnitOfWork unitOfWork, IUserTabColumnsRepository UserTabColumnsRepository)
        {
            this.unitOfWork = unitOfWork;

            userTabColumnsRepository = UserTabColumnsRepository;
        }

        public List<UserTabColumnEntity> GetDataDetail(string dataName, string configID)
        {
            userTabColumnsRepository.ChangeDB(configID);

            var data = userTabColumnsRepository.QueryListByCondition(f => f.TableName == dataName);

            return data;
        }
    }
}

