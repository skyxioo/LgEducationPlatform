using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.Model;
using Lg.EducationPlatform.ViewModel;
using Lg.EducationPlatform.WebHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IUsersService _usersService = OperateHelper.Current._serviceSession.UsersService;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            EditUserViewModel userModel = new EditUserViewModel();
            var userDto = ViewBag.User as UserDto;
            var user = _usersService.GetEntity(userDto.UserId);
            userModel.UserId = user.Id;
            userModel.UserName = user.UserName;
            userModel.RealName = user.RealName;
            userModel.Phone = user.Phone;
            userModel.Email = user.Email;
            return View(userModel);
        }

        public ActionResult EditProfile(EditUserViewModel model)
        {
            UserDto userDto = ViewBag.User as UserDto;
            var propertyNames = new List<string>();
            Users user = new Users();
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                user.UserName = model.UserName;
                propertyNames.Add("UserName");
            }
            if (!string.IsNullOrWhiteSpace(model.RealName))
            {
                user.RealName = model.RealName;
                propertyNames.Add("RealName");
            }
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                user.Email = model.Email;
                propertyNames.Add("Email");
            }
            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                user.Phone = model.Phone;
                propertyNames.Add("Phone");
            }
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.HashPassword = Security.StrToMD5(model.Password);
                propertyNames.Add("HashPassword");
            }

            int result = 0;
            user.LastModificationTime = DateTime.Now;
            user.LastModifierUserId = userDto.UserId;
            propertyNames.Add("LastModificationTime");
            propertyNames.Add("LastModifierUserId");

            result = _usersService.UpdateBy(user, p => p.Id == model.UserId, true, propertyNames.ToArray());
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
    }
}