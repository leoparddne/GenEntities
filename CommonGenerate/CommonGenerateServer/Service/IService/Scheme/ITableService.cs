using Domain.DBScheme.Oracle;
using Domain.DTO;
using System.Collections.Generic;

namespace Service.IService.Scheme
{
    public interface ITableService
    {
        UserTabCommentsEntity GetByName(string dataName, string configID);
        IList<UserTabColumnOutDto> GetDataDetail(string table, string configID);
        List<UserTabCommentsEntity> GetList(string configID);
    }
}
