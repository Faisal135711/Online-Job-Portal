using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JOBPORTAL.Models;

namespace JOBPORTAL.Controllers
{
    public class JobSeekedsController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: JobSeekeds
        public ActionResult Index()
        {
            var jobSeekeds = db.JobSeekeds.Include(j => j.Job).Include(j => j.JobSeeker);
            return View(jobSeekeds.ToList());
        }

        // GET: JobSeekeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeked jobSeeked = db.JobSeekeds.Find(id);
            if (jobSeeked == null)
            {
                return HttpNotFound();
            }
            return View(jobSeeked);
        }

        // GET: JobSeekeds/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "JobId", "JobName");
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername");
            return View();
        }

        // POST: JobSeekeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobSeekedID,JobSeekerId,JobID")] JobSeeked jobSeeked)
        {
            if (ModelState.IsValid)
            {
                db.JobSeekeds.Add(jobSeeked);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "JobId", "JobName", jobSeeked.JobID);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", jobSeeked.JobSeekerId);
            return View(jobSeeked);
        }

        // GET: JobSeekeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeked jobSeeked = db.JobSeekeds.Find(id);
            if (jobSeeked == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobId", "JobName", jobSeeked.JobID);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", jobSeeked.JobSeekerId);
            return View(jobSeeked);
        }

        // POST: JobSeekeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobSeekedID,JobSeekerId,JobID")] JobSeeked jobSeeked)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobSeeked).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobId", "JobName", jobSeeked.JobID);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", jobSeeked.JobSeekerId);
            return View(jobSeeked);
        }

        // GET: JobSeekeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeked jobSeeked = db.JobSeekeds.Find(id);
            if (jobSeeked == null)
            {
                return HttpNotFound();
            }
            return View(jobSeeked);
        }

        // POST: JobSeekeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobSeeked jobSeeked = db.JobSeekeds.Find(id);
            db.JobSeekeds.Remove(jobSeeked);
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
    }
}
