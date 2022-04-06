using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class CreateAppraisalController : Controller
    {
        // GET: CreateAppraisal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateAppraisal()
        {
            Session["ErrorMessage"] = "";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "active";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            //check if user is logged
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                if ((object)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    LoadAppraisals("New");

                    //ApplicableTo.Items.Clear();
                    //ApplicableTo.Items.Insert(0, new ListItem("Position", "Position"));
                    //ApplicableTo.Items.Insert(0, new ListItem("Org. Unit", "OrgUnit"));
                    //ApplicableTo.Items.Insert(0, new ListItem("Individual Employee", "IndividualEmployee"));
                    //ApplicableTo.Items.Insert(0, new ListItem("All Employees", "AllEmployees"));
                    //ApplicableTo.Items.Insert(0, new ListItem(" ", ""));
                }
            }
            return View();
        }
        private void LoadAppraisals(string status)
        {

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable dt = new DataTable();

            if (status == "New")
            {
                dt = CreateAppraisalXMLREquests.GetAppraisalsToFill(username);
            }

            // DataTable 
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
            ViewBag.table = strText;
            //placeholder.Controls.Add(new Literal { Text = html.ToString() });
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
        
        public static string DeleteAppraisal(string param1)
        {
            string AppraisalHeaderNo = param1;
            string response = "";
            string status = "";
            string xmlresponse = CreateAppraisalXMLREquests.DeleteAppraisal(AppraisalHeaderNo);

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
      
        public static string Submit(string param1, string param2, string param3, string param4, string param5)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";
            ////
            string ApplicableTo = param1;
            string _ApplicableToPersons = param2;
            string AppraisalStartDay = param3;// AppFunctions.ConvertToNAVTime(param3); //Convert.ToDateTime(param3).ToString("MM/dd/yyyy");
            string AppraisalEndDay = param4;// AppFunctions.ConvertToNAVTime(param4); //Convert.ToDateTime(param4).ToString("MM/dd/yyyy");
            string AppraisalName = param5;
            AppraisalName = AppFunctions.EscapeInvalidXMLCharacters(AppraisalName);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] values = _ApplicableToPersons.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

                dictionary.Add(values[i].ToString().Trim('"'), values[i].ToString().Trim('"'));
            }


            try
            {
                string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalHeader");
                dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

                Status = jsonDocumentNo.Status;

                if (Status == "000")
                {
                    string DocumentNo = jsonDocumentNo.DocumentNo;

                    string createAppraisalXMLResponse = CreateAppraisalXMLREquests.CreateAppraisal(DocumentNo, username, AppraisalName, ApplicableTo, AppraisalStartDay, AppraisalEndDay);

                    dynamic json = JObject.Parse(createAppraisalXMLResponse);

                    Status = json.Status;
                    Msg = json.Msg;
                    string AppraisalHeaderNo = json.No;
                    //Msg = TrainingNo;
                    //insert TrainingMemberList
                    //loop throu dictionary and insert
                    foreach (var kvp in dictionary)
                    {

                        string ApplicableToPersons = kvp.Value;
                        string createTrainingMemebrListXML = CreateAppraisalXMLREquests.CreateAppraisalMembersList(AppraisalHeaderNo, ApplicableToPersons);
                    }

                }
                else
                {
                    Msg = jsonDocumentNo.Msg;
                }

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
       
        public static string FetchAppraisalDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string AppraisalHEaderNumber = param1;

            string createAppraisalXMLResponse = CreateAppraisalXMLREquests.GetAppraisalDetails(AppraisalHEaderNumber, username);

            return createAppraisalXMLResponse;
        }
        
        public static string ReleaseAppraisal(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";


            string AppraisalHeaderNo = param1;

            if (param2 == "1")
            {
                //fetch appraisal targets 
                //copy these to current appraisal
                string quenss = DefineAppraisalSectionsXMLRequests.GetQuestionsToFillJson("", "", "ExportAllTargets", "Section Here");

                dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

                foreach (var QuestionsToFill in stuff)
                {
                    string QuestnNumber = QuestionsToFill.QuestionNumber;
                    string QuestionDescription = QuestionsToFill.QuestionDescription;
                    string QuestionType = QuestionsToFill.PerformanceMeasurementType;
                    string weightScoreValue = QuestionsToFill.WeightScoreValue;
                    string AppraisalSection = QuestionsToFill.AppraisalSection;
                    //get new Appraisal Target Number      
                    string PMType = "";
                    if (QuestionType == "1")
                    {
                        PMType = "Description";
                    }
                    else if (QuestionType == "2")
                    {
                        PMType = "Options";
                    }
                    else if (QuestionType == "3")
                    {
                        PMType = "WeightedScore";
                    }


                    if (QuestionsToFill.QuestionDescription != "")
                    {
                        string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTarget");
                        dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);
                        Status = jsonDocumentNo.Status;
                        string QuestionNumber = "";


                        if (Status == "000")
                        {
                            QuestionNumber = jsonDocumentNo.DocumentNo;

                            string TargetResponse = CreateAppraisalQuestionXMLRequests.CreateNewAppraisalQuestion(QuestionNumber, username, AppraisalHeaderNo, QuestionDescription, PMType, weightScoreValue, AppraisalSection);

                        }
                        //foreach appraisal targets, fetch objectives



                        foreach (var QuestionObjectives in DefineAppraisalSectionsXMLRequests.GetQuestionObjectives("", QuestnNumber))
                        {
                            if (QuestionObjectives.Value.Length > 1)
                            {
                                string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTargetObjective");
                                dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                                Status = jsonObjectiveNo.Status;

                                if (Status == "000")
                                {
                                    string ObjectiveNo = jsonObjectiveNo.DocumentNo;
                                    string QuestionObjectiveNumber = ObjectiveNo;

                                    CreateAppraisalQuestionXMLRequests.CreateAppraisalQuestionObjective(username, AppraisalHeaderNo, QuestionObjectiveNumber, QuestionNumber, QuestionObjectives.Value);
                                }
                            }
                        }
                    }
                }

                //release here
                string xmlresponse = CreateAppraisalXMLREquests.ReleaseAppraisal(AppraisalHeaderNo);
                dynamic json = JObject.Parse(xmlresponse);
                Msg = json.Msg;
                Status = json.Status;
            }
            else
            {
                //release here
                string xmlresponse = CreateAppraisalXMLREquests.ReleaseAppraisal(AppraisalHeaderNo);
                dynamic json = JObject.Parse(xmlresponse);
                Msg = json.Msg;
                Status = json.Status;
            }

            var _RequestResponse = new RequestResponse
            {
                Message = Msg,
                Status = Status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}