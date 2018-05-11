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
using System.Data;
using System.Text;
using System.IO;
using Ionic.Zip;
using Lg.EducationPlatform.Enum;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class StudentController : BaseController
    {
        private const string OPEN_START_DATE_KEY = "SYSTEM_OPEN_START_DATE";
        private const string OPEN_END_DATE_KEY = "SYSTEM_OPEN_END_DATE";
        private const string PERIOD_KEY = "SYSTEM_CURRENT_PERIOD";
        private IUsersService _userService = OperateHelper.Current._serviceSession.UsersService;
        private IStudentsService _studentsService = OperateHelper.Current._serviceSession.StudentsService;
        private IWebSettingsService _webSettingsService = OperateHelper.Current._serviceSession.WebSettingsService;

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

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Show(int id)
        {
            var s = _studentsService.GetEntity(id);
            var model = new StudentViewModel {
                Id = s.Id,
                SurName = s.SurName,
                Sex = s.Sex,
                IdCard = s.IdCard,
                Period = s.Period,
                ExaminationLevelStr = GetEducationalLevel(s.ExaminationLevel),
                MajorName = s.MajorName,
                Nationality = s.Nationality,
                PoliticalStatus = s.PoliticalStatus,
                EducationalLevelStr = s.EducationalLevel == 1 ? "专科" : "专升本",
                Address = s.Address,
                Phone = s.Phone,
                Remark = s.Remark,
                Status = s.Status,
                TestFreeCondition = s.TestFreeCondition,
                IdCardFrontPath = string.IsNullOrWhiteSpace(s.IdCardFrontPath) ? "/Content/images/nopic.png" : s.IdCardFrontPath,
                IdCardBackPath = string.IsNullOrWhiteSpace(s.IdCardBackPath) ? "/Content/images/nopic.png" : s.IdCardBackPath,
                BareheadedPhotoPath = string.IsNullOrWhiteSpace(s.BareheadedPhotoPath) ? "/Content/images/nopic.png" : s.BareheadedPhotoPath
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
                new SelectListItem{ Text = "专升本", Value = "2", Selected = true }
            };
            List<SelectListItem> majorItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "交通土建工程", Value = "交通土建工程" },
                new SelectListItem { Text = "工程财务管理", Value = "工程财务管理" },
                new SelectListItem { Text = "汽车运用工程", Value = "汽车运用工程" }
            };
            ViewBag.EduLevelItemList = eduItems;
            ViewBag.ExamLevelItemList = examItems;
            ViewBag.MajorItemList = majorItems;

            ViewBag.Title = "长沙理工大学综合管理系统|学生管理|添加";
            ViewBag.Opened = 0;
            ViewBag.EndDate = "";
            StudentViewModel model = new StudentViewModel();

            if(id != null && id.Value > 0)
            {
                ViewBag.Title = "长沙理工大学综合管理系统|学生管理|编辑";

                var student = _studentsService.GetEntity(id.Value);
                model.Address = student.Address;
                model.BareheadedPhotoPath = student.BareheadedPhotoPath;
                model.IdCard = student.IdCard;
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
                model.IdCardFrontPath = string.IsNullOrWhiteSpace(student.IdCardFrontPath) ? "/Content/images/nopic.png" : student.IdCardFrontPath;
                model.IdCardBackPath = string.IsNullOrWhiteSpace(student.IdCardBackPath) ? "/Content/images/nopic.png" : student.IdCardBackPath;
                model.BareheadedPhotoPath = string.IsNullOrWhiteSpace(student.BareheadedPhotoPath) ? "/Content/images/nopic.png" : student.BareheadedPhotoPath;
            }
            else
            {
                List<string> keys = new List<string> { OPEN_START_DATE_KEY, OPEN_END_DATE_KEY, PERIOD_KEY };
                List<WebSettings> list = _webSettingsService.GetListByKeys(keys).ToList();
                WebSettings startDateSetting = list.Where(p => p.ConfigKey == OPEN_START_DATE_KEY).FirstOrDefault();
                WebSettings endDateSetting = list.Where(p => p.ConfigKey == OPEN_END_DATE_KEY).FirstOrDefault();
                WebSettings periodSetting = list.Where(p => p.ConfigKey == PERIOD_KEY).FirstOrDefault();

                if (periodSetting != null)
                {
                    model.Period = periodSetting.ConfigValue;
                }
                else
                {
                    var period = DateTime.Now.ToString("yyyy");
                    if (DateTime.Now.Month <= 6)
                        period = period + "春季";
                    else
                        period = period + "秋季";
                    model.Period = period;
                }

                int opened = 1;
                try
                {
                    if (DateTime.Parse(startDateSetting.ConfigValue) < DateTime.Now && DateTime.Now < DateTime.Parse(endDateSetting.ConfigValue))
                    {
                        opened = 1;
                        ViewBag.EndDate = endDateSetting.ConfigValue;
                    }
                    else
                        opened = 0;
                }
                catch(Exception ex)
                {
                    LoggerHelper.Error("开放学生信息注册起始时间格式有误");
                }
                ViewBag.Opened = opened;
                TempData["Opened"] = opened;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ImportXls()
        {
            var files = Request.Files;
            if (files.Count == 0)
            {
                return Json(new
                {
                    Status = 0,
                    Message = "上传文件为空，请选择要上传的excel文件"
                });
            }
            else
            {
                UserDto user = ViewBag.User as UserDto;
                string savePath = Server.MapPath("~/Temp/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + Path.GetExtension(files[0].FileName);
                files[0].SaveAs(savePath);

                string error = string.Empty;
                string info = "";
                ExcelHelper excelHelper = new ExcelHelper(savePath);
                DataTable dt = excelHelper.ExcelToDataTable(out error);
                if (string.IsNullOrEmpty(error))
                {
                    WebSettings periodSetting = _webSettingsService.GetWebSettingByKey(PERIOD_KEY);

                    int idx = 2;
                    List<Students> list = new List<Students>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Students student = new Students();
                        student.SurName = dr["姓名"].ToString();
                        student.Sex = dr["性别"].ToString() == "女" ? (byte)0 : (byte)1;
                        student.Nationality = dr["民族"].ToString();
                        student.IdCard = dr["身份证号"].ToString();
                        student.Phone = dr["手机"].ToString();
                        student.Period = periodSetting.ConfigValue;
                        student.PoliticalStatus = dr["政治面貌"].ToString();
                        student.Address = dr["地址"].ToString();
                        student.EducationalLevel = GetEducationalLevel(dr["文化层次"].ToString());
                        student.ExaminationLevel = dr["报考层次"].ToString() == "专科" ? (byte)1 : (byte)2;
                        student.MajorName = dr["专业"].ToString();
                        student.TestFreeCondition = dr["免试条件"].ToString();
                        student.Remark = dr["备注"].ToString();
                        if (string.IsNullOrWhiteSpace(student.SurName) ||
                            string.IsNullOrWhiteSpace(student.Nationality) ||
                            string.IsNullOrWhiteSpace(student.IdCard) ||
                            string.IsNullOrWhiteSpace(student.Phone) ||
                            string.IsNullOrWhiteSpace(student.PoliticalStatus) ||
                            string.IsNullOrWhiteSpace(student.Address) ||
                            string.IsNullOrWhiteSpace(student.MajorName) ||
                            student.IdCard.Length != 18 ||
                            student.Phone.Length != 11)
                        {
                            info += "第" + idx + "行导入失败，必填项存在空值或身份证、手机号输入有误，请检查\r\n";
                            continue;
                        }
                        else
                            list.Add(student);
                    }

                    list.ForEach(p =>
                    {
                        p.CreatorUserId = user.UserId;
                        p.CreationTime = DateTime.Now;
                        p.Status = 0;
                        p.IsDeleted = false;
                    });
                    int result = _studentsService.AddRange(list);
                    if (result > 0)
                    {
                        return Json(new
                        {
                            Status = 1,
                            Message = "导入成功\r\n" + info
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = 0,
                            Message = "导入失败"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = error
                    });
                }
            }
        }

        private byte GetEducationalLevel(string education)
        {
            byte level = 0;
            switch(education)
            {
                case "无":
                    level = 0;
                    break;
                case "初中":
                    level = 1;
                    break;
                case "高中":
                    level = 2;
                    break;
                case "中专":
                    level = 3;
                    break;
                case "大专在读":
                    level = 4;
                    break;
                case "大专毕业":
                    level = 5;
                    break;
                default:
                    level = 0;
                    break;
            }
            return level;
        }

        private string GetEducationalLevel(byte education)
        {
            var level = "无";
            switch (education)
            {
                case 0:
                    level = "无";
                    break;
                case 1:
                    level = "初中";
                    break;
                case 2:
                    level = "高中";
                    break;
                case 3:
                    level = "中专";
                    break;
                case 4:
                    level = "大专在读";
                    break;
                case 5:
                    level = "大专毕业";
                    break;
            }
            return level;
        }

        [HttpPost]
        public JsonResult Add(StudentViewModel model)
        {
            if (model.Id == 0)
            {
                object opened = TempData["Opened"];
                if (opened == null)
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "页面已过期，请重新进入本页面"
                    });
                }
                else
                {
                    if(opened.ToString() == "0")
                    {
                        return Json(new
                        {
                            Status = 0,
                            Message = "开放注册时间已过，不允许添加学生信息"
                        });
                    }
                }
            }

            UserDto user = ViewBag.User as UserDto;
            Students stu = new Students {
                Address = model.Address,
                BareheadedPhotoPath = model.BareheadedPhotoPath,
                IdCard = model.IdCard,
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


            // 文件上传后的保存路径
            string filePath = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            if (model.BareheadedPhoto != null)
            { 
                string fileName = Path.GetFileName(model.BareheadedPhoto.FileName);// 原始文件名称
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                model.BareheadedPhoto.SaveAs(filePath + saveName);
                stu.BareheadedPhotoPath = "/Uploads/" + saveName;
            }
            if (model.IdCardFront != null)
            {
                string fileName = Path.GetFileName(model.IdCardFront.FileName);// 原始文件名称
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                model.IdCardFront.SaveAs(filePath + saveName);
                stu.IdCardFrontPath = "/Uploads/" + saveName;
            }
            if (model.IdCardBack != null)
            {
                string fileName = Path.GetFileName(model.IdCardBack.FileName);// 原始文件名称
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                model.IdCardBack.SaveAs(filePath + saveName);
                stu.IdCardBackPath = "/Uploads/" + saveName;
            }


            int result = 0;
            if (model.Id > 0)//编辑
            {
                if (ViewBag.RoleId == (int)UserRole.管理员 || _studentsService.GetEntity(model.Id).Status == 0)
                {
                    stu.LastModificationTime = DateTime.Now;
                    stu.LastModifierUserId = user.UserId;
                    var propertyNames = model.GetType().GetProperties()
                        .Where(p => p.Name != "Id")
                        .Select(p => p.Name)
                        .ToArray();
                    result = _studentsService.UpdateBy(stu, p => p.Id == model.Id, true, propertyNames);
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
                else
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "编辑失败，已审信息不允许修改"
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
        public ActionResult GetStudents(jqDataTableParameter tableParam, string realname, string creator, string status, string period, string major_name, string examination_level)
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
                {
                    var userId = int.Parse(creator);
                    whereExp = whereExp.And(p => p.CreatorUserId == userId);
                }
            }
            //学生姓名
            if (!string.IsNullOrEmpty(realname))
                whereExp = whereExp.And(p => p.SurName == realname.Trim());
            //审核状态
            if (!string.IsNullOrEmpty(status))
            {
                var s = byte.Parse(status);
                whereExp = whereExp.And(p => p.Status == s);
            }
            //专业名称
            if (!string.IsNullOrEmpty(major_name))
                whereExp = whereExp.And(p => p.MajorName == major_name);
            //届别
            if (!string.IsNullOrEmpty(period))
                whereExp = whereExp.And(p => p.Period.Contains(period));
            //报考层次
            if (!string.IsNullOrEmpty(examination_level))
            {
                var level = int.Parse(examination_level);
                whereExp = whereExp.And(p => p.ExaminationLevel == level);
            }

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
                           IdCard = s.IdCard,
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

                builder.Append("<Row ss: Height = \"34\">");
                builder.Append("    <Cell ss: StyleID = \"s55\">");
                builder.Append("       <Data ss: Type = \"Number\">").Append(rowIndex).Append("</Data>");
                builder.Append("    </Cell >");
                builder.Append("    <Cell ss: StyleID = \"s56\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.SurName).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s56\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Sex == 0 ? "女" : "男").Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s55\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Nationality).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s57\">");
                builder.Append("        <Data ss: Type = \"String\" x:Ticked = \"1\">").Append(stu.IdCard).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s57\">");
                builder.Append("        <Data ss: Type = \"String\" x:Ticked = \"1\">").Append(stu.MajorName).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s58\">");
                builder.Append("        <Data ss: Type = \"String\" x: Ticked = \"1\">").Append(stu.ExaminationLevel == 1 ? "专科" : "专升本").Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s59\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Address).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s55\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Phone).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s55\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Users.RealName).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s61\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.CreationTime.ToString("yyyy/MM/dd")).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s55\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(stu.Period).Append("</Data>");
                builder.Append("    </Cell>");
                builder.Append("    <Cell ss: StyleID = \"s56\">");
                builder.Append("        <Data ss: Type = \"String\">").Append(GetStatus(stu.Status)).Append("</Data>");
                builder.Append("    </Cell>");
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

        private string GetStatus(int status)
        {
            string result = "未确认";
            switch(status)
            {
                case 1:
                    result = "已注册";
                    break;
                case 2:
                    result = "退学";
                    break;
            }
            return result;
        }

        public FileResult DownLoad()
        {
            Expression<Func<Students, bool>> expression = OperateHelper.Current.Session["Expression"] as Expression<Func<Students, bool>>;
            var students = _studentsService.GetDataListBy(expression).ToList();
            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
            {
                string basePath = Server.MapPath("~");
                foreach (var stu in students)
                {
                    if (!string.IsNullOrWhiteSpace(stu.BareheadedPhotoPath))
                        zip.AddFile(basePath + stu.BareheadedPhotoPath, stu.SurName);
                    if (!string.IsNullOrWhiteSpace(stu.IdCardBackPath))
                        zip.AddFile(basePath + stu.IdCardBackPath, stu.SurName);
                    if (!string.IsNullOrWhiteSpace(stu.IdCardFrontPath))
                        zip.AddFile(basePath + stu.IdCardFrontPath, stu.SurName);
                }
                var savePath = Server.MapPath("~/Temp/"+DateTime.Now.ToString("yyyyMMddhhmmssfff")+".zip");
                zip.Save(savePath);
                return File(savePath, "application/zip", "图片.zip");
            }
        }

        public ActionResult Delete(long id)
        {
            var status = _studentsService.GetEntity(id).Status;
            if (status == 0)
            {
                Students stu = new Students();
                stu.IsDeleted = true;
                stu.DeletionTime = DateTime.Now;
                stu.LastModifierUserId = (ViewBag.User as UserDto).UserId;
                stu.LastModificationTime = DateTime.Now;
                var propertyNames = new string[] { "IsDeleted", "DeletionTime", "LastModifierUserId", "LastModificationTime" };
                int result = _studentsService.UpdateBy(stu, p => p.Id == id, true, propertyNames);
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
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "删除失败，已审信息不允许删除"
                });
            }
        }

        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        [HttpPost]
        public ActionResult Audit(int id)
        {
            byte status = byte.Parse(Request.Form["status"]);
            UserDto user = ViewBag.User as UserDto;
            var student = _studentsService.GetEntity(id);
            var newStu = new Students() {
                Status = status,
                LastModifierUserId = user.UserId,
                LastModificationTime = DateTime.Now
            };
            var propertyNames = new string[] { "Status", "LastModifierUserId", "LastModificationTime" };
            int result = _studentsService.UpdateBy(newStu, p => p.Id == id, true, propertyNames);
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