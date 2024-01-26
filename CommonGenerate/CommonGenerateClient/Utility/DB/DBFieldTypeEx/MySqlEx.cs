namespace Utility.DB.DBFieldTypeEx
{
    public class MySqlEx : DBExBase
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
            if (dbTypeName.StartsWith("varchar"))
            {
                type = "string";
            }
            if (dbTypeName.Contains("text"))
            {
                type = "string";
            }
            if (dbTypeName.StartsWith("int"))
            {
                type = "int";
            }
            if (dbTypeName.StartsWith("float"))
            {
                type = "double";
            }
            if (dbTypeName.StartsWith("doulbe"))
            {
                type = "double";
            }
            if (dbTypeName.StartsWith("decimal"))
            {
                type = "decimal";
            }
            if (dbTypeName.StartsWith("datetime"))
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
