using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.IBLL
{
    /// <summary>
    /// 业务父接口，对业务操作进行规范
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> where T : class, new()
    {
        #region 1.0 新增实体
        /// <summary>
        /// 新增实体，此model中不能为空的属性一定要进行赋值
        /// </summary>
        /// <param name="model">model对象</param>
        /// <returns>受影响的行数</returns>
        int Add(T model, bool autoSave = true);

        int AddRange(IEnumerable<T> list, bool autoSave = true);
        #endregion

        #region 2.1 根据id删除
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>受影响的行数</returns>
        int Delete(long id, bool autoSave = true);
        #endregion

        #region 2.1 删除一个model
        /// <summary>
        /// 删除一个model,该model必须在数据库中存在
        /// </summary>
        /// <param name="model">用户需要构造一个model，此model一定在数据库中存在</param>
        /// <returns>受影响的行数</returns>
        int Delete(T model, bool autoSave = true);
        #endregion

        #region 2.2 根据条件删除
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="delCondition">删除条件</param>
        /// <returns>受影响的行数</returns>
        int DeleteBy(System.Linq.Expressions.Expression<Func<T, bool>> delCondition, bool autoSave = true);
        #endregion

        #region 3.1 修改某一个实体
        /// <summary>
        /// 根据Id修改实体
        /// </summary>
        /// <param name="model">手动new一个要修改成的实体,该model的id必须存在</param>
        /// <param name="propertyNames">要修改的属性名称数组</param>
        /// <returns>受影响的行数</returns>
        int Update(T model, bool autoSave = true, params string[] propertyNames);
        #endregion

        #region 3.2  按条件修改
        /// <summary>
        /// 批量修改满足条件的实体集合
        /// </summary>
        /// <param name="model">要修改成的实体数据</param>
        /// <param name="whereLambda">查询条件;根据它查询出要进行修改的实体集合，并将它们的值改为参数model</param>
        /// <param name="PropertyNames">要修改的属性名称数组</param>
        /// <returns>受影响的行数</returns>
        int UpdateBy(T model, Expression<Func<T, bool>> whereLambda, bool autoSave = true, params string[] PropertyNames);
        #endregion

        #region 6.0 根据主键id查询实体
        /// <summary>
        /// 根据主键id查询实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果未在上下文或数据源中找到该实体,则返回null</returns>
        T GetEntity(long id);
        #endregion

        #region 4.1 根据条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>List集合</returns>
        IQueryable<T> GetDataListBy(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 4.2 根据条件查并排序
        /// <summary>
        /// 根据条件查询并根据条件排序
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderLambda">排序表达式</param>
        /// <returns>List集合</returns>
        IQueryable<T> GetDataListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc);
        #endregion

        #region 5 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">分页查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>List集合</returns>
        IQueryable<T> GetPagedList<TKey>(int displayStart, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc);
        #endregion

        int SaveChanges();
    }
}
