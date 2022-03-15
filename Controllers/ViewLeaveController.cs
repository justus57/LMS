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
using System.Web.UI.WebControls;
using System.Xml;

namespace LMS.Controllers
{
    public class ViewLeaveController : Controller
    {
        static DateTime _LeaveStartDay;
        string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        string fileforDownload = "";
        string attachmentName = "";
        static string documentNo = "";
        private object Leave_Type;
        private object data;
        private string Msg;
        private string Status;
        private string HasAttachment;
        private string DownloadAttachment;

        public bool LeaveCode { get; private set; }

        // GET: ViewLeave
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewLeave()
        {
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

            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            if (log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if (log == "yes")
            {
                if (passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    string s = Request.QueryString["id"].Trim();

                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        string LeaveID = AppFunctions.Base64Decode(s);
                        documentNo = LeaveID;
                        ViewBag.WordHtml = LeaveID;
                        documentNo = LeaveID;
                        ViewLeave view = new ViewLeave();
                        try
                        {
                            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
                            string x = ViewLeaveXMLRequest.GetLeaveData(LeaveID, username);
                            string datax = Assest.Utility.GetJSONResponse(x);
                            dynamic json = JObject.Parse(datax);
                            string StartDate = json.StartDate;
                            string EndDate = json.EndDate;
                            string LeaveDays = json.LeaveDaysApplied;
                            string Return_Date = json.ReturnDate;
                            string ApproverName = json.ApproverName;
                            string Description = json.Description;
                            string RejectionComment = json.RejectionComment;
                            string AttachmentName = json.AttachmentName;
                            string LeaveCode = json.LeaveType;

                            XmlDocument xmlSoapRequest = new XmlDocument();
                            xmlSoapRequest.LoadXml(x);
                            //get elements
                            XmlNode HeaderDocumentTypeNode = xmlSoapRequest.GetElementsByTagName("Soap:Envelope")[0];
                            string HeaderDocumentType = HeaderDocumentTypeNode.InnerText;
                            //
                            XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("Soap:Body")[0];
                            string HeaderNo = NodeHeaderNo.InnerText;

                            if (LeaveCode != "")
                            {
                                try
                                {
                                    string username1 = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable
                                    string GetLeaveDetailsresponseString = ViewLeaveXMLRequest.GetLeaveDetails(username1, LeaveCode);
                                    dynamic json1 = JObject.Parse(GetLeaveDetailsresponseString);
                                    view.Leave_Opening_Balance = json1.OpeningBalance;
                                    view.Leave_Entitled = json1.Entitled;
                                    view.Leave_Accrued_Days = json1.Accrued;
                                    view.Leave_Days_Taken = json1.LeaveTaken;
                                    view.Leave_Balance = json1.Remaining;
                                    view.LeaveStartDay = StartDate;
                                    view.LeaveEndDay = EndDate;
                                    view.LeaveDaysApplied = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                                    view.ReturnDate =Return_Date;
                                    view.LeaveApprover = ApproverName;
                                    view.Leave_comments = Description;
                                    view.Reject_Comments = RejectionComment;
                                    view.LeaveCodeTxt = LeaveID;
                                    //_LeaveStartDay = AppFunctions.GetDateTime(StartDate);
                                    fileforDownload = folderPath + AttachmentName;
                                    attachmentName = AttachmentName;
                                    if (HasAttachment == "Yes")
                                    {
                                        if (System.IO.File.Exists(fileforDownload))
                                        {
                                            System.IO.File.Delete(fileforDownload);
                                        }
                                        DownloadAttachment = AttachmentName;

                                        GetAttachment(HeaderNo);
                                    }
                                    else if (HasAttachment == "No")
                                    {
                                        DownloadAttachment = "";
                                        //  Attacho.Visible = false;
                                    }
                                    HasAttachment = "No";
                                }
                                catch (Exception es)
                                {
                                    Console.Write(es);
                                }
                                GetLeaves();
                            }
                            else
                            {
                                Response.Redirect(Request.UrlReferrer.ToString());
                            }
                        }
                        catch (Exception es)
                        {
                            Console.Write(es);
                        }
                        data = view;
                    }
                }
            }
        
            return View(data);
        }

        public void GetLeaves()
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

