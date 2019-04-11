using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JOBPORTAL.Models;
using JOBPORTAL.Controllers;
using JOBPORTAL.Migrations;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;


namespace JOBPORTAL.Controllers
{
    public class JobSeekersController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: JobSeekers
        public ActionResult Index()
        {
            var jobSeekers = db.JobSeekers.Include(j => j.Employer);
            //   return View(jobSeekers.ToList());
            return View(db.JobSeekers.ToList());
        }


        public ActionResult ShowEnquiredSeekers()
        {
            string a = Session["Searched2"].ToString();
            return View(db.JobSeekers.Where(x => x.EducationalQualification.Contains(a)).ToList());
        }


        /* public ActionResult ShowEnquiredList()
         {

             return View(db.JobSeekers.Where(x => x.JobSeekerUsername.Contains(Session["Searched"])).ToList());
         }*/



        // GET: JobSeekers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker jobSeeker = db.JobSeekers.Find(id);
            if (jobSeeker == null)
            {
                return HttpNotFound();
            }
            return View(jobSeeker);
        }

        // GET: JobSeekers/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            return View();
        }

        // POST: JobSeekers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId")] JobSeeker jobSeeker)
        {
            if (ModelState.IsValid)
            {
                db.JobSeekers.Add(jobSeeker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", jobSeeker.EmployerId);
            return View(jobSeeker);
        }

        // GET: JobSeekers/Create
        public ActionResult Registration()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            return View();
        }

        // POST: JobSeekers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId")] JobSeeker jobSeeker)
        {
            bool registrationsuccess = false;
            Session["SeekerId"] = jobSeeker.JobSeekerId;
            Session["SeekerUsername"] = jobSeeker.JobSeekerUsername;
            try
            {
                if (ModelState.IsValid)
                {
                  //  Session["SeekerId"] = jobSeeker.JobSeekerId.ToString();
                    //Session["SeekerUsername"] = jobSeeker.JobSeekerUsername.ToString();
                    db.JobSeekers.Add(jobSeeker);
                    db.SaveChanges();
                    registrationsuccess = true;
                   // Session["SeekerId"] = jobSeeker.JobSeekerId.ToString();
                    //Session["SeekerUsername"] = jobSeeker.JobSeekerUsername.ToString();
                          return RedirectToAction("AfterLogin", "JobSeekers");
                }
            }
            catch(Exception e)
            {
                Session["dummy1"] = "Please fill out all the fields accordingly".ToString();
                return RedirectToAction("Registration");
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", jobSeeker.EmployerId);
            return RedirectToAction("AfterLogin", "JobSeekers");
        }

        // GET: JobSeekers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker jobSeeker = db.JobSeekers.Find(id);
            if (jobSeeker == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", jobSeeker.EmployerId);
            return View(jobSeeker);
        }

        // POST: JobSeekers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId")] JobSeeker jobSeeker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobSeeker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", jobSeeker.EmployerId);
            return View(jobSeeker);
        }

        // GET: JobSeekers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker jobSeeker = db.JobSeekers.Find(id);
            if (jobSeeker == null)
            {
                return HttpNotFound();
            }
            return View(jobSeeker);
        }

        // POST: JobSeekers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobSeeker jobSeeker = db.JobSeekers.Find(id);
            db.JobSeekers.Remove(jobSeeker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //login
        public ActionResult SeekerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SeekerLogin(JobSeeker user)
        {
            var loginSuccess = false;
            using (JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities())
            {
                try
                {
                    var usr = db.JobSeekers.Single(u => u.JobSeekerUsername == user.JobSeekerUsername && u.JobSeekerPassword == user.JobSeekerPassword);
                    if (usr != null)
                    {
                        Session["SeekerId"] = usr.JobSeekerId.ToString();
                        /* Session["EmployerId1"] = usr.EmployerId;
                         ViewBag.id = usr.EmployerId;

                         Convert.ToInt32(Session["EmployerId1"]);*/

                        loginSuccess = true;
                        if (usr.ProfilePictureName != null)
                        {
                            Session["SeekerPic"] = usr.ProfilePictureName.ToString();
                        }

                        Session["SeekerUsername"] = usr.JobSeekerUsername.ToString();
                        return RedirectToAction("AfterLogin", "JobSeekers");

                    }
                }
                catch (InvalidOperationException e)
                {

                }
            }

            if (loginSuccess == false)
            {
                //ModelState.AddModelError("", " username or password is wrong");
                Session["EmployerLoginFail1"] = "username or password is wrong";
                return RedirectToAction("SeekerLogin", "JobSeekers");
            }

            return View();
        }

        public ActionResult LogoutSeeker()
        {
            Session.Clear();
            return RedirectToAction("Index1", "Home");
        }

        public ActionResult ShowShortListSeekers()
        {

            int a = Convert.ToInt32(Session["EmployerId"]);
            return RedirectToAction("ShortListedSeekers", "JobSeekersEitaDeleteKorleoMair");
           // var query = db.JobSeekers.SqlQuery("Select *from JobSeeker where JobSeekerId IN  (Select JobSeekerId from EmployerSelectsSeeker where EmployerId=" + a + ")").ToList();
            //return View(query);
        }

        public ActionResult Profile()
        {

            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult AfterLogin()
        {
            return View();
        }

        public ActionResult Search(String SearchByTitle, string SearchByLocation)

        {

            Session["Searched"] = SearchByTitle.ToString();
            Session["Searched1"] = SearchByLocation.ToString();

            return RedirectToAction("ShowEnquiredListOfJobsDescriptionAndLocation", "Jobs");
        }

        public ActionResult ProfileInformation()
        {
            int a = Convert.ToInt32(Session["SeekerId"]);
            var query2 = db.JobSeekers.SqlQuery("Select * From JobSeeker where JobSeekerId=" + a + "").ToList();
            return View(query2);
        }

        public ActionResult ViewForPictureUpload()
        {
            return View();
        }

        public ActionResult ProPicInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProPicInsert(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/PROFILEPICTURE"), filename);
                file.SaveAs(filepath);
                int a = Convert.ToInt32(Session["SeekerId"]);
                var b = new JOB_PORTAL_3Entities();
                b.Database.ExecuteSqlCommand("Update Jobseeker set ProfilePictureName='" + filename + "' where JobSeekerId=" + a + "");
                db.SaveChanges();
                //return RedirectToAction("Index");
                //resume.CvFileText = filepath;

            }
            return RedirectToAction("AfterLogin");
        }

        public ActionResult EmployersWhoShortListedMe()
        {
            int a = Convert.ToInt32(Session["SeekerId"]);
            return RedirectToAction("ListEmployersWhoShortListedASeeker","Employers");
            //var query2 = db.Employers.SqlQuery("Select *from Employer where EmployerId IN(Select EMployerId from EmployerSelectsSeeker where JobSeekerId=" + a + ")").ToList();
            //return View(query2);

        }
    }
}
