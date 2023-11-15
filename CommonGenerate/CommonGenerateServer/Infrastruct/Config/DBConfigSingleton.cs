using Newtonsoft.Json;
using System;
using Toolkit.Helper;

namespace Infrastruct.Config
{
    public class DBConfigSingleton : DBConfigBase
    {
        private static readonly object _lock = new object();

        private static DBConfigSingleton Config { get; set; } = null;


        private DBConfigSingleton()
        {
        }

        public static DBConfigSingleton GetConfig()
        {
            if (Config != null)
            {
                return Config;
            }

            lock (_lock)
            {
                if (Config != null)
                {
                    return Config;
                }


                DBConfigSingleton dBConfigSingleton2 = new DBConfigSingleton();
                dBConfigSingleton2.ConnectionString = AppSettingsHelper.GetSetting("ConnectionString");
                dBConfigSingleton2.ConnectionType = AppSettingsHelper.GetSetting("ConnectionType") ?? "Oracle";
                dBConfigSingleton2.RedisConnectionString = AppSettingsHelper.GetSetting("RedisConnectionString");
                dBConfigSingleton2.GateWay = AppSettingsHelper.GetSetting("GateWay");
                dBConfigSingleton2.MongoDB = AppSettingsHelper.GetSetting("MongoDB");
                Config = dBConfigSingleton2;
                string setting2 = AppSettingsHelper.GetSetting("SqlAutoToLower");
                bool result = true;
                if (!string.IsNullOrWhiteSpace(setting2) && bool.TryParse(setting2, out result))
                {
                    Config.SqlAutoToLower = result;
                }
            }

            return Config;
        }
    }
}
