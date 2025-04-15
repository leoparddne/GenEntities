using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.WebAPI.Attr;
using Server.WebAPI.Model;
using System;
using System.Linq;

namespace Server.WebAPI.Ex
{
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 0;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    if (action.Filters.Any(delegate (IFilterMetadata e)
                    {
                        ProducesResponseTypeAttribute producesResponseTypeAttribute = e as ProducesResponseTypeAttribute;
                        return producesResponseTypeAttribute != null && producesResponseTypeAttribute.StatusCode == 200;
                    }) || action.Attributes.Any((object f) => f is ApiIgnoreAttribute) || !(action.ActionMethod.ReturnType != null))
                    {
                        continue;
                    }

                    Type type = typeof(APIResponseModel<>);
                    if (action.ActionMethod.ReturnType.IsGenericType && action.ActionMethod.ReturnType.GetGenericTypeDefinition() == type)
                    {
                        continue;
                    }

                    Type type2 = null;
                    if (action.Attributes.Any((object f) => f is SwaggerRetDataTypeAttribute))
                    {
                        type2 = ((SwaggerRetDataTypeAttribute)action.Attributes.First((object f) => f is SwaggerRetDataTypeAttribute)).RetType;
                    }

                    try
                    {
                        if (action.ActionMethod.ReturnType != typeof(void))
                        {
                            type = type.MakeGenericType((type2 == null) ? action.ActionMethod.ReturnType : type2);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    action.Filters.Add(new ProducesResponseTypeAttribute(type, 200));
                }
            }
        }

    }
}
