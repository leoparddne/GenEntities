using Domain.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Oracle;
using Infrastruct.Base.UOF;
using Service.IService.Scheme.Oracle;
using SqlSugar;
using System.Collections.Generic;

namespace Service.Service.Scheme.Oracle
{
    public class UserConstraintsService : IUserConstraintsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserConstraintsRepository userConstraintsRepository;

        public UserConstraintsService(IUnitOfWork unitOfWork, IUserConstraintsRepository UserConstraintsRepository)
        {
            this.unitOfWork = unitOfWork;

            userConstraintsRepository = UserConstraintsRepository;
        }

        /// <summary>
        /// 获取指定表主键字段
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<string> GetPK(string table, string configID)
        {
            userConstraintsRepository.ChangeDB(configID);


            var pkColumns = userConstraintsRepository.QueryMuch<UserConstraintsEntity, UserConsColumnsEntity, string>(
                (con, columns) => columns.ColumnName,
                (con, columns) => new object[] { JoinType.Inner, con.ConstraintName == columns.ConstraintName },
                (con, columns) => con.ConstraintType == "P" && con.TableName == table && columns.TableName == table, null
                );
            return pkColumns;
        }
    }
}

