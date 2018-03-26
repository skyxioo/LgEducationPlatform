using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.Model
{
    /// <summary>
    /// 定制Json统一格式
    /// </summary>
    public class JsonModel
    {
        public object CoreData { get; set; }  //核心数据
        public int Status { get; set; }       //状态码 1
        public string Message { get; set; }   //状态信息
        public string GotoUrl { get; set; }   //请求地址
        public string PageNavStr { get; set; }//分页html数据
    }
}
