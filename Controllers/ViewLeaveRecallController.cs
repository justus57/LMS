using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;

namespace LMS.Controllers
{
    public class ViewLeaveRecallController : Controller
    {
        static DateTime _LeaveStartDay;

        static string _ReturnDate = null;
        static string _Description = null;
        static string _StartDate = null;
        static string _EndDate = null;
        static string _LeaveDays = null;
        static string _LeaveType = null;
        // GET: ViewLeaveRecall
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewLeaveRecall()
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
                        ViewLeaveRecall view = new ViewLeaveRecall();
                        view.LeaveCodeTxt = LeaveID;

                        GetLeaveData(LeaveID);
                    }
                }
            }
            return View();
        }
        private void GetLeaveData(string LeaveID)
        {
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();

                string LoadLeaveRecallresponse = ViewLeaveRecallXMLRequests.LoadLeaveRecallData(LeaveID, username);
                XmlDocument xmlSoapRequest = new XmlDocument();
                xmlSoapRequest.LoadXml(LoadLeaveRecallresponse);
                //get elements
                XmlNode HeaderDocumentTypeNode = xmlSoapRequest.GetElementsByTagName("HeaderDocumentType")[0];
                string HeaderDocumentType = HeaderDocumentTypeNode.InnerText;
                //
                XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[0];
                string HeaderNo = NodeHeaderNo.InnerText;
                //
                if (HeaderNo != "")
                {
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
                    ViewLeaveRecall recall = new ViewLeaveRecall();
                    LoadLeaveDetails(LeaveCode);
                    recall.LeaveType = LeaveCode;
                    recall.LeaveStartDay = AppFunctions.ConvertTime(StartDate);
                    recall.LeaveEndDay = AppFunctions.ConvertTime(EndDate);
                    recall.LeaveDaysApplied = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                    recall.ReturnDate = AppFunctions.ConvertTime(Return_Date);
                    recall.LeaveApprover = ApproverName;
                    recall.Leave_comments = Description;
                    //

                    _LeaveType = LeaveCode;
                    _LeaveStartDay = AppFunctions.GetDateTime(StartDate);
                    _ReturnDate = AppFunctions.ConvertTime(Return_Date);
                    _Description = Description;
                    _StartDate = AppFunctions.ConvertTime(StartDate); ;
                    _EndDate = AppFunctions.ConvertTime(EndDate);
                    _LeaveDays = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                }
                else
                {
                    Response.Redirect(Request.UrlReferrer.ToString());
                }

                ////
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
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable
                string LoadLeaveDetailsresponseString = ViewLeaveRecallXMLRequests.LoadLeaveDetails(username, LeaveCode);
                //{"Code":"ANNUAL","Description":"Annual Leave","Entitled":"23","OpeningBalance":"0","LeaveTaken":"14","Accrued":"40","Remaining":"26"}
                dynamic json = JObject.Parse(LoadLeaveDetailsresponseString);
                ViewLeaveRecall viewLeave = new ViewLeaveRecall();

                viewLeave.Leave_Opening_Balance = json.OpeningBalance;
                viewLeave.Leave_Entitled = json.Entitled;
                viewLeave.Leave_Accrued_Days = json.Accrued;
                viewLeave. Leave_Days_Taken = json.LeaveTaken;
                viewLeave.Leave_Balance = json.Remaining;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
        }
       
        public JsonResult SubmitOpenLeaveRecall(string param1)
        {
            string LeaveHeaderNo = param1;
            string response = null;
            string status = null;

            try
            {
                //SendApprovalRequest
                string SubmitOpenLeaveRecallresponseString = ViewLeaveRecallXMLRequests.SubmitOpenLeaveRecall(LeaveHeaderNo);

                dynamic json = JObject.Parse(SubmitOpenLeaveRecallresponseString);

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
       
        public JsonResult DeleteOpenLeaveRecall(string param1)
        {
            //decode          

            string status = null;
            string Message = null;

            try
            {
                string DeleteOpenLeaveRecallresponseString = ViewLeaveRecallXMLRequests.DeleteOpenLeaveRecall(param1);


                dynamic json = JObject.Parse(DeleteOpenLeaveRecallresponseString);

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
       
        public JsonResult Cancel(string param1)
        {
            //decode          

            string status = null;
            string Message = null;

            try
            {
                string responseString = ViewLeaveRecallXMLRequests.CancelSubmittedeaveRecall(param1);

                dynamic json = JObject.Parse(responseString);

                status = json.Status;
                Message = json.Message;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = "Action sent successfully"
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}
