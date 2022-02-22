using LMS.CustomsClasses;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI.WebControls;
using LMS.Models;
using System.Web.Script.Serialization;

namespace LMS.Controllers
{
    public class LeaveRecallApplicationController : Controller
    {
        // GET: LeaveRecallApplication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LeaveRecallApplication()
        {

            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "active";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["Logged"] = "";
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
                }
            }
          
                //LoadLeaves();

            return View();
        }
        [HttpPost]
        //private void LoadLeaves()
        //{
        //    try
        //    {
        //        string username = System.Web.HttpContext.Current.Session["Username"].ToString();

        //        string GetLeavesResponseString = RecallApplicationXMLRequests.GetLeaves(username);

        //        Leave_Type.Items.Clear();

        //        foreach (var kvp in AppFunctions.BreakDynamicJSON(GetLeavesResponseString))
        //        {
        //            Leave_Type.Items.Insert(0, new ListItem(kvp.Value, kvp.Key));
        //        }

        //        Leave_Type.Items.Insert(0, new ListItem(" ", ""));
        //    }
        //    catch (Exception es)
        //    {
        //        Console.Write(es);
        //    }
        //}
        [WebMethod]
        public static string GetLeaveDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string OpeningBalance = "";
            string Entitled = "";
            string Accrued = "";
            string LeaveTaken = "";
            string Remaining = "";
            string Description = "";
            string LeaveCode = "";

            try
            {
                string GetLeaveDetailsResponseString = RecallApplicationXMLRequests.GetLeaveDetails(username, param1);
                dynamic json = JObject.Parse(GetLeaveDetailsResponseString);

                OpeningBalance = json.OpeningBalance;
                Entitled = json.Entitled;
                Accrued = json.Accrued;
                LeaveTaken = json.LeaveTaken;
                Remaining = json.Remaining;
                Description = json.Description;
                LeaveCode = json.Code;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
            var _LeaveCodeDetails = new LeaveCodeDetails
            {
                Accrued = Accrued,
                Description = Description,
                EntitledDays = Entitled,
                LeaveCode = LeaveCode,
                LeaveTaken = LeaveTaken,
                OpeningBalance = OpeningBalance,
                Remaining = Remaining
            };
            return JsonConvert.SerializeObject(_LeaveCodeDetails);
        }
        [WebMethod]
        public static string LoadApprovedLeaves(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable
            List<AprrovedLeave> respmsg = new List<AprrovedLeave>();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            try
            {
                string ApprovedLeavesResponse = RecallApplicationXMLRequests.GetApprovedLeaves(username, param1);

                var LeaveRecords = ser.Deserialize<List<AprrovedLeave>>(ApprovedLeavesResponse);

                foreach (var LeaveDetail in LeaveRecords)
                {
                    respmsg.Add(new AprrovedLeave { LeaveNo = LeaveDetail.LeaveNo, StartDate = AppFunctions.ConvertTime(LeaveDetail.StartDate), EndDate = AppFunctions.ConvertTime(LeaveDetail.EndDate), Qty = LeaveDetail.Qty });
                }

            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return JsonConvert.SerializeObject(respmsg);
        }
        [WebMethod]
        public static string ApprovedLeaveDetails(string param1)
        {
            string startDate = "";
            string enddate = "";
            string returndate = "";
            string quantity = "";
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable

                string ApprovedLeaveDetails = RecallApplicationXMLRequests.GetApprovedLeaveDetails(username, param1);

                dynamic json = JObject.Parse(ApprovedLeaveDetails);
                startDate = json.StartDate;
                enddate = json.EndDate;
                returndate = json.ReturnDate;
                quantity = json.Qty;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
            var Leave = new SelectedLeaveEndDateAndReturnDate
            {
                EndDate = AppFunctions.ConvertTime(enddate),
                Quantity = quantity,
                ReturnDate = AppFunctions.ConvertTime(returndate),
                StartDate = AppFunctions.ConvertTime(startDate)
            };
            return JsonConvert.SerializeObject(Leave);
        }
        [WebMethod]
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6)
        {
            string Msg = "";
            try
            {
                string DocumentNo = GetDocumentNumber();

                if (DocumentNo.Length > 15)
                {
                    Msg = "Leave Recall No.s must be setup";
                }
                else
                {
                    string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
                    string EmployeeName = System.Web.HttpContext.Current.Session["UserFullName"].ToString();
                    string RequestDate = DateTime.Now.ToString("dd/MM/yyyy");//d/m/Y
                    string DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
                    string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
                    string ReturnDate = param1;
                    string LeaveCode = param2;
                    string Description = param3;
                    Description = AppFunctions.EscapeInvalidXMLCharacters(Description);
                    string StartDate = param4;
                    string EndDate = param5;
                    string LeaveDays = param6;//qty

                    RecallApplicationXMLRequests.SaveLeaveRecallApplication(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                    Msg = "Leave Recall application has been saved successfully";
                }


            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = Msg
            };
            return JsonConvert.SerializeObject(_RequestResponse);
        }
        private static string GetDocumentNumber()
        {
            //get Leave number
            string DocumentNumber = null;
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                string GetLeaveNewNoReqponse = RecallApplicationXMLRequests.GetDocumentNumber(username);
                dynamic json = JObject.Parse(GetLeaveNewNoReqponse);

                string status = json.Status;

                if (status != "1200")
                {
                    DocumentNumber = json.DocumentNo;
                }
                else
                {
                    DocumentNumber = "Leave Recall No.s must be setup";
                }

                //
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return DocumentNumber;
        }
        [WebMethod]
        public static string Submit(string param1, string param2, string param3, string param4, string param5, string param6)
        {
            string response = null;
            string status = null;

            //send XML submit request

            try
            {
                string DocumentNo = GetDocumentNumber();
                string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
                string EmployeeName = System.Web.HttpContext.Current.Session["UserFullName"].ToString();
                string RequestDate = DateTime.Now.ToString("dd/MM/yyyy");//d/m/Y
                string DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
                string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
                string ReturnDate = param1;
                string LeaveCode = param2;
                string Description = param3;
                Description = AppFunctions.EscapeInvalidXMLCharacters(Description);
                string StartDate = param4;
                string EndDate = param5;
                string LeaveDays = param6;//qty

                RecallApplicationXMLRequests.SaveLeaveRecallApplication(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);
                //SendApprovalRequest
                string ApprovalRequestResponse = RecallApplicationXMLRequests.SendApprovalRequest(DocumentNo);
                dynamic json = JObject.Parse(ApprovalRequestResponse);

                response = json.Msg;
                status = json.Status;

            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}