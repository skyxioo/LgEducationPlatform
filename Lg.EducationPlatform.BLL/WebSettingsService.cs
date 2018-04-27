using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.Model;

namespace Lg.EducationPlatform.BLL
{
    public partial class WebSettingsService : IBaseService<WebSettings>, IWebSettingsService
    {
        public WebSettings GetWebSettingByKey(string key)
        {
            var whereExp = PredicateBuilder.True<WebSettings>();
            whereExp = whereExp.And(p => p.ConfigKey == key);
            return CurrentDAL.GetDataListBy(whereExp).FirstOrDefault();
        }

        public IQueryable<WebSettings> GetListByKeys(List<string> keys)
        {
            var whereExp = PredicateBuilder.True<WebSettings>();
            whereExp = whereExp.And(p => keys.Contains(p.ConfigKey));
            return CurrentDAL.GetDataListBy(whereExp);
        }
    }
}
