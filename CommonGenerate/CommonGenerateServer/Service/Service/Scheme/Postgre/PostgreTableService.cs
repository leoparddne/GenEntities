using CodeDesigner.Application.IService.Scheme.Postgre;
using CodeDesigner.Domain.Entity.DBScheme.Oracle;
using CodeDesigner.Domain.IRepository.DBScheme.Postgre;
using MES.Server.Infrastruct.ServiceExtension;
using MES.Server.Infrastruct.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service.Scheme.Postgre
{
    public class PostgreTableService : BaseService, IPostgreTableService
    {
        private readonly IUnitOfWork unitOfWork;
        //private IAuthenticationService authenticationService;
        private readonly IPostgreTableRepository postgreTableRepository;

        public PostgreTableService(IUnitOfWork unitOfWork,/* IAuthenticationService authenticationService,*/ IPostgreTableRepository PostgreTableRepository) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            //this.authenticationService = authenticationService;
            postgreTableRepository = PostgreTableRepository;
        }

        public List<UserTabCommentsEntity> GetList(string configID)
        {
            postgreTableRepository.ChangeDB(configID);

            var result = postgreTableRepository.SqlQuery<UserTabCommentsEntity>(@"select relname as TableName,cast(obj_description(relfilenode,'pg_class') as varchar) as Comments from pg_class c
where relkind = 'r' and relname not like 'pg_%' and relname not like 'sql_%' order by relname");
            //postgreTableRepository.
            //var tables = postgreTableRepository.QueryListByCondition(f => !f.TableName.StartsWith("pg") && f.SchemaName == "public");
            //if (!tables.IsNullOrEmpty())
            //{
            //    return tables.Select(f => new UserTabCommentsEntity
            //    {
            //        Comments = "",
            //        TableName = f.TableName,
            //        TableType = ""
            //    }).ToList();
            //}
            return result;
        }

        public UserTabCommentsEntity GetByName(string dataName, string configID)
        {
            postgreTableRepository.ChangeDB(configID);

            var result = postgreTableRepository.SqlQuery<UserTabCommentsEntity>(@$"select relname as TableName,cast(obj_description(relfilenode,'pg_class') as varchar) as Comments from pg_class c
where relkind = 'r' and relname not like 'pg_%' and relname not like 'sql_%' and relname='{dataName}' order by relname")?.FirstOrDefault();

            return result;
        }
    }
}

