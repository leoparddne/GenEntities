using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CommonGenerateClient.Win.Models
{
    public class DBConfigSingleton
    {
        private static string ConfigFileName = "DBConfig.json";
        private static readonly object _lock = new object();
        private static DBConfigSingleton Ins { get; set; }
        private DBConfigSingleton()
        {

        }

        #region 
        public List<DBConfigInfo> DBConfig { get; set; }
        #endregion

        public static DBConfigSingleton Instance
        {
            get
            {
                if (Ins != null)
                {
                    return Ins;
                }

                lock (_lock)
                {
                    {
                        if (Ins != null)
                        {
                            return Ins;
                        }

                        var appsettingPath = AppDomain.CurrentDomain.BaseDirectory + ConfigFileName;

                        if (!File.Exists(appsettingPath))
                        {
                            throw new Exception("setting err");
                        }

                        var appsettingContent = File.ReadAllText(appsettingPath);
                        if (string.IsNullOrEmpty(appsettingContent))
                        {
                            throw new Exception("setting cannot parse");
                        }

                        try
                        {
                            Ins = JsonConvert.DeserializeObject<DBConfigSingleton>(appsettingContent);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("setting cannot parse," + e.Message);
                        }
                    }
                }

                return Ins;
            }
        }
    }
}
