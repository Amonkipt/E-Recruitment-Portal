using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recruitment_Portal.Models
{
    public class CV
    {
        [Key]
        public int id { get; set; }
        public string Path { get; set; }
        public string Applicant_No { get; set; }
        public string Job_No { get; set; }
        public string File_Name { get; set; }
        public string uploadname { get; set; }
    }
    public class UploadFile
    {
        public HttpPostedFileBase Attachment { get; set; }
    }
}