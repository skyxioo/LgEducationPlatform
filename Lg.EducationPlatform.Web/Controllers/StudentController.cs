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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

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
            if (user.RoleId == (int)UserRole.管理员 || user.RoleId == (int)UserRole.超级管理员)
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
                ExaminationLevelStr = s.ExaminationLevel == 1 ? "专科" : "专升本",                
                MajorName = s.MajorName,
                Nationality = s.Nationality,
                PoliticalStatus = s.PoliticalStatus,
                EducationalLevelStr = GetEducationalLevel(s.EducationalLevel),
                Address = s.Address,
                Phone = s.Phone,
                Remark = s.Remark,
                Birthplace = s.Birthplace,
                Status = s.Status,
                //TestFreeCondition = s.TestFreeCondition,
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
                new SelectListItem{ Text = "初中及以下", Value = "0", Selected = true },
                new SelectListItem{ Text = "高中或中专", Value = "1" },
                new SelectListItem{ Text = "大专", Value = "2" },
                new SelectListItem{ Text = "本科及以上", Value = "3" }
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
            List<SelectListItem> politicalItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "群众", Value = "群众" },
                new SelectListItem { Text = "党员", Value = "党员" },
                new SelectListItem { Text = "无党派人士", Value = "无党派人士" },
                new SelectListItem { Text = "其他", Value = "其他" }
            };
            ViewBag.EduLevelItemList = eduItems;
            ViewBag.ExamLevelItemList = examItems;
            ViewBag.MajorItemList = majorItems;
            ViewBag.PoliticalItemList = politicalItems;

            ViewBag.Title = "长沙理工海南自考部综合管理系统|学生管理|添加";
            ViewBag.Opened = 0;
            ViewBag.EndDate = "";
            StudentViewModel model = new StudentViewModel();

            if(id != null && id.Value > 0)
            {
                ViewBag.Title = "长沙理工海南自考部综合管理系统|学生管理|编辑";

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
                model.Birthplace = student.Birthplace;
                //model.TestFreeCondition = student.TestFreeCondition;
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
                        try
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
                            student.Birthplace = dr["籍贯"].ToString();
                            student.Remark = dr["备注"].ToString();
                            if (string.IsNullOrWhiteSpace(student.SurName) ||
                                string.IsNullOrWhiteSpace(student.Nationality) ||
                                string.IsNullOrWhiteSpace(student.Birthplace) ||
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
                        catch (Exception ex)
                        {
                            LoggerHelper.Error("导入的Excel格式错误", ex);
                            info = "导入的Excel格式错误";
                            break;
                        }
                    }

                    list.ForEach(p =>
                    {
                        p.CreatorUserId = user.UserId;
                        p.CreationTime = DateTime.Now;
                        p.Status = 0;
                        p.IsDeleted = false;
                    });

                    if (list.Count > 0)
                    {
                        int result = _studentsService.AddRange(list);
                        if (result > 0)
                        {
                            return Json(new
                            {
                                Status = 1,
                                Message = "导入成功\r\n" + info
                            });
                        }
                    }


                    return Json(new
                    {
                        Status = 0,
                        Message = "导入失败\r\n" + info
                    });
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
                case "初中及以下":
                    level = 0;
                    break;
                case "高中或中专":
                    level = 1;
                    break;
                case "大专":
                    level = 2;
                    break;
                case "本科及以上":
                    level = 3;
                    break;
                default:
                    level = 0;
                    break;
            }
            return level;
        }

        private string GetEducationalLevel(byte education)
        {
            var level = "初中及以下";
            switch (education)
            {
                case 0:
                    level = "初中及以下";
                    break;
                case 1:
                    level = "高中或中专";
                    break;
                case 2:
                    level = "大专";
                    break;
                case 3:
                    level = "本科及以上";
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
                Birthplace = model.Birthplace,
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
                SurName = model.SurName
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
                    //需要修改的字段
                    var propertyNames = model.GetType().GetProperties()
                        .Where(p => p.Name != "Id")
                        .Select(p => p.Name)
                        .ToList();
                    if (model.BareheadedPhoto == null)
                        propertyNames.Remove("BareheadedPhotoPath");
                    if (model.IdCardFront == null)
                        propertyNames.Remove("IdCardFrontPath");
                    if (model.IdCardBack == null)
                        propertyNames.Remove("IdCardBackPath");

                    result = _studentsService.UpdateBy(stu, p => p.Id == model.Id, true, propertyNames.ToArray());
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
        public ActionResult GetStudents(jqDataTableParameter tableParam, string realname, string idcard, string creator, string status, string period, string major_name, string examination_level)
        {
            UserDto user = ViewBag.User as UserDto;

            #region 组合查询条件
            var whereExp = PredicateBuilder.True<Students>();
            whereExp = whereExp.And(p => !p.IsDeleted);
            //创建人
            if (ViewBag.RoleId != (int)UserRole.超级管理员 && ViewBag.RoleId != (int)UserRole.管理员)
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
            if (!string.IsNullOrWhiteSpace(realname))
                whereExp = whereExp.And(p => p.SurName == realname.Trim());
            //身份证号
            if (!string.IsNullOrWhiteSpace(idcard))
                whereExp = whereExp.And(p => p.IdCard == idcard.Trim());
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
            if (!string.IsNullOrWhiteSpace(period))
                whereExp = whereExp.And(p => p.Period.Contains(period.Trim()));
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
            return File(Encoding.UTF8.GetBytes(template), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "学生信息表.xlsx");
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
                var savePath = Server.MapPath("~/Temp/"+DateTime.Now.ToString("yyyyMMddHHmmssfff")+".zip");
                zip.Save(savePath);
                return File(savePath, "application/zip", "图片.zip");
            }
        }

        public FileResult DownloadPdf(long id)
        {
            var student = _studentsService.GetEntity(id);
            //文件临时存储路径
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "/Temp/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";
            BaomingHtml(filePath, student);
            return File(filePath, "application/pdf", student.SurName + "的报名表.pdf");
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

        /// <summary>
        /// html转pdf
        /// </summary>
        /// <param name="filePath">文件存储路径</param>
        private void BaomingHtml(string filePath, Students student)
        {
            //获得已拼接好的
            StringBuilder sb = PdfHtml(student);
            Document document = new Document();
            //设置页面大小是A4纸大小
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //创建文档
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            //指定文件預設開檔時的縮放為100%
            //PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, document.PageSize.Height, 1f);
            document.Open();

            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());//字串轉成byte[]
            MemoryStream msInput = new MemoryStream(data);

            //使用XMLWorkerHelper把Html parse到PDF檔裡
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msInput, null, Encoding.UTF8, new UnicodeFontFactory());

            document.Close();
        }

        private StringBuilder PdfHtml(Students student)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<center><h2>高等教育自学考试长沙理工大学（海南）报名登记表</h2></center>");
            sb.Append("<table><tr><td>报名点：010180 长沙市 芙蓉区 长沙理工大学继续教育学院高新助学站 </td></tr></table>");
            sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0 auto;\">");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"width: 11%; height: 50px; line-height: 50px; text-align: center;border-left:0.5px solid #000;border-top:0.5px solid #000;font-size: 12pt;\" >姓名</td>");
            sb.Append("         <td style=\"width: 15%;border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >");
            sb.Append(              student.SurName);
            sb.Append("         </td>");
            sb.Append("         <td style=\"width: 10%;text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;font-size: 12pt;\" >性别</td>");
            sb.Append("         <td style=\"width: 14%;border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >");
            sb.Append(              student.Sex == 0 ? "女" : "男");
            sb.Append("         </td>");
            sb.Append("         <td style=\"width: 11%;text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;font-size: 12pt;\" >民族</td>");
            sb.Append("         <td style=\"width: 16%;border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >");
            sb.Append(              student.Nationality);
            sb.Append("         </td>");
            sb.Append("         <td rowspan=\"4\" style=\"width: 23%;text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;border-right: 0.5px solid #000;\">");
            if(!string.IsNullOrWhiteSpace(student.BareheadedPhotoPath))
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + student.BareheadedPhotoPath + "\" height=\"196px\" />");
            sb.Append("         </td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"height: 50px; line-height: 50px; text-align: center;border-top:0.5px solid #000; border-left:0.5px solid #000;\" >出生日期</td>");
            sb.Append("         <td style=\"border-top:0.5px solid #000; border-left:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.IdCard.Substring(6, 4) + "-" + student.IdCard.Substring(10, 2) + "-" + student.IdCard.Substring(12, 2)).Append("</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-top:0.5px solid #000; border-left:0.5px solid #000;\" >籍贯</td>");
            sb.Append("         <td style=\"border-top:0.5px solid #000; border-left:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.Birthplace).Append("</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-top:0.5px solid #000; border-left:0.5px solid #000;\" >政治面貌</td>");
            sb.Append("         <td style=\"border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.PoliticalStatus).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"height: 50px; line-height: 50px; text-align: center;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >证件类型</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >身份证</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >证件 <br /> 号码 </td>");
            sb.Append("         <td colspan=\"3\" style=\"border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.IdCard).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"height: 50px; line-height: 50px; text-align: center;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >文化程度</td>");
            sb.Append("         <td style=\"border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(GetEducationalLevel(student.EducationalLevel)).Append("</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;\">报考<br /> 层次</td>");
            if (student.ExaminationLevel == 1)
            {
                sb.Append("     <td style=\"text-align: right;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;font-size:11pt;\" >");
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + @"/Content/images/checkbox.png" + "\" width=\"14px\" height=\"14px\" /> 专科");
                sb.Append("     </td>");
                sb.Append("     <td colspan=\"2\" style=\"text-align: center;vertical-align: middle;border-top:0.5px solid #000;font-size:11pt;\" >");
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + @"/Content/images/checkboxline.png" + "\" width=\"14px\" height=\"14px\" /> 专升本");
                sb.Append("     </td>");
            }
            else
            {
                sb.Append("     <td style=\"text-align: right;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;font-size:11pt;\" >");
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + @"/Content/images/checkboxline.png" + "\" width=\"14px\" height=\"14px\" /> 专科");
                sb.Append("     </td>");
                sb.Append("     <td colspan=\"2\" style=\"text-align: center;vertical-align: middle;border-top:0.5px solid #000;font-size:11pt;\" >");
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + @"/Content/images/checkbox.png" + "\" width=\"14px\" height=\"14px\" /> 专升本");
                sb.Append("     </td>");
            }
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"height: 50px; line-height: 50px; text-align: center;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >报考专业</td>");
            sb.Append("         <td colspan=\"3\" style=\"border-left:0.5px solid #000;border-top:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.MajorName).Append("</td>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;\" >联系电话</td>");
            sb.Append("         <td colspan=\"2\" style=\"border-left:0.5px solid #000;border-top:0.5px solid #000;border-right:0.5px solid #000; font-size:11pt; text-align: center;\" >").Append(student.Phone).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td colspan=\"7\" style=\"border-left:0.5px solid #000;border-top:0.5px solid #000; border-right: 0.5px solid #000; font-size:11pt;\" >");
            sb.Append("             <br /><div style=\"text-align: center; height:20px;line-height: 20px;\">诚信承诺书</div>");
            sb.Append("             <p style=\"line-height: 20px; margin-left:10px;\" >&nbsp;&nbsp;本人知悉学院高等继续教育相关政策，未通过中介，自愿报名参加学习，按照学院规定缴纳报名费、学费、报考费和其它规定费用，保证入学后认真学习，积极作业，诚信考试，任何考试中不找人替考、舞弊，遵守考试纪律，如有违反，甘愿接受学校一切处理。");
            sb.Append("             <br /> &nbsp;&nbsp;本人提供真实有效有身份证、毕业证等证明材料，如有任何不实之处所造成的一切后果由本人自己承担，助学站点和学校不承担任何责任。</p>");
            sb.Append("             <p style=\"text-align: right; margin-right: 200px; margin-top: 15px; \" >承诺人签字：</p>");
            sb.Append("             <p style=\"text-align: right; margin: 15px 100px; 20px 0px \" >2018年&nbsp;月&nbsp;日 </p>");
            sb.Append("         </td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td colspan=\"4\" style=\"height: 240px;text-align: center;vertical-align: middle; border-left:0.5px solid #000;border-top:0.5px solid #000;\" >");
            if (!string.IsNullOrWhiteSpace(student.IdCardFrontPath))
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + student.IdCardFrontPath + "\" />");
            sb.Append("         </td>");
            sb.Append("         <td colspan=\"3\" style=\"text-align: center;vertical-align: middle; border-left:0.5px solid #000;border-top:0.5px solid #000; border-right: 0.5px solid #000;\" >");
            if (!string.IsNullOrWhiteSpace(student.IdCardBackPath))
                sb.Append("         <img src=\"" + AppDomain.CurrentDomain.BaseDirectory + student.IdCardBackPath + "\" />");
            sb.Append("         </td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"text-align: center;vertical-align: middle;border-left:0.5px solid #000;border-top:0.5px solid #000;border-bottom:0.5px solid #000;\" > 注意 <br /> 事项 </td>");
            sb.Append("         <td colspan=\"6\" style=\"border:0.5px solid #000;font-size:11pt;\">");
            sb.Append("             <br />");
            sb.Append("             <ol style=\"margin-bottom: 20px;\">");
            sb.Append("                 <li> 本人对以上报名信息记录的真实性做了仔细核对，确认无误。如存在错误虚假信息或资格造假而影响正常参加考试与毕业，概由本人负责。 </li>");
            sb.Append("                 <li> 2018年自学考试时间：4月、10月中旬；函授考试时间：10月中下旬。请提前安排好时间准备参加考试，外地考生请提前安排好住宿，考前请项准备好各自的考前准备工作，以免影响考试。 </li>");
            sb.Append("                 <li> 打印纸张为A4纸张，不得折叠损坏，保持页面整洁。由招考部门统一送省教育考试院扫描，此表存档到学员毕业。</li>");
            sb.Append("             </ol>");
            sb.Append("         </td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td colspan=\"7\" style=\"text-align: right; font-size:9pt; height: 20px; \"> 中国高等教育学生信息网（学信网）http://www.chsi.com.cn</td>");
            sb.Append("     </tr>");
            sb.Append("</table>");

            return sb;
        }
    }
}