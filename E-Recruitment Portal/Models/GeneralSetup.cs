using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace E_Recruitment_Portal.Models
{
    public class GeneralSetup
    {
        public static string CompanyLogo = ConfigurationManager.AppSettings["CompanyLogo"];
        public static string CompanyName = ConfigurationManager.AppSettings["CompanyName"];
        public static string PortalName = ConfigurationManager.AppSettings["PortalName"];
        public static string Path = ConfigurationManager.AppSettings["Path1"];
        public static string UploadUrl = ConfigurationManager.AppSettings["UploadUrl"];
        public static string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
        public static string Path1 = ConfigurationManager.AppSettings["Path"];
        public static string FilePath = ConfigurationManager.AppSettings["FilePath"];
        public static string soapUsername = ConfigurationManager.AppSettings["SoapUsername"].ToString();
        public static string soapPassword = ConfigurationManager.AppSettings["SoapPassword"].ToString();

        public static string CompanyWebsite = ConfigurationManager.AppSettings["CompanyWebsite"].ToString();
        public static string MailDisplayName = ConfigurationManager.AppSettings["MailDisplayName"].ToString();
        public static string Regards = ConfigurationManager.AppSettings["Regards"].ToString();
        public static string MailUsername = ConfigurationManager.AppSettings["MailUsername"].ToString();
        public static string MailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
        public static string EmailLogo = ConfigurationManager.AppSettings["EmailLogo"].ToString();
        public static string MailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
        public static string MailPort = ConfigurationManager.AppSettings["MailPort"].ToString();
        public static string Error = "Oops! An error occurred while processing your request.";
    }
}