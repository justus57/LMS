using LMS.CustomsClasses;
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
    public class EditAppraisalController : Controller
    {
        // GET: EditAppraisal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditAppraisal()
        {
            return View();
        }
        static string _AppraisalHeader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    ApplicableTo.Items.Clear();
                    ApplicableTo.Items.Insert(0, new ListItem("Position", "POSITION"));
                    ApplicableTo.Items.Insert(0, new ListItem("Org. Unit", "ORGUNIT"));
                    ApplicableTo.Items.Insert(0, new ListItem("Individual Employee", "INDIVIDUALEMPLOYEE"));
                    ApplicableTo.Items.Insert(0, new ListItem("All Employees", "ALLEMPLOYEES"));
                    ApplicableTo.Items.Insert(0, new ListItem(" ", ""));

                    string s = Request.QueryString["id"].Trim();

                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        if (s == "")
                        {
                            Response.Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            string AppraisalHeader = AppFunctions.Base64Decode(s);
                            _AppraisalHeader = AppraisalHeader;
                            //load appraisal

                            FetchAppraisalDetails(AppraisalHeader);

                        }
                    }
                }
            }
        }
        public void FetchAppraisalDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string AppraisalHEaderNumber = param1;
            string createAppraisalXMLResponse = CreateAppraisalXMLREquests.GetAppraisalDetails(AppraisalHEaderNumber, username);
            dynamic json = JObject.Parse(createAppraisalXMLResponse);

            string AppraisalNameText = json.AppraisalDescription;
            string AppraisalStartDayText = json.AppraisalStartDate;
            string AppraisalEndDayText = json.AppraisalEndDate;
            string AppraisalApplicableTo = json.AppraisalApplicableTo;

            AppraisalName.Text = AppraisalNameText;
            AppraisalStartDay.Text = Convert.ToDateTime(AppraisalStartDayText).ToString("MM/dd/yyyy");
            AppraisalEndDay.Text = Convert.ToDateTime(AppraisalEndDayText).ToString("MM/dd/yyyy");

            //applicable To
            ApplicableTo.SelectedValue = AppraisalApplicableTo;

        }
        public static string GetAppraisalMemberList()
        {
            var publicationTable = new List<object>();

            foreach (var kvp in AppraisalsXMLRequests.GetAppraisalMemberList(_AppraisalHeader))
            {
                publicationTable.Add(new[] { kvp.Value });
            }

            return (new JavaScriptSerializer()).Serialize(publicationTable);
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
        
        public static string SubmitAppraisal(string param1, string param2, string param3, string param4, string param5)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string response = "";
            string status = "";
            string ApplicableToPersons = param1;
            string ApplicableTo = param2;
            string AppraisalStartDay = param3;
            string AppraisalEndDay = param4;
            string AppraisalName = param5;
            AppraisalName = AppFunctions.EscapeInvalidXMLCharacters(AppraisalName);

            string xmlresponse = CreateAppraisalXMLREquests.UpdateAppraisal(_AppraisalHeader, username, AppraisalName, ApplicableTo, AppraisalStartDay, AppraisalEndDay);

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
    }
}