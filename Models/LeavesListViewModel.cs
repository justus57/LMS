using System;
using System.Linq;

namespace LMS.Models
{
    public class LeavesListViewModel
    {
        public string DateCreated { get; set; }
        public string LeaveCode { get; set; }
        public string HeaderNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LeaveDays { get; set; }
        public string HeaderNoLink { get; set; }
        public string ApprovalStatus { get; set; }
    }
}