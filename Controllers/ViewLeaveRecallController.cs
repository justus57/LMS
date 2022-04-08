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
        public object viewLeave { get; private set; }
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
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                if ((object)passRequired == "true")
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
                        ViewLeaveRecall viewLeave = new ViewLeaveRecall();
                        try
                        {
                            string LeaveID = AppFunctions.Base64Decode(s);
                            viewLeave.LeaveCodeTxt = LeaveID;
                            ViewBag.LeaveID = LeaveID;
                            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

                            string LoadLeaveRecallresponse = ViewLeaveRecallXMLRequests.LoadLeaveRecallData(LeaveID, username);
                            string datax = Assest.Utility.GetJSONResponse(LoadLeaveRecallresponse);
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

                            if (LeaveID != "")
                            {
                                string username1 = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();// get session variable
                                string LoadLeaveDetailsresponseString = ViewLeaveRecallXMLRequests.LoadLeaveDetails(username1, LeaveCode);
                                //{"Code":"ANNUAL","Description":"Annual Leave","Entitled":"23","OpeningBalance":"0","LeaveTaken":"14","Accrued":"40","Remaining":"26"}
                                dynamic json1 = JObject.Parse(LoadLeaveDetailsresponseString);

                                viewLeave.Leave_Opening_Balance = json1.OpeningBalance;
                                viewLeave.Leave_Entitled = json1.Entitled;
                                viewLeave.Leave_Accrued_Days = json1.Accrued;
                                viewLeave.Leave_Days_Taken = json1.LeaveTaken;
                                viewLeave.Leave_Balance = json1.Remaining;
                                //
                                viewLeave.LeaveType = LeaveCode;
                                viewLeave.LeaveStartDay = AppFunctions.ConvertTime(StartDate);
                                viewLeave.LeaveEndDay = AppFunctions.ConvertTime(EndDate);
                                viewLeave.LeaveDaysApplied = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                                viewLeave.ReturnDate = AppFunctions.ConvertTime(Return_Date);
                                viewLeave.LeaveApprover = ApproverName;
                                viewLeave.Leave_comments = Description;
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
                }
            }
            return View(viewLeave);
        }
        private void GetLeaveData(string LeaveID)
        {
            ViewLeaveRecall recall = new ViewLeaveRecall();
            try
            {
                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

                string LoadLeaveRecallresponse = ViewLeaveRecallXMLRequests.LoadLeaveRecallData(LeaveID, username);
                string datax = Assest.Utility.GetJSONResponse(LoadLeaveRecallresponse);
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

                if (LeaveID != "")
                {
                    
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
