using CommonGenerateClient.Resource.Dto.Out;

namespace Utility.DB.ORM
{
    public class ORMEx
    {

        /// <summary>
        /// 生成orm对应实体的特性
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GenerateDBAttrbute(UserTabColumnOutDto column)
        {
            string isPrimaryKey = column.ISPrimaryKey ? "IsPrimaryKey = true," : "";
            return $"[SugarColumn( {isPrimaryKey} ColumnDescription = \"{column.Comment}\",  ColumnName = \"{column.ColumnName}\")]";
        }
    }
}
