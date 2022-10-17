using E_Recruitment_Portal.Models;
using NPOI.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace E_Recruitment_Portal.Controllers
{
    [Authorize]
    public class HomePageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllEmployees());
        }
        ApplicantCard.ApplicantCard GetAllEmployees()
        {
            var email = "";
            var hobby = ServicesDTO.GetProfile(email).FirstOrDefault();
            return hobby;
        }
        public ActionResult AddOrEdit(int id = 0)
        {
            var key = "";
            var email = "";
            if (email == null)
            {
                TempData["info"] = "Your session has expired. Please login and try again";
                return RedirectToAction("Login", "Account");
            }
            var profile = ServicesDTO.GetProfile(email).FirstOrDefault().Hobbies.Where(m => m.Key == key).FirstOrDefault();
            return View(profile);
        }
        [HttpPost]
        public ActionResult AddOrEdit(ApplicantHobbies.ApplicantHobbies model)
        {
            try
            {
                if (model.Key != null)
                {
                    Services.ApplicantHobbies().Update(ref model);
                    return Json("Your hobby has been added successfully");
                }
                else
                {
                    Random random = new Random();
                    model.No = "APP-000037";
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantHobbies().Create(ref model);
                    return Json(new { success = true, html = GlobalClass.RenderPartialViewToString(this, "ViewAll", GetAllEmployees()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return Json(new { success = false, message = GeneralSetup.Error }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStates(string uoption)
        {
            var ser = ServicesDTO.GetAcademicQualifications().Where(m => m.Education_Level.ToString() == uoption);

            string quote_no = string.Empty;
            IList<string> quoteno = new List<string>();
            foreach (var item in ser)
            {
                quote_no = item.Field_of_Study;
                quoteno.Add(quote_no);
            }
            var fos = quoteno.Distinct();

            return Json(fos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStates1(string uoption, string uoption1)
        {
            var ser = ServicesDTO.GetAcademicQualifications().Where(m => m.Field_of_Study == uoption && m.Education_Level.ToString() == uoption1);

            string quote_no = string.Empty;
            IList<string> quoteno = new List<string>();
            foreach (var item in ser)
            {
                quote_no = item.Description;
                quoteno.Add(quote_no);
            }
            var qc = quoteno;

            return Json(qc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var confirmprofile = ServicesDTO.GetProfile(email).Count();

                if (confirmprofile == 0 && email != null)
                {
                    var user = db.Users.Where(m => m.UserName == email).FirstOrDefault();
                    ApplicantCard.ApplicantCard model = new ApplicantCard.ApplicantCard();
                    model.First_Name = user.First_Name;
                    model.Middle_Name = user.Middle_Name;
                    model.Last_Name = user.Last_Name;
                    model.E_Mail = user.Email;
                    Services.ApplicantCard().Create(ref model);
                }

                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                var No = profile.No;
                var prof = ServicesDTO.GetApplicantProfile(No).FirstOrDefault();
                if (prof.ID_Number != null && prof.Date_Of_Birth != DateTime.MinValue)
                {
                    ViewBag.profile = 1;
                }
                else
                {
                    ViewBag.profile = 0;
                }
                ViewBag.education = ServicesDTO.GetEducation(No).Count();
                ViewBag.experience = ServicesDTO.GetExperience(No).Count();
                ViewBag.referees = ServicesDTO.GetReferees(No).Count();

                return View();
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Dashboard1()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public new ActionResult Profile(string id, string nc)
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.countries = new SelectList(ServicesDTO.GetCountries(), "Code", "Name");
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Education()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                ViewBag.fos = ServicesDTO.GetFieldOfStudy();
                //ViewBag.county = ServicesDTO.GetCounties();
                ViewBag.countries = ServicesDTO.GetCountries();

                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult ApplicantEducation(string jobid, string no)
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var confirmprofile = ServicesDTO.GetProfile(email).Count();

                if (confirmprofile == 0 && email != null)
                {
                    var user = db.Users.Where(m => m.UserName == email).FirstOrDefault();
                    ApplicantCard.ApplicantCard model = new ApplicantCard.ApplicantCard();
                    model.First_Name = user.First_Name;
                    model.Middle_Name = user.Middle_Name;
                    model.Last_Name = user.Last_Name;
                    model.E_Mail = user.Email;
                    Services.ApplicantCard().Create(ref model);
                }

                if (string.IsNullOrWhiteSpace(no) || string.IsNullOrWhiteSpace(jobid))
                {
                    return RedirectToAction("OpenApplications");
                }
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                ViewBag.fos = ServicesDTO.GetFieldOfStudy();
                //ViewBag.county = ServicesDTO.GetCounties();
                ViewBag.countries = ServicesDTO.GetCountries();
                ViewBag.countries11 = new SelectList(ServicesDTO.GetCountries(), "Code", "Name");
                ViewBag.cji = ServicesDTO.GetCompanyJobIndustries();
                ViewBag.pm = ServicesDTO.GetMemberships();
                var filter = AcademicQualifications.Education_Level.Professional.ToString();
                ViewBag.pc = ServicesDTO.GetProfessionalCourse(filter);
                ViewBag.model = ServicesDTO.GetJobDetails(jobid).FirstOrDefault();
                ViewBag.JobId = jobid;
                ViewBag.nos = no;

                ViewBag.uploadlist = db.CVs.Where(m => m.Applicant_No == profile.No && m.Job_No == no).ToList();

                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public JsonResult UploadFL()
        {
            // check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;

                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath("~/uploads/"));
                    string path = Path.Combine(Server.MapPath("~/uploads/"), fileName);

                    // save the file
                    file.SaveAs(path);
                    return Json("File uploaded successfully");
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }

            return Json("no files were selected !");
        }
        [HttpPost]
        public JsonResult UploadFiles(string applino, string jobno)
        {
            DefaultJsonResponse response = new DefaultJsonResponse();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string _FileName = Path.GetFileName(file.FileName);
                    var fileName = applino + "-" + Guid.NewGuid().ToString() + ".pdf";
                    var name = GeneralSetup.UploadUrl + fileName;
                    string filepath = @"" + GeneralSetup.UploadPath + fileName;
                    file.SaveAs(filepath);

                    CV models = new CV();

                    models.Path = name;
                    models.Applicant_No = applino;
                    models.Job_No = jobno;
                    models.File_Name = fileName;
                    models.uploadname = _FileName;
                    db.CVs.Add(models);
                    db.SaveChanges();

                    var applicantNo = models.Applicant_No;
                    var filePath = models.Path;
                    var jobNo = models.Job_No;
                    var fname = models.uploadname;
                    var id = models.id;

                    response.message = "File uploaded successfully";
                    response.status = 1;
                    response.data = new { applicantNo = applicantNo, filePath = filePath, jobNo = jobNo, fileName = fileName, Id = id, fname = fname };
                }
                else
                {
                    response.message = "You have not uploaded any file!";
                    response.status = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = 0;

                return Json(response);
            }
        }
        [HttpPost]
        public ActionResult RemoveCV(int Id)
        {
            DefaultJsonResponse response = new DefaultJsonResponse();
            try
            {
                var record = db.CVs.Find(Id);

                if (record != null)
                {
                    var filepath = @"" + GeneralSetup.UploadPath + record.File_Name;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }

                    db.CVs.Remove(record);
                    db.SaveChanges();

                    response.message = "File removed successfully";
                    response.status = 1;
                }
                else
                {
                    response.message = "File removed successfully";
                    response.status = 1;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                response.message = "File removed successfully";
                response.status = 1;

                return Json(response);
            }
        }
        public ActionResult WorkExperience()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                ViewBag.countries = ServicesDTO.GetCountries();
                ViewBag.cji = ServicesDTO.GetCompanyJobIndustries();
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult SubmittedApplications()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();

                var record = ServicesDTO.GetApplicantjobapplied(profile.No).Where(m=> m.Application_Date != DateTime.MinValue);
                return View(record);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Referees()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Hobbies()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Professional()
        {
            try
            {
                var filter = AcademicQualifications.Education_Level.Professional.ToString();
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                ViewBag.pc = ServicesDTO.GetProfessionalCourse(filter);
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult Membership()
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();
                ViewBag.pm = ServicesDTO.GetMembership();
                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult OpenApplications()
        {
            try
            {
                var applications1 = ServicesDTO.GetJob().Where(m => m.Submitted_To_Portal == true && m.Shortlisting_Started == false).ToList();
                var applications2 = ServicesDTO.GetJob().Where(m => m.Submitted_To_Portal == true ).ToList();
                var applications3 = ServicesDTO.GetJob().Where(m =>  m.Shortlisting_Started == false).ToList();
                var applications = ServicesDTO.GetJob().Where(m => m.End_Date >= DateTime.Today && m.Submitted_To_Portal == true && m.End_Date != DateTime.MinValue && m.Shortlisting_Started == false).ToList();

                return View(applications);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        public ActionResult ApplicationDetails(string jobid, string no)
        {
            try
            {
                var email = ServicesDTO.GetCurrentUser().Email;
                if (email == null)
                {
                    TempData["info"] = "Your session has expired. Please login and try again";
                    return RedirectToAction("Login", "Account");
                }
                var profile = ServicesDTO.GetProfile(email).Count();
                if (profile != 0)
                {
                    var profiles = ServicesDTO.GetProfile(email).FirstOrDefault();
                    ViewBag.No = profiles.No;
                }

                var model = ServicesDTO.GetJobDetails(jobid).FirstOrDefault();
                if (no != null)
                {
                    ViewBag.JobId = jobid;
                    ViewBag.nos = no;
                    ViewBag.OpenJob = ServicesDTO.GetOpenJobDetails(no);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult ManageProfile(ApplicantCard.ApplicantCard model, string AppNo, string jobid, string no)
        {
            try
            {
                DateTime dob = Convert.ToDateTime(model.Date_Of_Birth);
                string text = CalculateYourAge(dob);
                int age = CalculateAge(dob);
                if (age < 18)
                {
                    TempData["failed"] = "You age is below 18 years - " + text;
                    return RedirectToAction("ApplicantEducation", new { jobid = jobid, no = no });
                }

                model.No = AppNo;
                if (!String.IsNullOrEmpty(model.No))
                {
                    //Update
                    model.Key = Services.ApplicantCard().Read(model.No).Key;

                    model.GenderSpecified = true;
                    model.Date_Of_BirthSpecified = true;
                    model.DisabledSpecified = true;
                    model.Marital_StatusSpecified = true;
                    Services.ApplicantCard().Update(ref model);
                    //DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    //{
                    //    status = 1,
                    //    message = "Your profile has been updated successfully"
                    //};
                    //return Json(defaultJsonResponse);
                    TempData["success"] = "Your profile has been updated successfully";
                    return RedirectToAction("ApplicantEducation", new { jobid = jobid, no = no });
                }
                else
                {
                    //Create 
                    if (model.Gender != 0)
                    {
                        model.GenderSpecified = true;
                    }
                    if (model.Marital_Status != 0)
                    {
                        model.Marital_StatusSpecified = true;
                    }
                    if (model.Date_Of_Birth != DateTime.MinValue)
                    {
                        model.Date_Of_BirthSpecified = true;
                    }
                    model.Date_Of_BirthSpecified = true;
                    if (model.Disabled != 0)
                    {
                        model.DisabledSpecified = true;
                    }
                    model.Application_Date = DateTime.Now;
                    model.Application_DateSpecified = true;
                    Services.ApplicantCard().Create(ref model);

                    TempData["success"] = "Your profile has been created successfully";
                    return RedirectToAction("ApplicantEducation", new { jobid = jobid, no = no });
                }

            }
            catch (Exception e)
            {
                TempData["failed"] = e.Message;
                return RedirectToAction("ApplicantEducation", new { jobid = jobid, no = no });
            }
        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
        public string CalculateYourAge(DateTime dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
            DateTime PastYearDate = dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            Years, Months, Days, Hours, Seconds);
        }
        [HttpPost]
        public ActionResult ManageEducation(ApplicantJobEducation.ApplicantJobEducation model)
        {
            try
            {
                DateTime startdate = Convert.ToDateTime(model.Start_Date);
                DateTime enddate = Convert.ToDateTime(model.End_Date);
                if ( enddate > DateTime.Now)
                {
                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 0,
                        message = "End date cannot be a futute date!"
                    };

                    return Json(defaultJsonResponse);
                }
                if (startdate > enddate)
                {
                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 0,
                        message = "Start date cannot be greater than end date!"
                    };

                    return Json(defaultJsonResponse);
                }
                if (model.Key != null)
                {
                    if (model.Start_Date != DateTime.MinValue)
                    {
                        model.Start_DateSpecified = true;
                    }
                    if (model.End_Date != DateTime.MinValue)
                    {
                        model.End_DateSpecified = true;
                    }
                    if (model.Education_Type != 0)
                    {
                        model.Education_TypeSpecified = true;
                    }
                    if (model.Education_Level != 0)
                    {
                        model.Education_LevelSpecified = true;
                    }
                    if (model.Proficiency_Level != 0)
                    {
                        model.Proficiency_LevelSpecified = true;
                    }
                    Services.ApplicantJobEducation().Update(ref model);
                    return RedirectToAction("Education", "HomePage");
                }
                else
                {
                    //var Field_of_Study = ServicesDTO.GetFieldOfStudy().Where(m => m.Description == model.Field_of_Study).FirstOrDefault();
                    //model.Field_of_Study = Field_of_Study.Code;
                    if (!string.IsNullOrWhiteSpace(model.Qualification_Code))
                    {
                        var resc = model.Description;
                        //var userss1 = servicesdto.getacademicqualifications();
                        var userss10 = ServicesDTO.GetAcademicQualifications10(model.Description, model.Education_Level.ToString()).Where(m => m.Description == model.Description);
                        var userss11 = ServicesDTO.GetAcademicQualifications11(resc);
                        var course = ServicesDTO.GetAcademicQualifications().Where(m => m.Education_Level.ToString() == model.Education_Level.ToString() && m.Description == model.Qualification_Code.ToString()).FirstOrDefault();
                        //var course = servicesdto.getacademicqualifications().where(m => m.description == model.qualification_code /*&& m.education_level == model.education_level*/).firstordefault();
                        model.Qualification_Code = course.Code;
                    }
                    if (model.End_Date >= DateTime.Now)
                    {
                        return Json(new { success = 1, message = "End date cannot be greater than today!" });
                    }
                    if (model.Start_Date != DateTime.MinValue)
                    {
                        model.Start_DateSpecified = true;
                    }
                    if (model.End_Date != DateTime.MinValue)
                    {
                        model.End_DateSpecified = true;
                    }
                    if (model.Education_Type != 0)
                    {
                        model.Education_TypeSpecified = true;
                    }
                    if (model.Education_Level != 0)
                    {
                        model.Education_LevelSpecified = true;
                    }
                    if (model.Proficiency_Level != 0)
                    {
                        model.Proficiency_LevelSpecified = true;
                    }
                    Random random = new Random();
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantJobEducation().Create(ref model);

                    var newObj = new
                    {
                        start_Date = model.Start_Date.ToString("dd/MM/yyyy"),
                        end_Date = model.End_Date.ToString("dd/MM/yyyy"),
                        education_Type = model.Education_Type,
                        field_of_Study = model.Field_of_Study,
                        qualification_Code = model.Qualification_Code,
                        qualification_Name = model.Qualification_Name,
                        institution_Name = model.Institution_Name,
                        proficiency_Level = model.Proficiency_Level,
                        country = model.Country,
                        region = model.Region,
                        description = model.Description,
                        grade = model.Grade,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Your Education has been added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        [HttpPost]
        public ActionResult ManageExperience(ApplicantJobExperience.ApplicantJobExperience model)
        {
            try
            {
                DateTime startdate = Convert.ToDateTime(model.Start_Date);
                DateTime enddate = Convert.ToDateTime(model.End_Date);
                if (DateTime.Now.DayOfYear < enddate.DayOfYear)
                {
                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 0,
                        message = "End date cannot be a futute date!"
                    };

                    return Json(defaultJsonResponse);
                }
                if (startdate > enddate)
                {
                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 0,
                        message = "Start date cannot be greater than end date!"
                    };

                    return Json(defaultJsonResponse);
                }
                int years = CalculateYears(startdate, enddate);
                model.No_of_Years = years;

                if (model.Key != null)
                {
                    if (model.Start_Date != DateTime.MinValue)
                    {
                        model.Start_DateSpecified = true;
                    }
                    if (model.End_Date != DateTime.MinValue)
                    {
                        model.End_DateSpecified = true;
                    }
                    if (model.Hierarchy_Level != 0)
                    {
                        model.Hierarchy_LevelSpecified = true;
                    }
                    if (model.No_of_Years != 0)
                    {
                        model.No_of_YearsSpecified = true;
                    }
                    Services.ApplicantJobExperience().Update(ref model);
                    return RedirectToAction("WorkExperience", "HomePage");
                }
                else
                {
                    if (model.Start_Date != DateTime.MinValue)
                    {
                        model.Start_DateSpecified = true;
                    }
                    if (model.End_Date >= DateTime.Now)
                    {
                        return Json(new { success = 1, message = "End date cannot be greater than today!" });
                    }
                    if (model.End_Date != DateTime.MinValue)
                    {
                        model.End_DateSpecified = true;
                    }
                    if (model.Hierarchy_Level != 0)
                    {
                        model.Hierarchy_LevelSpecified = true;
                    }
                    if (model.No_of_Years != 0)
                    {
                        model.No_of_YearsSpecified = true;
                    }

                    Random random = new Random();
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantJobExperience().Create(ref model);

                    var newObj = new
                    {
                        start_Date = model.Start_Date.ToString("dd/MM/yyyy"),
                        end_Date = model.End_Date.ToString("dd/MM/yyyy"),
                        employer = model.Employer,
                        industry = model.Industry,
                        hierarchy_Level = model.Hierarchy_Level,
                        functional_Area = model.Functional_Area,
                        job_Title = model.Job_Title,
                        no_of_Years = model.No_of_Years,
                        country = model.Country,
                        description = model.Description,
                        location = model.Location,
                        employer_Email_Address = model.Employer_Email_Address,
                        employer_Postal_Address = model.Employer_Postal_Address,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Your work experience has been added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        private static int CalculateYears(DateTime startdate, DateTime enddate)
        {
            int years = 0;
            years = enddate.Year - startdate.Year;
            if (DateTime.Now.DayOfYear < startdate.DayOfYear)
                years = years - 1;

            return years;
        }
        [HttpPost]
        public ActionResult SubmitApplication(string jobId, string applicationNo)
        {
            DefaultJsonResponse response = new DefaultJsonResponse();
            try
            {
                Applicantjobapplied.Applicantjobapplied model = new Applicantjobapplied.Applicantjobapplied();

                model.Application_No = applicationNo;
                model.Need_Code = jobId;

                Services.Applicantjobapplied().Create(ref model);
                Services.Portal().SubmitApplication(model.No);

                CV cV = new CV();

                var uploads = db.CVs.Where(m => m.Applicant_No == applicationNo && m.Job_No == jobId).ToList();

                //foreach (CV item in uploads)
                //{
                //    var upload = Services.Portal().UploadApplicantDocument(applicationNo, item.Path, "Applicant");
                //}

                response.status = 1;
                TempData["success"] = "Your job application has been submitted successfully. Thank you for taking your time to apply for this job.";

                return Json(response);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                response.status = 0;
                response.message = "You have already applied for this job!";

                return Json(response);
            }
        }
        [HttpPost]
        public ActionResult ManageReferees(Referees.Referees model)
        {
            try
            {
                if (model.Key != null)
                {
                    Services.Referees().Update(ref model);
                    return Json("Your referee has been updated successfully");
                }
                else
                {
                    //Random random = new Random();
                    //model.Line_No = random.Next(1000, 100000);
                    //model.Line_NoSpecified = true;
                    Services.Referees().Create(ref model);

                    var newObj = new
                    {
                        names = model.Names,
                        designation = model.Designation,
                        company = model.Company,
                        address = model.Address,
                        tel = model.Telephone_No,
                        email = model.E_Mail,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Referee added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult EducationLevel(string id)
        {
            try
            {
                var fos = ServicesDTO.GetAcademicQualifications1(id);
                var fos1 = ServicesDTO.GetAcademicQualifications();
                var ser = fos1.Where(m => m.Education_Level.ToString() == id);

                string quote_no = string.Empty;
                IList<string> quoteno = new List<string>();
                foreach (var item in fos)
                {
                    quote_no = item.Field_of_Study.ToString();
                    quoteno.Add(quote_no);
                }
                var code = quoteno;

                var fields = ServicesDTO.GetFieldOfStudy();

                HashSet<string> strset = new HashSet<string>(code);
                bool strsetContainsX8 = strset.Contains("X");

                var fiedOS = fields.Where(u => strset.Contains(u.Code)).Count();
                ViewBag.fosw = fields.Where(u => strset.Contains(u.Code));

                ViewBag.fos3 = new SelectList(fields.Where(u => strset.Contains(u.Code)).ToList(), "Code", "Description");


                TempData["fos"] = ServicesDTO.GetFieldOfStudy();
                ViewBag.qc = ServicesDTO.GetQualificationCode();
                //ViewBag.county = ServicesDTO.GetCounties();
                ViewBag.countries = ServicesDTO.GetCountries();
                var email = ServicesDTO.GetCurrentUser().Email;
                var profile = ServicesDTO.GetProfile(email).FirstOrDefault();

                return View("Education", profile);
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return View("Education");
            }
        }
        [HttpPost]
        public ActionResult ManageHobbies(ApplicantHobbies.ApplicantHobbies model)
        {
            try
            {
                if (model.Key != null)
                {
                    Services.ApplicantHobbies().Update(ref model);
                    return Json("Your hobby has been added successfully");
                }
                else
                {
                    Random random = new Random();
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantHobbies().Create(ref model);

                    var newObj = new
                    {
                        hobbies = model.Hobbies,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Your hobby has been added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        [HttpPost]
        public ActionResult ManageProfessional(ApplicantJobProfessionalCourse.ApplicantJobProfessionalCourse model)
        {
            try
            {
                if (model.Key != null)
                {
                    if (model.Section_Level != 0)
                    {
                        model.Section_LevelSpecified = true;
                    }
                    Services.ApplicantJobProfessionalCourse().Update(ref model);
                    return RedirectToAction("Professional", "HomePage");
                }
                else
                {
                    if (model.Section_Level != 0)
                    {
                        model.Section_LevelSpecified = true;
                    }
                    Random random = new Random();
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantJobProfessionalCourse().Create(ref model);
                    TempData["success"] = "";

                    var newObj = new
                    {
                        qualification_Code = model.Qualification_Code,
                        qualification_Name = model.Qualification_Name,
                        section_Level = model.Section_Level,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Your professional course has been added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                //TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        [HttpPost]
        public ActionResult ManageMembership(ApplicantJobprofessionalmembership.ApplicantJobprofessionalmembership model)
        {
            try
            {
                if (model.Key != null)
                {
                    Services.ApplicantJobprofessionalmembership().Update(ref model);
                    return RedirectToAction("Membership", "HomePage");
                }
                else
                {
                    Random random = new Random();
                    model.Line_No = random.Next(1000, 10000);
                    model.Line_NoSpecified = true;
                    Services.ApplicantJobprofessionalmembership().Create(ref model);

                    var newObj = new
                    {
                        professional_Body = model.Professional_Body,
                        description = model.Description,
                        membershipNo = model.MembershipNo,
                        key = model.Key
                    };

                    DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                    {
                        status = 1,
                        message = "Membership added successfully",
                        data = newObj
                    };

                    return Json(defaultJsonResponse);
                }
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult RemoveEducation(string key)
        {
            try
            {
                Services.ApplicantJobEducation().Delete(key);

                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your education has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult Uploads(HttpPostedFileBase file)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Uploades(HttpPostedFileBase file)
        {
            try
            {
                var path = GeneralSetup.UploadUrl + "Stabex Loyalty Portal (5)";

                var upload = Services.Portal().UploadRFQDocument("SHAR0026", "QT-00007", path);


                //var recId = "APP-000048";
                //var InputFileName = Path.GetFileName(file.FileName);
                //var fileName = Path.GetFileName(file.FileName);
                //var name = GeneralSetup.UploadUrl + fileName;
                //string filepath = @"" + GeneralSetup.UploadPath + fileName;
                //file.SaveAs(filepath);

                //var upload = Services.Portal().UploadApplicantDocument(recId, name, "");
                //var uploads = Services.Portal().UploadApplicantDocument(recId, name, "");

                //string theFileName = Path.GetFileName(file.FileName);
                //byte[] bytes = new byte[file.ContentLength];
                //using (BinaryReader theReader = new BinaryReader(file.InputStream))
                //{
                //    bytes = theReader.ReadBytes(file.ContentLength);
                //}
                //string base64 = Convert.ToBase64String(bytes);
                ////var recId = "APP-000040";              
                //Services.Portal().AttachDocument(recId.ToString(), base64);










                ////string userName = "user@Tenant.onmicrosoft.com";
                //string userName = "erp@agilebiz.co.ke";
                //string Password = "Pass@7046.";
                //var securePassword = new SecureString();
                //foreach (char c in Password)
                //{
                //    securePassword.AppendChar(c);
                //}
                //using (var ctx = new ClientContext("https://sharepointdemo.agilebiz.co.ke/Shared%20Documents/test"))
                //{
                //    ctx.Credentials = new Microsoft.SharePoint.Client.SharePointOnlineCredentials(userName, securePassword);
                //    Web web = ctx.Web;
                //    ctx.Load(web);
                //    ctx.ExecuteQuery();
                //    List byTitle = ctx.Web.Lists.GetByTitle("LibraryName");

                //    // New object of "ListItemCreationInformation" class
                //    ListItemCreationInformation listItemCreationInformation = new ListItemCreationInformation();

                //    // Below are options.
                //    // (1) File - This will create a file in the list or document library
                //    // (2) Folder - This will create a foder in list(if folder creation is enabled) or documnt library
                //    listItemCreationInformation.UnderlyingObjectType = FileSystemObjectType.Folder;

                //    // This will et the internal name/path of the file/folder
                //    listItemCreationInformation.LeafName = "RecruitmentPortalUploads";

                //    ListItem listItem = byTitle.AddItem(listItemCreationInformation);

                //    // Set folder Name
                //    listItem["Title"] = "RecruitmentPortalUploads";

                //    listItem.Update();
                //    ctx.ExecuteQuery();
                //}


                ////string userName = "user@Tenant.onmicrosoft.com";
                ////string Password = "*******";
                ////var securePassword = new SecureString();
                //foreach (char c in Password)
                //{
                //    securePassword.AppendChar(c);
                //}
                //using (var ctx = new ClientContext("https://sharepointdemo.agilebiz.co.ke"))
                //{
                //    ctx.Credentials = new Microsoft.SharePoint.Client.SharePointOnlineCredentials(userName, securePassword);
                //    Web web = ctx.Web;
                //    ctx.Load(web);
                //    ctx.ExecuteQuery();
                //    FileCreationInformation newFile = new FileCreationInformation();
                //    newFile.Content = System.IO.File.ReadAllBytes("D:\\std\\Test.pdf");
                //    newFile.Url = @"document.pdf";
                //    List byTitle = ctx.Web.Lists.GetByTitle("Documents");
                //    Folder folder = byTitle.RootFolder.Folders.GetByUrl("RecruitmentPortalUploads");
                //    ctx.Load(folder);
                //    ctx.ExecuteQuery();
                //    Microsoft.SharePoint.Client.File uploadFile = folder.Files.Add(newFile);
                //    uploadFile.CheckIn("checkin", CheckinType.MajorCheckIn);
                //    ctx.Load(byTitle);
                //    ctx.Load(uploadFile);
                //    ctx.ExecuteQuery();
                //    Console.WriteLine("done");
                //}





                ////var InputFileName = Path.GetFileName(file.FileName);
                ////var fileName = Path.GetFileName(model.Need_Code + "_" + model.Application_No + "_" + file.FileName);
                ////var filepath = Path.Combine(Server.MapPath("https://sharepointdemo.agilebiz.co.ke/Shared%20Documents/test"), fileName);


                ////Variablen für die Verarbeitung
                //string source_file = @"D:\std\Test.pdf";
                ////string web_url = "https://sharepointdemo.agilebiz.co.ke/Shared%20Documents/test";
                //string web_url = "https://sharepointdemo.agilebiz.co.ke/Shared%20Documents/Forms/AllItems.aspx";
                //string library_name = "Documents";
                //string admin_name = "erp";
                //string admin_password = "Pass@7046.";

                ////Verbindung mit den Login-Daten herstellen
                ////var sercured_password = new SecureString();
                ////foreach (var c in admin_password) sercured_password.AppendChar(c);
                ////SharePointOnlineCredentials credent = new
                ////SharePointOnlineCredentials(admin_name, sercured_password);

                //NetworkCredential credent = new NetworkCredential(admin_name, admin_password, "");

                ////Context mit Credentials erstellen
                //ClientContext context = new ClientContext(web_url);
                //context.Credentials = credent;

                ////Bibliothek festlegen
                //var library = context.Web.Lists.GetByTitle(library_name);

                ////Ausgewählte Datei laden
                //FileStream fs = System.IO.File.OpenRead(source_file);

                ////Dateinamen aus Pfad ermitteln
                //string source_filename = Path.GetFileName(source_file);

                ////Datei ins SharePoint-Verzeichnis hochladen
                //FileCreationInformation fci = new FileCreationInformation();
                //fci.Overwrite = true;
                //fci.ContentStream = fs;
                //fci.Url = source_filename;
                //var file_upload = library.RootFolder.Files.Add(fci);

                ////Ausführen
                //context.Load(file_upload);
                //context.ExecuteQuery();

                ////Datenübertragen schließen
                //fs.Close();
                return View();
            }
            catch (Exception ex)
            {
                TempData["failed"] = GeneralSetup.Error; var error = ex.Message;
                return RedirectToAction("Uploads");
            }
        }
        public ActionResult RemoveMembership(string key)
        {
            try
            {
                Services.ApplicantJobprofessionalmembership().Delete(key);
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your membership has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult RemoveHobby(string key)
        {
            try
            {
                Services.ApplicantHobbies().Delete(key);
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your hobby has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult RemoveProfessional(string key)
        {
            try
            {
                Services.ApplicantJobProfessionalCourse().Delete(key);
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your professional course has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult RemoveReferee(string key)
        {
            try
            {
                Services.Referees().Delete(key);
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your referee has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
        public ActionResult RemoveWorkExperience(string key)
        {
            try
            {
                Services.ApplicantJobExperience().Delete(key);

                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 1,
                    message = "Your job experience has been removed successfully"
                };

                return Json(defaultJsonResponse);
            }
            catch (Exception ex)
            {
                DefaultJsonResponse defaultJsonResponse = new DefaultJsonResponse
                {
                    status = 0,
                    message = ex.Message
                };

                return Json(defaultJsonResponse);
            }
        }
    }
}