﻿using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;


namespace LMS.Controllers
{
    public class ViewApprovalEntryController : Controller
    {
        static string _ReturnDate = null;
        static string _Description = null;
        static string _StartDate = null;
        static string _EndDate = null;
        static string _LeaveDays = null;
        static string _LeaveType = null;
        static DateTime _LeaveStartDay;
        string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        string fileforDownload = "";
        string attachmentName = "";
        private string HasAttachment;
        private string DownloadAttachment;
        static string Parent = null;

        // GET: ViewApprovalEntry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewApprovalEntry()
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "active";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
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
                if ((string)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    string s = Request.QueryString["id"].Trim();
                    string Emp = Request.QueryString["Emp"].Trim();

                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        ViewApprovalEntry entry = new ViewApprovalEntry();
                        string LeaveID = AppFunctions.Base64Decode(s);
                        entry.LeaveCodeTxt = LeaveID;
                        ViewBag.WordHtml = LeaveID;
                        
                        string status = Request.QueryString["status"].Trim();
                        string owner = Request.QueryString["parent"].Trim();

                        if (owner == "Leaves")
                        {
                            Parent = "Leave";
                        }
                        else if (owner == "LeaveRecalls")
                        {
                            Parent = "LeaveRecall";
                        }
                        try
                        {
                            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                            string LeaveData = ViewApprovalEntryXMLRequests.GetLeaveData(LeaveID, username, Parent);
                            string datax = Assest.Utility.GetJSONResponse(LeaveData);
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
                            xmlSoapRequest.LoadXml(LeaveData);
                            //get elements
                            XmlNode HeaderDocumentTypeNode = xmlSoapRequest.GetElementsByTagName("HeaderDocumentType")[0];
                            string HeaderDocumentType = HeaderDocumentTypeNode.InnerText;
                            //
                            XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[0];
                            string HeaderNo = NodeHeaderNo.InnerText;
                            //
                            if (LeaveID != "")
                            {
                                //LoadLeaveDetails(LeaveCode);
                                try
                                {
                                    string username1 = System.Web.HttpContext.Current.Session["Username"].ToString();// get session variable
                                    string GetLeaveDetailsresponseString = ViewApprovalEntryXMLRequests.GetLeaveDetails(username1, LeaveCode);
                                    //json 
                                    dynamic json1 = JObject.Parse(GetLeaveDetailsresponseString);
                                    entry.Leave_Opening_Balance = json1.OpeningBalance;
                                    entry.Leave_Entitled = json1.Entitled;
                                    entry.Leave_Accrued_Days = json1.Accrued;
                                    entry.Leave_Days_Taken = json1.LeaveTaken;
                                    entry.Leave_Balance = json1.Remaining;
                                }
                                catch (Exception es)
                                {
                                    Console.Write(es);
                                }
                                entry.LeaveType = LeaveCode;
                                entry.LeaveStartDay = AppFunctions.ConvertTime(StartDate);
                                entry.LeaveEndDay = AppFunctions.ConvertTime(EndDate);
                                entry.LeaveDaysApplied = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();
                                entry.ReturnDate = AppFunctions.ConvertTime(Return_Date);
                                entry.LeaveApprover = ApproverName;
                                entry.Leave_comments = Description;
                                entry.Reject_Comments = RejectionComment;
                                //
                                _LeaveType = LeaveCode;
                                _LeaveStartDay = AppFunctions.GetDateTime(StartDate);
                                _ReturnDate = AppFunctions.ConvertTime(Return_Date);
                                _Description = Description;
                                _StartDate = AppFunctions.ConvertTime(StartDate); ;
                                _EndDate = AppFunctions.ConvertTime(EndDate);
                                _LeaveDays = Convert.ToInt16(decimal.Parse(LeaveDays)).ToString();

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
                }
            }
            return View();
        }
        public ActionResult btn_download_Click()
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
            // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
            Response.TransmitFile(fileforDownload);
            Response.End();
            return View();
        }
        public ActionResult View_Click()
        {
            string pdfPath = fileforDownload;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
            return View();
        }
        private void GetAttachment(string DocumentNo)
        {
            WebserviceConfig.ObjNav.ExportAttachmentToFile("Absence", DocumentNo, folderPath);
        }
        public JsonResult ApproveApprovalRequest(string param1)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string response = null;
            string status = null;
            //SendApprovalRequest           

            try
            {
                string res = ApprovalEntiesXMLRequests.SendApprovalRequest(LeaveHeaderNo, username);
                //break json
                dynamic json = JObject.Parse(res);
                string Status = json.Status;
                string Message = json.Msg;

                response = Message;
                status = Status;
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
        public JsonResult RejectApprovalRequest(string param1, string param2)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string response = null;
            string status = null;

            //param 2 shouldnt be blank
            if (param2 != "")
            {
                string rejectionComment = null;
                //param2 trim to 200 chars
                try
                {
                    if (param2.Length > 250)
                    {
                        var truncated = param2.Substring(0, 250);

                        rejectionComment = truncated.ToString();
                    }
                    else
                    {
                        rejectionComment = param2;
                    }
                    rejectionComment = AppFunctions.EscapeInvalidXMLCharacters(rejectionComment);
                    ViewApprovalEntryXMLRequests.RejectApprovalRequest(LeaveHeaderNo, username);
                    ViewApprovalEntryXMLRequests.SaveRejectionComment(LeaveHeaderNo, username, rejectionComment);
                    response = "Leave application has been rejected successfully.";
                    status = "999";
                }
                catch (Exception es)
                {
                    Console.Write(es);
                }

            }
            else
            {
                response = "You must give the rejection comment";
                status = "999";
            }


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return  Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}