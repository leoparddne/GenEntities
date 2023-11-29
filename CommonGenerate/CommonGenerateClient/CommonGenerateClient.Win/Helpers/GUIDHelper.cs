using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Helpers
{
    public static class GUIDHelper
    {
        public static string NewGuid => Guid.NewGuid().ToString("N");
    }
}
