using Domain.DBScheme.Oracle;
using Domain.DTO;
using Infrastruct.Base.Service;
using System.Collections.Generic;

namespace Service.IService.Scheme
{
    public interface ITableService: IBaseService
    {
        UserTabCommentsEntity GetByName(string dataName, string configID);
        IList<UserTabColumnOutDto> GetDataDetail(string table, string configID);
        List<UserTabCommentsEntity> GetList(string configID);
    }
}
