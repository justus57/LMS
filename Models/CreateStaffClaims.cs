using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LMS.Models
{
    public class CreateStaffClaims
    {
        public string SelectedAdvanceRequest { get; set; }
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
        public string UnitofMeasure { get; set; }
        public string ItemDescription { get; set; }
        public string ActualAmount { get; set; }
        public string Remarks { get; set; }
        public string DimCode2Label { get; set; }
        public string DimCode3Label { get; set; }
        public string DimCode4Label { get; set; }
        public string DimCode5Label { get; set; }
        public DropDownList DimCode1 { get; set; }
        public DropDownList DimCode2 { get; set; }
        public DropDownList DimCode3 { get; set; }
        public DropDownList DimCode4 { get; set; }
        public DropDownList DimCode5 { get; set; }
        public DropDownList DimCode6 { get; set; }
        public DropDownList DimCode7 { get; set; }
        public DropDownList DimCode8 { get; set; }
        public string DimCode1Label { get; set; }
    }
}