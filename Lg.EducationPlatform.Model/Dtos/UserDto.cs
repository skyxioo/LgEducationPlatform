using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.Model
{
    public class UserDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }

        public string PassWord { get; set; }

        public int RoleId { get; set; }
    }
}
