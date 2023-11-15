using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Helper.Model;

namespace Toolkit.Helper
{
    public static class EnumHelper
    {
        public static string GetDesc(this Enum value)
        {
            Type type = value.GetType();
            List<string> list = Enum.GetNames(type).ToList();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (list.Contains(fieldInfo.Name) && !(value.ToString() != fieldInfo.Name))
                {
                    DescriptionAttribute[] array = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                    if (array.Length != 0)
                    {
                        return array[0].Description;
                    }

                    return "";
                }
            }

            return "";
        }

        public static List<EnumInfo> GetEnumDescList(Type type)
        {
            List<EnumInfo> list = new List<EnumInfo>();
            List<string> list2 = Enum.GetNames(type).ToList();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (list2.Contains(fieldInfo.Name))
                {
                    DescriptionAttribute[] array = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                    if (array.Length != 0)
                    {
                        list.Add(new EnumInfo
                        {
                            Name = fieldInfo.Name,
                            Desc = array[0].Description,
                            Value = (int)fieldInfo.GetValue(type)
                        });
                    }
                    else
                    {
                        list.Add(new EnumInfo
                        {
                            Name = fieldInfo.Name,
                            Value = (int)fieldInfo.GetValue(type)
                        });
                    }
                }
            }

            return list;
        }
    }
}
