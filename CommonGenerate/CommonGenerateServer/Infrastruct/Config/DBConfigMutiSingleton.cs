using System.Collections.Generic;
using Toolkit.Helper;

namespace Infrastruct.Config
{
    public class DBConfigMutiSingleton : DBConfigBase
    {
        private static readonly object _lock = new object();

        private static List<DBConfigBase> Config { get; set; } = null;


        private DBConfigMutiSingleton()
        {
        }

        public static List<DBConfigBase> GetConfig()
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


                Config = AppSettingsHelper.GetObject<List<DBConfigBase>>("DBList");
            }

            return Config;
        }
    }
}
