using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LMS.Controllers
{
    public class LeaveApplicationController : Controller
    {
        public object _RequestResponse { get; private set; }

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
                        items.Add(new SelectListItem { Text = keyList[i], Value = keyList[i] });
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

        public JsonResult GetLeaveDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable
            string OpeningBalance = null;
            string Entitled = "";
            string Accrued = "";
            string LeaveTaken = "";
            string Remaining = "";
            string Description = "";
            string LeaveCode = "";
            string RequiresAttachment = "";
            string AttachmentMandatory = "";
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

            var Leave = new LeaveApplication
            {
                Leave_Accrued_Days = Accrued,

                Leave_Entitled = Entitled,
                RequiresAttachment = RequiresAttachment,
                Leave_Days_Taken = LeaveTaken,
                Leave_Opening_Balance = OpeningBalance,
                Leave_Balance = Remaining,

            };
            // return JsonConvert.SerializeObject(Leave);
            return Json(JsonConvert.SerializeObject(Leave), JsonRequestBehavior.AllowGet);
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
                Qty = json.EndDate;
            }
            else
            {
                validity = false;
                Msg = "Failed";

            }


            var Leave = new LeaveApplication
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

            var LeaveEndReturnDates = new LeaveApplication
            {
                LeaveEndDay = CustomsClasses.AppFunctions.ConvertTime(EndDate),
                ReturnDate = CustomsClasses.AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return Json(JsonConvert.SerializeObject(LeaveEndReturnDates), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            string response = "";
            string status = "000";
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string DocumentNo = "";
            string DocumentNoResponse = GetDocumentNumber();
            dynamic json = JObject.Parse(DocumentNoResponse);
            status = json.Status;

            if (status == "000")
            {

                DocumentNo = json.DocumentNo;
                string EmployeeID = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
                string EmployeeName = System.Web.HttpContext.Current.Session["Username"].ToString();
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
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public static void UploadAttachment1(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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

        public static void UploadAttachment(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

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

        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            //get Leave number 
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
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
                    string EmployeeID = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
                    string EmployeeName = System.Web.HttpContext.Current.Session["Username"].ToString();
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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        private static string GetDocumentNumber()
        {
            //get Leave number
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <GetNewDocumentNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                            <hRDocumentType>Absence</hRDocumentType>
                            <employeeNo>" + username + @"</employeeNo>
                            <subType>Leave</subType>
                        </GetNewDocumentNo>
                    </Body>
                </Envelope>";
            var response = Assest.Utility.CallWebService(req);
            var GetDocumentNumber = Assest.Utility.GetJSONResponse(response);
            return GetDocumentNumber;
        }

        public ActionResult FileUploadHandler()
        {
            object Message = null;
            if (Request.Files.Count > 1)
            {
                //Fetch the Uploaded File.
                HttpPostedFileBase postedFile = Request.Files[0];
                //Set the Folder Path.
                string folderPath = Server.MapPath("~/Uploads/");

                //Set the File Name.
                string fileName = Path.GetFileName(postedFile.FileName);

                string filePath = folderPath + fileName;

                //if exists delete
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                //Save the File in Folder.
                postedFile.SaveAs(folderPath + fileName);

                //Send File details in a JSON Response.
                string json = new JavaScriptSerializer().Serialize(
                    new
                    {
                        name = fileName,
                        path = filePath,
                        uploadspath = folderPath
                    });
                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.ContentType = "text/json";
                Response.Write(json);
                Response.End();
                var _RequestResponse = new RequestResponse
                {
                    Message = Response.StatusCode.ToString(),

                    Status = "000"
                };
            }
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }

}