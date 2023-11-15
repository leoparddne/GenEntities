using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Toolkit.Helper
{

    public static class MapperHelper
    {
        public static TOut AutoMap<TIn, TOut>(TIn obj, TOut outObj, bool ignorDesc = true) where TOut : new()
        {
            return AutoMap(obj, ignorDesc, outObj);
        }

        public static TOut AutoMap<TIn, TOut>(TIn obj, bool ignorDesc = true) where TOut : new()
        {
            TOut result = new TOut();
            return AutoMap(obj, ignorDesc, result);
        }

        private static TOut AutoMap<TIn, TOut>(TIn obj, bool ignorDesc, TOut result) where TOut : new()
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dictionary.Add(propertyInfo.Name, propertyInfo);
            }

            array = result.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo2 in array)
            {
                try
                {
                    DescriptionAttribute descriptionAttribute = (DescriptionAttribute)propertyInfo2.GetCustomAttributes(inherit: false).FirstOrDefault((object f) => f.GetType() == typeof(DescriptionAttribute));
                    if (descriptionAttribute != null && !ignorDesc)
                    {
                        string description = descriptionAttribute.Description;
                        if (dictionary.ContainsKey(description))
                        {
                            propertyInfo2.SetValue(result, dictionary[description].GetValue(obj));
                        }
                    }
                    else if (dictionary.ContainsKey(propertyInfo2.Name))
                    {
                        propertyInfo2.SetValue(result, dictionary[propertyInfo2.Name].GetValue(obj));
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        propertyInfo2.SetValue(result, Activator.CreateInstance(propertyInfo2.PropertyType));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("转换前后类型不一致");
                    }
                }
            }

            return result;
        }

        public static IList<TOut> AutoMap<TIn, TOut>(this List<TIn> list, bool ignorDesc = true) where TOut : new()
        {
            List<TOut> list2 = new List<TOut>();
            foreach (TIn item in list)
            {
                try
                {
                    list2.Add(AutoMap<TIn, TOut>(item, ignorDesc));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return list2;
        }

        public static object AutoMapByType(object obj, Type outType, bool ignorDesc = false)
        {
            object obj2 = Activator.CreateInstance(outType);
            PropertyInfo[] properties = obj.GetType().GetProperties();
            Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dictionary.Add(propertyInfo.Name, propertyInfo);
            }

            array = outType.GetProperties();
            foreach (PropertyInfo propertyInfo2 in array)
            {
                try
                {
                    DescriptionAttribute descriptionAttribute = (DescriptionAttribute)propertyInfo2.GetCustomAttributes(inherit: false).FirstOrDefault((object f) => f.GetType() == typeof(DescriptionAttribute));
                    if (descriptionAttribute != null && !ignorDesc)
                    {
                        string description = descriptionAttribute.Description;
                        if (dictionary.ContainsKey(description))
                        {
                            propertyInfo2.SetValue(obj2, dictionary[description].GetValue(obj));
                        }
                    }
                    else if (dictionary.ContainsKey(propertyInfo2.Name))
                    {
                        propertyInfo2.SetValue(obj2, dictionary[propertyInfo2.Name].GetValue(obj));
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        propertyInfo2.SetValue(obj2, Activator.CreateInstance(propertyInfo2.PropertyType));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("转换前后类型不一致");
                    }
                }
            }

            return obj2;
        }
    }
}
