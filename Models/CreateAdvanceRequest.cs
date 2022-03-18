using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class CreateAdvanceRequest
    {
        public string DateOfRequest { get; set; }
        public string DateDue { get; set; }
        public string Balance { get; set; }
        public string MissionSummary { get; set; }
        public string NoOfUnits { get; set; }
        public string Amount { get; set; }
        public string ExchangeRate { get; set; }
        public string Purpose { get; set; }
        public string AdvanceRequestLineNo { get; set; }
        public string BudgetLineCode { get; set; }
        public string UnitCost { get; set; }
        public string ItemDescription { get; set; }
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