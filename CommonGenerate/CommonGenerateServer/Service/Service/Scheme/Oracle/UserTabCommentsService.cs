using Domain.DBScheme.Oracle;
using Service.IService.Scheme.Oracle;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserTabCommentsService : IUserTabCommentsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserTabCommentsRepository userTabCommentsRepository;

        public UserTabCommentsService(IUnitOfWork unitOfWork, IUserTabCommentsRepository UserTabCommentsRepository)
        {
            this.unitOfWork = unitOfWork;

            userTabCommentsRepository = UserTabCommentsRepository;
        }

        public UserTabCommentsEntity GetByName(string dataName, string configID)
        {
            userTabCommentsRepository.ChangeDB(configID);

            return userTabCommentsRepository.QuerySingleByCondition(f => f.TableName == dataName);
        }

        public List<UserTabCommentsEntity> GetList(string configID)
        {
            userTabCommentsRepository.ChangeDB(configID);

            return userTabCommentsRepository.QueryAllList();
        }
    }
}

