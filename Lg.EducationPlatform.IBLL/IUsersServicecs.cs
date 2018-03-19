using Lg.EducationPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.IBLL
{
    public partial interface IUsersService : IBaseService<Users>
    {
        UserDto GetUser(string userName);
    }
}
