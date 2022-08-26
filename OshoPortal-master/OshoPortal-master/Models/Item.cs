using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal.Models
{
    public class Item
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ItemDescription
    {
        public string Status { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Cost { get; set; }
        public string Response { get; set; }
        public string DocumentNumber { get;  set; }
    }
}