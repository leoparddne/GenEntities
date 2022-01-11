using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleEx.Model
{
    [Table("USER_TAB_COLUMNS")]
    public class UserTabColumn
    {
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }

        [Column("DATA_TYPE")]
        public string DataType { get; set; }

        [Column("NULLABLE")]
        public string Nullable { get; set; }

        /// <summary>
        /// 是否可以为空值
        /// </summary>
        [NotMapped]
        public bool IsNullable => Nullable.Trim() == "Y";

        [Column("DATA_DEFAULT")]
        public string DataDefault { get; set; }
    }
}
