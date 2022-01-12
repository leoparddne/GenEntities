using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleEx.Ex
{
    public static class TypeEx
    {
        public static string CalcType(string typeStr)
        {
            string type = "string";
            if (typeStr.StartsWith("VARCHAR"))
            {
                type = "string";
            }
            if (typeStr.Trim() == "INT")
            {
                type = "int";
            }
            if (typeStr.StartsWith("NUMBER"))
            {
                type = "double";
            }
            if (typeStr.StartsWith("CHAR"))
            {
                type = "string";
            }
            if (typeStr.StartsWith("DATE"))
            {
                type = "DateTime";
            }

            return type;
        }
    }
}
