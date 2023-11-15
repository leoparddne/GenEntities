using Domain.DBScheme.Oracle;
using Service.IService.Scheme.Oracle;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserColCommentsService : IUserColCommentsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserColCommentsRepository userColCommentsRepository;

        public UserColCommentsService(IUnitOfWork unitOfWork, IUserColCommentsRepository UserColCommentsRepository)
        {
            this.unitOfWork = unitOfWork;

            userColCommentsRepository = UserColCommentsRepository;
        }

        public List<UserColCommentsEntity> GetList(string table, IList<string> columnsName, string configID)
        {
            userColCommentsRepository.ChangeDB(configID);

            return userColCommentsRepository.QueryListByCondition(f => f.TableName == table && columnsName.Contains(f.ColumnName));
        }
    }
}

