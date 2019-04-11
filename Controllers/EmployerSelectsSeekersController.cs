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
    public class EmployerSelectsSeekersController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: EmployerSelectsSeekers
        public ActionResult Index()
        {
            var employerSelectsSeekers = db.EmployerSelectsSeekers.Include(e => e.Employer).Include(e => e.JobSeeker);
            return View(employerSelectsSeekers.ToList());
        }

        // GET: EmployerSelectsSeekers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployerSelectsSeeker employerSelectsSeeker = db.EmployerSelectsSeekers.Find(id);
            if (employerSelectsSeeker == null)
            {
                return HttpNotFound();
            }
            return View(employerSelectsSeeker);
        }

        // GET: EmployerSelectsSeekers/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername");
            return View();
        }

        // POST: EmployerSelectsSeekers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployerSelectsSeekerID,EmployerId,JobSeekerId")] EmployerSelectsSeeker employerSelectsSeeker)
        {
            if (ModelState.IsValid)
            {
                db.EmployerSelectsSeekers.Add(employerSelectsSeeker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", employerSelectsSeeker.EmployerId);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", employerSelectsSeeker.JobSeekerId);
            return View(employerSelectsSeeker);
        }

        // GET: EmployerSelectsSeekers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployerSelectsSeeker employerSelectsSeeker = db.EmployerSelectsSeekers.Find(id);
            if (employerSelectsSeeker == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", employerSelectsSeeker.EmployerId);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", employerSelectsSeeker.JobSeekerId);
            return View(employerSelectsSeeker);
        }

        // POST: EmployerSelectsSeekers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployerSelectsSeekerID,EmployerId,JobSeekerId")] EmployerSelectsSeeker employerSelectsSeeker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employerSelectsSeeker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", employerSelectsSeeker.EmployerId);
            ViewBag.JobSeekerId = new SelectList(db.JobSeekers, "JobSeekerId", "JobSeekerUsername", employerSelectsSeeker.JobSeekerId);
            return View(employerSelectsSeeker);
        }

        // GET: EmployerSelectsSeekers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployerSelectsSeeker employerSelectsSeeker = db.EmployerSelectsSeekers.Find(id);
            if (employerSelectsSeeker == null)
            {
                return HttpNotFound();
            }
            return View(employerSelectsSeeker);
        }

        // POST: EmployerSelectsSeekers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployerSelectsSeeker employerSelectsSeeker = db.EmployerSelectsSeekers.Find(id);
            db.EmployerSelectsSeekers.Remove(employerSelectsSeeker);
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
