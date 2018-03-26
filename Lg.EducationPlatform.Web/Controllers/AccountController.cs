using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.ViewModel;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.WebHelper;
using Lg.EducationPlatform.Model;
using System.Web.Security;
using Newtonsoft.Json;

namespace Lg.EducationPlatform.Web.Controllers
{    
    public class AccountController : Controller
    {
        private IUsersService _userService = OperateHelper.Current._serviceSession.UsersService;

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser userModel)
        {
            //实体验证成功的话 进一步验证
            if (ModelState.IsValid)
            {
                //校验成功,将用户信息保存到Session中，并将票据写入cookie,跳转至后台首页
                //后台中的每个Controller都要继承一个BaseController,BaseController中要先校验用户有没有登录,
                //之后才能进行Action操作
                //该用户校验通过：写完cookie和sesion后跳转到首页
               var userDto =  _userService.GetUser(userModel.UserName);
                if (userDto != null && userDto.PassWord == Security.StrToMD5(userModel.PassWord))
                {
                    ////写Session //跳转到首页 
                    //Session.Add("loginuser", userModel);

                    userDto.PassWord = string.Empty;
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        2,
                        userModel.UserName,
                        DateTime.Now,
                        DateTime.Now.AddHours(2),
                        true,
                        JsonConvert.SerializeObject(userDto)
                    );
                    HttpCookie cookie = new HttpCookie("LgEduTicket");
                    string ticketString = FormsAuthentication.Encrypt(ticket);
                    cookie.Value = ticketString;
                    cookie.Expires = DateTime.Now.AddHours(2);  //cookie的过期时间
                    this.Response.Cookies.Add(cookie);

                    return Json(new
                    {
                        Status = 1,
                        CoreData = "/Home/Index"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "用户名或密码错误"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "没通过验证,请核对信息"
                });
            }
        }

        /// <summary>
        /// 退出登录：删除session,跳转到登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            ////删除Session
            //if (Session["loginuser"] != null)
            //    Session.Remove("loginuser");

            HttpCookie cookie = new HttpCookie("LgEduTicket");
            cookie.Expires = new DateTime(1990 - 11 - 23);
            this.Response.Cookies.Add(cookie);

            //跳转到登录页
            return Redirect("Login");
        }
    }
}