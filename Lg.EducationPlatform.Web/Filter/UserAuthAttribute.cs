using Lg.EducationPlatform.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lg.EducationPlatform.Web
{
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
            // 忽略可匿名访问的方法
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
            {
                return;
            }

            // 检查用户登录
            if (!System.Security.Authentication.IsLogin)
            {
                filterContext.Result = new RedirectResult(Common.NotAuthUrl());
                //filterContext.Result = new RedirectResult(System.Configuration.ConfigurationManager.AppSettings["ssologin"]);
                return;
            }

            //检查用权限
            if ((AllowRole & Authentication.LoginUserInfo.Role) == UserRole.None)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "_Error",
                    ViewData = { { "errorMsg", Error ?? "您没有浏览该模块的权限" } }
                };
                return;
            }
        }
    }
}