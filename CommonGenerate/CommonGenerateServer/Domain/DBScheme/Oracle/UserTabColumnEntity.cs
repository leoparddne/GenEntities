using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DBScheme.Oracle
{
    [SugarTable("USER_TAB_COLUMNS")]

    public class UserTabColumnEntity
    {
        [SugarColumn(ColumnDescription = "", ColumnName = "TABLE_NAME")]
        public string TableName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "COLUMN_NAME")]
        public string ColumnName { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "DATA_TYPE")]
        public string DataType { get; set; }

        [SugarColumn(ColumnDescription = "", ColumnName = "NULLABLE")]
        public string Nullable { get; set; }


        [SugarColumn(ColumnDescription = "", ColumnName = "DATA_DEFAULT")]
        public string DataDefault { get; set; }



        /// <summary>
        /// 是否可以为空值
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        private bool IsNullable => Nullable.Trim() == "Y";
    }
}
