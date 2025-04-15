using SqlSugar;
using System;
using System.Data;

namespace Infrastruct.Base.UOF
{
    public interface IUnitOfWork
    {
        //
        // 摘要:
        //     获取Sqlsugar连接
        SqlSugarScope GetInstance();

        //
        // 摘要:
        //     事务开始
        void BeginTran(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        //
        // 摘要:
        //     事务提交
        void CommitTran();

        //
        // 摘要:
        //     事务回滚
        void RollbackTran();

        void AutoTran(Action action, Action<Exception> exceptionAction = null);
    }
}
