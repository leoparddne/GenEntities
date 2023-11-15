using Autofac;
using Infrastruct.Base.Repository;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Server.WebAPI.AutofacIOC
{
    public class AutofacModuleRegister
    {
        public ContainerBuilder Create(ContainerBuilder builder)
        {
            return BaseCreate(builder);
        }


        public ContainerBuilder BaseCreate(ContainerBuilder builder)
        {
            //List<Assembly> assemblyList = GetAssemblyList(libraryName);
            //builder.RegisterAssemblyTypes(assemblyList.ToArray()).AsImplementedInterfaces().PropertiesAutowired()
            //    .InstancePerLifetimeScope();
            builder.RegisterType(typeof(BaseService)).As(typeof(IBaseService)).SingleInstance();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).SingleInstance();
            builder.RegisterType<BaseRepositoryExtension>().As<IBaseRepositoryExtension>().AsImplementedInterfaces()
                .PropertiesAutowired()
                .SingleInstance();
            builder.RegisterGeneric(typeof(AutoServiceBase<>)).As(typeof(IAutoServiceBase<>)).SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces()
                .PropertiesAutowired()
                .SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            return builder;
        }


        public static List<Assembly> GetAssemblyList(string libraryName)
        {
            List<Assembly> assemblyList = new List<Assembly>();
            DependencyContext @default = DependencyContext.Default;
            @default.CompileLibraries.Where((CompilationLibrary lib) => !lib.Serviceable && lib.Type != "package").ToList().ForEach(delegate (CompilationLibrary item)
            {
                if (item.Name.Contains(libraryName))
                {
                    Assembly item2 = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(item.Name));
                    assemblyList.Add(item2);
                }
            });
            return assemblyList;
        }
    }
}
