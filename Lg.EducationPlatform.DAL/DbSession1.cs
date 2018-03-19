
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.IDAL;

namespace Lg.EducationPlatform.DAL
{
    /// <summary>
    /// DbSession：本质就是一个简单工厂+一个SaveChange方法，从抽象意义来说，它就是整个数据库访问层的统一入口
    /// 因为拿到了DbSession就能拿到整个数据库访问层所有的Dal。之前是：		
	/// public IUserInfoDal UserInfoDal
    ///    {
    ///        get { return new UserInfoDal(); }
    ///    }
	/// 现在是:私有的字段，公共属性。get中先判断有没有当前对象
	/// 因为当new一个对象时，是先初始化其字段值，再执行构造函数？
	/// 最后将Dal以接口的形式返回
    /// </summary>
	public partial class DbSession : IDbSession
    {
		#region 01 数据接口 StudentsDAL
		private IStudentsDAL _StudentsDAL;
		public IStudentsDAL StudentsDAL
		{
			get
			{
				if(_StudentsDAL == null)
					_StudentsDAL = new StudentsDAL();
				return _StudentsDAL;
			}
			set
			{
				_StudentsDAL = value;
			}
		}
		#endregion

		#region 02 数据接口 UsersDAL
		private IUsersDAL _UsersDAL;
		public IUsersDAL UsersDAL
		{
			get
			{
				if(_UsersDAL == null)
					_UsersDAL = new UsersDAL();
				return _UsersDAL;
			}
			set
			{
				_UsersDAL = value;
			}
		}
		#endregion

    }

}