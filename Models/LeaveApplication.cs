using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LMS.Models
{
    public class LeaveApplication
    {
        public string Leave_Opening_Balance { get; set; }
        public List<SelectListItem> Leave_Type { get; set; }
        public string Leave_Entitled { get; set; }
        public string Leave_Accrued_Days { get; set; }
        public string Leave_Days_Taken { get; set; }
        public string Leave_Balance { get; set; }
        public string RequiresAttachment { get; set; }
        public string LeaveStartDay { get; set; }
        public string LeaveEndDay { get; set; }
        public string LeaveDaysApplied { get; set; }
        public string ReturnDate { get; set; }
        public string postedFile { get; set; }
        public string Leave_comments { get; set; }
        public string Message { get; set; }
        public bool Validity { get; set; }
    }


}