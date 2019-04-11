using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JOBPORTAL.Models;
using System.Data.Entity.Validation;

namespace JOBPORTAL.Controllers
{
    public class JobsController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: Jobs
        public async Task<ActionResult> Index()
        {
            var jobs = db.Jobs.Include(j => j.Employer).Include(j => j.JobCategory);
            return View(await jobs.ToListAsync());
        }

        public ActionResult ShowEnquiredListOfJobs()
        {
            string a = Session["Searched"].ToString();
            return View(db.Jobs.Where(x => x.JobDescription.Contains(a)).ToList());
        }
        public ActionResult ShowEnquiredListOfJobsLocation()
        {
            string a = Session["Searched1"].ToString();
            return View(db.Jobs.Where(x => x.JobLocation.Contains(a)).ToList());
        }
        public ActionResult ShowEnquiredListOfJobsDescriptionAndLocation()
        {

          
            string a = Session["Searched"].ToString();
            string b = Session["Searched1"].ToString();
            var j=db.Jobs.Where(x => x.JobDescription.Contains(a) || x.JobLocation.Contains(b));

            return View(j.ToList());
        }

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername");
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobId,JobName,JobRequirements,JobDescription,JobPosition,JobLocation,JobSalary,JobWorkingHour,JobCategoryId")] Job job)
        {
            if (ModelState.IsValid)
            {
                job.EmployerId = Convert.ToInt32(Session["EmployerId1"]);

                db.Jobs.Add(job);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException e)
                {

                    throw;
                }
               // db.SaveChanges();
                return RedirectToAction("AfterLogin","Employers");
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", job.EmployerId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", job.EmployerId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JobId,JobName,JobRequirements,JobDescription,JobPosition,JobLocation,JobSalary,JobWorkingHour,EmployerId,JobCategoryId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "EmployerUsername", job.EmployerId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            db.Jobs.Remove(job);
            await db.SaveChangesAsync();
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


        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Job account)
        {
            if (ModelState.IsValid)
            {
                using (JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities())
                {
                    db.Jobs.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.EmployerRegistrationMessage = account.JobName + " successfully registered";
            }
            return View();
        }

        //SHow Searched Seekers,Temporary For Now
        public ActionResult SearchByDegree(String SearchbyDegree)

        {

            Session["Searched2"] = SearchbyDegree.ToString();


            return RedirectToAction("ShowEnquiredSeekers", "JobSeekersEitaDeleteKorleoMair");
        }

        public ActionResult ShortListJobs()
        {
            JobSeeked es = new JobSeeked();
            es.JobSeekerId = Convert.ToInt32(Session["SeekerId"]);
            es.JobID = Convert.ToInt32(Session["JobId"]);

            db.JobSeekeds.Add(es);
            db.SaveChanges();
            return RedirectToAction("Index", "JobSeekeds");
        }

        public ActionResult ShowShortListJobs()
        {
            // string a = Session["SeekerId"].ToString();
            //int b =Convert.ToInt32(a);
            // var query = db.JobSeekers.Where(x => x.JobSeekerId.Equals(12));
             int a = Convert.ToInt32(Session["SeekerId"]);
             var query1 = db.Jobs.SqlQuery("Select * From Job where JobId IN(Select JobId from JobSeeked where JobSeekerId ="+a+")").ToList();
            //var query2 = db.JobSeekers.SqlQuery("Select * From Job where JobId=2").ToList();
            // var q = db.Jobs.Where(y => y.JobId.Equals(db.JobSeekers.Where(x => x.JobSeekerId.Equals(12)))).ToList();
            // var w= "Select * From Job where JobId IN(Select JobId from JobSeeked where JobSeekerId = 12)";
            //return View(db.Jobs.Where(y => y.JobId.Equals(db.JobSeekers.Where(x => x.JobSeekerId.Equals(12)))).ToList());
            //return View(db.Jobs.Where(y => y.JobId.Equals(2)).ToList());
            return View(query1);

        }


        public ActionResult ListOfPublishedJobsByAnEmployer()
        {

            int a = Convert.ToInt32(Session["EmployerId"]);

            var query1 = db.Jobs.SqlQuery("Select * From Job where EmployerId="+a+"").ToList();
            return View(query1);
        }
    }
}
