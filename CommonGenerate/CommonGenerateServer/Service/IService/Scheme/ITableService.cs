using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Service;
using System.Collections.Generic;

namespace Service.IService.Scheme
{
    public interface ITableService: IBaseService
    {
        UserTabCommentsEntity GetByName(string dataName, string configID);
        IList<UserTabColumnOutDto> GetColumnInfo(string table, string configID);
        List<UserTabCommentsEntity> GetTableList(string configID);
    }
}
