using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastruct.Base.Repository
{
    public interface IBaseRepository<T> : IBaseRepositoryExtension, IDisposable where T : class
    {
        //
        // 摘要:
        //     根据主键查询单条数据
        //
        // 参数:
        //   pkValue:
        //     主键值
        T QueryById(object pkValue);

        //
        // 摘要:
        //     根据主键值列表查询数据
        //
        // 参数:
        //   pkValueList:
        //     主键值列表
        List<T> QueryByIdList<PkType>(List<PkType> pkValueList);

        //
        // 摘要:
        //     查询所有数据
        List<T> QueryAllList();

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
        List<T> QueryListByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryListByCondition<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryListByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        ISugarQueryable<TResult> QueryQueryableByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        T QuerySingleByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);

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
        TResult QuerySingleByCondition<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithTake<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, int take, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields, Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition, Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition, Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryGroupBy<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> groupCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchGroupBy<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> groupCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchGroupBy<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> groupCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        TResult QuerySingleGroupBy<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> groupCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryGroupByHaving<TResult>(Expression<Func<T, TResult>> selectFields, Expression<Func<T, object>> groupCondition, Expression<Func<T, bool>> havingCondition, Expression<Func<T, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);

        //
        // 摘要:
        //     写入实体数据
        //
        // 参数:
        //   entity:
        //     实体类
        int Insert(T entity);

        //
        // 摘要:
        //     批量写入实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        int InsertRange(List<T> entityList);

        void CreateOrUpdate(T entity, Expression<Func<T, bool>> whereCondition);

        //
        // 摘要:
        //     更新实体数据
        //
        // 参数:
        //   entity:
        //     实体类
        bool Update(T entity);

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
        bool Update(T entity, Expression<Func<T, bool>> whereCondition);

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
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereCondition);

        //
        // 摘要:
        //     批量更新实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        bool UpdateRange(List<T> entityList);

        //
        // 摘要:
        //     批量更新实体数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        //
        //   columns:
        //     不更新字段
        bool UpdateRange(List<T> entityList, Expression<Func<T, object>> columns);

        //
        // 摘要:
        //     删除数据
        //
        // 参数:
        //   entity:
        //     实体类
        bool Delete(T entity);

        //
        // 摘要:
        //     删除数据
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        bool Delete(Expression<Func<T, bool>> whereCondition);

        //
        // 摘要:
        //     删除指定主键数据
        //
        // 参数:
        //   pkValue:
        //     主键值
        bool DeleteById(object pkValue);

        //
        // 摘要:
        //     批量删除指定主键数据
        //
        // 参数:
        //   pdValueList:
        //     主键值列表
        bool DeleteByIdList<PkType>(List<PkType> pdValueList);

        //
        // 摘要:
        //     批量删除数据
        //
        // 参数:
        //   entityList:
        //     实体类列表
        bool DeleteRange(List<T> entityList);

        //
        // 摘要:
        //     判断数据是否存在 True-存在 False-不存在
        //
        // 参数:
        //   whereCondition:
        //     条件表达式
        bool Exists(Expression<Func<T, bool>> whereCondition);

        //
        // 摘要:
        //     获取数据数量
        //
        // 参数:
        //   condition:
        //     条件表达式
        int GetCount(Expression<Func<T, bool>> condition);

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
        TResult GetSum<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition);

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
        TResult GetMax<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition);

        List<T> ToList(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);
    }
}
