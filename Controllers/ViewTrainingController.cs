using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LMS.Controllers
{
    public class ViewTrainingController : Controller
    {
        static string TrainingNo = null;
        ViewTraining Training = new ViewTraining();
        // GET: ViewTraining
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewTraining()
        {

            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "active";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";

            if (Session["Logged"].Equals("No"))
            {
                Response.Redirect("Login.aspx");
            }
            else if (Session["Logged"].Equals("Yes"))
            {
                if (Session["RequirePasswordChange"].Equals("TRUE"))
                {
                    Response.Redirect("OneTimePass.aspx");
                }
                else
                {
                    LoadRequirementOfTraining();
                    LoadApplicableTo();


                    string i = Request.QueryString["id"].Trim();
                    string _TrainingNo = AppFunctions.Base64Decode(i);
                    TrainingNo = _TrainingNo;
                    LoadTraining(_TrainingNo);
                }
            }
            return View();
        }
        private void LoadApplicableTo()
          {

            List<ApplicableTo> applicables = new List<ApplicableTo>()
            {
               new ApplicableTo(){Id="Position",Name="POSITION"},
               new ApplicableTo(){Id="Org. Unit",Name="ORGUNIT"},
               new ApplicableTo(){Id="Individual Employee",Name="INDIVIDUALEMPLOYEE"},
               new ApplicableTo(){Id="All Employees",Name="ALLEMPLOYEES"},
               new ApplicableTo(){Id="",Name=""}
            };
            ViewBag.ApplicableTo = applicables;
              }
        private void LoadRequirementOfTraining()
        {
        List<RequirementOfTraining> requirementOfs = new List<RequirementOfTraining>()
        {
            new RequirementOfTraining(){Id="Identification on individual need",Name="POSITION"},
            new RequirementOfTraining(){Id="Identification on performance need",Name="PERFORMANCENEED"},
            new RequirementOfTraining(){Id="Professional requirement",Name="PROFESSIONALREQUIREMENT"},
            new RequirementOfTraining(){Id="Organizational/Departmental Needs",Name="ORGANIZATIONALDEPARTMENTALNEEDS"},
            new RequirementOfTraining(){Id="",Name=""}
        };
        ViewBag.ApplicableTo = requirementOfs;
            }
        private void LoadTraining(string v)
        {
            string TrainingDetailsResponse = TrainingsXMLRequests.GetTrainingDetail(v);
            dynamic json = JObject.Parse(TrainingDetailsResponse);


            string _TrainingStartDateTime = "";
            string _TrainingEndDateTime = "";
            string _TrainingDescription = json.Description;
            string _TrainingStartDate = json.PlannedStartDate;
            string _TrainingStartTime = json.PlannedStartTime;
            string _TrainingEndDate = json.PlannedEndDate;
            string _TrainingEndTime = json.PlannedEndTime;
            string _Venue = json.Venue;
            string _Room = json.Room;
            string _TrainingInstitution = json.TrainingInstitution;
            string _TrainingCost = json.TotalCost;
            string _ApplicableTo = json.ApplicableTo;
            string _CourseDescription = json.CourseDescription;
            string _Approver = json.Approver;
            string _SupervisorName = TrainingsXMLRequests.GetSupervisorFullName(_Approver);
            string _RequirementOfTraining = json.RequirementOfTrainingRequest;

            // _Approver = TrainingsXMLRequests.GetSupervisorFullName(json.Approver);


            if (_TrainingStartDate != "")
            {
                //convert date to this 13/11/2019 07:00 foemat
                DateTime __TrainingStartDateTime = DateTime.Parse(_TrainingStartDate);

                if (_TrainingStartTime != "")
                {
                    string StartTime = _TrainingStartTime;
                    var starttime = TimeSpan.Parse(StartTime);
                    var TrainingStartdateTime = __TrainingStartDateTime.Add(starttime);
                    _TrainingStartDateTime = TrainingStartdateTime.ToString("dd/MM/yyyy HH:mm");
                }
            }

            if (_TrainingEndDate != "")
            {
                //convert date to this 13/11/2019 07:00 foemat
                DateTime __TrainingEndDateTime = DateTime.Parse(_TrainingEndDate);

                if (_TrainingEndTime != "")
                {
                    string EndTime = _TrainingEndTime;
                    var endtime = TimeSpan.Parse(EndTime);
                    var TrainingEnddateTime = __TrainingEndDateTime.Add(endtime);
                    _TrainingEndDateTime = TrainingEnddateTime.ToString("dd/MM/yyyy HH:mm");
                }
            }


        //Convert this date time to datepicker timeformat

        //display
        Training.TrainingDescription = _TrainingDescription;
        Training.TrainingStartDateTime = _TrainingStartDateTime;
        Training.TrainingEndDateTime = _TrainingEndDateTime;
        Training.Venue = _Venue;
        Training.Room = _Room;
        Training.TrainingInstitution = _TrainingInstitution;
        Training.TrainingCost = _TrainingCost;
            ApplicableTo.SelectedValue = _ApplicableTo;
            // ApplicableTo.Items.FindByValue(_ApplicableTo).Selected = true;
        Training.CourseDescription = _CourseDescription;
        Training.Approver = _SupervisorName;

            //if (_RequirementOfTraining == "0")
            //{
            //    RequirementOfTraining.Items.FindByValue("ORGANIZATIONALDEPARTMENTALNEEDS").Selected = true;
            //}
            //else if (_RequirementOfTraining == "1")
            //{
            //    RequirementOfTraining.Items.FindByValue("PROFESSIONALREQUIREMENT").Selected = true;
            //}
            //else if (_RequirementOfTraining == "2")
            //{
            //    RequirementOfTraining.Items.FindByValue("PERFORMANCENEED").Selected = true;
            //}
            //else if (_RequirementOfTraining == "3")
            //{
            //    RequirementOfTraining.Items.FindByValue("INDIVIDUALNEED").Selected = true;
            //}
        }
        public static string GetTrainingMemberList()
        {
            var publicationTable = new List<object>();

            foreach (var kvp in TrainingsXMLRequests.GetTrainingMemberList(TrainingNo))
            {
                publicationTable.Add(new[] { kvp.Value });
            }

            return (new JavaScriptSerializer()).Serialize(publicationTable);
        }           
        public static string GetApplicableToEmployees(string param1)
        {
            List<Employee> employeeObject = new List<Employee>(); //TrainingNo

            foreach (var kvp in LeaveRecallForOtherXMLRequests.GetEmpoyeeList())
            {
                employeeObject.Add(new Employee { EmployeeCode = kvp.Key, EmployeeName = kvp.Value });
            }

            return JsonConvert.SerializeObject(employeeObject);
        }           
        public static string GetEmployeeList(string param1)
        {
            List<Employee> employeeObject = new List<Employee>();

            foreach (var kvp in LeaveRecallForOtherXMLRequests.GetEmpoyeeList())
            {
                employeeObject.Add(new Employee { EmployeeCode = kvp.Key, EmployeeName = kvp.Value });
            }

            return JsonConvert.SerializeObject(employeeObject);
        }         
        public static string GetOrgUnitList(string param1)
        {
            List<OrgUnit> OrgUnitObject = new List<OrgUnit>();

            foreach (var kvp in CreateAppraisalXMLREquests.GetOrgUnitList())
            {
                OrgUnitObject.Add(new OrgUnit { Code = kvp.Key, Name = kvp.Value });
            }

            return JsonConvert.SerializeObject(OrgUnitObject);
        }         
        public static string HRPositionUnitList(string param1)
        {
            List<HRPosition> HRPositionObject = new List<HRPosition>();

            foreach (var kvp in CreateAppraisalXMLREquests.HRPositionList())
            {
                HRPositionObject.Add(new HRPosition { Code = kvp.Key, Description = kvp.Value });
            }

            return JsonConvert.SerializeObject(HRPositionObject);
        }          
        public static string SubmitTraining()
        {
            string _Status = "";
            string _Message = "";

            string SubmitTrainingResponse = TrainingsXMLRequests.SubmitTraining(TrainingNo);

            dynamic json = JObject.Parse(SubmitTrainingResponse);

            _Status = json.Status;
            _Message = json.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }         
        public static string Approve()
        {
            string _Status = "";
            string _Message = "";

            string Username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string ApproveTrainingRequestResponse = TrainingsXMLRequests.ApproveTrainingRequest(TrainingNo, Username);

            dynamic json = JObject.Parse(ApproveTrainingRequestResponse);

            _Status = json.Status;
            _Message = json.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }         
        public static string Reject()
        {
            string _Status = "";
            string _Message = "";

            string Username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string ApproveTrainingRequestResponse = TrainingsXMLRequests.RejectTrainingRequest(TrainingNo, Username);

            dynamic json = JObject.Parse(ApproveTrainingRequestResponse);

            _Status = json.Status;
            _Message = json.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}