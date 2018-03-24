﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.ViewModel
{
    public class StudentViewModel
    {
        public long Id { get; set; }
        public string SurName { get; set; }
        public string Sex { get; set; }
        public string Period { get; set; }
        public byte ExaminationLevel { get; set; }
        public string MajorName { get; set; }
        public string Nationality { get; set; }
        public string Birthday { get; set; }
        public string PoliticalStatus { get; set; }
        public byte EducationalLevel { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string TestFreeCondition { get; set; }
        public string Remark { get; set; }
        public string IdCardFrontPath { get; set; }
        public string IdCardBackPath { get; set; }
        public string BareheadedPhotoPath { get; set; }
        public bool Status { get; set; }
    }
}