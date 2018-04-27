using Lg.EducationPlatform.ViewModel;
using Lg.EducationPlatform.WebHelper;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lg.EducationPlatform.Web.Controllers
{
    public class WebSettingController : BaseController
    {
        private const string _open_start_date = "SYSTEM_OPEN_START_DATE";
        private const string _open_end_date = "SYSTEM_OPEN_END_DATE";
        private const string _current_period = "SYSTEM_CURRENT_PERIOD";
        private IWebSettingsService _webSettingsService = OperateHelper.Current._serviceSession.WebSettingsService;

        // GET: WebSetting
        public ActionResult OpenTime()
        {
            WebSettingViewModel model = new WebSettingViewModel();
            var period = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month <= 6)
                period = period + "春季";
            else
                period = period + "秋季";
            model.Period = period;
            
            List<string> keys = new List<string> { _open_start_date, _open_end_date, _current_period };
            List<WebSettings> list = _webSettingsService.GetListByKeys(keys).ToList();
            WebSettings startDateSetting = list.Where(p => p.ConfigKey == _open_start_date).FirstOrDefault();
            WebSettings endDateSetting = list.Where(p => p.ConfigKey == _open_end_date).FirstOrDefault();
            WebSettings periodSetting = list.Where(p => p.ConfigKey == _current_period).FirstOrDefault();
            if (startDateSetting != null)
            {
                model.OpenStartDate = startDateSetting.ConfigValue;
            }
            if(endDateSetting != null)
            {
                model.OpenEndDate = endDateSetting.ConfigValue;
            }
            if(periodSetting != null)
            {
                model.Period = periodSetting.ConfigValue;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult OpenTime(WebSettingViewModel model)
        {
            var property = "ConfigValue";
            WebSettings startDateSetting = _webSettingsService.GetWebSettingByKey(_open_start_date);            
            if (startDateSetting == null)
            {
                startDateSetting = new WebSettings();
                startDateSetting.ConfigKey = _open_start_date;
                startDateSetting.ConfigValue = model.OpenStartDate;
                startDateSetting.CreationTime = DateTime.Now;
                _webSettingsService.Add(startDateSetting, false);
            }
            else
            {
                startDateSetting.ConfigValue = model.OpenStartDate;
                startDateSetting.LastModificationTime = DateTime.Now;
                _webSettingsService.Update(startDateSetting, false, property);
            }

            WebSettings endDateSetting = _webSettingsService.GetWebSettingByKey(_open_end_date);
            if (endDateSetting == null)
            {
                endDateSetting = new WebSettings();
                endDateSetting.ConfigKey = _open_end_date;
                endDateSetting.ConfigValue = model.OpenEndDate;
                endDateSetting.CreationTime = DateTime.Now;
                _webSettingsService.Add(endDateSetting, false);
            }
            else
            {
                endDateSetting.ConfigValue = model.OpenEndDate;
                endDateSetting.LastModificationTime = DateTime.Now;
                _webSettingsService.Update(endDateSetting, false, property);
            }

            WebSettings periodSetting = _webSettingsService.GetWebSettingByKey(_current_period);
            if (periodSetting == null)
            {
                periodSetting = new WebSettings();
                periodSetting.ConfigKey = _current_period;
                periodSetting.ConfigValue = model.Period;
                periodSetting.CreationTime = DateTime.Now;
                _webSettingsService.Add(periodSetting, false);
            }
            else
            {
                periodSetting.ConfigValue = model.Period;
                periodSetting.LastModificationTime = DateTime.Now;
                _webSettingsService.Update(periodSetting, false, property);
            }

            int result = _webSettingsService.SaveChange();
            if (result > 0)
            {
                return Json(new
                {
                    Status = 1,
                    Message = "保存信息成功"
                });
            }
            else
            {
                return Json(new
                {
                    Status = 0,
                    Message = "保存信息失败"
                });
            }
        }
    }
}