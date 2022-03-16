using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.Common;
using Utility.DB.ORM;

namespace Utility.DB
{
    public class GenerateEx
    {
        OracleEx dbUtil = new OracleEx();
        ORMEx ormUtil = new ORMEx();

        /// <summary>
        /// 生成字段信息
        /// </summary>
        /// <param name="column"></param>
        /// <param name="generateNullableField"></param>
        /// <returns></returns>
        public string GenerateField(UserTabColumnOutDto column, bool generateNullableField = false)
        {
            string fieldType = dbUtil.GetTypeByDBType(column.DataType);
            string fieldName = CamelCaseUtility.Convert2Camel(column.ColumnName);
            return $"public {fieldType}{(generateNullableField ? "?" : "")} {fieldName} {{ get; set; }}";
        }

        /// <summary>
        /// 生成注释
        /// </summary>
        /// <param name="commendText"></param>
        /// <returns></returns>
        public string GenerateComment(string commendText)
        {
            return $"/// <summary>\n/// {commendText}\n/// </summary>";
        }

        /// <summary>
        /// 生成orm字段特性
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GenerateDBAttrbute(UserTabColumnOutDto column)
        {
            return ormUtil.GenerateDBAttrbute(column);
        }

        /// <summary>
        /// 生成字段、注释等基础信息
        /// </summary>
        /// <param name="column"></param>
        /// <param name="generateDBAttrbute"></param>
        /// <param name="generateNullableField"></param>
        /// <returns></returns>
        public string GenerateTableField(UserTabColumnOutDto column, bool generateDBAttrbute = true, bool generateNullableField = false)
        {
            StringBuilder result = new StringBuilder();
            string comment = GenerateComment(column.Comment);
            result.Append(comment);
            result.Append("\n");

            if (generateDBAttrbute)
            {
                string dbAttrbute = GenerateDBAttrbute(column);
                result.Append(dbAttrbute);
                result.Append("\n");
            }

            string fieldInfo = GenerateField(column, generateNullableField);
            result.Append(fieldInfo);
            return result.ToString();
        }

        /// <summary>
        /// 生成表信息
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="generateDBAttrbute">是否生成数据库字段注解</param>
        /// <param name="generateNullableField">字段生成是否需要判断可为空</param>
        /// <param name="needSkipField">是否需要跳过字段</param>
        /// <returns></returns>
        public string GenerateTable(List<UserTabColumnOutDto> columns, bool generateDBAttrbute = true, bool generateNullableField = false, bool needSkipField = false)
        {
            StringBuilder result = new StringBuilder();
            foreach (UserTabColumnOutDto column in columns)
            {
                if (needSkipField == true && column.NeedSkip == true)
                {
                    continue;
                }
                string fieldInfo = GenerateTableField(column, generateDBAttrbute, generateNullableField);
                result.Append(fieldInfo);
                result.Append("\n");
                result.Append("\n");
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取主键字段名称
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string GetPrimaryKeyName(List<UserTabColumnOutDto> columns)
        {
            if (columns.Any(f => f.ISPrimaryKey == true))
            {
                return CamelCaseUtility.Convert2Camel(columns.FirstOrDefault(f => f.ISPrimaryKey == true).ColumnName);
            }
            return string.Empty;
        }
    }
}
