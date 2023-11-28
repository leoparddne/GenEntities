using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.DB.DBFieldTypeEx
{
    public class OracleEx : DBExBase
    {
        /// <summary>
        /// 根据数据库字段名称获取对应实体字段的类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        public override string GetTypeByDBType(string dbTypeName)
        {
            string type = "string";
            if (dbTypeName.StartsWith("VARCHAR"))
            {
                type = "string";
            }
            if (dbTypeName.Trim() == "INT")
            {
                type = "int";
            }
            if (dbTypeName.StartsWith("NUMBER"))
            {
                type = "int";
                //type = "double";
            }
            if (dbTypeName.StartsWith("CHAR"))
            {
                type = "string";
            }
            if (dbTypeName.StartsWith("DATE"))
            {
                type = "DateTime";
            }

            return type;
        }
    }
}
