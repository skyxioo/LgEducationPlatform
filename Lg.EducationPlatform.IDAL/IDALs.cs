
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.Model;

namespace Lg.EducationPlatform.IDAL
{
    /// <summary>
    /// 自动化生成各子IDAL的定义
    /// </summary>
	public partial interface IStudentsDAL : IBaseDAL<Students>
    {
    }

	public partial interface IUsersDAL : IBaseDAL<Users>
    {
    }

	public partial interface IWebSettingsDAL : IBaseDAL<WebSettings>
    {
    }


}