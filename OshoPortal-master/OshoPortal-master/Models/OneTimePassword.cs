using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal.Models
{
    public class OneTimePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}