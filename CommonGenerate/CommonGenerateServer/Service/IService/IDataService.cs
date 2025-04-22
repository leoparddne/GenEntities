using Domain.DBScheme.Oracle;
using Domain.DTO;
using System.Collections.Generic;

namespace Service.IService
{
    public interface IDataService
    {
        List<UserTabCommentsEntity> GetTableList(string configID);
        IList<UserTabColumnOutDto> GetTableFieldList(string table, string configID);
    }
}
