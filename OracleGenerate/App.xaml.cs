using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OracleEx;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OracleGenerate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ServiceProvider serviceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var service=new ServiceCollection();

            ConfigurationService(service);

            serviceProvider = service.BuildServiceProvider();

            var mainView = serviceProvider.GetRequiredService<MainWindow>();
            mainView.Show();

            base.OnStartup(e);
        }

        private void ConfigurationService(ServiceCollection service)
        {
            service.AddTransient(typeof(MainWindow));
            //service.AddDbContext<SchemeContext>();

            IConfiguration configuration;
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(System.Environment.CurrentDirectory+"\\AppSetting.json", false, true);
            configuration = builder.Build();

            var value=configuration.GetSection("Oracle");
            if(value == null)
            {
                MessageBox.Show("请配置数据库连接");
                Application.Current.Shutdown();
            }


            service.AddDbContext<SchemeContext>(option => { option.UseOracle(value.Value, f => f.UseOracleSQLCompatibility("12")); });
        }
    }
}
