using Infrastruct.Base.Model;
using Infrastruct.Base.UOF;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastruct.Base.Repository
{
    public class BaseRepository<T> : BaseRepositoryExtension, IBaseRepository<T>, IBaseRepositoryExtension, IDisposable where T : class, new()
    {
        public BaseRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        //
        // 摘要:
        //     根据主键查询单条数据
        //
        // 参数:
        //   pkValue:
        //     主键值
        public T QueryById(object pkValue)
        {
            return base.sqlSugarClient.Queryable<T>().InSingle(pkValue);
        }

        //
        // 摘要:
        //     根据主键值列表查询数据
        //
        // 参数:
        //   pkValueList:
        //     主键值列表
        public List<T> QueryByIdList<PkType>(List<PkType> pkValueList)
        {
            return base.sqlSugarClient.Queryable<T>().In(pkValueList).ToList();
        }

        //
        // 摘要:
        //     查询所有数据
        public List<T> QueryAllList()
        {
            return base.sqlSugarClient.Queryable<T>().ToList();
        }

        //
        // 摘要:
        //     根据条件查询数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        public List<T> QueryListByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .ToList();
        }

        //
        // 摘要:
        //     根据条件查询数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        public ISugarQueryable<T> QueryableListByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType);
        }

        //
        // 参数:
        //   queryable:
        //     为null时将从上下文创建
        //
        //   where:
        //
        //   parameters:
        public ISugarQueryable<T> QueryListByCondition(ISugarQueryable<T> queryable, string where, object parameters)
        {
            return queryable ?? base.sqlSugarClient.Queryable<T>().Where(where, parameters);
        }

        public List<T> ToList(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            if (queryable == null)
            {
                return base.sqlSugarClient.Queryable<T>().OrderByIF(orderByCondition != null, orderByCondition, orderByType).ToList();
            }

            return queryable.OrderByIF(orderByCondition != null, orderByCondition, orderByType).ToList();
        }

        public PageModel<T> ToPage<T>(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            if (queryable == null)
            {
                queryable = base.sqlSugarClient.Queryable<T>();
            }

            if (orderByCondition != null)
            {
                queryable = queryable.OrderBy(orderByCondition, orderByType);
            }

            List<T> dataList = queryable.ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<T>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     根据条件查询数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public List<TResult> QueryListByCondition<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     根据条件查询数据(去重)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public List<TResult> QueryListByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            return QueryQueryableByConditionWithDistinct(selectFields, whereCondition, orderByCondition, orderByType).ToList();
        }

        //
        // 摘要:
        //     根据条件查询数据(去重)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        //
        // 返回结果:
        //     Queryable
        public ISugarQueryable<TResult> QueryQueryableByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields);
        }

        //
        // 摘要:
        //     根据条件查询数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        public T QuerySingleByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .First();
        }

        //
        // 摘要:
        //     根据条件查询数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public TResult QuerySingleByCondition<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     查询两表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        public PageModel<TResult> QueryMuchPageWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     查询两表关联列表数据(去重)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询三表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询三表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询三表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   take:
        //     查询记录条数
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchWithTake<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, int take, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .Take(take)
                .ToList();
        }

        //
        // 摘要:
        //     查询四表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuch<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询四表关联列表数据(去重)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询五表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuch<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询五表关联列表数据(去重)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Distinct()
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询六表关联列表数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   T6:
        //     实体6
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuch<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询两表关联单条数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public TResult QueryMuchSingle<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     查询三表关联单条数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public TResult QueryMuchSingle<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     查询四表关联单条数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   TResult:
        //     返回对象
        public TResult QueryMuchSingle<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     查询五表关联单条数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   TResult:
        //     返回对象
        public TResult QueryMuchSingle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     单表分组查询
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   groupCondition:
        //     分组表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public List<TResult> QueryGroupBy<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> groupCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).GroupBy(groupCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询二表关联列表数据(GroupBy)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   groupCondition:
        //     分组表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchGroupBy<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> groupCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).GroupBy(groupCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询三表关联列表数据(GroupBy)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   groupCondition:
        //     分组表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public List<TResult> QueryMuchGroupBy<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> groupCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).GroupBy(groupCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     查询三表关联列表数据(GroupBy)
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   groupCondition:
        //     分组表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public ISugarQueryable<TResult> QueryMuchGroupByToQueryable<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> groupCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).GroupBy(groupCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields);
        }

        public PageModel<TResult> ToPage<T1, T2, T3, TResult>(ISugarQueryable<TResult> sugarQueryable, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = sugarQueryable.ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     单表分组查询
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   groupCondition:
        //     分组表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public TResult QuerySingleGroupBy<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> groupCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).GroupBy(groupCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .First();
        }

        //
        // 摘要:
        //     单表分组查询
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   groupCondition:
        //     分组表达式
        //
        //   havingCondition:
        //     聚合条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public List<TResult> QueryGroupByHaving<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, object>> groupCondition, Expression<Func<T, bool>> havingCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc)
        {
            return base.sqlSugarClient.Queryable<T>().GroupBy(groupCondition).Having(havingCondition)
                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToList();
        }

        //
        // 摘要:
        //     写入实体数据
        //
        // 参数:
        //   entity:
        //     实体类
        public int Insert(T entity)
        {
            return base.sqlSugarClient.Insertable(entity).ExecuteCommand();
        }

        //
        // 摘要:
        //     批量写入实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        public int InsertRange(List<T> entityList)
        {
            return base.sqlSugarClient.Insertable(entityList).ExecuteCommand();
        }

        //
        // 摘要:
        //     根据条件新增或更新数据 根据条件查询数据,如果存在此数据则执行更新逻辑,否则执行新增
        //
        // 参数:
        //   entity:
        //
        //   whereCondition:
        public void CreateOrUpdate(T entity, Expression<Func<T, bool>> whereCondition)
        {
            if (Exists(whereCondition))
            {
                Update(entity, whereCondition);
            }
            else
            {
                Insert(entity);
            }
        }

        //
        // 摘要:
        //     更新实体数据
        //
        // 参数:
        //   entity:
        //     实体类
        public bool Update(T entity)
        {
            return base.sqlSugarClient.Updateable(entity).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     根据条件更新实体数据
        //
        // 参数:
        //   entity:
        //     实体类
        //
        //   whereCondition:
        //     条件表达式
        public bool Update(T entity, Expression<Func<T, bool>> whereCondition)
        {
            return base.sqlSugarClient.Updateable(entity).Where(whereCondition).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     更新某几个字段
        //
        // 参数:
        //   columns:
        //     字段表达式
        //
        //   whereCondition:
        //     条件表达式
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereCondition)
        {
            return base.sqlSugarClient.Updateable<T>().SetColumns(columns).Where(whereCondition)
                .ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     批量更新实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        public bool UpdateRange(List<T> entityList)
        {
            return base.sqlSugarClient.Updateable(entityList).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     批量更新指定字段实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        //
        //   columns:
        //     不更新字段表达式
        public bool UpdateRange(List<T> entityList, Expression<Func<T, object>> columns)
        {
            return base.sqlSugarClient.Updateable(entityList).IgnoreColumns(columns).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     删除数据
        //
        // 参数:
        //   entity:
        //     实体类
        public bool Delete(T entity)
        {
            return base.sqlSugarClient.Deleteable(entity).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     删除数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        public bool Delete(Expression<Func<T, bool>> whereCondition)
        {
            return base.sqlSugarClient.Deleteable(whereCondition).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     删除指定主键数据
        //
        // 参数:
        //   pkValue:
        //     主键值
        public bool DeleteById(object pkValue)
        {
            return base.sqlSugarClient.Deleteable<T>(pkValue).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     批量删除指定主键数据
        //
        // 参数:
        //   pdValueList:
        //     主键值列表
        public bool DeleteByIdList<PkType>(List<PkType> pdValueList)
        {
            return base.sqlSugarClient.Deleteable<T>().In(pdValueList).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     批量删除数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        public bool DeleteRange(List<T> entityList)
        {
            return base.sqlSugarClient.Deleteable(entityList).ExecuteCommandHasChange();
        }

        //
        // 摘要:
        //     判断数据是否存在 True-存在 False-不存在
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return base.sqlSugarClient.Queryable<T>().Where(whereCondition).Any();
        }

        //
        // 摘要:
        //     获取数据数量
        //
        // 参数:
        //   condition:
        //     条件表达式
        public int GetCount(Expression<Func<T, bool>> condition)
        {
            return base.sqlSugarClient.Queryable<T>().Count(condition);
        }

        //
        // 摘要:
        //     获取数据某个字段的合计
        //
        // 参数:
        //   field:
        //     字段
        //
        //   condition:
        //     条件表达式
        //
        // 类型参数:
        //   TResult:
        //     字段类型
        public TResult GetSum<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Sum(field);
        }

        //
        // 摘要:
        //     获取某个字段最大值
        //
        // 参数:
        //   field:
        //     字段
        //
        //   condition:
        //     表达式
        //
        // 类型参数:
        //   TResult:
        //     字段类型
        public TResult GetMax<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Max(field);
        }

        //
        // 摘要:
        //     获取某个字段最小值
        //
        // 参数:
        //   field:
        //     字段
        //
        //   condition:
        //     表达式
        //
        // 类型参数:
        //   TResult:
        //     字段类型
        public TResult GetMin<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return base.sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Min(field);
        }

        //
        // 摘要:
        //     单表查询分页数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        public PageModel<T> QueryPageList(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<T> dataList = base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<T>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     单表查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable<T>().WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     两表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     两表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderFileds:
        //     排序字段
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, string orderFileds, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     两表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     三表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderFileds:
        //     排序字段
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, string orderFileds, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     四表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     四表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderFileds:
        //     排序字段
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, string orderFileds, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     五表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     五表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderFileds:
        //     排序字段
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, string orderFileds, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     六表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderByCondition:
        //     排序表达式
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        //   orderByType:
        //     排序顺序(asc/desc)
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   T6:
        //     实体6
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        //
        // 摘要:
        //     六表关联查询分页数据
        //
        // 参数:
        //   selectFields:
        //     返回字段
        //
        //   joinCondition:
        //     关联表达式
        //
        //   whereCondition:
        //     条件表达式
        //
        //   orderFileds:
        //     排序字段
        //
        //   pageIndex:
        //     当前页码
        //
        //   pageSize:
        //     每页数量
        //
        // 类型参数:
        //   T1:
        //     实体1
        //
        //   T2:
        //     实体2
        //
        //   T3:
        //     实体3
        //
        //   T4:
        //     实体4
        //
        //   T5:
        //     实体5
        //
        //   T6:
        //     实体6
        //
        //   TResult:
        //     返回对象
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition, string orderFileds, int pageIndex = 1, int pageSize = 20)
        {
            int totalNumber = 0;
            int totalPages = 0;
            List<TResult> dataList = base.sqlSugarClient.Queryable(joinCondition).WhereIF(whereCondition != null, whereCondition).OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                .Select(selectFields)
                .ToPageList(pageIndex, pageSize, ref totalNumber);
            if (pageSize > 0 && totalNumber > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalNumber / (decimal)pageSize);
            }

            return new PageModel<TResult>
            {
                TotalCount = totalNumber,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                DataList = dataList
            };
        }
    }
}
