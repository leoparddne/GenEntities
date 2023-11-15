using Domain.DTO;
using System.Collections.Generic;

namespace Service.IService.Scheme.Postgre
{
    public interface IPostgreTableColumnService
    {
        List<UserTabColumnOutDto> GetDataDetail(string table, string configID);
        List<string> GetPK(string table, string configID);
    }
}
