using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JOBPORTAL.Models;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using JOBPORTAL.Controllers;
using JOBPORTAL.Migrations;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;

namespace JOBPORTAL.Controllers
{
    public class JobSeekersEitaDeleteKorleoMairController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: JobSeekersEitaDeleteKorleoMair
        public ActionResult Index()
        {
            var jobSeekers = db.JobSeekers.Include(j => j.Employer);
            return View(jobSeekers.ToList());
        }

        // GET: JobSeekersEitaDeleteKorleoMair/Details/5
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

        // GET: JobSeekersEitaDeleteKorleoMair/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            return View();
        }

        // POST: JobSeekersEitaDeleteKorleoMair/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId,EducationalQualification,ResumeFileName")] JobSeeker jobSeeker)
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

        // GET: JobSeekersEitaDeleteKorleoMair/Edit/5
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

        // POST: JobSeekersEitaDeleteKorleoMair/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId,EducationalQualification,ResumeFileName")] JobSeeker jobSeeker)
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

        // GET: JobSeekersEitaDeleteKorleoMair/Delete/5
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

        // POST: JobSeekersEitaDeleteKorleoMair/Delete/5
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


        public ActionResult ShowEnquiredSeekers()
        {
            string a = Session["Searched2"].ToString();
            return View(db.JobSeekers.Where(x => x.EducationalQualification.Contains(a)).ToList());
        }

        public ActionResult ShortList()
        {
            EmployerSelectsSeeker es = new EmployerSelectsSeeker();
           es.JobSeekerId= Convert.ToInt32(Session["SeekerId"]);
           es.EmployerId= Convert.ToInt32(Session["EmployerId"]);

            db.EmployerSelectsSeekers.Add(es);
            db.SaveChanges();
            return RedirectToAction("Index","EmployerSelectsSeekers");
        }





        //Original Job Controller Er Shob Alada Function Eikhaane Annte hobe
        //SHob Na Aanleo CHolbe,Khali Registration Function ta change kora laagtese qualification er jonno
        //niche registration baade baki shob function na thaakleo chole.shob original controller eo aase.But thakuk tao.


        public ActionResult ShowShortListSeekers()
        {

            int a = Convert.ToInt32(Session["EmployerId"]);
            var query = db.JobSeekers.SqlQuery("Select *from JobSeeker where JobSeekerId IN  (Select JobSeekerId from EmployerSelectsSeeker where EmployerId=" + a + ")").ToList();
            return View(query);
        }

        public ActionResult LogoutSeeker()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        JobSeeker gjs = new JobSeeker();
        public ActionResult InputCvName()
        {
            return View();
        }

        [HttpPost]

        public ActionResult InputCvName(HttpPostedFileBase file)
        {

            
                string filename = Path.GetFileName(file.FileName);
                Session["FILENAME"] = filename;
                gjs.ResumeFileName = filename;
           
            // jobSeeker.ResumeFileName = filename;

            // string filepath = Path.Combine(Server.MapPath("~/CVFILE"), filename);

            return RedirectToAction("Registration");
        }


        public ActionResult Registration()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            return View();
        }

        // POST: JobSeekersEitaDeleteKorleoMair/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "JobSeekerId,JobSeekerUsername,JobSeekerName,JobSeekerContactNo,JobSeekerEmail,JobSeekerPassword,EmployerId,EducationalQualification")] JobSeeker jobSeeker)
        {
            
            if (ModelState.IsValid)
            {

                //string filename = Path.GetFileName(file.FileName);
                // jobSeeker.ResumeFileName = filename;

                // string filepath = Path.Combine(Server.MapPath("~/CVFILE"), filename);

                    //jobSeeker.ResumeFileName =filename;
                    db.JobSeekers.Add(jobSeeker);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", jobSeeker.EmployerId);
            return View(jobSeeker);
        }


        public ActionResult ListofSeekersWhoShortListedAEmployer()
        {
            int a = Convert.ToInt32(Session["EmployerId"]);
            
            var query2 = db.JobSeekers.SqlQuery("Select *from Jobseeker where JobSeekerID in(Select JobSeekerID from JobSeeked where JobId IN(Select JobiD From Job where EmployerId=" + a + "))").ToList();
            return View(query2);
        }

        public ActionResult ShortListedSeekers()
        {
            int a = Convert.ToInt32(Session["EmployerId"]);
            var query = db.JobSeekers.SqlQuery("Select *from JobSeeker where JobSeekerId IN  (Select JobSeekerId from EmployerSelectsSeeker where EmployerId=" + a + ")").ToList();
            return View(query);
        }






    }


    
}
