using E_Recruitment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace E_Recruitment_Portal
{
    public class Services
    {
        public static string GetUrl(string slug)
        {
            try
            {
                return GeneralSetup.Path1 + slug + "";
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.ToString());
                return "Link Unavailable";
            }
        }
        private static NetworkCredential credential = new NetworkCredential(GeneralSetup.soapUsername, GeneralSetup.soapPassword);
        public static OnlinePortal.OnlinePortal Portal()
        {
            OnlinePortal.OnlinePortal service = new OnlinePortal.OnlinePortal();
            service.Url = GetUrl("Codeunit/OnlinePortal");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            //service.Credentials = CredentialCache.DefaultCredentials;
            return service;
        }
        public static CompanyJobCard.CompanyJobCard_Service JobCard()
        {
            CompanyJobCard.CompanyJobCard_Service service = new CompanyJobCard.CompanyJobCard_Service();
            service.Url = GetUrl("Page/CompanyJobCard");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static Recruitment.Recruitment_Service Recruitment()
        {
            Recruitment.Recruitment_Service service = new Recruitment.Recruitment_Service();
            service.Url = GetUrl("Page/Recruitment");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static AcademicQualifications.AcademicQualifications_Service AcademicQualifications()
        {
            AcademicQualifications.AcademicQualifications_Service service = new AcademicQualifications.AcademicQualifications_Service();
            service.Url = GetUrl("Page/AcademicQualifications");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ProfMemberships.ProfMemberships_Service ProfMemberships()
        {
            ProfMemberships.ProfMemberships_Service service = new ProfMemberships.ProfMemberships_Service();
            service.Url = GetUrl("Page/ProfMemberships");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ProfMembership.ProfMembership_Service ProfMembership()
        {
            ProfMembership.ProfMembership_Service service = new ProfMembership.ProfMembership_Service();
            service.Url = GetUrl("Page/ProfMembership");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantJobProfessionalCourse.ApplicantJobProfessionalCourse_Service ApplicantJobProfessionalCourse()
        {
            ApplicantJobProfessionalCourse.ApplicantJobProfessionalCourse_Service service = new ApplicantJobProfessionalCourse.ApplicantJobProfessionalCourse_Service();
            service.Url = GetUrl("Page/ApplicantJobProfessionalCourse");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static CountriesRegions.CountriesRegions_Service CountriesRegions()
        {
            CountriesRegions.CountriesRegions_Service service = new CountriesRegions.CountriesRegions_Service();
            service.Url = GetUrl("Page/CountriesRegions");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static CompanyJobIndustries.CompanyJobIndustries_Service CompanyJobIndustries()
        {
            CompanyJobIndustries.CompanyJobIndustries_Service service = new CompanyJobIndustries.CompanyJobIndustries_Service();
            service.Url = GetUrl("Page/CompanyJobIndustries");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static FieldsofStudy.FieldsofStudy_Service FieldsofStudy()
        {
            FieldsofStudy.FieldsofStudy_Service service = new FieldsofStudy.FieldsofStudy_Service();
            service.Url = GetUrl("Page/FieldsofStudy");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static Qualifications.Qualifications_Service Qualifications()
        {
            Qualifications.Qualifications_Service service = new Qualifications.Qualifications_Service();
            service.Url = GetUrl("Page/Qualifications");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        //public static CountyList.CountyList_Service CountyList()
        //{
        //    CountyList.CountyList_Service service = new CountyList.CountyList_Service();
        //    service.Url = GetUrl("Page/CountyList");
        //    service.UseDefaultCredentials = false;
        //    service.Credentials = credential;
        //    return service;
        //}
        public static Applicantjobapplied.Applicantjobapplied_Service Applicantjobapplied()
        {
            Applicantjobapplied.Applicantjobapplied_Service service = new Applicantjobapplied.Applicantjobapplied_Service();
            service.Url = GetUrl("Page/Applicantjobapplied");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantCard.ApplicantCard_Service ApplicantCard()
        {
            ApplicantCard.ApplicantCard_Service service = new ApplicantCard.ApplicantCard_Service();
            service.Url = GetUrl("Page/ApplicantCard");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantJobEducation.ApplicantJobEducation_Service ApplicantJobEducation()
        {
            ApplicantJobEducation.ApplicantJobEducation_Service service = new ApplicantJobEducation.ApplicantJobEducation_Service();
            service.Url = GetUrl("Page/ApplicantJobEducation");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantHobbies.ApplicantHobbies_Service ApplicantHobbies()
        {
            ApplicantHobbies.ApplicantHobbies_Service service = new ApplicantHobbies.ApplicantHobbies_Service();
            service.Url = GetUrl("Page/ApplicantHobbies");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantJobprofessionalmembership.ApplicantJobprofessionalmembership_Service ApplicantJobprofessionalmembership()
        {
            ApplicantJobprofessionalmembership.ApplicantJobprofessionalmembership_Service service = new ApplicantJobprofessionalmembership.ApplicantJobprofessionalmembership_Service();
            service.Url = GetUrl("Page/ApplicantJobprofessionalmembership");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static Referees.Referees_Service Referees()
        {
            Referees.Referees_Service service = new Referees.Referees_Service();
            service.Url = GetUrl("Page/Referees");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static Jobsapplied.Jobsapplied_Service Jobsapplied()
        {
            Jobsapplied.Jobsapplied_Service service = new Jobsapplied.Jobsapplied_Service();
            service.Url = GetUrl("Page/Jobsapplied");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
        public static ApplicantJobExperience.ApplicantJobExperience_Service ApplicantJobExperience()
        {
            ApplicantJobExperience.ApplicantJobExperience_Service service = new ApplicantJobExperience.ApplicantJobExperience_Service();
            service.Url = GetUrl("Page/ApplicantJobExperience");
            service.UseDefaultCredentials = false;
            service.Credentials = credential;
            return service;
        }
    }
}