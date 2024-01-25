using Domain.DBScheme.MySql;
using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Repository;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Service.IService.Scheme.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Scheme.MySql
{
    public class MySqlTableService : BaseService, IMysqlTableService
    {
        public MySqlTableService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IList<UserTabColumnOutDto> GetDataDetail(string table, string configID)
        {
            ChangeDB(configID);
            var x = SqlQuery<MySqlTableEntity>($"DESCRIBE {table};");

            throw new NotImplementedException();
        }

        public List<UserTabCommentsEntity> GetList(string configID)
        {
            ChangeDB(configID);
            var x = SqlQuery<object>("show tables");
            throw new NotImplementedException();
        }
    }
}
