using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.FormatModel;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.WebHelper;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class StudentController : BaseController
    {
        private IStudentsService _studentsService = OperateHelper.Current._serviceSession.StudentsService;
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

        [HttpPost]
        public ActionResult Add(Students model)
        {            
            UserDto user = ViewBag.User as UserDto;
            model.CreationTime = DateTime.Now;
            model.CreatorUserId = user.UserId;
            
            int result = _studentsService.Add(model);
            if(result > 0)
            {
                return Json(new
                {
                    Status = 1,
                    Message = "新增成功"
                });
            }
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "新增失败"
                });
            }
        }
    }
}