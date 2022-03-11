using OracleEx.Model;
using System.Collections.Generic;
using System.Linq;

namespace OracleEx.Service
{
    public class CommonService
    {
        public IQueryable<UserTabComments> GetAllTable(SchemeContext context)
        {
            List<string> allTable = context.UserTables.Select(f => f.TableName).ToList();

            return context.UserTabComments.Where(f => allTable.Contains(f.TableName));
        }
    }
}
