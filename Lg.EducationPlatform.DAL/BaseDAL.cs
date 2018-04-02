using Lg.EducationPlatform.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using LinqKit;

namespace Lg.EducationPlatform.DAL
{
    public abstract class BaseDAL<T> : IBaseDAL<T> where T : class, new()
    {
        public BaseDAL()
        {
            SetDbContext();
        }
        public abstract void SetDbContext();

        protected DbContext dbContext;
        

        #region 1.0 新增实体
        /// <summary>
        /// 新增实体，此model中不能为空的属性一定要进行赋值
        /// </summary>
        /// <param name="model">model对象</param>
        /// <returns>受影响的行数</returns>
        public int Add(T model)
        {
            dbContext.Set<T>().Add(model);

            return dbContext.SaveChanges();
        }

        public int AddRange(IEnumerable<T> list)
        {
            dbContext.Set<T>().AddRange(list);
            return dbContext.SaveChanges();
        }
        #endregion

        #region 2.1 根据id删除
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>受影响的行数</returns>
        public int Delete(long id)
        {
            T model = dbContext.Set<T>().Find(id);
            DbEntityEntry<T> entry = dbContext.Entry<T>(model);
            entry.State = EntityState.Deleted;

            return dbContext.SaveChanges();
        }
        #endregion


        #region 2.1 根据实体model删除
        /// <summary>
        /// 删除指定model
        /// </summary>
        /// <param name="model">此model必须是当前的dbContext查出来的，因此本删除方法不实用</param>
        /// <returns>受影响的行数</returns>
        public int Delete(T model)
        {
            dbContext.Set<T>().Remove(model);

            return dbContext.SaveChanges();
        }
        #endregion

        #region 2.2 根据条件删除
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="delCondition">删除条件</param>
        /// <returns></returns>
        public int DeleteBy(System.Linq.Expressions.Expression<Func<T, bool>> whereExp)
        {
            var batch = dbContext.Set<T>().Where(whereExp);
            //batch.ForEach(p => {
            //    _dbContext.Set<T>().Remove(p);
            //});
            dbContext.Set<T>().RemoveRange(batch);
            return dbContext.SaveChanges();
        }
        #endregion


        #region 3.1 修改某一个实体（的相关属性），不用先查询再修改（避免两次链接数据库）
        /// <summary>
        /// 根据Id修改实体
        /// </summary>
        /// <param name="model">new一个要修改成的实体,主键必须存在于数据库</param>
        /// <param name="propertyNames">要修改的属性名称数组</param>
        /// <returns></returns>
        public int Update(T model, params string[] propertyNames)
        {
            DbEntityEntry<T> entry = dbContext.Entry<T>(model);

            entry.State = EntityState.Unchanged;

            foreach (string pperty in propertyNames)
            {
                entry.Property(pperty).IsModified = true;
            }

            return dbContext.SaveChanges();
        }
        #endregion

        #region 3.2 按条件修改(批量修改)
        /// <summary>
        /// 批量修改满足条件的实体集合
        /// </summary>
        /// <param name="model">要修改成的实体数据</param>
        /// <param name="whereLambda">查询条件;根据它查询出要进行修改的实体集合，并将它们的值改为参数model</param>
        /// <param name="PropertyNames">要修改的属性名称数组</param>
        /// <returns></returns>
        public int UpdateBy(T model, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, params string[] PropertyNames)
        {
            List<T> listUpdating = dbContext.Set<T>().Where(whereLambda).ToList();
            Type t = typeof(T);
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            proInfos.ForEach(p =>
            {
                if (PropertyNames.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });

            foreach (string proName in PropertyNames)
            {
                if (dictPros.ContainsKey(proName))
                {
                    PropertyInfo proInfo = dictPros[proName];
                    object newValue = proInfo.GetValue(model, null);
                    foreach (T item in listUpdating)
                    {
                        proInfo.SetValue(item, newValue, null);
                    }
                }
            }

            return dbContext.SaveChanges();
        }
        #endregion

        #region 6.0 根据主键id查询获得实体
        /// <summary>
        /// 根据主键id查询实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果未在上下文或数据源中找到该实体,则返回null</returns>
        public T GetEntity(long id)
        {
            return dbContext.Set<T>().Find(id);
        }
        #endregion

        #region 4.1 根据条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        public IQueryable<T> GetDataListBy(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return dbContext.Set<T>().AsExpandable().Where(whereLambda);
        }
        #endregion

        #region 4.2 根据条件查询-并根据条件排序
        /// <summary>
        /// 根据条件查询并根据条件排序
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderLambda">排序表达式</param>
        /// <returns>List<T></returns>
        public IQueryable<T> GetDataListBy<TKey>(System.Linq.Expressions.Expression<Func<T, bool>> whereExp, System.Linq.Expressions.Expression<Func<T, TKey>> orderExp, bool isAsc)
        {
            if (isAsc)
            {
                return dbContext.Set<T>().AsExpandable().Where(whereExp).OrderBy<T, TKey>(orderExp);
            }
            else
            {
                return dbContext.Set<T>().AsExpandable().Where(whereExp).OrderByDescending<T, TKey>(orderExp);
            }
        }
        #endregion

        #region 5.0 条件分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">分页查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <returns>List<T>集合</returns>
        public IQueryable<T> GetPagedList<TKey>(int displayStart, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            if (isAsc)
            {
                return dbContext.Set<T>()
                                .AsExpandable()
                                .Where<T>(whereLambda)
                                .OrderBy<T, TKey>(orderLambda)
                                .Skip(displayStart)
                                .Take(pageSize);
            }
            else
            {
                return dbContext.Set<T>()
                                .AsExpandable()
                               .Where<T>(whereLambda)
                               .OrderByDescending<T, TKey>(orderLambda)
                               .Skip(displayStart)
                               .Take(pageSize);
            }
        }
        #endregion
    }
}
