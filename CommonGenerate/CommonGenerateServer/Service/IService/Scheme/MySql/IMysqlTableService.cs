using Domain.DBScheme.Oracle;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService.Scheme.MySql
{
    public interface IMysqlTableService
    {
        UserTabCommentsEntity GetByName(string dataName, string configID);
        IList<UserTabColumnOutDto> GetDataDetail(string table, string configID);
        List<UserTabCommentsEntity> GetList(string configID);
    }
}
