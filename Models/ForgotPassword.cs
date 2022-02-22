using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage ="Please enter your Employee Number")]
        public string EmployeeNumber { get; set; }
    }
}