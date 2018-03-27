using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class TeacherController : BaseController
    {
        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        public ActionResult Index()
        {
            return View();
        }
    }
}