using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace E_Recruitment_Portal.Models
{
    public class ServicesDTO
    {
        public static ApplicationUser GetCurrentUser()
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Where(x => x.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                return user;
            }
        }
        public static async Task<string> GetEmailTemplate(string template)
        {
            var templateFilePath = HostingEnvironment.MapPath("~/Content/Templates/") + template + ".cshtml";
            StreamReader objstreamreaderfile = new StreamReader(templateFilePath);
            var body = await objstreamreaderfile.ReadToEndAsync();
            objstreamreaderfile.Close();
            return body;
        }
        public static List<Recruitment.Recruitment> GetJob()
        {
            return Services.Recruitment().ReadMultiple(null, null, 0).ToList();
        }
        public static List<CountriesRegions.CountriesRegions> GetCountries()
        {
            return Services.CountriesRegions().ReadMultiple(null, null, 0).ToList();
        }
        public static List<CompanyJobIndustries.CompanyJobIndustries> GetCompanyJobIndustries()
        {
            return Services.CompanyJobIndustries().ReadMultiple(null, null, 0).ToList();
        }
        public static List<ProfMembership.ProfMembership> GetMemberships()
        {
            return Services.ProfMembership().ReadMultiple(null, null, 0).ToList();
        }
        public static List<ProfMemberships.ProfMemberships> GetMembership()
        {
            return Services.ProfMemberships().ReadMultiple(null, null, 0).ToList();
        }
        public static List<FieldsofStudy.FieldsofStudy> GetFieldOfStudy()
        {
            return Services.FieldsofStudy().ReadMultiple(null, null, 0).ToList();
        }
        public static List<Qualifications.Qualifications> GetQualificationCode()
        {
            return Services.Qualifications().ReadMultiple(null, null, 0).ToList();
        }
        public static List<AcademicQualifications.AcademicQualifications> GetAcademicQualifications()
        {
            return Services.AcademicQualifications().ReadMultiple(null, null, 0).ToList();
        }
        //public static List<CountyList.CountyList> GetCounties()
        //{
        //    return Services.CountyList().ReadMultiple(null, null, 0).ToList();
        //}
        public static List<AcademicQualifications.AcademicQualifications> GetAcademicQualifications11(string desc)
        {
            List<AcademicQualifications.AcademicQualifications_Filter> filters = new List<AcademicQualifications.AcademicQualifications_Filter>();
            AcademicQualifications.AcademicQualifications_Filter filter = new AcademicQualifications.AcademicQualifications_Filter();
            filter.Field = AcademicQualifications.AcademicQualifications_Fields.Description;
            filter.Criteria = desc;
            filters.Add(filter);
            var res = Services.AcademicQualifications().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<AcademicQualifications.AcademicQualifications> GetAcademicQualifications10(string desc, string level)
        {
            List<AcademicQualifications.AcademicQualifications_Filter> filters = new List<AcademicQualifications.AcademicQualifications_Filter>();
            AcademicQualifications.AcademicQualifications_Filter filter = new AcademicQualifications.AcademicQualifications_Filter();
            filter.Field = AcademicQualifications.AcademicQualifications_Fields.Description;
            filter.Criteria = desc;
            filters.Add(filter);
            AcademicQualifications.AcademicQualifications_Filter filter1 = new AcademicQualifications.AcademicQualifications_Filter();
            filter1.Field = AcademicQualifications.AcademicQualifications_Fields.Education_Level;
            filter1.Criteria = level;
            filters.Add(filter1);
            var res = Services.AcademicQualifications().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<CompanyJobCard.CompanyJobCard> GetJobDetails(string jobid)
        {
            List<CompanyJobCard.CompanyJobCard_Filter> filters = new List<CompanyJobCard.CompanyJobCard_Filter>();
            CompanyJobCard.CompanyJobCard_Filter filter = new CompanyJobCard.CompanyJobCard_Filter();
            filter.Field = CompanyJobCard.CompanyJobCard_Fields.Job_ID;
            filter.Criteria = jobid;
            filters.Add(filter);
            var res = Services.JobCard().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<AcademicQualifications.AcademicQualifications> GetAcademicQualifications1(string jobid)
        {
            List<AcademicQualifications.AcademicQualifications_Filter> filters = new List<AcademicQualifications.AcademicQualifications_Filter>();
            AcademicQualifications.AcademicQualifications_Filter filter = new AcademicQualifications.AcademicQualifications_Filter();
            filter.Field = AcademicQualifications.AcademicQualifications_Fields.Education_Level;
            filter.Criteria = jobid;
            filters.Add(filter);
            var res = Services.AcademicQualifications().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<AcademicQualifications.AcademicQualifications> GetProfessionalCourse(string edutype)
        {
            List<AcademicQualifications.AcademicQualifications_Filter> filters = new List<AcademicQualifications.AcademicQualifications_Filter>();
            AcademicQualifications.AcademicQualifications_Filter filter = new AcademicQualifications.AcademicQualifications_Filter();
            filter.Field = AcademicQualifications.AcademicQualifications_Fields.Education_Level;
            filter.Criteria = edutype;
            filters.Add(filter);
            var res = Services.AcademicQualifications().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<Recruitment.Recruitment> GetOpenJobDetails(string no)
        {
            List<Recruitment.Recruitment_Filter> filters = new List<Recruitment.Recruitment_Filter>();
            Recruitment.Recruitment_Filter filter = new Recruitment.Recruitment_Filter();
            filter.Field = Recruitment.Recruitment_Fields.No;
            filter.Criteria = no;
            filters.Add(filter);
            var res = Services.Recruitment().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<Applicantjobapplied.Applicantjobapplied> GetApplicantjobapplied(string no)
        {
            List<Applicantjobapplied.Applicantjobapplied_Filter> filters = new List<Applicantjobapplied.Applicantjobapplied_Filter>();
            Applicantjobapplied.Applicantjobapplied_Filter filter = new Applicantjobapplied.Applicantjobapplied_Filter();
            filter.Field = Applicantjobapplied.Applicantjobapplied_Fields.Application_No;
            filter.Criteria = no;
            filters.Add(filter);
            var res = Services.Applicantjobapplied().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<ApplicantCard.ApplicantCard> GetProfile(string email)
        {
            List<ApplicantCard.ApplicantCard_Filter> filters = new List<ApplicantCard.ApplicantCard_Filter>();
            ApplicantCard.ApplicantCard_Filter filter = new ApplicantCard.ApplicantCard_Filter();
            filter.Field = ApplicantCard.ApplicantCard_Fields.E_Mail;
            filter.Criteria = email;
            filters.Add(filter);
            var res = Services.ApplicantCard().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<ApplicantCard.ApplicantCard> GetApplicantProfile(string No)
        {
            List<ApplicantCard.ApplicantCard_Filter> filters = new List<ApplicantCard.ApplicantCard_Filter>();
            ApplicantCard.ApplicantCard_Filter filter = new ApplicantCard.ApplicantCard_Filter();
            filter.Field = ApplicantCard.ApplicantCard_Fields.No;
            filter.Criteria = No;
            filters.Add(filter);
            var res = Services.ApplicantCard().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<ApplicantJobEducation.ApplicantJobEducation> GetEducation(string No)
        {
            List<ApplicantJobEducation.ApplicantJobEducation_Filter> filters = new List<ApplicantJobEducation.ApplicantJobEducation_Filter>();
            ApplicantJobEducation.ApplicantJobEducation_Filter filter = new ApplicantJobEducation.ApplicantJobEducation_Filter();
            filter.Field = ApplicantJobEducation.ApplicantJobEducation_Fields.Applicant_No;
            filter.Criteria = No;
            filters.Add(filter);
            var res = Services.ApplicantJobEducation().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<ApplicantJobExperience.ApplicantJobExperience> GetExperience(string No)
        {
            List<ApplicantJobExperience.ApplicantJobExperience_Filter> filters = new List<ApplicantJobExperience.ApplicantJobExperience_Filter>();
            ApplicantJobExperience.ApplicantJobExperience_Filter filter = new ApplicantJobExperience.ApplicantJobExperience_Filter();
            filter.Field = ApplicantJobExperience.ApplicantJobExperience_Fields.Applicant_No;
            filter.Criteria = No;
            filters.Add(filter);
            var res = Services.ApplicantJobExperience().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<Referees.Referees> GetReferees(string No)
        {
            List<Referees.Referees_Filter> filters = new List<Referees.Referees_Filter>();
            Referees.Referees_Filter filter = new Referees.Referees_Filter();
            filter.Field = Referees.Referees_Fields.No;
            filter.Criteria = No;
            filters.Add(filter);
            var res = Services.Referees().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<Recruitment.Recruitment> GetLatestJob(string startdate)
        {
            List<Recruitment.Recruitment_Filter> filters = new List<Recruitment.Recruitment_Filter>();
            Recruitment.Recruitment_Filter filter = new Recruitment.Recruitment_Filter();
            filter.Field = Recruitment.Recruitment_Fields.Start_Date;
            filter.Criteria = startdate;
            filters.Add(filter);
            var res = Services.Recruitment().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
        public static List<CompanyJobCard.CompanyJobCard> CheckApplicantNumber(string jobNo)
        {
            List<CompanyJobCard.CompanyJobCard_Filter> filters = new List<CompanyJobCard.CompanyJobCard_Filter>();
            CompanyJobCard.CompanyJobCard_Filter filter = new CompanyJobCard.CompanyJobCard_Filter();
            filter.Field = CompanyJobCard.CompanyJobCard_Fields.Job_ID;
            filter.Criteria = jobNo;
            filters.Add(filter);
            var res = Services.JobCard().ReadMultiple(filters.ToArray(), null, 0).ToList();
            return res;
        }
    }
}