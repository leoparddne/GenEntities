using SqlSugar;

namespace CommonGenerateClient.Win.Models
{
    public class DBConfigInfo
    {
        public string ConfigID { get; set; }

        public string ConnectionString { get; set; }

        public DbType ConnectionType { get; set; }
    }
}
