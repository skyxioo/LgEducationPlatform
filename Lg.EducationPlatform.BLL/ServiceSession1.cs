
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.IBLL;

namespace Lg.EducationPlatform.BLL
{
	public partial class ServiceSession : IServiceSession
    {
		#region 01 业务接口 IStudentsService (实际为类 依赖接口)
		IStudentsService _StudentsService;
		public IStudentsService StudentsService
		{
			get
			{
				if(_StudentsService == null)
					_StudentsService = new StudentsService();
				return _StudentsService;
			}
			set
			{
				_StudentsService = value;
			}
		}
		#endregion

		#region 02 业务接口 IUsersService (实际为类 依赖接口)
		IUsersService _UsersService;
		public IUsersService UsersService
		{
			get
			{
				if(_UsersService == null)
					_UsersService = new UsersService();
				return _UsersService;
			}
			set
			{
				_UsersService = value;
			}
		}
		#endregion

    }

}