namespace Infrastruct.Config
{
    public class DBConfigBase
    {
        public string ConfigID { get; set; }

        public string ConnectionString { get; set; }

        public string ConnectionType { get; set; } = "Oracle";


        public string RedisConnectionString { get; set; }

        public string GateWay { get; set; }

        public string MongoDB { get; set; }

        //
        // 摘要:
        //     是否需要自动转小写
        public bool SqlAutoToLower { get; set; }
    }
}
