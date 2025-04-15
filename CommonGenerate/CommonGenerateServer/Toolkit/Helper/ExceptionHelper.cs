using System;
using Toolkit.Exceptions;

namespace Toolkit.Helper
{
    public static class ExceptionHelper
    {
        public static void CheckException(bool check, string errMsg, int? httpCode = null)
        {
            if (check)
            {
                Exec(new Exception(errMsg), httpCode);
            }
        }

        public static void CheckException(bool check, Enum enumValue, int? httpCode = null)
        {
            CheckException(check, enumValue.GetDesc(), httpCode);
        }

        public static void CheckException(bool check, Exception exception, int? httpCode = null)
        {
            if (check)
            {
                Exec(exception, httpCode);
            }
        }

        public static void Exec(Exception exception, int? httpCode = null)
        {
            if (!httpCode.HasValue)
            {
                throw exception;
            }

            throw new HttpCodeException(httpCode.Value, exception.Message);
        }
    }
}
