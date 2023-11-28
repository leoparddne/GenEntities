using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.DB.DBFieldTypeEx
{
    public abstract class DBExBase
    {
        /// <summary>
        /// 根据数据库字段名称获取对应实体字段的类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        public abstract string GetTypeByDBType(string dbTypeName);
    }
}
