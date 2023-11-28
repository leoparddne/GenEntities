using System;
using System.Text;

namespace Utility.Common
{
    public class CamelCaseUtility
    {
        /// <summary>
        /// 驼峰
        /// </summary>
        /// <param name="rawStr"></param>
        /// <returns></returns>
        public static string Convert2Camel(string rawStr, string prevMask = "")
        {
            string tmp = rawStr;
            //移除前缀
            if (!string.IsNullOrWhiteSpace(prevMask))
            {
                if (tmp.StartsWith(prevMask))
                {
                    tmp = tmp.Substring(prevMask.Length);
                }
            }

            StringBuilder result = new StringBuilder();
            bool start = true;

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == '_')
                {
                    start = true;
                    continue;
                }

                //首字母转大写
                if (start)
                {
                    result.Append(char.ToUpper(tmp[i]));
                    start = false;
                    continue;
                }

                //大小转小写
                if (tmp[i] >= 65 && tmp[i] <= (65 + 25))
                {
                    result.Append((char)(tmp[i] + 32));
                }
                else
                {
                    //其他字符原样输出
                    result.Append(tmp[i]);
                }
            }



            return result.ToString();
        }
    }
}
