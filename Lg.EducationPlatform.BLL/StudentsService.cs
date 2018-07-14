using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.BLL
{
    public partial class StudentsService : BaseService<Students>, IStudentsService
    {
        public Students GetStudentByIdCard(string idcard)
        {
            var student = CurrentDAL.GetDataListBy(p => !p.IsDeleted && p.IdCard == idcard).FirstOrDefault();
            return student;
        }
    }
}
