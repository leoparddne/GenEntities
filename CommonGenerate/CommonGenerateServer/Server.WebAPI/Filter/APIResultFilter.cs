using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Server.WebAPI.Model;
using Server.WebAPI.Model.Constants;
using Server.WebAPI.Attr;

namespace Server.WebAPI.Filter
{
    public class APIResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null && controllerActionDescriptor.EndpointMetadata.Any((object a) => a.GetType().Equals(typeof(ApiIgnoreAttribute))))
            {
                return;
            }

            if (context.Result != null)
            {
                Type typeFromHandle = typeof(APIResponseModel<>);
                IActionResult result = context.Result;
                if (!(result is OkResult))
                {
                    ObjectResult objectResult = result as ObjectResult;
                    if (objectResult == null)
                    {
                        JsonResult jsonResult = result as JsonResult;
                        if (jsonResult == null)
                        {
                            EmptyResult emptyResult = result as EmptyResult;
                            if (emptyResult != null)
                            {
                                context.Result = new JsonResult(new APIResponseModel<object>
                                {
                                    Code = ResponseEnum.Success.GetHashCode(),
                                    Message = ""
                                });
                            }
                            else
                            {
                                ObjectResult objectResult2 = context.Result as ObjectResult;
                                Type typeFromHandle2 = typeof(APIResponseModel<>);
                                if (objectResult2.Value != null)
                                {
                                    Type type = objectResult2.Value.GetType();
                                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeFromHandle2)
                                    {
                                        return;
                                    }
                                }

                                context.Result = new JsonResult(new APIResponseModel<object>
                                {
                                    Code = ResponseEnum.Success.GetHashCode(),
                                    Message = "",
                                    Data = objectResult2.Value
                                });
                            }
                        }
                        else
                        {
                            if (jsonResult.Value != null)
                            {
                                Type type2 = jsonResult.Value.GetType();
                                if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeFromHandle)
                                {
                                    return;
                                }
                            }

                            context.Result = new JsonResult(new APIResponseModel<object>
                            {
                                Code = ResponseEnum.Success.GetHashCode(),
                                Message = "",
                                Data = jsonResult.Value
                            });
                        }
                    }
                    else
                    {
                        ObjectResult objectResult3 = context.Result as ObjectResult;
                        if (objectResult3.Value != null)
                        {
                            Type type3 = objectResult3.Value.GetType();
                            if (type3.IsGenericType && type3.GetGenericTypeDefinition() == typeFromHandle)
                            {
                                return;
                            }
                        }

                        context.Result = new JsonResult(new APIResponseModel<object>
                        {
                            Code = ResponseEnum.Success.GetHashCode(),
                            Message = "",
                            Data = objectResult3.Value
                        });
                    }
                }
                else
                {
                    context.Result = new JsonResult(new APIResponseModel<object>
                    {
                        Code = ResponseEnum.Success.GetHashCode(),
                        Message = ""
                    });
                }
            }

            base.OnActionExecuted(context);
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
