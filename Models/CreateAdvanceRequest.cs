using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LMS.Models
{
    public class CreateAdvanceRequest
    {
        [System.Web.Mvc.AllowHtml]
        public string DateOfRequest { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DateDue { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string Balance { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string MissionSummary { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string NoOfUnits { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ID { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ExchangeRate { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string Purpose { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string AdvanceRequestLineNo { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string BudgetLineCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string UnitCost { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string AttachmentDescription { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string LoadPreferredPaymentMethod { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ClaimedAmountLCY { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DimCode2Label { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DimCode3Label { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DimCode4Label { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DimCode5Label { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode1 {
            get;
            set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode2 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode3 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode4 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode5 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode6 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode7 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public DropDownList DimCode8 { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string DimCode1Label { get; set; }
    }
    public class PreferredPaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}