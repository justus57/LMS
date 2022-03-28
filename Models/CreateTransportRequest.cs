using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string Purpose { get; set; }
    }
}