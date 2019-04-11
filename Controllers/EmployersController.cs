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
    public class EmployersController : Controller
    {
        private JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities();

        // GET: Employers
        public async Task<ActionResult> Index()
        {
            return View(await db.Employers.ToListAsync());
        }

        // GET: Employers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = await db.Employers.FindAsync(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // GET: Employers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployerId,EmployerUsername,EmployerName,EmployerContactNo,EmployerEmail,EmployerPassword")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Employers.Add(employer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employer);
        }



        // GET: Employers/Create
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration([Bind(Include = "EmployerId,EmployerUsername,EmployerName,EmployerContactNo,EmployerEmail,EmployerPassword")] Employer employer)
        {
            bool registrationsuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employers.Add(employer);
                    await db.SaveChangesAsync();
                    registrationsuccess = true;
                    return RedirectToAction("AfterLogin");
                }
            }
            catch (Exception e)
            {
                Session["dummy"] = "Please fill out all the fields accordingly".ToString();
                return RedirectToAction("Registration");
            }
            if (registrationsuccess != true)
            {
                Session["dummy"] = "Please fill out all the fields accordingly".ToString();
                return RedirectToAction("Registration");
            }
            else
            {
                return RedirectToAction("AfterLogin");
            }
        }

        // GET: Employers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = await db.Employers.FindAsync(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployerId,EmployerUsername,EmployerName,EmployerContactNo,EmployerEmail,EmployerPassword")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employer);
        }

        // GET: Employers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = await db.Employers.FindAsync(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employer employer = await db.Employers.FindAsync(id);
            db.Employers.Remove(employer);
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




        //login
        public ActionResult EmployerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployerLogin(Employer user)
        {
            var loginSuccess = false;
            
            using (JOB_PORTAL_3Entities db = new JOB_PORTAL_3Entities())
            {
                

                try
                {
                    var usr = db.Employers.Single(u => u.EmployerUsername == user.EmployerUsername && u.EmployerPassword == user.EmployerPassword);
                   
                    if (usr != null)
                    {
                        Session["EmployerId"] = usr.EmployerId.ToString();
                        Session["EmployerId1"] = usr.EmployerId.ToString();
                        if (usr.ProfilePictureName != null)
                        {
                             Session["EmployerProPic"] = usr.ProfilePictureName.ToString();
                        }
                        ViewBag.id = usr.EmployerId;

                        //Convert.ToInt32(Session["EmployerId1"]);

                        Session["EmployerUsername"] = usr.EmployerUsername.ToString();
                        loginSuccess = true;
        
                        return RedirectToAction("AfterLogin", "Employers");
                    }
                }
                catch(InvalidOperationException e)
                {
                    Session["EmployerLoginFail"] = "username or password is wrong";
                    return RedirectToAction("EmployerLogin", "Employers");

                }
            }
            if (loginSuccess == false)
            {
                //ModelState.AddModelError("", " username or password is wrong");
                Session["EmployerLoginFail"] = "username or password is wrong";
                return RedirectToAction("EmployerLogin", "Employers");
            }
            return View();
        }



        public ActionResult LogoutEmployer()
        {
            Session.Clear();
            return RedirectToAction("Index1", "Home");

        }


        public ActionResult SendToAddJob()
        {

            return RedirectToAction("Create","Jobs");
           // return View();
        }

        public ActionResult Profile1()
        {
            return View();
        }

        public ActionResult AfterLogin()
        {
            return View();
        }

        public ActionResult SearchByDegree(String SearchbyDegree)

        {

            Session["Searched2"] = SearchbyDegree.ToString();


            return RedirectToAction("ShowEnquiredSeekers", "JobSeekersEitaDeleteKorleoMair");
        }

        public ActionResult ShowInsertedJobs()
        {
            int a = Convert.ToInt32(Session["EmployerId"]);
            return RedirectToAction("ListOfPublishedJobsByAnEmployer", "Jobs");
            //var query1 = db.Jobs.SqlQuery("Select * From Job where EmployerId="+a+"").ToList();
            //return View(query1);
        }

        public ActionResult ProfileInfo()
        {
            int a = Convert.ToInt32(Session["EmployerId"]);
            var query2 = db.Employers.SqlQuery("Select * From Employer where EmployerId=" + a + "").ToList();
            return View(query2);
            
        }

        public ActionResult ViewForPictureUpload1()
        {

            return View();
        }

        public ActionResult ProPicInsert1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProPicInsert1(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/EMPLOYERPROFILEPICTURE"), filename);
                file.SaveAs(filepath);
                int a = Convert.ToInt32(Session["EmployerId"]);
                var b = new JOB_PORTAL_3Entities();
                b.Database.ExecuteSqlCommand("Update Employer set ProfilePictureName='" + filename + "' where EmployerId=" + a + "");
                db.SaveChanges();
                //return RedirectToAction("Index");
                //resume.CvFileText = filepath;

            }
            return RedirectToAction("AfterLogin");
        }

        public ActionResult SeekerWhoShortListedMe()
        {

            int a = Convert.ToInt32(Session["EmployerId"]);
            return RedirectToAction("ListofSeekersWhoShortListedAEmployer", "JobSeekersEitaDeleteKorleoMair");
            //var query2 = db.JobSeekers.SqlQuery("Select *from Jobseeker where JobSeekerID in(Select JobSeekerID from JobSeeked where JobId IN(Select JobiD From Job where EmployerId=" + a + "))").ToList();
            //return View(query2);
        }

        public ActionResult ListEmployersWhoShortListedASeeker()
        {

            int a = Convert.ToInt32(Session["SeekerId"]);
            var query2 = db.Employers.SqlQuery("Select *from Employer where EmployerId IN(Select EMployerId from EmployerSelectsSeeker where JobSeekerId=" + a + ")").ToList();
            return View(query2);
        }


    }
}
