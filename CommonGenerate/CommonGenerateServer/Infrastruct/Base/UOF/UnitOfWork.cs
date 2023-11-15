using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruct.Base.UOF
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISqlSugarClient sqlSugarClient;

        public UnitOfWork(ISqlSugarClient SqlSugarClient)
        {
            sqlSugarClient = SqlSugarClient;
        }

        //
        // 摘要:
        //     获取Sqlsugar连接
        public SqlSugarScope GetInstance()
        {
            return sqlSugarClient as SqlSugarScope;
        }

        //
        // 摘要:
        //     事务开始
        public void BeginTran(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            GetInstance().Ado.BeginTran(isolationLevel);
        }

        //
        // 摘要:
        //     事务提交
        public void CommitTran()
        {
            GetInstance().Ado.CommitTran();
        }

        //
        // 摘要:
        //     事务回滚
        public void RollbackTran()
        {
            GetInstance().Ado.RollbackTran();
        }

        public void AutoTran(Action action, Action<Exception> exceptionAction = null)
        {
            DbResult<bool> dbResult = GetInstance().Ado.UseTran(delegate
            {
                action?.Invoke();
            }, exceptionAction);
        }
    }
}
