using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LMS.Models
{
    public class CreateTransportRequest
    {
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Destination { get; set; }
        public string Balance { get; set; }
        public string EstimatedDistance { get; set; }
        public string EstimatedCostOfTrip { get; set; }
        public DropDownList VehicleClass { get; set; }
        public string DimCode2Label { get; set; }
        public string DimCode3Label { get; set; }
        public string DimCode4Label { get; set; }
        public string DimCode6Label { get; set; }
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