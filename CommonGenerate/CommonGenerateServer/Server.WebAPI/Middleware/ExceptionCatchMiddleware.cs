using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Server.WebAPI.Model;
using Server.WebAPI.Model.Constants;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Server.WebAPI.Middleware
{
    public class ExceptionCatchMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionCatchMiddleware(RequestDelegate Next)
        {
            next = Next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            Exception execErr = null;

            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await ExceptionCatchAsync(httpContext, ex);
            }
        }


        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        private static async Task ExceptionCatchAsync(HttpContext httpContext, Exception exception)
        {
            APIResponseModel<object> result;
            switch (exception)
            {
                //case TokenExpiredException:
                //    result = new APIResponseModel<object>
                //    {
                //        Code = ResponseEnum.TimeExpired.GetHashCode(),
                //        Message = "TimeExpired"
                //    };
                //    break;
                //case UnAuthorizedException:
                //    result = new APIResponseModel<object>
                //    {
                //        Code = ResponseEnum.Unauthorized.GetHashCode(),
                //        Message = "Unauthorized"
                //    };
                //    break;
                //case HttpCodeException httpCodeException:
                //    result = new APIResponseModel<object>
                //    {
                //        Code = ResponseEnum.Fail.GetHashCode(),
                //        Message = httpCodeException.Message
                //    };
                //    httpContext.Response.StatusCode = httpCodeException.HttpCode;
                //    break;
                default:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.Fail.GetHashCode(),
                        Message = exception.Message
                    };
                    break;
            }
            httpContext.Response.ContentType = "application/json";

            string strJson = JsonConvert.SerializeObject(result);

            await httpContext.Response.WriteAsync(strJson);
        }
    }
}
