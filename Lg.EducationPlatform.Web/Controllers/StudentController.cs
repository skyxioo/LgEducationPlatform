using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            List<SelectListItem> eduItems = new List<SelectListItem>
            {
                new SelectListItem{ Text = "无", Value = "0", Selected = true },
                new SelectListItem{ Text = "初中", Value = "1" },
                new SelectListItem{ Text = "高中", Value = "2" },
                new SelectListItem{ Text = "中专", Value = "3" },
                new SelectListItem{ Text = "大专在读", Value = "4" },
                new SelectListItem{ Text = "大专毕业", Value = "5" }
            };
            List<SelectListItem> examItems = new List<SelectListItem>
            {
                new SelectListItem{ Text = "专科", Value = "1" },
                new SelectListItem{ Text = "本科", Value = "2", Selected = true }
            };
            ViewBag.EduLevelItemList = eduItems;
            ViewBag.ExamLevelItemList = examItems;
            return View();
        }
    }
}