            string array = LeaveApplicationXMLRequests.GetUserLeaves(username);

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
                    items.Add(new SelectListItem { Text = keyList[i], Value = keyList[i] ,Selected = LeaveCode });
                }
                ViewBag.Leaves = keyList;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            
        }
        private void LoadLeaveDetails(string LeaveCode)
        {
            try
            {
                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable
                string GetLeaveDetailsresponseString = ViewLeaveXMLRequest.GetLeaveDetails(username, LeaveCode);

                dynamic json = JObject.Parse(GetLeaveDetailsresponseString);
                ViewLeave viewLeave = new ViewLeave();
                viewLeave.Leave_Opening_Balance = json.OpeningBalance;
                viewLeave.Leave_Entitled = json.Entitled;
                viewLeave.Leave_Accrued_Days = json.Accrued;
                viewLeave.Leave_Days_Taken = json.LeaveTaken;
                viewLeave.Leave_Balance = json.Remaining;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
        }
        private void GetAttachment(string DocumentNo)
        {
            WebserviceConfig.ObjNav.ExportAttachmentToFile("Absence", DocumentNo, folderPath);
        }
        public ActionResult btn_download_Click(string param1)
        {
            attachmentName = param1;
            try
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
                // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
                Response.TransmitFile(fileforDownload);
                Response.End();
            }
            catch(Exception es)
            {
                Msg = es.Message;
                Status = "999";
            }
            var _LeaveCodeDetails = new CustomsClasses.LoginResponse
            {
                Msg = Msg,
                Status = Status
            };
             return Json(JsonConvert.SerializeObject(_LeaveCodeDetails), JsonRequestBehavior.AllowGet);
        }
        public ActionResult View_Click()
        {
            try
            {
                string pdfPath = fileforDownload;
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(pdfPath);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch(Exception es)
            {
                Msg = es.Message;
                Status = "999";
            }
            var _LeaveCodeDetails = new CustomsClasses.LoginResponse
            {

                Msg = Msg,
                Status = Status
            };
            return Json(JsonConvert.SerializeObject(_LeaveCodeDetails), JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult DeleteAttachment(string param1)
        {
            ViewLeaveXMLRequest.DeleteAttachment(param1);

            var _RequestResponse = new RequestResponse
            {
                Message = "Deleted soon",
                Status = "000"
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
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

            var _LeaveCodeDetails = new LeaveCodeDetails
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
            return Json(JsonConvert.SerializeObject(_LeaveCodeDetails), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetLeaveState(string param1, string param2, string param3)
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
                string GetLeaveEndDateAndReturnDateResponseString = LeaveApplicationXMLRequests.GetLeaveEndDateAndReturnDate(employeeNo, causeofAbsenceCode, startDate, qty);
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

            var _LeaveEndDateAndReturnDate = new LeaveEndDateAndReturnDate
            {
                EndDate = AppFunctions.ConvertTime(EndDate),
                ReturnDate = AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return Json(JsonConvert.SerializeObject(_LeaveEndDateAndReturnDate), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SubmitLeave(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            string response = null;
            string status = null;

            //can user apply a backdated Leaave?
            string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();

            if (_LeaveStartDay < DateTime.Today && CanApplyBackdatedLeave == "FALSE")
            {
                response = "Leave Start Date must be on or later than the current date";
                status = "999";
            }
            else
            {

                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

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
                string uploadpath = param7; //param7.Replace(@"\", @"\\");
                string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
                string documentpath = folderPath + param7;
                string LeaveHeaderNo = param8;

                try
                {
                    string mMessage = LeaveApplicationXMLRequests.SaveLeaveApplication(LeaveHeaderNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                    UploadAttachment(documentpath, LeaveHeaderNo);

                    //SendApprovalRequest
                    string SendApprovalRequestresponseString = WebserviceConfig.ObjNav.SendApprovalRequest("Absence", LeaveHeaderNo);

                    dynamic json = JObject.Parse(SendApprovalRequestresponseString);

                    response = json.Msg;
                    status = json.Status;
                }
                catch (Exception es)
                {
                    response = es.Message;
                    status = "999";
                    AppFunctions.WriteLog(es.Message);
                }
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
     
        public JsonResult DeleteLeave(string param1)
        {
            //decode          

            string status = null;
            string Message = null;

            try
            {
                string documentNo = param1;
                //send XML req to delete record
                string DeleteLeaveresponseString = WebserviceConfig.ObjNav.DeleteHRActivityHeader("Absence", documentNo);

                dynamic json = JObject.Parse(DeleteLeaveresponseString);

                status = json.Status;
                Message = json.Message;
            }
            catch (Exception es)
            {
                AppFunctions.WriteLog(es.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult CancelLeave(string param1)
        {
            string documentNo = param1;
            string status = null;
            string Message = null;

            try
            {
                WebserviceConfig.ObjNav.CancelApprovalRequest("Absence", documentNo);

                status = "000";
                Message = "Leave cancelled successfully";
            }
            catch (Exception es)
            {
                status = "999";
                Message = es.Message;
                AppFunctions.WriteLog(es.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = Message,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DelegatePendingLeave(string param1)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string response = null;
            string status = null;

            //Send delegate Request          

            try
            {
                string xmlresponse = LeavesXMLRequests.DelegateApprovalRequest(LeaveHeaderNo, username);

                dynamic json = JObject.Parse(xmlresponse);

                response = json.Msg;
                status = json.Status;
            }
            catch (Exception e)
            {
                response = e.Message;
            }


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            string response = null;
            string status = "000";

            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

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
            string uploadpath = param7; //param7.Replace(@"\", @"\\");
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string documentpath = folderPath + param7;
            string LeaveHeaderNo = param8;


            try
            {
                string mMessage = LeaveApplicationXMLRequests.SaveLeaveApplication(LeaveHeaderNo, EmployeeID, EmployeeName, RequestDate, DateCreated, AccountId, LeaveCode, Description, StartDate, EndDate, LeaveDays, ReturnDate);

                UploadAttachment(documentpath, LeaveHeaderNo);

                response = "Leave application has been saved successfully";
                //response = uploadpath;
            }
            catch (Exception es)
            {
                response = es.Message;
                status = "999";
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,

                Status = status
            };
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public static void UploadAttachment(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

            string UploadPath = param1;//full path+file name
            string DocumentNo = param2;

            //save attachment if sick leave
            LeaveApplicationXMLRequests.UploadFile(DocumentNo, UploadPath);
        }

      
    }
}