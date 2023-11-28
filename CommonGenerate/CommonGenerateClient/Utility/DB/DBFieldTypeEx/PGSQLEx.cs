using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.DB.DBFieldTypeEx
{
    public class PGSQLEx : DBExBase
    {
        /// <summary>
        /// 根据数据库字段名称获取对应实体字段的类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        public override string GetTypeByDBType(string dbTypeName)
        {
            string type = "string";
            if (dbTypeName.StartsWith("char"))
            {
                type = "string";
            }
            if (dbTypeName.Trim() == "varchar")
            {
                type = "string";
            }
            if (dbTypeName.StartsWith("int"))
            {
                type = "int";
                //type = "double";
            }
            if (dbTypeName.StartsWith("timestamp"))
            {
                type = "DateTime";
            }

            if (dbTypeName.StartsWith("date"))
            {
                type = "DateTime";
            }

            if (dbTypeName.StartsWith("time"))
            {
                type = "DateTime";
            }
            return type;
        }
    }
}
