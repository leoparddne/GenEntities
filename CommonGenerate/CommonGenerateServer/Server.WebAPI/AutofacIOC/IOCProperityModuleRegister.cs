using Autofac;
using Domain.IRepository.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Postgre;
using Domain.Repository.DBScheme.Oracle;
using Domain.Repository.DBScheme.Postgre;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.IService.Scheme;
using Service.Service;
using Service.Service.Scheme;
using System;

namespace Server.WebAPI.AutofacIOC
{
    public class IOCProperityModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Type controllerBaseType = typeof(ControllerBase);

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                        .PropertiesAutowired();

            builder.RegisterType<DataService>().As<IDataService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            builder.RegisterType<UserColCommentsRepository>().As<IUserColCommentsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConsColumnsRepository>().As<IUserConsColumnsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConstraintsRepository>().As<IUserConstraintsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabColumnsRepository>().As<IUserTabColumnsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabCommentsRepository>().As<IUserTabCommentsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTablesRepository>().As<IUserTablesRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<PostgreTableRepository>().As<IPostgreTableRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();


            builder.RegisterType<TableService>().As<ITableService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

        }
    }
}
