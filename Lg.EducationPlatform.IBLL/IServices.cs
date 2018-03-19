
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lg.EducationPlatform.Model;

namespace Lg.EducationPlatform.IBLL
{
    /// <summary>
    /// 子业务接口I<>BLL 继承 IBaseBLL父接口
    /// </summary>
	public partial interface IStudentsService : IBaseService<Students>
    {
    }

	public partial interface IUsersService : IBaseService<Users>
    {
    }


}