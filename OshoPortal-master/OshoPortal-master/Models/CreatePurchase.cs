using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal.Models
{
    public class CreatePurchase
    {
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string cost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string NOofItems { get; set; }
        public string Comment { get; set; }
        public string SelectDate { get; set; }
        public string Amount { get; set; }
    }
    public class Items
    {
        public string Value { get; set; }
        public string Key { get; set; }
    }
    public  class itemdetails
    {
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string LineType { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UoMCode { get; set; }
        public string UnitCost { get; set; }
        public string LineAmount { get; set; }

    }
}