using System.Web;

namespace Lg.EducationPlatform.ViewModel
{
    public class StudentViewModel
    {
        public long Id { get; set; }
        public string SurName { get; set; }
        public byte Sex { get; set; }
        public string Period { get; set; }
        public byte ExaminationLevel { get; set; }
        public string ExaminationLevelStr { get; set; }
        public string MajorName { get; set; }
        public string Nationality { get; set; }
        public string IdCard { get; set; }
        public string PoliticalStatus { get; set; }
        public byte EducationalLevel { get; set; }
        public string EducationalLevelStr { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte Status { get; set; }
        public string TestFreeCondition { get; set; }
        public string Remark { get; set; }
        public string IdCardFrontPath { get; set; }
        public string IdCardBackPath { get; set; }
        public string BareheadedPhotoPath { get; set; }
        public HttpPostedFileBase BareheadedPhoto { get; set; }
        public HttpPostedFileBase IdCardFront { get; set; }
        public HttpPostedFileBase IdCardBack { get; set; }
    }
}
