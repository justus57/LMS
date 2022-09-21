using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoftwareEngineerTest.Models
{
    public class Home
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string address { get; set; }
    }

    public class Items
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public int Age { get; set; }
    }

}