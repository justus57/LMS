using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Please enter your Employee Number")]
        public string EmployeeNumber { get; set; }
    }
}