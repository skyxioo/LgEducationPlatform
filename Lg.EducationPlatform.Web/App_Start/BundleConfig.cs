using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Lg.EducationPlatform.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                        "~/Content/js/plugins/jquery-1.7.min.js",
                        "~/Content/js/plugins/jquery.cookie.js",
                        "~/Content/js/plugins/jquery.flot.min.js",
                        "~/Content/js/plugins/jquery.flot.resize.min.js",
                        "~/Content/js/plugins/jquery.slimscroll.js",
                        "~/Content/layer/layer.js"));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Content/js/plugins/jquery.form.js",
                        "~/Content/js/plugins/jquery.validate.min.js",
                        "~/Content/js/plugins/jquery.uniform.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                       "~/Content/js/plugins/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                       "~/Content/js/plugins/jquery.base64.js",
                       "~/Content/js/custom/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                        "~/Content/js/plugins/jquery-ui-1.8.16.custom.min.js",
                       "~/Content/js/plugins/moment.min.js",
                       "~/Content/js/plugins/jquery-ui-timepicker-addon.js",
                       "~/Content/js/plugins/jquery-ui-timepicker-zh-CN.js",
                       "~/Content/js/custom/calendar.zh-cn.js"));

            bundles.Add(new ScriptBundle("~/bundles/table").Include(
                       "~/Content/js/plugins/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                       "~/Content/js/custom/general.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/style.default.css",
                      "~/Content/font-awesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/table").Include(
                      "~/Content/css/style.datatable.css"));

            BundleTable.EnableOptimizations = false;  //是否打包压缩
        }
    }
}