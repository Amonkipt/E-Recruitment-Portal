using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recruitment_Portal.Models
{
    public class Education
    {
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Education_Type { get; set; }
        public string Field_of_Study { get; set; }
        public string Qualification_Code { get; set; }
        public string Qualification_Name { get; set; }
        public string Institution_Name { get; set; }
        public string Proficiency_Level { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
    }
    public class ApplicationModel
    {
        public List<Education> Education { get; set; }
    }
}