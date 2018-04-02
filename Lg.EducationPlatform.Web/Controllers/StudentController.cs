using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.WebHelper;
using Lg.EducationPlatform.jqDataTableModel;
using Lg.EducationPlatform.ViewModel;
using System.Linq.Expressions;
using System.Text;
using System.IO;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class StudentController : BaseController
    {
        private IUsersService _userService = OperateHelper.Current._serviceSession.UsersService;
        private IStudentsService _studentsService = OperateHelper.Current._serviceSession.StudentsService;
        // GET: Student
        public ActionResult Index()
        {
            UserDto user = ViewBag.User as UserDto;
            if (user.RoleId == 0)
            {
                var list = _userService.GetTeacherItems();
                list.Insert(0, new ItemModel { Text = "全部", Value = "" });
                SelectList itemList = new SelectList(list, "Value", "Text");
                ViewBag.UserList = itemList.AsEnumerable<SelectListItem>();
            }
            else
            {
                ViewBag.UserList = new List<SelectListItem>
                {
                    new SelectListItem { Text = user.UserName, Value = user.UserId.ToString() }
                };
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            var s = _studentsService.GetEntity(id);
            var model = new StudentViewModel {
                Id = s.Id,
                SurName = s.SurName,
                Sex = s.Sex,
                Birthday = s.Birthday,
                Period = s.Period,
                ExaminationLevel = s.ExaminationLevel,
                MajorName = s.MajorName,
                Nationality = s.Nationality,
                PoliticalStatus = s.PoliticalStatus,
                EducationalLevel = s.EducationalLevel,
                Address = s.Address,
                Phone = s.Phone,
                Remark = s.Remark,
                Status = s.Status,
                TestFreeCondition = s.TestFreeCondition,
                IdCardFrontPath = s.IdCardFrontPath,
                IdCardBackPath = s.IdCardBackPath,
                BareheadedPhotoPath = s.BareheadedPhotoPath
            };
            return View(model);
        }

        public ActionResult Add(int? id)
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

            ViewBag.Title = "长沙理工大学综合管理系统|学生管理|添加";
            StudentViewModel model = new StudentViewModel();
            if(id != null && id.Value > 0)
            {
                ViewBag.Title = "长沙理工大学综合管理系统|学生管理|编辑";

                var student = _studentsService.GetEntity(id.Value);
                model.Address = student.Address;
                model.BareheadedPhotoPath = student.BareheadedPhotoPath;
                model.Birthday = student.Birthday;
                model.EducationalLevel = student.EducationalLevel;
                model.ExaminationLevel = student.ExaminationLevel;
                model.Id = student.Id;
                model.IdCardBackPath = student.IdCardBackPath;
                model.IdCardFrontPath = student.IdCardFrontPath;
                model.MajorName = student.MajorName;
                model.Nationality = student.Nationality;
                model.Period = student.Period;
                model.Phone = student.Phone;
                model.PoliticalStatus = student.PoliticalStatus;
                model.Remark = student.Remark;
                model.Sex = student.Sex;
                model.SurName = student.SurName;
                model.TestFreeCondition = student.TestFreeCondition;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(StudentViewModel model)
        {
            UserDto user = ViewBag.User as UserDto;
            Students stu = new Students {
                Address = model.Address,
                BareheadedPhotoPath = model.BareheadedPhotoPath,
                Birthday = model.Birthday,
                EducationalLevel = model.EducationalLevel,
                ExaminationLevel = model.ExaminationLevel,
                IdCardBackPath = model.IdCardBackPath,
                IdCardFrontPath = model.IdCardFrontPath,
                MajorName = model.MajorName,
                Nationality = model.Nationality,
                Period = model.Period,
                Phone = model.Phone,
                PoliticalStatus = model.PoliticalStatus,
                Remark = model.Remark,
                Sex = model.Sex,
                SurName = model.SurName,
                TestFreeCondition = model.TestFreeCondition
            };

            int result = 0;
            if (model.Id > 0)//编辑
            {
                stu.LastModificationTime = DateTime.Now;
                stu.LastModifierUserId = user.UserId;
                var propertyNames = model.GetType().GetProperties()
                    .Where(p => p.Name != "Id")
                    .Select(p => p.Name)
                    .ToArray();
                result = _studentsService.UpdateBy(stu, p => p.Id == model.Id, propertyNames);
                if (result > 0)
                {
                    return Json(new
                    {
                        Status = 1,
                        Message = "编辑成功"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "编辑失败"
                    });
                }
            }
            else//新增
            {
                stu.CreationTime = DateTime.Now;
                stu.CreatorUserId = user.UserId;

                result = _studentsService.Add(stu);
                if (result > 0)
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
        
        [HttpPost]
        public ActionResult GetStudents(jqDataTableParameter tableParam, string realname, string creator, string status, string start_date, string end_date, string major_name)
        {
            UserDto user = ViewBag.User as UserDto;

            #region 组合查询条件
            var whereExp = PredicateBuilder.True<Students>();
            whereExp = whereExp.And(p => !p.IsDeleted);
            //创建人
            if (ViewBag.RoleId != 0)
                whereExp = whereExp.And(p => p.CreatorUserId == user.UserId);
            else
            {
                if (!string.IsNullOrEmpty(creator))
                    whereExp = whereExp.And(p => p.CreatorUserId == int.Parse(creator));
            }
            //学生姓名
            if (!string.IsNullOrEmpty(realname))
                whereExp = whereExp.And(p => p.SurName == realname.Trim());
            //审核状态
            if (!string.IsNullOrEmpty(status))
                whereExp = whereExp.And(p => p.Status == bool.Parse(status));
            //专业名称
            if (!string.IsNullOrEmpty(major_name))
                whereExp = whereExp.And(p => p.MajorName == major_name);
            //创建时间
            if (!string.IsNullOrEmpty(start_date))
                whereExp = whereExp.And(p => p.CreationTime >= DateTime.Parse(start_date));
            if (!string.IsNullOrEmpty(end_date))
                whereExp = whereExp.And(p => p.CreationTime <= DateTime.Parse(end_date));

            OperateHelper.Current.Session["Expression"] = whereExp;
            #endregion


            //1.0 首先获取datatable提交过来的参数
            string echo = tableParam.sEcho;  //用于客户端自己的校验
            int displayStart = tableParam.iDisplayStart;//要请求的该页第一条数据的序号
            int pageSize = tableParam.iDisplayLength;//每页容量（=-1表示取全部数据）

            var total = _studentsService.GetDataListBy(whereExp).Count();
            var students = _studentsService.GetPagedList<DateTime>(displayStart, pageSize, whereExp, p => p.CreationTime, false);
            var data = (from s in students
                       select new StudentOutput
                       {
                           Id = s.Id,
                           SurName = s.SurName,
                           Sex = s.Sex,
                           Period = s.Period,
                           ExaminationLevel = s.ExaminationLevel,
                           MajorName = s.MajorName,
                           Nationality = s.Nationality,
                           EducationalLevel = s.EducationalLevel,
                           Address = s.Address,
                           Phone = s.Phone,
                           Remark = s.Remark,
                           Creator = s.Users.RealName,
                           CreationTime = s.CreationTime,
                           Status = s.Status
                       }).ToList();
            return Json(new
            {
                sEcho = echo,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                aaData = data
            });
        }

        public FileResult Export()
        {
            Expression<Func<Students, bool>> expression = OperateHelper.Current.Session["Expression"] as Expression<Func<Students, bool>>;
            var students = _studentsService.GetDataListBy(expression).ToList();

            StringBuilder builder = new StringBuilder();
            int rowIndex = 0;
            foreach (var stu in students)
            {
                rowIndex++;
                builder.Append("<Row ss: Height = \"40\">");
                builder.Append("    <Cell ss: StyleID = \"s56\">");
                builder.Append("       <Data ss: Type = \"Number\">").Append(rowIndex).Append("</Data>");
                builder.Append("    </Cell >");
                builder.Append("    <Cell ss: StyleID = \"s57\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.SurName).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s57\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Sex == 0 ? "女" : "男").Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s56\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Nationality).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s58\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Phone).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s58\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Period).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s59\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.PoliticalStatus).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s60\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Address).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s60\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.EducationalLevel).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s64\">");
                builder.Append("        <Data ss: Type = \"String\" x: Ticked = \"1\">").Append(stu.ExaminationLevel).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s60\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.MajorName).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s65\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.TestFreeCondition).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s56\" />");
                builder.Append("</Row>");
            }

            var filePath = Server.MapPath("~") + "Template\\学生信息表.xml";
            var template = string.Empty;
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                template = reader.ReadToEnd();
            }
            template = template.Replace("{content}", builder.ToString());
            return File(Encoding.UTF8.GetBytes(template), "application/msexcel", "学生信息表.xls");
        }

        public ActionResult Delete(int id)
        {
            Students stu = new Students();
            stu.IsDeleted = true;
            stu.DeletionTime = DateTime.Now;
            stu.LastModifierUserId = (ViewBag.User as UserDto).UserId;
            stu.LastModificationTime = DateTime.Now;
            var propertyNames = new string[] { "IsDeleted", "DeletionTime", "LastModifierUserId", "LastModificationTime" };
            int result = _studentsService.UpdateBy(stu, p => p.Id == id, propertyNames);
            if (result > 0)
            {
                return Json(new
                {
                    Status = 1,
                    Message = "删除成功"
                });
            }
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "删除失败"
                });
            }
        }

        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        [HttpPost]
        public ActionResult Audit(int id)
        {
            UserDto user = ViewBag.User as UserDto;
            var student = _studentsService.GetEntity(id);
            var newStu = new Students() {
                Status = !student.Status,
                LastModifierUserId = user.UserId,
                LastModificationTime = DateTime.Now
            };
            var propertyNames = new string[] { "Status", "LastModifierUserId", "LastModificationTime" };
            int result = _studentsService.UpdateBy(newStu, p => p.Id == id, propertyNames);
            if (result > 0)
            {
                return Json(new
                {
                    Status = 1,
                    Message = "修改成功"
                });
            }
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "修改失败"
                });
            }
        }
    }
}