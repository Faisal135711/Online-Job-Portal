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
    public class ResumesController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: Resumes1
        public ActionResult Index()
        {
            var resumes = db.Resumes.Include(r => r.JobSeeker);
            return View(resumes.ToList());
        }

        // GET: Resumes1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = db.Resumes.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // GET: Resumes1/Create
        public ActionResult Create()
        {
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername");
            return View();
        }

        // POST: Resumes1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResumeId,CvFileText")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                resume.ResumeUploadDate = System.DateTime.Now;
                resume.JobSeekerId = Convert.ToInt32(Session["SeekerId"]);
                resume.CvFile = Encoding.UTF8.GetBytes(resume.CvFileText);

               

                db.Resumes.Add(resume);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", resume.JobSeekerId);
            return View(resume);
        }

      

        // POST: Resumes1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "ResumeId,CvFileText")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                resume.ResumeUploadDate = System.DateTime.Now;
                resume.JobSeekerId = Convert.ToInt32(Session["SeekerId"]);
                resume.CvFile = Encoding.UTF8.GetBytes(resume.CvFileText);
                db.Resumes.Add(resume);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", resume.JobSeekerId);
            return View(resume);
        }

        // GET: Resumes1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = db.Resumes.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", resume.JobSeekerId);
            return View(resume);
        }

        // POST: Resumes1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResumeId,ResumePath,ResumeUploadDate,ResumeLastModifiedDate,JobSeekerId,CvFile,CvFileText")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resume).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", resume.JobSeekerId);
            return View(resume);
        }

        // GET: Resumes1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = db.Resumes.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // POST: Resumes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resume resume = db.Resumes.Find(id);
            db.Resumes.Remove(resume);
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



        
        public ActionResult Upload33()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload33(HttpPostedFileBase file)
        {

            Resume res = new Resume();


            if (file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/CVFILE"), filename);
                file.SaveAs(filepath);
              
                res.CvFileText = filepath;
                res.CvFileName = filename;
                int a = Convert.ToInt32(Session["SeekerId"]);
                var b = new JOB_PORTAL_3Entities();
                b.Database.ExecuteSqlCommand("Update Jobseeker set ResumeFileName='" + filename + "' where JobSeekerId=" + a + "");
                db.SaveChanges();
                //return RedirectToAction("Index");
                //resume.CvFileText = filepath;

            }
            JOB_PORTAL_3Entities db1 = new JOB_PORTAL_3Entities();


        
            res.JobSeekerId = Convert.ToInt32(Session["SeekerId"]);
            res.ResumeUploadDate = System.DateTime.Now;
            res.CvFile= Encoding.UTF8.GetBytes(res.CvFileText);
            db.Resumes.Add(res);
            db.SaveChanges();
            return RedirectToAction("AfterLogin","JobSeekers");
        }

    }
}
