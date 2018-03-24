using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.ViewModel
{
    public class StudentOutput
    {
        public long Id { get; set; }
        public string SurName { get; set; }
        public byte Sex { get; set; }
        public string Period { get; set; }
        public byte ExaminationLevel { get; set; }
        public string MajorName { get; set; }
        public string Nationality { get; set; }
        public byte EducationalLevel { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Status { get; set; }
    }
}
