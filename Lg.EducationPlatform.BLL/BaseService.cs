using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lg.EducationPlatform.Inject;
using Lg.EducationPlatform.IDAL;
using Lg.EducationPlatform.IBLL;

namespace Lg.EducationPlatform.BLL
{
    public abstract class BaseService<T> : IBaseService<T> where T : class, new()
    {
        public BaseService()
        {
            SetDAL();
        }
        public abstract void SetDAL();
        protected IBaseDAL<T> CurrentDAL;
        private IDbSession _currentDbSession;
        public IDbSession CurrentDbSession
        {
            get
            {
                if (_currentDbSession == null)
                {
                    //通过Spring.Net进行控制反转 （依赖注入）
                    IDbSessionFactory dbsessionFactory = SpringHelper.GetObject<IDbSessionFactory>("DbSessionFactory");
                    _currentDbSession = dbsessionFactory.GetDbSession();
                }

                return _currentDbSession;
            }
        }

        #region 1.0 新增实体
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model">新增对象</param>
        /// <returns>受影响的行数</returns>
        public int Add(T model)
        {
            return CurrentDAL.Add(model);
        }
        #endregion

        #region 2.1 根据Id删除实体
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>受影响的行数</returns>
        public int Delete(int id)
        {
            return CurrentDAL.Delete(id);
        }
        #endregion

        #region 2.2 删除model实体
        /// <summary>
        /// 删除一个model
        /// </summary>
        /// <param name="model">用户需要构造一个model，此model一定在数据库中存在</param>
        /// <returns>受影响的行数</returns>
        public int Delete(T model)
        {
            return CurrentDAL.Delete(model);
        }
        #endregion

        #region 2.2 根据条件批量删除
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="delCondition">删除条件</param>
        /// <returns></returns>
        public int DeleteBy(System.Linq.Expressions.Expression<Func<T, bool>> delCondition)
        {
            return CurrentDAL.DeleteBy(delCondition);
        }
        #endregion

        #region 3.1 修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">要修改成的实体</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        public int Update(T model, params string[] propertyNames)
        {
            return CurrentDAL.Update(model, propertyNames);
        }
        #endregion

        #region 3.2 批量修改
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="model">要修改的实体数据</param>
        /// <param name="whereLambda">查询条件;根据它查询出要进行修改的实体集合，并将它们改为参数model</param>
        /// <param name="PropertyNames">要修改的属性名称数组</param>
        /// <returns></returns>
        public int UpdateBy(T model, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, params string[] PropertyNames)
        {
            return CurrentDAL.UpdateBy(model, whereLambda, PropertyNames);
        }
        #endregion

        #region 6.0 根据主键id查询获得实体
        /// <summary>
        /// 根据主键id查询实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果未在上下文或数据源中找到该实体,则返回null</returns>
        public T GetEntity(int id)
        {
            return CurrentDAL.GetEntity(id);
        }
        #endregion

        #region 4.1 根据条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns></returns>
        public IQueryable<T> GetDataListBy(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDAL.GetDataListBy(whereLambda);
        }
        #endregion

        #region 4.2 根据条件查询并根据条件排序
        /// <summary>
        /// 根据条件查询并根据条件排序
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderLambda">排序表达式</param>
        /// <returns>List<T></returns>
        public IQueryable<T> GetDataListBy<TKey>(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            return CurrentDAL.GetDataListBy(whereLambda, orderLambda, isAsc);
        }
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
        /// <returns>List<T>集合</returns>
        public IQueryable<T> GetPagedList<TKey>(int displayStart, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            return CurrentDAL.GetPagedList(displayStart, pageSize, whereLambda, orderLambda, isAsc);
        }
        #endregion

    }
}
