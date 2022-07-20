using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Models
{
    public class EditTrainingRequest
    {
        public string TrainingDescription { get; set; }
        public string CourseDescription { get; set; }
        public string TrainingCost { get; set; }
        public string TrainingInstitution { get; set; }
        public string TrainingEndDateTime { get; set; }
        public string Venue { get; set; }
        public string Room { get; set; }
        public string TrainingStartDateTime { get; set; }
        public string ApplicableTo { get; set; }
        public string Id { get; set; }
    }
    public class ApplicableTo
    {
        internal static string SelectedValue;

        public string Id { get; set; }
        public string Name{ get; set; }
    }
    public class RequirementOfTraining
    {
        public string Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
}