using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JOBPORTAL.Controllers;
using JOBPORTAL.Models;

namespace JOBPORTAL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }


        public ActionResult AllTableLists()
        {
            return View();
        }

        public ActionResult ContactSubmit(string a,string b,string c,string d)
        {
            JOB_PORTAL_3Entities DD = new JOB_PORTAL_3Entities();
            ContactU cc = new ContactU();
            cc.FullName = a;
            cc.Email = b;
            cc.Phone = c;
            cc.Message = d;
            DD.ContactUs.Add(cc);
            DD.SaveChanges();
            return RedirectToAction("Index1");
        }

        public ActionResult Search(String SearchByTitle,string SearchByLocation)

        {

            Session["Searched"] = SearchByTitle.ToString();
            Session["Searched1"] = SearchByLocation.ToString();

            return RedirectToAction("ShowEnquiredListOfJobsDescriptionAndLocation", "Jobs");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult registraionemployee()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult home()
        {
            return View();
        }
    }
}