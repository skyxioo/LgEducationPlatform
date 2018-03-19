using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.Inject
{
    public class SpringHelper
    {
        //Spring容器上下文  以属性形式提供
        private static IApplicationContext SpringContext
        {
            get
            {
                return Spring.Context.Support.ContextRegistry.GetContext();
            }
        }

        //获取web.config里面的object对象
        /// <summary>
        /// 通过Spring.net依赖注入对象，
        /// </summary>
        /// <typeparam name="T">T被约束为引用类型</typeparam>
        /// <param name="objectName">以接口形式返回ServiceSession</param>
        /// <returns></returns>
        public static T GetObject<T>(string objectName) where T : class
        {
            return SpringContext.GetObject(objectName) as T;
        }
    }
}
