using Autofac;
using Domain.IRepository.DBScheme.Oracle;
using Domain.IRepository.DBScheme.Postgre;
using Domain.Repository.DBScheme.Oracle;
using Domain.Repository.DBScheme.Postgre;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.IService.Scheme.MySql;
using Service.IService.Scheme.Oracle;
using Service.IService.Scheme.Postgre;
using Service.Service;
using Service.Service.Scheme.MySql;
using Service.Service.Scheme.Oracle;
using Service.Service.Scheme.Postgre;
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

            builder.RegisterType<UserColCommentsService>().As<IUserColCommentsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConsColumnsService>().As<IUserConsColumnsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConstraintsService>().As<IUserConstraintsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabColumnsService>().As<IUserTabColumnsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabCommentsService>().As<IUserTabCommentsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTablesService>().As<IUserTablesService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<PostgreTableService>().As<IPostgreTableService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<PostgreTableColumnService>().As<IPostgreTableColumnService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            builder.RegisterType<UserColCommentsRepository>().As<IUserColCommentsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConsColumnsRepository>().As<IUserConsColumnsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserConstraintsRepository>().As<IUserConstraintsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabColumnsRepository>().As<IUserTabColumnsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTabCommentsRepository>().As<IUserTabCommentsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserTablesRepository>().As<IUserTablesRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<PostgreTableRepository>().As<IPostgreTableRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();


            builder.RegisterType<MySqlTableService>().As<IMysqlTableService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

        }
    }
}
