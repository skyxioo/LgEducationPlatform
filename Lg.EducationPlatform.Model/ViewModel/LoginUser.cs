using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.ViewModel
{
    public class LoginUser
    {
        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(10, ErrorMessage = "用户名长度不能超过10个字符")]

        public string UserName { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(15, ErrorMessage = "密码长度不能超过15个字符")]
        //       [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", 
        //ErrorMessage = "请输入正确的Email格式\n示例：abc@123.com")]        
        public string PassWord { get; set; }
    }
}
