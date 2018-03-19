
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lg.EducationPlatform.IBLL
{
	public partial interface IServiceSession
    {
		IStudentsService StudentsService{get;set;}
		IUsersService UsersService{get;set;}
    }

}