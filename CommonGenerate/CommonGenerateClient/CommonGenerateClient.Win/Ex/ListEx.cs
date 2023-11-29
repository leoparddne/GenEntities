using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Ex
{
    public static class ListEx
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }

            return false;
        }

        public static List<string> DistinctString(this List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }

            list.RemoveAll((string x) => string.IsNullOrWhiteSpace(x));
            return list.Distinct().ToList();
        }
    }
}
