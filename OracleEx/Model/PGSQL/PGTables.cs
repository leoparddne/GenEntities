using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleEx.Model.PGSQL
{
    [Table("pg_tables")]
    public class PGTables
    {
        [Key]
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public string tableowner { get; set; }
        public string tablespace { get; set; }
    }
}
