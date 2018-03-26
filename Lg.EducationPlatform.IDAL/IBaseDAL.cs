using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lg.EducationPlatform.Model;
using System.Linq.Expressions;

namespace Lg.EducationPlatform.IDAL
{
    public interface IBaseDAL<T> where T : class, new()
    {
        //对数据操作的规定

        #region 1.0 新增实体
        int Add(T model);

        int AddRange(IEnumerable<T> list);
        #endregion

        #region 2.1 根据id删除
        int Delete(int id);
        #endregion

        #region 2.2 根据model删除
        int Delete(T model);
        #endregion

        #region 2.3 根据条件删除
        int DeleteBy(System.Linq.Expressions.Expression<Func<T, bool>> delCondition);
        #endregion

        #region 4.0 修改实体
        int Update(T model, params string[] propertyNames);
        #endregion

        #region 5.0 批量修改
        int UpdateBy(T model, Expression<Func<T, bool>> whereLambda, params string[] PropertyNames);
        #endregion

        #region 6.0 根据主键id查询实体
        T GetEntity(int id);
        #endregion

        #region 6.0 根据条件查询
        IQueryable<T> GetDataListBy(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 7.0 根据条件查询并排序
        IQueryable<T> GetDataListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc);
        #endregion

        #region 8.0 分页查询
        IQueryable<T> GetPagedList<TKey>(int displayStart, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc);
        #endregion
    }
}
