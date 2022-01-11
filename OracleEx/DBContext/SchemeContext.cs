using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OracleEx.Model;
using System;
using System.Diagnostics;

namespace OracleEx
{
    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            //if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
            //        && logLevel == LogLevel.Information)
            //{
            var logContent = formatter(state, exception);

            //Console.WriteLine();
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine(logContent);
            //Console.ResetColor();
            Trace.WriteLine(logContent);
            //}
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }

    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }

    public class SchemeContext : DbContext
    {
        //public SchemeContext() : base()
        //{

        //}

        public SchemeContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521)))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = TEST)));User ID=test;Password=test;", f => f.UseOracleSQLCompatibility("12"));

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //判断当前数据库是Oracle 需要手动添加Schema(DBA提供的数据库账号名称)
            //if (this.Database.IsOracle())
            //{
            //modelBuilder.HasDefaultSchema("MES");
            //}
            //    base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserTables>().HasKey(f => f.TableName);

            modelBuilder.Entity<UserTabColumn>().HasKey(f => new { f.TableName, f.ColumnName });
            modelBuilder.Entity<UserTabComments>().HasKey(f => f.TableName);

            modelBuilder.Entity<UserColComments>().HasKey(f => new { f.TableName, f.ColumnName });

            modelBuilder.Entity<UserConstraints>().HasKey(f => f.TableName);
            modelBuilder.Entity<UserConsColumns>().HasKey(f => new { f.TableName, f.ColumnName });

        }

        //public DbSet<UserTable> UserTable { get; set; }
        /// <summary>
        /// 用户创建的表
        /// </summary>
        public DbSet<UserTables> UserTables { get; set; }

        /// <summary>
        /// 表字段
        /// </summary>
        public DbSet<UserTabColumn> UserTabColumns { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public DbSet<UserTabComments> UserTabComments { get; set; }

        /// <summary>
        /// 字段注释
        /// </summary>
        public DbSet<UserColComments> UserColComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserConstraints> UserConstraints { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserConsColumns> UserConsColumns { get; set; }

    }
}
