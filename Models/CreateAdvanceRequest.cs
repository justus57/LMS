using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class CreateAdvanceRequest
    {
        public string No { get; set; }
        public string Item { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public string NoOfUnits { get; set; }
        public string UnitCost { get; set; }
        public string Currency { get; set; }
        public string Purpose { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentId { get; set; }
        public string AdvancedAmount { get; set; }
        public string AdvancedAmountLCY { get; set; }
        public string ClaimedAmount { get; set; }
        public string ClaimedAmountLCY { get; set; }
        public string SurrenderedAmount { get; set; }
        public string SurrenderedAmountLCY { get; set; }
        public string GlobalDimCode1 { get; set; }
        public string GlobalDimCode2 { get; set; }
        public string DimCode1 { get; set; }
        public string DimCode2 { get; set; }
        public string DimCode3 { get; set; }
        public string DimCode4 { get; set; }
        public string DimCode5 { get; set; }
        public string DimCode6 { get; set; }
        public string DimCode7 { get; set; }
        public string DimCode8 { get; set; }
        public string Remarks { get; set; }
    }
}