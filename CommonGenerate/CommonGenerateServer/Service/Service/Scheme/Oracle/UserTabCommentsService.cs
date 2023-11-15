using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.UOF;
using Service.IService.Scheme.Oracle;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserTabCommentsService : IUserTabCommentsService
    {
        private readonly IUserTabCommentsRepository userTabCommentsRepository;

        public UserTabCommentsService( IUserTabCommentsRepository UserTabCommentsRepository)
        {
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

