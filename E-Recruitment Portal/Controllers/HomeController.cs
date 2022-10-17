using E_Recruitment_Portal.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Recruitment_Portal.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var appl = from s in ServicesDTO.GetJob().Where(m=> m.End_Date != DateTime.MinValue && m.Submitted_To_Portal == true && m.Shortlisting_Started == false)
                               select s;
            var applications1 = appl.Where(m => m.End_Date <= DateTime.Today );
            var applications = appl.Where(m => m.End_Date >= DateTime.Today );
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.No.Contains(searchString)
                                                || s.Date.ToShortDateString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    applications = applications.OrderByDescending(s => s.No);
                    break;
                case "Date":
                    applications = applications.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    applications = applications.OrderByDescending(s => s.Date);
                    break;
                default:  // Name ascending 
                    applications = applications.OrderBy(s => s.No);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (applications.Count() != 0 )
            {
                string biggest = applications.Max(r => r.Start_Date).ToShortDateString();

                if (biggest != null)
                {
                    ViewBag.LatestJob = ServicesDTO.GetLatestJob(biggest).Where(m=> m.Submitted_To_Portal == true && m.Shortlisting_Started == false);
                }
            }

            return View(applications.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult JobDetails(string jobid, string no)
        {
            var model = ServicesDTO.GetJobDetails(jobid).FirstOrDefault();
            if (no != null)
            {
                ViewBag.OpenJob = ServicesDTO.GetOpenJobDetails(no);
            }
            return View(model);
        }
    }
}