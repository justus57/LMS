using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal.Models
{
    public class EditRequisition
    {
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string cost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string NOofItems { get; set; }
        public string No { get; set; }
        public string SelectDate { get; set; }
        public string Amount { get; set; }
    }
}