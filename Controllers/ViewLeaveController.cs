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

        public object leavetype { get; private set; }

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
                        ViewLeave leave = new ViewLeave();
                        leave.LeaveCodeTxt = LeaveID;

                        GetLeaveData(LeaveID);


                    }
                }
            }
        
            return View();
        }
        private void GetLeaveData(string LeaveID)
        {
            documentNo = LeaveID;
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                string x = ViewLeaveXMLRequest.GetLeaveData(LeaveID, username);
                XmlDocument xmlSoapRequest = new XmlDocument();
                xmlSoapRequest.LoadXml(x);
                //get elements
                //
                XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[0];
                string HeaderNo = NodeHeaderNo.InnerText;
                ViewLeave view = new ViewLeave();
                view.LeaveStartDay = AppFunctions.ConvertTime(HeaderNo);
                //
                if (HeaderNo != "")
                {
                    XmlNode HasAttachmentNode = xmlSoapRequest.GetElementsByTagName("HasAttachment")[0];
                    string HasAttachment = HasAttachmentNode.InnerText;

                    XmlNode AttachmentNameNode = xmlSoapRequest.GetElementsByTagName("AttachmentName")[0];
                    string AttachmentName = AttachmentNameNode.InnerText;

                    XmlNode NodeEmployeeID = xmlSoapRequest.GetElementsByTagName("EmployeeID")[0];
                    string EmployeeID = NodeEmployeeID.InnerText;
                    //            
                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[0];
                    string EmployeeName = NodeEmployeeName.InnerText;
                    //
                    XmlNode NodeRequestDate = xmlSoapRequest.GetElementsByTagName("RequestDate")[0];
                    string RequestDate = NodeRequestDate.InnerText;
                    //
                    XmlNode NodeApprovalStatus = xmlSoapRequest.GetElementsByTagName("ApprovalStatus")[0];
                    string ApprovalStatus = NodeApprovalStatus.InnerText;
                    //
                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[0];
                    string DateCreated = NodeDateCreated.InnerText;
                    //
                    XmlNode NodeApproverID = xmlSoapRequest.GetElementsByTagName("ApproverID")[0];
                    string ApproverID = NodeApproverID.InnerText;
                    //
                    XmlNode NodeApproverName = xmlSoapRequest.GetElementsByTagName("ApproverName")[0];
                    string ApproverName = NodeApproverName.InnerText;
                    //
                    XmlNode NodeLeaveSubType = xmlSoapRequest.GetElementsByTagName("LeaveSubType")[0];
                    string LeaveSubType = NodeLeaveSubType.InnerText;
                    //
                    XmlNode NodeRejectionComment = xmlSoapRequest.GetElementsByTagName("RejectionComment")[0];
                    string RejectionComment = NodeRejectionComment.InnerText;
                    //
                    XmlNode NodeAppliedBy = xmlSoapRequest.GetElementsByTagName("AppliedBy")[0];
                    string AppliedBy = NodeAppliedBy.InnerText;
                    //
                    XmlNode NodeLineDocumentNo = xmlSoapRequest.GetElementsByTagName("LineDocumentNo")[0];
                    string LineDocumentNo = NodeLineDocumentNo.InnerText;
                    //
                    XmlNode NodeLineDocumentType = xmlSoapRequest.GetElementsByTagName("LineDocumentType")[0];
                    string LineDocumentType = NodeLineDocumentType.InnerText;
                    //
                    XmlNode NodeLineNo = xmlSoapRequest.GetElementsByTagName("LineNo")[0];
                    string LineNo = NodeLineNo.InnerText;
                    //
                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[0];
                    string LeaveCode = NodeLeaveCode.InnerText;
                    //
                    XmlNode NodeExternalDocNo = xmlSoapRequest.GetElementsByTagName("ExternalDocNo")[0];
                    string ExternalDocNo = NodeExternalDocNo.InnerText;
                    //
                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[0];
                    string Description = NodeDescription.InnerText;
                    //
                    XmlNode NodeUnitOfMeasure = xmlSoapRequest.GetElementsByTagName("UnitOfMeasure")[0];
                    string UnitOfMeasure = NodeUnitOfMeasure.InnerText;
                    //
                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[0];
                    string StartDate = NodeStartDate.InnerText;
                    //
                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[0];
                    string EndDate = NodeEndDate.InnerText;
                    //
                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[0];
                    string LeaveDays = NodeLeaveDays.InnerText;
                    //
                    XmlNode NodeReturnDate = xmlSoapRequest.GetElementsByTagName("ReturnDate")[0];
                    string Return_Date = NodeReturnDate.InnerText;
                    //
                    XmlNode NodeApprovedStartDate = xmlSoapRequest.GetElementsByTagName("ApprovedStartDate")[0];
                    string ApprovedStartDate = NodeApprovedStartDate.InnerText;
                    //
                    XmlNode NodeApprovedEndDate = xmlSoapRequest.GetElementsByTagName("ApprovedEndDate")[0];
                    string ApprovedEndDate = NodeApprovedEndDate.InnerText;
                    //
                    XmlNode NodeApprovedQty = xmlSoapRequest.GetElementsByTagName("ApprovedQty")[0];
                    string ApprovedQty = NodeApprovedQty.InnerText;
                    //
                    XmlNode NodeApprovedReturnDate = xmlSoapRequest.GetElementsByTagName("ApprovedReturnDate")[0];
                    string ApprovedReturnDate = NodeApprovedReturnDate.InnerText;

                    LoadLeaveDetails(LeaveCode);
                    // LeaveType.Text = LeaveCode;
                    //set to DropDownList


                    view.LeaveStartDay = AppFunctions.ConvertTime(StartDate);
                    view.LeaveEndDay = AppFunctions.ConvertTime(EndDate);
                    view.LeaveDaysApplied = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                    view.ReturnDate = AppFunctions.ConvertTime(Return_Date);
                    view.LeaveApprover = ApproverName;
                    view.Leave_comments = Description;
                    view.Reject_Comments = RejectionComment;
                    _LeaveStartDay = AppFunctions.GetDateTime(StartDate);
                    fileforDownload = folderPath + AttachmentName;
                    attachmentName = AttachmentName;



                    //if (HasAttachment == "Yes")
                    //{
                    //    if (System.IO.File.Exists(fileforDownload))
                    //    {
                    //        System.IO.File.Delete(fileforDownload);
                    //    }

                    //    view.DownloadAttachment = AttachmentName;

                    //    GetAttachment(HeaderNo);

                    //    UploadDiv.Visible = false;
                    //}
                    //else if (HasAttachment == "No")
                    //{
                    //    view.DownloadAttachment = "";
                    //    Attacho.Visible = false;
                    //    UploadDiv.Visible = true;
                    //    //Download.Enabled = false;
                    //    //View.Enabled = false;
                    //}
                    HasAttachment = "No";

                    GetLeaves();
                     //making sure the previous selection has been cleared
                 
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
        }
        public JsonResult GetLeaves()
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string userLeaves = LeaveApplicationXMLRequests.GetUserLeaves(username);

            List<LeaveTypes> leavetype = new List<LeaveTypes>();
            foreach (var kvp in AppFunctions.BreakDynamicJSON(userLeaves))
            {
                leavetype.Add(new LeaveTypes { LeaveCode = kvp.Key, LeaveName = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(leavetype), JsonRequestBehavior.AllowGet);;
        }
        private void LoadLeaveDetails(string LeaveCode)
        {
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable
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
        protected void btn_download_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
            // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
            Response.TransmitFile(fileforDownload);
            Response.End();
        }
        protected void View_Click(object sender, EventArgs e)
        {
            string pdfPath = fileforDownload;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
        }
       
        public static string DeleteAttachment(string param1)
        {
            ViewLeaveXMLRequest.DeleteAttachment(param1);

            var _RequestResponse = new RequestResponse
            {
                Message = "Deleted son",
                Status = "000"
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
      
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
            return JsonConvert.SerializeObject(_LeaveCodeDetails);
        }
        
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


            var _LeaveQuantityAndReturnDate = new LeaveQuantityAndReturnDate
            {
                Quantity = Qty,
                ReturnDate = AppFunctions.ConvertTime(Return_Date),
                Message = Msg,
                Validity = validity
            };
            return JsonConvert.SerializeObject(_LeaveQuantityAndReturnDate);
        }
        
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
            return JsonConvert.SerializeObject(_LeaveEndDateAndReturnDate);
        }
        
        public static string SubmitLeave(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
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

                string username = System.Web.HttpContext.Current.Session["Username"].ToString();

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
     
        public static string DeleteLeave(string param1)
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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
       
        public static string CancelLeave(string param1)
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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string DelegatePendingLeave(string param1)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
       
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            string response = null;
            string status = "000";

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

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
            return JsonConvert.SerializeObject(_RequestResponse);
        }
        public static void UploadAttachment(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string UploadPath = param1;//full path+file name
            string DocumentNo = param2;

            //save attachment if sick leave
            LeaveApplicationXMLRequests.UploadFile(DocumentNo, UploadPath);
        }
    }
}