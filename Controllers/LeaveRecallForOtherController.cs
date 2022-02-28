using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    //[Authorize(Roles ="Manager")]
    public class LeaveRecallForOtherController : Controller
    {
        // GET: LeaveRecallForOther
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaveRecallForOther()
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
                        items.Add(new SelectListItem { Text = keyList[i] });

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

        public JsonResult LoadApprovedLeaves(string param1, string param2)
        {
            List<AprrovedLeave> respmsg = new List<AprrovedLeave>();
            try
            {
                string ApprovedLeavesResponse = LeaveRecallForOtherXMLRequests.GetApprovedLeaves(param2, param1);

                JavaScriptSerializer ser = new JavaScriptSerializer();

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

            return Json(JsonConvert.SerializeObject(respmsg), JsonRequestBehavior.AllowGet);
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

        public JsonResult GetUserLeaves(string param1)
        {

            string array = LeaveRecallForOtherXMLRequests.GetUserLeaves(param1);
            List<LeaveTypes> leavetype = new List<LeaveTypes>();
            var username1 = System.Web.HttpContext.Current.Session["PayrollNo"];
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
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return Json(JsonConvert.SerializeObject(leavetype), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetLeaveDetails(string param1, string param2)
        {
            string OpeningBalance = "";
            string Entitled = "";
            string Accrued = "";
            string LeaveTaken = "";
            string Remaining = "";
            string Description = "";
            string LeaveCode = "";

            try
            {
                string LeaveDetailsresponseString = LeaveRecallForOtherXMLRequests.GetLeaveDetails(param1, param2);

                dynamic json = JObject.Parse(LeaveDetailsresponseString);

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
            return Json(JsonConvert.SerializeObject(_LeaveCodeDetails), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveState(string param1, string param2, string param3)
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
                string GetLeaveStateresponseString = LeaveRecallForOtherXMLRequests.GetLeaveState(employeeNo, causeofAbsenceCode, startDate, endDate);
                dynamic json = JObject.Parse(GetLeaveStateresponseString);
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
            return Json(JsonConvert.SerializeObject(_LeaveQuantityAndReturnDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveEndDateAndReturnDate(string param1, string param2, string param3)
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
                string LeaveEndDateAndReturnDateResponseString = LeaveRecallForOtherXMLRequests.GetLeaveEndDateAndReturnDate(employeeNo, causeofAbsenceCode, startDate, qty);

                dynamic json = JObject.Parse(LeaveEndDateAndReturnDateResponseString);
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
            return Json(JsonConvert.SerializeObject(_LeaveEndDateAndReturnDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovedLeaveDetails(string param1, string param2)
        {
            string startDate = "";
            string enddate = "";
            string returndate = "";
            string quantity = "";
            try
            {
                string ApprovedLeaveDetails = RecallApplicationXMLRequests.GetApprovedLeaveDetails(param2, param1);

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
            var _SelectedLeaveEndDateAndReturnDate = new SelectedLeaveEndDateAndReturnDate
            {
                EndDate = AppFunctions.ConvertTime(enddate),
                Quantity = quantity,
                ReturnDate = AppFunctions.ConvertTime(returndate),
                StartDate = AppFunctions.ConvertTime(startDate)
            };
            return Json(JsonConvert.SerializeObject(_SelectedLeaveEndDateAndReturnDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            //LeaveRecall
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

            LeaveRecallForOtherXMLRequests.SaveLeaveRecallForOther(DocumentNo, EmployeeID, username, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

            var _RequestResponse = new RequestResponse
            {
                Message = " Leave recall application has been saved successfully "
            };
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string EmployeeID = param7.Trim();
            string EmployeeName = param8.Trim();
            string DocumentNo = GetDocumentNumber(EmployeeID);
            string RequestDate = DateTime.Now.ToString("dd/MM/yyyy");
            string DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
            string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
            string ReturnDate = param1;
            string LeaveCode = param2;
            string Description = param3;
            Description = AppFunctions.EscapeInvalidXMLCharacters(Description);
            string StartDate = param4;
            string EndDate = param5;
            string LeaveDays = param6;

            LeaveRecallForOtherXMLRequests.SaveLeaveRecallForOther(DocumentNo, EmployeeID, username, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

            string SendApprovalRequestResponse = LeaveRecallForOtherXMLRequests.SendApprovalRequest(DocumentNo);

            dynamic json = JObject.Parse(SendApprovalRequestResponse);

            var response = json.Msg;
            var status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        private static string GetDocumentNumber(string username)
        {
            string DocumentNumberResponse = LeaveRecallForOtherXMLRequests.GetDocumentNumber(username);
            dynamic json = JObject.Parse(DocumentNumberResponse);
            return json.DocumentNo;
        }
    }
}