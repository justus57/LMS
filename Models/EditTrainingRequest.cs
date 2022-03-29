using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class EditTrainingRequest
    {
    }
    public class ApplicableTo
    {
        public string Id { get; set; }
        public string Name
        {
            get;
            set;
        }
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