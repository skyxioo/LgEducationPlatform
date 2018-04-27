using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lg.EducationPlatform.Model;

namespace Lg.EducationPlatform.IBLL
{
    public partial interface IWebSettingsService : IBaseService<WebSettings>
    {
        WebSettings GetWebSettingByKey(string key);
        IQueryable<WebSettings> GetListByKeys(List<string> keys);
    }
}
