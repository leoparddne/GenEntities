using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Server.WebAPI.Ex
{
    public static class SwaggerEx
    {

        public static void AddCustomSwaggerHeader(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());
        }
    }
}
