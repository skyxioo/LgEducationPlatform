
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lg.EducationPlatform.IDAL
{
	public partial interface IDbSession
    {
		IStudentsDAL StudentsDAL{get;set;}
		IUsersDAL UsersDAL{get;set;}
    }

}