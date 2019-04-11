using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace JOBPORTAL.Models
{
    public class OurDBcontext: DbContext
    {
        public DbSet<JobSeekerAccount> JobSeeker { get; set; }
    }
}