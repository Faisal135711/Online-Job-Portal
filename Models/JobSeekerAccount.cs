using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JOBPORTAL.Models
{
    public class JobSeekerAccount
    {
        [Key]
        public int JobSeekerId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string JobSeekerName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string JobSeekerUsername { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string JobSeekerEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string JobSeekerPassword { get; set; }

        [Required(ErrorMessage = "Please Confirm the password.")]
        public string JobSeekerConfirmPassword { get; set; }
    }
}