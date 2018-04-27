
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.IBLL;

namespace Lg.EducationPlatform.BLL
{
    /// <summary>
    /// 各子业务层需要实现自己的I<>BLL，同时继承BaseBLL以便拥有各子BLL共有的CURD
    /// </summary>
	public partial class StudentsService : BaseService<Students>,  IStudentsService
    {
		public override void SetDAL()
		{
			CurrentDAL = CurrentDbSession.StudentsDAL;
		}
    }

	public partial class UsersService : BaseService<Users>,  IUsersService
    {
		public override void SetDAL()
		{
			CurrentDAL = CurrentDbSession.UsersDAL;
		}
    }

	public partial class WebSettingsService : BaseService<WebSettings>,  IWebSettingsService
    {
		public override void SetDAL()
		{
			CurrentDAL = CurrentDbSession.WebSettingsDAL;
		}
    }


}