using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LMS.Models
{
    public class CreateTrainingRequest
    {
        public string TrainingDescription { get; set; }
        public string TrainingStartDateTime { get; set; }
        public string TrainingEndDateTime { get; set; }
        public string CourseDescription { get; set; }
        public string Venue { get; set; }
        public string TrainingInstitution { get; set; }
        public string Room { get; set; }
        public string TrainingCost { get; set; }
   
    }
   
}