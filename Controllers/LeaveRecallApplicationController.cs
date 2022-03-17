using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            var username1 = System.Web.HttpContext.Current.Session["PayrollNo"];
            if (Session["Username"] != null)
            {
                string username = Convert.ToString(username1);

                string array = LeaveApplicationXMLRequests.GetUserLeaves(username);
               
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
            return View();
        }

        public JsonResult GetLeaveDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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
            var _LeaveCodeDetails = new LeaveRecallApplication
            {
                Leave_Accrued_Days = Accrued,
                Leave_Entitled = Entitled,
                Leave_Days_Taken = LeaveTaken,
                Leave_Opening_Balance = OpeningBalance,
                Leave_Balance = Remaining,
                Description = Description,
                LeaveCode = LeaveCode,
            };
            return Json(JsonConvert.SerializeObject(_LeaveCodeDetails), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLeaveState(string param1, string param2, string param3)
        {
            string employeeNo = System.Web.HttpContext.Current.Session["PayrollNo"].ToString(); ;
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
                Qty = json.LeaveDaysApplied;
            }
            else
            {
                validity = false;
                Msg = "Failed";

            }

            var Leave = new LeaveRecallApplication
            {
                LeaveDaysApplied = Qty,
                ReturnDate = CustomsClasses.AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return Json(JsonConvert.SerializeObject(Leave), JsonRequestBehavior.AllowGet); ;
        }
        public JsonResult GetLeaveEndDateAndReturnDate(string param1, string param2, string param3)
        {
            string employeeNo = System.Web.HttpContext.Current.Session["PayrollNo"].ToString(); ;
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
                    EndDate = json.LeaveDaysApplied;
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

            var LeaveEndReturnDates = new LeaveRecallApplication
            {
                LeaveEndDay = CustomsClasses.AppFunctions.ConvertTime(EndDate),
                ReturnDate = CustomsClasses.AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return Json(JsonConvert.SerializeObject(LeaveEndReturnDates), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadApprovedLeaves(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable
            List<AprrovedLeave> respmsg = new List<AprrovedLeave>();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            try
            {
                string ApprovedLeavesResponse = RecallApplicationXMLRequests.GetApprovedLeaves(username, param1);
                if (ApprovedLeavesResponse != "")
                {
                    var LeaveRecords = ser.Deserialize<List<AprrovedLeave>>(ApprovedLeavesResponse);

                    foreach (var LeaveDetail in LeaveRecords)
                    {
                        respmsg.Add(new AprrovedLeave { LeaveNo = LeaveDetail.LeaveNo, StartDate = AppFunctions.ConvertTime(LeaveDetail.StartDate), EndDate = AppFunctions.ConvertTime(LeaveDetail.EndDate), Qty = LeaveDetail.Qty });
                    }
                }
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return Json(JsonConvert.SerializeObject(respmsg), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovedLeaveDetails(string param1)
        {
            string startDate = "";
            string enddate = "";
            string returndate = "";
            string quantity = "";
            try
            {
                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable

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
            return Json(JsonConvert.SerializeObject(Leave), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6)
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
                    string EmployeeID = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
                    string EmployeeName = System.Web.HttpContext.Current.Session["UserFullName"].ToString();
                    string RequestDate = DateTime.Now.ToString("dd/MM/yyyy");//d/m/Y
                    string DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
                    string AccountId = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        private static string GetDocumentNumber()
        {
            //get Leave number
            string DocumentNumber = null;
            try
            {
                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return DocumentNumber;
        }

        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6)
        {
            string response = null;
            string status = null;

            //send XML submit request

            try
            {
                string DocumentNo = GetDocumentNumber();
                string EmployeeID = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}