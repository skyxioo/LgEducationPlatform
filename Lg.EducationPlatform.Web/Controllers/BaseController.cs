using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Lg.EducationPlatform.Model;
using Newtonsoft.Json;

namespace Lg.EducationPlatform.Web.Controllers
{
    /// <summary>
    /// Controller本身继承了各种FilterAttribute（IActionFilter、IResultFilter等）
    /// </summary>
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //获取客户端Cookie值
            HttpCookie cookie = Request.Cookies["LgEduTicket"];
            string name = string.Empty;
            if (cookie != null)
            {
                string ticketString = cookie.Value;
                //解密ticket值
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(ticketString);
                name = ticket.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    UserDto user = JsonConvert.DeserializeObject<UserDto>(ticket.UserData);
                    ViewBag.AdminUser = name;
                    ViewBag.User = user;
                    ViewBag.RoleId = user.RoleId;
                    return;
                }
                else
                {
                    //跳转到登录页
                    filterContext.HttpContext.Response.Redirect("/Account/Login" );
                    return;
                }
            }
            else
            {
                //跳转到登录页
                filterContext.HttpContext.Response.Redirect("/Account/Login");
                return;
            }
        }
    }
}