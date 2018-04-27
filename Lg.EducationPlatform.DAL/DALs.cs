
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.IDAL;

namespace Lg.EducationPlatform.DAL
{
    /// <summary>
    /// 各子DAL需要实现自己的I<>DAL，同时继承BaseDAL以便拥有各子DAL共有的CURD
    /// </summary>
	public partial class StudentsDAL : BaseDAL<Students>, IStudentsDAL
    {
	    public override void SetDbContext()
		{
			 dbContext = new EduDb4LgEntities();
			 dbContext.Configuration.ValidateOnSaveEnabled = false;
		}
    }

	public partial class UsersDAL : BaseDAL<Users>, IUsersDAL
    {
	    public override void SetDbContext()
		{
			 dbContext = new EduDb4LgEntities();
			 dbContext.Configuration.ValidateOnSaveEnabled = false;
		}
    }

	public partial class WebSettingsDAL : BaseDAL<WebSettings>, IWebSettingsDAL
    {
	    public override void SetDbContext()
		{
			 dbContext = new EduDb4LgEntities();
			 dbContext.Configuration.ValidateOnSaveEnabled = false;
		}
    }


}