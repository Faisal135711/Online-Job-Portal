using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace JOBPORTAL.Models
{
    public class EmployerAccount
    {

        [Key]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string EmployerName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string EmployerUsername { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string EmployerEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string EmployerPassword { get; set; }

        [Required(ErrorMessage = "Please Confirm the password.")]
        [DataType(DataType.Password)]
        public string EmployerConfirmPassword { get; set; }
    }
}