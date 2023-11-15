using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Exceptions
{
    public class HttpCodeException : Exception
    {
        public int HttpCode { get; set; }

        public HttpCodeException(int code, string message)
            : base(message)
        {
            HttpCode = code;
        }
    }
}
