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
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace LMS.Controllers
{
    public class LeaveApplicationController : Controller
    {
        // GET: LeaveApplication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaveApplication()
        {
            var username1 = System.Web.HttpContext.Current.Session["PayrollNo"];
            if (Session["Username"] != null)
            {
                string username = Convert.ToString(username1);

                string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveLookups xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <lookupType>CauseOfAbsenceCode</lookupType> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                 </ReturnLeaveLookups> 
                             </Body>
                         </Envelope>";

                string response = Assest.Utility.CallWebService(req);

                string array = Assest.Utility.GetJSONResponse(response);

                dynamic json = JObject.Parse(array);

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                try
                {
                    array = array.Substring(1, array.Length - 2);
                    string[] resultArray = array.Split(',');

                    foreach (var item in resultArray)
                    {
                        string[] result = item.ToString().Split(':');
                        dictionary.Add(result[0].ToString().Trim('"'), result[1].ToString().Trim('"'));
                    }

                    List<string> keyList = new List<string>(dictionary.Keys);
                    List<SelectListItem> items = new List<SelectListItem>();

                    for (int i = 0; i < keyList.Count; i++)
                    {
                        items.Add(new SelectListItem { Text = keyList[i], Selected = true });
                    }
                    ViewBag.Leaves = keyList;
                }
                catch (Exception es)
                {
                    Console.Write(es);
                }
            }
            else
            {
                Response.Redirect("/Account/Login");
            }
            return View();
        }
        [HttpPost]
        public static string GetUserLeaves(string param1)
        {
            string UserLeavesresponseString = LeaveForOtherXMLRequests.GetUserLeaves(param1);
            List<LeaveTypes> leavetype = new List<LeaveTypes>();
            foreach (var kvp in AppFunctions.BreakDynamicJSON(UserLeavesresponseString))
            {
                leavetype.Add(new LeaveTypes { LeaveCode = kvp.Key, LeaveName = kvp.Value });
            }
            return JsonConvert.SerializeObject(leavetype);
        }
        [HttpPost]
        public static string GetLeaveDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable
            string OpeningBalance = null;
            string Entitled = "";
            string Accrued = "";
            string LeaveTaken = "";
            string Remaining = "";
            string Description = "";
            string LeaveCode = "";
            string RequiresAttachment = "";
            string AttachmentMandatory = "";
            try
            {
                string SelectedLeaveDetails = LeaveApplicationXMLRequests.GetSelectedLeaveDetails(username, param1);

                dynamic json = JObject.Parse(SelectedLeaveDetails);

                OpeningBalance = json.OpeningBalance;
                Entitled = json.Entitled;
                Accrued = json.Accrued;
                LeaveTaken = json.LeaveTaken;
                Remaining = json.Remaining;
                Description = json.Description;
                LeaveCode = json.Code;
                RequiresAttachment = json.RequiresAttachment;
                AttachmentMandatory = json.AttachmentMandatory;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var Leave = new LeaveCodeDetails
            {
                Accrued = Accrued,
                Description = Description,
                EntitledDays = Entitled,
                LeaveCode = LeaveCode,
                LeaveTaken = LeaveTaken,
                OpeningBalance = OpeningBalance,
                Remaining = Remaining,
                RequiresAttachment = RequiresAttachment,
                AttachmentMandatory = AttachmentMandatory
            };
            return JsonConvert.SerializeObject(Leave);
        }     
        [HttpPost]
        public static string GetLeaveState(string param1, string param2, string param3)
        {
            string employeeNo = System.Web.HttpContext.Current.Session["Username"].ToString(); ;
            string causeofAbsenceCode = param1;
            string startDate = param2;
            string endDate = param3;

            string GetLeaveStateresponse = LeaveApplicationXMLRequests.GetLeaveQuantityAndReturnDate(employeeNo, causeofAbsenceCode, startDate, endDate);
            //{"Status":"000","EndDate":"10","ReturnDate":"22102018"}
            dynamic json = JObject.Parse(GetLeaveStateresponse);

            bool validity = false;
            string Msg = null;
            string status = json.Status;
            string Return_Date = null;
            string Qty = null;

            if (status == "000")
            {
                validity = true;
                Msg = "Successful";
                Return_Date = json.ReturnDate;
                Qty = json.EndDate;
            }
            else
            {
                validity = false;
                Msg = "Failed";

            }


            var Leave = new CustomsClasses.LeaveQuantityAndReturnDate
            {
                Quantity = Qty,
                ReturnDate = CustomsClasses.AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return JsonConvert.SerializeObject(Leave);
        }        
        [HttpPost]
        public static string GetLeaveEndDateAndReturnDate(string param1, string param2, string param3)
        {
            string employeeNo = System.Web.HttpContext.Current.Session["Username"].ToString(); ;
            string causeofAbsenceCode = param1;
            string startDate = param2;
            string qty = param3;
            bool validity = false;
            string Msg = null;
            string Return_Date = null;
            string EndDate = null;

            try
            {
                string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetEndDateAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                                    <startDate>" + startDate + @"</startDate>
                                    <qty>" + qty + @"</qty>
                                </GetEndDateAndReturnDate>
                            </Body>
                        </Envelope>";

                var response = Assest.Utility.CallWebService(req);
                string GetLeaveEndDateAndReturnDateResponseString = Assest.Utility.GetJSONResponse(response);

                //json 
                dynamic json = JObject.Parse(GetLeaveEndDateAndReturnDateResponseString);
                string status = json.Status;

                if (status == "000")
                {
                    validity = true;
                    Msg = "Successful";
                    Return_Date = json.ReturnDate;
                    EndDate = json.EndDate;
                }
                else
                {
                    validity = false;
                    Msg = "Failed";
                }

            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var LeaveEndReturnDates = new CustomsClasses.LeaveEndDateAndReturnDate
            {
                EndDate = CustomsClasses.AppFunctions.ConvertTime(EndDate),
                ReturnDate = CustomsClasses.AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return JsonConvert.SerializeObject(LeaveEndReturnDates);
        }
        [HttpPost]
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            string response = "";
            string status = "000";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = "";
            string DocumentNoResponse = GetDocumentNumber();
            dynamic json = JObject.Parse(DocumentNoResponse);
            status = json.Status;

            if (status == "000")
            {

                DocumentNo = json.DocumentNo;
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
                string uploadpath = param7; //param7.Replace(@"\", @"\\");
                string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
                string documentpath = folderPath + param7;


                try
                {
                    string mMessage = LeaveApplicationXMLRequests.SaveLeaveApplication(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                    UploadAttachment1(documentpath, DocumentNo);

                    response = "Leave application has been saved successfully";
                }
                catch (Exception es)
                {
                    response = es.ToString();
                    status = "999";
                }
            }
            else
            {
                response = json.Msg;
                status = "999";
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,

                Status = status
            };
            return JsonConvert.SerializeObject(_RequestResponse);
        }
        public static void UploadAttachment1(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string UploadPath = param1;//full path+file name
            string DocumentNo = param2;
            //save attachment if sick leave
            LeaveApplicationXMLRequests.UploadFile(DocumentNo, UploadPath);
            //if uploaded delete file from uploads folder
            if (System.IO.File.Exists(UploadPath))
            {
                System.IO.File.Delete(UploadPath);
            }
        }
        [HttpPost]
        public static void UploadAttachment(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string UploadPath = param1;//full path+file name
            string DocumentNo = param2;
            //save attachment if sick leave
            LeaveApplicationXMLRequests.UploadFile(DocumentNo, UploadPath);
            //if uploaded delete file from uploads folder
            if (System.IO.File.Exists(UploadPath))
            {
                System.IO.File.Delete(UploadPath);
            }
        }
        [HttpPost]
        public static string Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            //get Leave number 
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = "";
            string response = null;
            string status = null;

            DateTime LeaveStartDay = AppFunctions.GetDateTime(param4);

            //can user apply a backdated Leaave?
            string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();


            if (LeaveStartDay < DateTime.Today && CanApplyBackdatedLeave == "FALSE")
            {
                status = "999";
                response = "Leave Start Date must be on or later than the current date";
            }
            else
            {

                string DocumentNoResponse = GetDocumentNumber();
                dynamic json = JObject.Parse(DocumentNoResponse);
                status = json.Status;

                if (status == "000")
                {
                    DocumentNo = json.DocumentNo;
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
                    string uploadpath = param7; //param7.Replace(@"\", @"\\");
                    string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
                    string documentpath = folderPath + param7;


                    try
                    {
                        string mMessage = LeaveApplicationXMLRequests.SaveLeaveApplication(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                        UploadAttachment(documentpath, DocumentNo);

                        //SendApprovalRequest
                        string responseString = WebserviceConfig.ObjNav.SendApprovalRequest("Absence", DocumentNo);

                        dynamic jsonSendSubmitRequest = JObject.Parse(responseString);
                        response = jsonSendSubmitRequest.Msg;

                        status = json.Status;

                        if (status == "000")
                        {
                            response = "Your leave application has been successfully submitted and an approval request to your supervisor";
                        }
                        else
                        {
                            response = json.Msg;

                        }
                    }
                    catch (Exception es)
                    {
                        response = es.Message;
                        status = "999";
                        AppFunctions.WriteLog(es.Message);
                    }
                }
                else
                {
                    response = json.Msg;
                    status = "999";
                }
            }


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        private static string GetDocumentNumber()
        {
            //get Leave number
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo></documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveSubType>Leave</leaveSubType>
                                </GetLeaveNewNo>
                            </Body>
                        </Envelope>";
            var response = Assest.Utility.CallWebService(req);
            var GetDocumentNumber = Assest.Utility.GetJSONResponse(response);
            return GetDocumentNumber;
        }
       public void FileUploadHandlers(HttpContext content)
        {
            FileUploadHandler.ProcessRequest(content);
        }
    }
    
}