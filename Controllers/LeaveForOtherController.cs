using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class LeaveForOtherController : Controller
    {
        private dynamic items;
        private dynamic keyList;

        // GET: LeaveForOther
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaveForOther()
        {
            System.Web.HttpContext.Current.Session["logged"] = "yes";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "active";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["Username"] = "";
            var username1 = System.Web.HttpContext.Current.Session["PayrollNo"];
            if (Session["PayrollNo"] != null)
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
                        items.Add(new SelectListItem { Text = keyList[i]});
                        
                    }
                  
                    ViewBag.Leaves = items;
                    GetEmployeeList();
                }
                catch (Exception es)
                {
                    Console.Write(es);
                }
            }    
            return View();
        }
        public void GetEmployeeList()
        {
            var castedDico = LeaveForOtherXMLRequests.GetEmpoyeeList();
            var array = castedDico.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var myList = new List<KeyValuePair<string, string>>(array);
            Dictionary<string, string> dictionary = new Dictionary<string, string>(array);
            List<SelectListItem> itemz = new List<SelectListItem>();
            foreach (var val in myList)
            {
                itemz.Add(new SelectListItem { Value = val.Key, Text = val.Value });
            }
            ViewBag.employees = itemz;
        }
        [WebMethod]
        public static string GetUserLeaves(string param1)
        {
            string UserLeavesresponseString = LeaveForOtherXMLRequests.GetUserLeaves(param1);

            List<LeaveTypes> leavetype = new List<LeaveTypes>();

            /////break dynamic json and put it in a list, then serialize the list to json object
            foreach (var kvp in AppFunctions.BreakDynamicJSON(UserLeavesresponseString))
            {
                leavetype.Add(new LeaveTypes { LeaveCode = kvp.Key, LeaveName = kvp.Value });
            }

            return JsonConvert.SerializeObject(leavetype);

        }
        [WebMethod]
        public static string GetLeaveDetails(string param1, string param2)
        {
            string OpeningBalance = "";
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
                string LeaveDetailsresponseString = LeaveForOtherXMLRequests.GetLeaveCodeDetails(param1, param2);
                dynamic json = JObject.Parse(LeaveDetailsresponseString);

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
            catch (Exception e)
            {
                Console.Write(e);
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
        [WebMethod]
        public static string GetLeaveState(string param1, string param2, string param3)
        {
            string employeeNo = System.Web.HttpContext.Current.Session["Username"].ToString(); ;
            string causeofAbsenceCode = param1;
            string startDate = param2;
            string endDate = param3;
            bool validity = false;
            string Msg = null;
            string Return_Date = null;
            string Qty = null;

            try
            {
                string LeaveStateResponseString = LeaveForOtherXMLRequests.GetLeaveState(employeeNo, causeofAbsenceCode, startDate, endDate);

                dynamic json = JObject.Parse(LeaveStateResponseString);
                string status = json.Status;

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
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _LeaveQuantityAndReturnDate = new LeaveQuantityAndReturnDate
            {
                Quantity = Qty,
                ReturnDate = AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return JsonConvert.SerializeObject(_LeaveQuantityAndReturnDate);
        }
        [WebMethod]
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
                string LeaveForOtherResponse = LeaveForOtherXMLRequests.GetLeaveEndDateAndReturnDate(employeeNo, causeofAbsenceCode, startDate, qty);
                //json 
                dynamic json = JObject.Parse(LeaveForOtherResponse);
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

            var _LeaveEndDateAndReturnDate = new LeaveEndDateAndReturnDate
            {
                EndDate = AppFunctions.ConvertTime(EndDate),
                ReturnDate = AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return JsonConvert.SerializeObject(_LeaveEndDateAndReturnDate);
        }
        [WebMethod]
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string EmployeeID = param7.Trim();
            string EmployeeName = param8.Trim();
            string DocumentNo = GetDocumentNumber(EmployeeID);
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
            string Message = "";
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string documentpath = folderPath + param9;

            try
            {

                string x = LeaveForOtherXMLRequests.SaveLeaveApplicationForOther(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, username, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                UploadAttachment(documentpath, DocumentNo);

                Message = "Leave application for " + EmployeeName + " has been saved successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            var LeaveSubmitResponses = new RequestResponse
            {

                Message = Message
            };
            return JsonConvert.SerializeObject(LeaveSubmitResponses);
        }
        [WebMethod]
        public static string Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            //get Leave number
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            DateTime _LeaveStartDay = AppFunctions.GetDateTime(param4);
            string response = null;
            string status = null;

            try
            {
                //can user apply a backdated Leaave?
                string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();

                if (_LeaveStartDay < DateTime.Today && CanApplyBackdatedLeave == "FALSE")
                {
                    response = "You cannot apply a backdated leave";
                    status = "999";
                }
                else
                {
                    string EmployeeID = param7.Trim();
                    string EmployeeName = param8.Trim();
                    string DocumentNo = GetDocumentNumber(EmployeeID);

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

                    string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
                    string documentpath = folderPath + param9;


                    LeaveForOtherXMLRequests.SaveLeaveApplicationForOther(DocumentNo, EmployeeID, EmployeeName, RequestDate, DateCreated, username, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                    UploadAttachment(documentpath, DocumentNo);


                    ///send approval request here

                    string ApprovalRequestResponseString = LeaveForOtherXMLRequests.SendApprovalRequest(DocumentNo);

                    dynamic json = JObject.Parse(ApprovalRequestResponseString);

                    response = json.Msg;
                    status = json.Status;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        private static string GetDocumentNumber(string EmployeeNumber)
        {
            string DocumentNo = null;
            try
            {
                string DocumentNumberResponseString = LeaveForOtherXMLRequests.GetDocumentNumber(EmployeeNumber);
                dynamic json = JObject.Parse(DocumentNumberResponseString);
                DocumentNo = json.DocumentNo;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return DocumentNo;
        }
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

    }
}