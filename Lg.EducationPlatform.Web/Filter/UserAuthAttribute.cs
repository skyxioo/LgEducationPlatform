using Lg.EducationPlatform.Enum;
using Lg.EducationPlatform.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lg.EducationPlatform.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UserAuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 允许访问的用户角色，
        /// 默认允许所有角色<see cref="UserRole.All"/>访问
        /// </summary>
        public UserRole AllowRole { get; set; }
        /// <summary>
        /// 无权访问的错误提示
        /// </summary>
        public string Error { get; set; }

        public UserAuthAttribute()
        {
            AllowRole = UserRole.全部;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["LgEduTicket"];
            string ticketString = cookie.Value;
            //解密ticket值
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(ticketString);
            UserDto user = JsonConvert.DeserializeObject<UserDto>(ticket.UserData);
            //检查用权限
            if (AllowRole != UserRole.全部 && (int)AllowRole != user.RoleId)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "_Error",
                    ViewData = { { "RoleId", user.RoleId } }
                };
                return;
            }
        }
    }
}