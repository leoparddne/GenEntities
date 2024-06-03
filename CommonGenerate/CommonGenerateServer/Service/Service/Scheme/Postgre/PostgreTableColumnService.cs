using Domain.DTO;
using Infrastruct.Base.Service;
using Infrastruct.Base.UOF;
using Service.IService.Scheme.Postgre;
using System.Collections.Generic;

namespace Service.Service.Scheme.Postgre
{
    public class PostgreTableColumnService : BaseService, IPostgreTableColumnService
    {
        public PostgreTableColumnService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<UserTabColumnOutDto> GetDataDetail(string table, string configID)
        {
            ChangeDB(configID);


            //TODO 后续拆分
            return SqlQuery<UserTabColumnOutDto>(@$"
        SELECT '{table}' as TableName,
        col_description(a.attrelid, a.attnum) as Comment,
        pg_type.typname as DataType,
        a.attname as ColumnName,
        a.attnotnull as Nullable
FROM
    pg_class as c, pg_attribute as a
inner join pg_type on pg_type.oid = a.atttypid
where
    c.relname = '{table}' and a.attrelid = c.oid and a.attnum > 0");
        }

        public List<string> GetPK(string table, string configID)
        {
            ChangeDB(configID);

            //            return SqlQuery<string>(@"select 
            //        pg_attribute.attname as colname,
            //		pg_type.typname as typename,
            //		pg_constraint.conname as pk_name 
            //from pg_constraint  
            //inner join pg_class on pg_constraint.conrelid = pg_class.oid 
            //inner join pg_attribute on pg_attribute.attrelid = pg_class.oid and  pg_attribute.attnum = pg_constraint.conkey[1]
            //inner join pg_type on pg_type.oid = pg_attribute.atttypid
            //where pg_class.relname = 'file_bucket' and pg_constraint.contype='p'");

            //TODO 后续拆分
            return SqlQuery<string>((@$"select 
        pg_attribute.attname 
from pg_constraint  
inner join pg_class on pg_constraint.conrelid = pg_class.oid 
inner join pg_attribute on pg_attribute.attrelid = pg_class.oid and  pg_attribute.attnum = pg_constraint.conkey[1]
inner join pg_type on pg_type.oid = pg_attribute.atttypid
where pg_class.relname = '{table}' and pg_constraint.contype='p'");
        }
    }
}
