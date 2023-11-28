using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.DB.DBFieldTypeEx
{
    public static class DBFieldFactory
    {
        public static DBExBase Create(string dbType)
        {
            switch (dbType)
            {
                case "MySql":
                    break;

                case "SqlServer":
                    break;


                case "Sqlite":
                    break;


                case "Oracle":
                    return new OracleEx();

                    break;

                case "PostgreSQL":
                    return new PGSQLEx();

                    break;

                case "Dm":
                    break;

                case "Kdbndp":
                    break;

                case "Oscar":
                    break;

                default:
                    throw new ArgumentException("db type not implement");
            }
            throw new ArgumentException("db type not implement");
        }
    }
}
