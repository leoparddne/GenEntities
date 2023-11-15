using Domain.DBScheme.Oracle;
using Domain.DTO;
using System.Collections.Generic;

namespace Service.IService
{
    public interface IDataService
    {
        UserTabCommentsEntity GetByName(string dataName, string configID);
        List<UserTabCommentsEntity> GetList(string configID);
        IList<UserTabColumnOutDto> GetDataDetail(string table, string configID);
    }
}
