using System;

namespace Utility.DB
{
    public class OracleEx
    {
        /// <summary>
        /// 根据数据库字段名称获取对应实体字段的类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        public string GetTypeByDBType(string dbTypeName)
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
                type = "double";
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

        public object GetFieldName(string dbFieldName)
        {
            throw new NotImplementedException();
        }
    }
}
