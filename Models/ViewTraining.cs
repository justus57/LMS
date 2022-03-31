using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ViewTraining
    {
        public string TrainingDescription { get; set; }
        public string CourseDescription { get; set; }
        public string TrainingCost { get; set; }
        public string TrainingInstitution { get; set; }
        public string TrainingEndDateTime { get; set; }
        public string Venue { get; set; }
        public string Room { get; set; }
        public string TrainingStartDateTime { get; set; }
        public string Approver { get; set; }
        public string Id { get; set; }
    }
   
}