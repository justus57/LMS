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
    public class EditTrainingRequestController : Controller
    {
        static string TrainingNo = null;
        EditTrainingRequest request = new EditTrainingRequest();
        // GET: EditTrainingRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditTrainingRequest()
        {

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
            //display

            request.TrainingDescription = _TrainingDescription;
            request.TrainingStartDateTime = _TrainingStartDateTime;
            request.TrainingEndDateTime  = _TrainingEndDateTime;
            request.Venue = _Venue;
            request.Room  = _Room;
            request.TrainingInstitution  = _TrainingInstitution;
            request.TrainingCost  = _TrainingCost;
            request.ApplicableTo = _ApplicableTo;
            request.CourseDescription = _CourseDescription;

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
        
        public JsonResult GetApplicableToEmployees(string param1)
        {
            List<Employee> employeeObject = new List<Employee>(); //TrainingNo

            foreach (var kvp in LeaveRecallForOtherXMLRequests.GetEmpoyeeList())
            {
                employeeObject.Add(new Employee { EmployeeCode = kvp.Key, EmployeeName = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(employeeObject),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetEmployeeList(string param1)
        {
            List<Employee> employeeObject = new List<Employee>();

            foreach (var kvp in LeaveRecallForOtherXMLRequests.GetEmpoyeeList())
            {
                employeeObject.Add(new Employee { EmployeeCode = kvp.Key, EmployeeName = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(employeeObject),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetOrgUnitList(string param1)
        {
            List<OrgUnit> OrgUnitObject = new List<OrgUnit>();

            foreach (var kvp in CreateAppraisalXMLREquests.GetOrgUnitList())
            {
                OrgUnitObject.Add(new OrgUnit { Code = kvp.Key, Name = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(OrgUnitObject),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult HRPositionUnitList(string param1)
        {
            List<HRPosition> HRPositionObject = new List<HRPosition>();

            foreach (var kvp in CreateAppraisalXMLREquests.HRPositionList())
            {
                HRPositionObject.Add(new HRPosition { Code = kvp.Key, Description = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(HRPositionObject), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
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
                //string No = TrainingNo;// "TRAINING000002";
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
                //Msg = TrainingNo;
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

                if (status == "000")
                {
                    Msg = "The training was succesfully updated";
                }
                else
                {
                    Msg = "An error occured. The training was not updated";
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

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
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
                string No = TrainingNo;// "TRAINING000002";
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
                //////submit
                string SubmitResponse = SubmitTraining(TrainingNo);
                dynamic jsonSubmitResponse = JObject.Parse(createTrainingXML);

                status = jsonSubmitResponse.Status;



                if (status == "000")
                {
                    Msg = jsonSubmitResponse.Msg;
                }
                else
                {
                    Msg = "An error occured. The training was not updated";
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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public static string SubmitTraining(string TrainingNo)
        {
            return TrainingsXMLRequests.SubmitTraining(TrainingNo);
        }
    }
}