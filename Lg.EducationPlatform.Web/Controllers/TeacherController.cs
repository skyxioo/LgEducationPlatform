using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.WebHelper;
using Lg.EducationPlatform.ViewModel;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.jqDataTableModel;
using Lg.EducationPlatform.Enum;

namespace Lg.EducationPlatform.Web.Controllers
{
    [UserAuth(AllowRole = Enum.UserRole.管理员)]
    public class TeacherController : BaseController
    {
        private IUsersService _usersService = OperateHelper.Current._serviceSession.UsersService;

        public ActionResult Index()
        {
            return View();
        }

        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        public ActionResult Add(int? id)
        {
            TeacherViewModel teacher = new TeacherViewModel();
            var roleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "教师", Value = "1" }
            };
            if (ViewBag.RoleId == (int)UserRole.超级管理员)
                roleList.Add(new SelectListItem { Text = "管理员", Value = "3" });

            ViewBag.UserRole = roleList;


            if (id != null && id > 0)
            {
                var user = _usersService.GetEntity(id.Value);
                teacher.Id = user.Id;
                teacher.UserName = user.UserName;
                teacher.RealName = user.RealName;
                teacher.Phone = user.Phone;
                teacher.Email = user.Email;
            }

            return View(teacher);
        }

        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        [HttpPost]
        public ActionResult Add(TeacherViewModel model)
        {
            UserDto user = ViewBag.User as UserDto;
            Users teacher = new Users
            {
                UserName = model.UserName,
                HashPassword = Security.StrToMD5(model.Password),
                RealName = model.RealName,
                Email = model.Email,
                Phone = model.Phone,
                RoleId = model.RoleId,
                IsActive = true,
                IsDeleted = false
            };

            int result = 0;
            if (model.Id > 0)//编辑
            {
                teacher.LastModificationTime = DateTime.Now;
                teacher.LastModifierUserId = user.UserId;
                var propertyNames = model.GetType().GetProperties()
                    .Where(p => p.Name != "Id")
                    .Select(p => p.Name)
                    .ToList();
                propertyNames.Add("HashPassword");
                result = _usersService.UpdateBy(teacher, p => p.Id == model.Id, true, propertyNames.ToArray());
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
                teacher.CreationTime = DateTime.Now;
                teacher.CreatorUserId = user.UserId;

                result = _usersService.Add(teacher);
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
        public ActionResult ValidateUser(string username, string id)
        {
            var user = _usersService.GetUser(username);
            if (user == null || (user != null && user.UserId.ToString() == id))
                return Json(true);
            else
                return Json(false);
        }

        [UserAuth(AllowRole = Enum.UserRole.管理员)]
        [HttpPost]
        public ActionResult GetTeachers(jqDataTableParameter tableParam)
        {
            var whereExp = PredicateBuilder.True<Users>();
            whereExp = whereExp.And(p => !p.IsDeleted && (p.RoleId == (int)UserRole.教师 || p.RoleId == (int)UserRole.管理员));

            //1.0 首先获取datatable提交过来的参数
            string echo = tableParam.sEcho;  //用于客户端自己的校验
            int displayStart = tableParam.iDisplayStart;//要请求的该页第一条数据的序号
            int pageSize = tableParam.iDisplayLength;//每页容量（=-1表示取全部数据）

            var total = _usersService.GetDataListBy(whereExp).Count();
            var teachers = _usersService.GetPagedList<DateTime>(displayStart, pageSize, whereExp, p => p.CreationTime, false);
            var data = (from s in teachers
                        select new TeacherOutput
                        {
                            Id = s.Id,
                            UserName = s.UserName,
                            RealName = s.RealName,
                            Email = s.Email,
                            Phone = s.Phone,
                            RoleId = s.RoleId,
                            CreationTime = s.CreationTime,
                            IsActive = s.IsActive
                        }).ToList();
            return Json(new
            {
                sEcho = echo,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                aaData = data
            });
        }

        public ActionResult State(int id)
        {
            try
            {
                UserDto user = ViewBag.User as UserDto;
                string type = Request.QueryString["type"];
                Users teacher = new Users();
                teacher.IsActive = type == "1" ? true : false;
                teacher.LastModificationTime = DateTime.Now;
                teacher.LastModifierUserId = user.UserId;
                var propertyNames = new string[] { "IsActive", "LastModificationTime", "LastModifierUserId" };
                int result = _usersService.UpdateBy(teacher, p => p.Id == id, true, propertyNames);
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
            catch(Exception ex)
            {
                LoggerHelper.Error(ex.Message);
                return Json(new
                {
                    Status = 0,
                    Message = "编辑失败"
                });
            }
        }

        public ActionResult Delete(int id)
        {
            Users teacher = new Users();
            teacher.IsDeleted = true;
            teacher.DeletionTime = DateTime.Now;
            teacher.LastModifierUserId = (ViewBag.User as UserDto).UserId;
            teacher.LastModificationTime = DateTime.Now;
            var propertyNames = new string[] { "IsDeleted", "DeletionTime", "LastModifierUserId", "LastModificationTime" };
            int result = _usersService.UpdateBy(teacher, p => p.Id == id, true, propertyNames);
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
    }
}