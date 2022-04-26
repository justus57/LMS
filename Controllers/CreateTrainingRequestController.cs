using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class CreateTrainingRequestController : Controller
    {
        // GET: CreateTrainingRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateTrainingRequest()
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
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";

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
                }
            }
            return View();
        }
        private void LoadApplicableTo()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Position", Value = "Position" });

            items.Add(new SelectListItem { Text = "Org. Unit", Value = "Org. Unit" });

            items.Add(new SelectListItem { Text = "Individual Employee", Value = "Individual Employee" });

            items.Add(new SelectListItem { Text = "All Employees", Value = "All Employees" });

            items.Add(new SelectListItem { Text = "", Value = "" });

            ViewBag.LoadApplicableTo = items;
            
        }

        private void LoadRequirementOfTraining()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Organizational/Departmental Needs", Value = "ORGANIZATIONALDEPARTMENTALNEEDS" });

            items.Add(new SelectListItem { Text = "Identification on individual need", Value = "INDIVIDUALNEED" });

            items.Add(new SelectListItem { Text = "Identification on performance need", Value = "PERFORMANCENEED" });

            items.Add(new SelectListItem { Text = "Professional requirement", Value = "PROFESSIONALREQUIREMENT" });

            items.Add(new SelectListItem { Text = "", Value = "" });

            ViewBag.LoadRequirementOfTraining = items;
           
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
        
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();



            string status = "";
            string Msg = "";

            string _CourseDescription = param1;
            _CourseDescription = AppFunctions.EscapeInvalidXMLCharacters(_CourseDescription);
            string _TrainingDescription = param2;
            _TrainingDescription = AppFunctions.EscapeInvalidXMLCharacters(_TrainingDescription);
            string _TrainingStartDateTime = param3;
            string _TrainingEndDateTime = param4;
            string _Venue = param5;
            _Venue = AppFunctions.EscapeInvalidXMLCharacters(_Venue);
            string _Room = param6;
            _Room = AppFunctions.EscapeInvalidXMLCharacters(_Room);
            string _TrainingInstitution = param7;
            _TrainingInstitution = AppFunctions.EscapeInvalidXMLCharacters(_TrainingInstitution);
            string _TrainingCost = param8;
            string _ApplicableToPersons = param9;
            string _ApplicableTo = param10;
            string _RequirementOfTraining = param11;

            //foreach (NameValue nv in formVars)
            //{
            //    if (nv.name.StartsWith("CourseDescription"))
            //    {
            //        _CourseDescription = nv.value;
            //    }
            //    if (nv.name.StartsWith("TrainingDescription"))
            //    {
            //        _TrainingDescription = nv.value;
            //    }
            //    if (nv.name.StartsWith("TrainingStartDateTime"))
            //    {
            //        _TrainingStartDateTime = nv.value;
            //    }
            //    if (nv.name.StartsWith("TrainingEndDateTime"))
            //    {
            //        _TrainingEndDateTime = nv.value;
            //    }
            //    if (nv.name.StartsWith("Venue"))
            //    {
            //        _Venue = nv.value;
            //    }
            //    if (nv.name.StartsWith("Room"))
            //    {
            //        _Room = nv.value;
            //    }
            //    if (nv.name.StartsWith("TrainingInstitution"))
            //    {
            //        _TrainingInstitution = nv.value;
            //    }
            //    if (nv.name.StartsWith("TrainingCost"))
            //    {
            //        _TrainingCost = nv.value;
            //    }
            //    if (nv.name.StartsWith("ApplicableToEmployee"))
            //    {
            //        _ApplicableToPersons = nv.value;
            //    }
            //    if (nv.name.StartsWith("ApplicableTo"))
            //    {
            //        _ApplicableTo = nv.value;
            //    }
            //    if (nv.name.StartsWith("RequirementOfTraining"))
            //    {
            //        _RequirementOfTraining = nv.value;
            //    }
            //}

            _CourseDescription = AppFunctions.EscapeInvalidXMLCharacters(_CourseDescription);
            _TrainingDescription = AppFunctions.EscapeInvalidXMLCharacters(_TrainingDescription);
            _Venue = AppFunctions.EscapeInvalidXMLCharacters(_Venue);
            _Room = AppFunctions.EscapeInvalidXMLCharacters(_Room);
            _TrainingInstitution = AppFunctions.EscapeInvalidXMLCharacters(_TrainingInstitution);





            string TrainingStartDate = AppFunctions.ConvertToNavDate(_TrainingStartDateTime);
            string TrainingStartTime = AppFunctions.ConvertToNavTime(_TrainingStartDateTime);

            string TrainingEndDate = AppFunctions.ConvertToNavDate(_TrainingEndDateTime);
            string TrainingEndTime = AppFunctions.ConvertToNavTime(_TrainingEndDateTime);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] values = _ApplicableToPersons.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

                dictionary.Add(values[i].ToString().Trim('"'), values[i].ToString().Trim('"'));
            }



            try
            {
                string TrainingNo = "";
                string DocumentNoXMLResponse = TrainingsXMLRequests.GetDocumentNo("TrainingHeader");
                dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

                status = jsonDocumentNo.Status;

                if (status == "000")
                {
                    TrainingNo = jsonDocumentNo.DocumentNo;
                }
                // "TRAINING000002";
                string Description = _TrainingDescription;
                string PlannedStartDate = TrainingStartDate;
                string PlannedStartTime = TrainingStartTime;
                string PlannedEndDate = TrainingEndDate;
                string PlannedEndTime = TrainingEndTime;
                string TotalCost = _TrainingCost;
                string NoSeries = "";
                string CourseCode = "";
                string CourseDescription = _CourseDescription;
                string Trainer = "";
                string TrainerName = "";
                string Venue = _Venue;
                string Room = _Room;
                string TrainingInstitution = _TrainingInstitution;
                ///
                string ScheduledStartDate = TrainingStartDate;
                //string ScheduledStartTime = "";
                string ScheduledEndDate = TrainingStartDate;
                //string ScheduledEndTime = "";
                string ActualStartDate = TrainingStartDate;
                //string ActualStartTime = "";
                string ActualEndDate = TrainingStartDate;
                //string ActualEndTime = "";
                string CancellationCompletionDate = TrainingStartDate;
                string ProgressStatus = "";
                string LPONo = "";
                string Archived = "";
                string CancellationReason = "";
                string ActualCost = "0";
                string CreatedBy = username;
                string ApplicableTo = _ApplicableTo;
                status = "000";
                Msg = _RequirementOfTraining;

                string createTrainingXML = TrainingsXMLRequests.CreateTraining(TrainingNo, Description, PlannedStartDate, PlannedStartTime,
                                                                PlannedEndDate, PlannedEndTime, TotalCost, NoSeries, CourseCode, CourseDescription,
                                                                Trainer, TrainerName, Venue,
                                                                Room, TrainingInstitution, ProgressStatus, LPONo, Archived, CancellationReason,
                                                                ActualCost, CreatedBy, ApplicableTo, _RequirementOfTraining);

                dynamic json = JObject.Parse(createTrainingXML);

                status = json.Status;
                Msg = json.Msg;

                //insert TrainingMemberList
                //loop throu dictionary and insert
                foreach (var kvp in dictionary)
                {
                    string TrainingMemebrListNo = "";
                    string TrainingMemberListDocumentNoXMLResponse = TrainingsXMLRequests.GetDocumentNo("TrainingMemberList");
                    dynamic jsonTrainingMemberListDocumentNo = JObject.Parse(TrainingMemberListDocumentNoXMLResponse);

                    status = jsonTrainingMemberListDocumentNo.Status;

                    if (status == "000")
                    {
                        TrainingMemebrListNo = jsonTrainingMemberListDocumentNo.DocumentNo;

                        string ApplicableToPersons = kvp.Value;
                        string createTrainingMemebrListXML = TrainingsXMLRequests.CreateTrainingMemberList(TrainingMemebrListNo, TrainingNo, ApplicableToPersons);
                    }
                }

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }
            ////string status = "000";
            ////string Msg = "000 "+param1 +":"+ string.Join(";", dictionary.Select(x => x.Key + "=" + x.Value).ToArray());// + JsonConvert.DeserializeObject<string>(param1);
            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string status = "";
            string Msg = "";

            string _CourseDescription = param1;
            _CourseDescription = AppFunctions.EscapeInvalidXMLCharacters(_CourseDescription);
            string _TrainingDescription = param2;
            _TrainingDescription = AppFunctions.EscapeInvalidXMLCharacters(_TrainingDescription);
            string _TrainingStartDateTime = param3;
            string _TrainingEndDateTime = param4;
            string _Venue = param5;
            _Venue = AppFunctions.EscapeInvalidXMLCharacters(_Venue);
            string _Room = param6;
            _Room = AppFunctions.EscapeInvalidXMLCharacters(_Room);
            string _TrainingInstitution = param7;
            _TrainingInstitution = AppFunctions.EscapeInvalidXMLCharacters(_TrainingInstitution);
            string _TrainingCost = param8;
            string _ApplicableToPersons = param9;
            string _ApplicableTo = param10;
            string _RequirementOfTraining = param11;


            string TrainingStartDate = AppFunctions.ConvertToNavDate(_TrainingStartDateTime);
            string TrainingStartTime = AppFunctions.ConvertToNavTime(_TrainingStartDateTime);

            string TrainingEndDate = AppFunctions.ConvertToNavDate(_TrainingEndDateTime);
            string TrainingEndTime = AppFunctions.ConvertToNavTime(_TrainingEndDateTime);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] values = _ApplicableToPersons.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

                dictionary.Add(values[i].ToString().Trim('"'), values[i].ToString().Trim('"'));
            }



            try
            {
                string TrainingNo = "";
                string DocumentNoXMLResponse = TrainingsXMLRequests.GetDocumentNo("TrainingHeader");
                dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

                status = jsonDocumentNo.Status;

                if (status == "000")
                {
                    TrainingNo = jsonDocumentNo.DocumentNo;
                }
                string Description = _TrainingDescription;
                string PlannedStartDate = TrainingStartDate;
                string PlannedStartTime = TrainingStartTime;
                string PlannedEndDate = TrainingEndDate;
                string PlannedEndTime = TrainingEndTime;
                string TotalCost = _TrainingCost;
                string NoSeries = "";
                string CourseCode = "";
                string CourseDescription = _CourseDescription;
                string Trainer = "";
                string TrainerName = "";
                string Venue = _Venue;
                string Room = _Room;
                string TrainingInstitution = _TrainingInstitution;
                ///
                string ScheduledStartDate = TrainingStartDate;
                //string ScheduledStartTime = "";
                string ScheduledEndDate = TrainingStartDate;
                //string ScheduledEndTime = "";
                string ActualStartDate = TrainingStartDate;
                //string ActualStartTime = "";
                string ActualEndDate = TrainingStartDate;
                //string ActualEndTime = "";
                string CancellationCompletionDate = TrainingStartDate;
                string ProgressStatus = "";
                string LPONo = "";
                string Archived = "";
                string CancellationReason = "";
                string ActualCost = "0";
                string CreatedBy = username;
                string ApplicableTo = _ApplicableTo;

                string createTrainingXML = TrainingsXMLRequests.CreateTraining(TrainingNo, Description, PlannedStartDate, PlannedStartTime,
                                                                PlannedEndDate, PlannedEndTime, TotalCost, NoSeries, CourseCode, CourseDescription,
                                                                Trainer, TrainerName, Venue,
                                                                Room, TrainingInstitution, ProgressStatus, LPONo, Archived, CancellationReason,
                                                                ActualCost, CreatedBy, ApplicableTo, _RequirementOfTraining);
                dynamic json = JObject.Parse(createTrainingXML);

                status = json.Status;
                Msg = json.Msg;
                //insert TrainingMemberList
                //loop throu dictionary and insert
                foreach (var kvp in dictionary)
                {

                    string TrainingMemebrListNo = "";
                    string TrainingMemberListDocumentNoXMLResponse = TrainingsXMLRequests.GetDocumentNo("TrainingMemberList");
                    dynamic jsonTrainingMemberListDocumentNo = JObject.Parse(TrainingMemberListDocumentNoXMLResponse);

                    status = jsonTrainingMemberListDocumentNo.Status;

                    if (status == "000")
                    {
                        TrainingMemebrListNo = jsonTrainingMemberListDocumentNo.DocumentNo;

                        string ApplicableToPersons = kvp.Value;
                        string createTrainingMemebrListXML = TrainingsXMLRequests.CreateTrainingMemberList(TrainingMemebrListNo, TrainingNo, ApplicableToPersons);
                    }
                }
                ////submit
                string SubmitResponse = SubmitTraining(TrainingNo);

                dynamic jsonSubmitResponse = JObject.Parse(SubmitResponse);

                status = jsonSubmitResponse.Status;

                if (status == "000")
                {
                    Msg = "The training was successfully created and an approval request sent to the approver.";
                }

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        public static string SubmitTraining(string TrainingNo)
        {
            return TrainingsXMLRequests.SubmitTraining(TrainingNo);
        }
    }
}