using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.IO;

namespace Toolkit.Helper
{
    public class AppSettingsHelper
    {
        private static readonly object _lock;

        private static IConfiguration configuration;

        private static List<string> files { get; set; }

        static AppSettingsHelper()
        {
            _lock = new object();
            files = new List<string>();
            string text = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine("当前环境:" + text);
            AddFile("appsettings.json");
            AddFile("appsettings." + text + ".json");
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            foreach (string file in files)
            {
                configurationBuilder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }

            configuration = configurationBuilder.AddEnvironmentVariables().Build();
        }

        public static void AddFile(string fileName)
        {
            if (!File.Exists(AppContext.BaseDirectory + fileName))
            {
                Console.WriteLine("can not found " + AppContext.BaseDirectory + fileName);
                return;
            }

            files.Add(fileName);
            Console.WriteLine("add file " + fileName);
        }

        public static string GetSetting(params string[] sections)
        {
            if (sections != null && sections.Length != 0)
            {
                if (sections.Length == 1)
                {
                    return configuration?.GetSection(sections[0]).Value;
                }

                return configuration?[string.Join(":", sections)];
            }

            return "";
        }

        public static T GetObject<T>(string section) where T : class, new()
        {
            T val = new T();
            IConfigurationSection section2 = configuration.GetSection(section);
            if (section2 == null)
            {
                return null;
            }

            section2.Bind(val);
            return val;
        }
    }
}
