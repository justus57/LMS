using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;


namespace LMS.Controllers
{
    public class ApprovalEntriesController : Controller
    {
        // GET: ApprovalEntries
        public ActionResult Index()
        {
            return View();
        }
       static string parent = "";
        public ActionResult ApprovalEntries()
        {
            Session["ErrorMessage"] = "";
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
            System.Web.HttpContext.Current.Session["IsStaffAdvanceApprover"] = "";
            System.Web.HttpContext.Current.Session["IsLeaveApprover"] = "";
            System.Web.HttpContext.Current.Session["IsAppraisalSupervisor"] = "";
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            System.Web.HttpContext.Current.Session["Company"]= "KRCS GF Management Unit";
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
                    ////make sure only approver can see records
                    if (System.Web.HttpContext.Current.Session["IsStaffAdvanceApprover"].ToString() == "TRUE" || System.Web.HttpContext.Current.Session["IsLeaveApprover"].ToString() == "TRUE"|| System.Web.HttpContext.Current.Session["IsAppraisalSupervisor"].ToString() == "TRUE" || System.Web.HttpContext.Current.Session["IsHRManager"].ToString() == "TRUE")
                    {
                        string status = Request.QueryString["status"].Trim();
                    ViewBag.status = status; 
                        parent = Request.QueryString["parent"].Trim();
                    ViewBag.parent = parent;
                    string endpoint = Url.Action("ViewApprovalEntry", "ViewApprovalEntry", new { id = "" });

                    if (status == "" || parent == "")
                        {
                            Response.Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            LoadTable(status, parent, endpoint);
                        }
                    }
                }
            }
            return View();
        }
        private void LoadTable(string status, string parent,string endpoint)
        {
            DataTable dt = null;

            if (parent == "Leaves")
            {
                dt = ApprovalEntiesXMLRequests.GetLeavePageData(status, endpoint);
            }
            else if (parent == "LeaveRecalls")
            {
                dt = ApprovalEntiesXMLRequests.GetLeaveRecallPageData(status, endpoint);
            }
            else if (parent == "Trainings")
            {
                dt = ApprovalEntiesXMLRequests.GetTrainingPageData(status, endpoint);
            }
            else if (parent == "Appraisals")
            {
                //HR or supervisor?

                string statusparm = "";
                string requestAs = "";
                string owner = Request.QueryString["owner"].Trim();
                string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

                if (owner == "Approver")
                {
                    if (status == "Open")
                    {
                        statusparm = "Open";
                        requestAs = "Supervisor";
                        dt = ApprovalEntiesXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Submitted")
                    {
                        statusparm = "Submitted";
                        requestAs = "Supervisor";
                        dt = ApprovalEntiesXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                }
                else if (owner == "HR")
                {
                    if (status == "Open")
                    {
                        statusparm = "Open";
                        requestAs = "HRManager";

                        dt = ApprovalEntiesXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Closed")
                    {
                        statusparm = "Closed";
                        requestAs = "HRManager";
                        dt = ApprovalEntiesXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                }
            }
            else if (parent == "AdvanceRequests")
            {
                dt = ApprovalEntiesXMLRequests.GetApprovalEntries("AdvanceRequests", status);
            }
            else if (parent == "AdvanceSurrenders")
            {
                dt = ApprovalEntiesXMLRequests.GetApprovalEntries("AdvanceRequests", status);
            }
            else if (parent == "StaffClaims")
            {
                dt = ApprovalEntiesXMLRequests.GetApprovalEntries("AdvanceRequests", status);
            }
            else if (parent == "TransportRequests")
            {
                dt = ApprovalEntiesXMLRequests.GetApprovalEntries("TransportRequests", status);
            }

            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
            ViewBag.Table = strText;
        }
        public JsonResult ApproveApplication(string param1)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string response = null;
            string status = null;
            //SendApprovalRequest          
            try
            {
                string xmlresponse = ApprovalEntiesXMLRequests.SendApprovalRequest(LeaveHeaderNo, username);
                dynamic json = JObject.Parse(xmlresponse);
                response = json.Msg;
                status = json.Status;
            }
            catch (Exception e)
            {
                response = e.ToString();
            }
            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
     
        public JsonResult RejectApplication(string param1, string param2)
        {
            string ApprovalEntryNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string response = "";
            string status = "";

            //param 2 shouldnt be blank
            if (param2 != "")
            {
                string rejectionComment = null;
                //param2 trim to 200 chars
                try
                {
                    if (param2.Length > 200)
                    {
                        var truncated = param2.Substring(0, 200);

                        rejectionComment = truncated.ToString();
                    }
                    else
                    {
                        rejectionComment = AppFunctions.EscapeInvalidXMLCharacters(param2); //escape XML strings   

                        //sort document type: Leave, Training      
                        if (parent == "AdvanceRequests" || parent == "StaffClaims" || parent == "AdvanceSurrenders" || parent == "TransportRequests")
                        {
                            string RejectWorkflowApprovalRequestResponse = WebService.RejectWorkflowApprovalRequest(ApprovalEntryNo);
                            dynamic json = JObject.Parse(RejectWorkflowApprovalRequestResponse);

                            status = json.Status;
                            response = json.Msg;

                            if (parent == "AdvanceRequests")
                            {
                                WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(ApprovalEntryNo), 0, rejectionComment);
                            }
                            else if (parent == "StaffClaims")
                            {
                                WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(ApprovalEntryNo), 1, rejectionComment);
                            }
                            else if (parent == "AdvanceSurrenders")
                            {
                                WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(ApprovalEntryNo), 2, rejectionComment);
                            }
                            else if (parent == "TransportRequests")
                            {
                                WebserviceConfig.ObjNav.SaveApprovalRejectComment("TransportRequests", Convert.ToInt32(ApprovalEntryNo), 0, rejectionComment);
                            }
                        }
                        else if (parent == "Trainings")
                        {
                            string RejectApprovalRequestXMLResponse = ApprovalEntiesXMLRequests.RejectApprovalRequest(ApprovalEntryNo, username, "Training");

                            dynamic jsonRejectApprovalRequestXMLResponse = JObject.Parse(RejectApprovalRequestXMLResponse);
                            status = jsonRejectApprovalRequestXMLResponse.Status;

                            if (status == "000")
                            {
                                string SaveRejectionCommentResponse = ApprovalEntiesXMLRequests.SaveRejectionComment(ApprovalEntryNo, username, rejectionComment, "Training");
                                dynamic jsonSaveRejectionCommentResponse = JObject.Parse(SaveRejectionCommentResponse);
                                status = jsonSaveRejectionCommentResponse.Status;

                                if (status == "000")
                                {
                                    response = "Training request has been rejected successfully.";
                                }
                                else
                                {
                                    response = "Training request has been rejected successfully. but rejection comment was not submitted";
                                }
                            }
                            else
                            {
                                response = "Training request rejection failed.";
                            }
                        }
                        else
                        {
                            string RejectApprovalRequestXMLResponse = ApprovalEntiesXMLRequests.RejectApprovalRequest(ApprovalEntryNo, username, "Absence");

                            dynamic jsonRejectApprovalRequestXMLResponse = JObject.Parse(RejectApprovalRequestXMLResponse);
                            status = jsonRejectApprovalRequestXMLResponse.Status;

                            if (status == "000")
                            {
                                string SaveRejectionCommentResponse = ApprovalEntiesXMLRequests.SaveRejectionComment(ApprovalEntryNo, username, rejectionComment, "Absence");
                                dynamic jsonSaveRejectionCommentResponse = JObject.Parse(SaveRejectionCommentResponse);
                                status = jsonSaveRejectionCommentResponse.Status;

                                if (status == "000")
                                {
                                    response = "Leave application has been rejected successfully.";
                                }
                                else
                                {
                                    response = "Leave application has been rejected successfully. but rejection comment was not submitted";
                                }
                            }
                            else
                            {
                                response = "Leave application rejection failed.";
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    response = e.ToString();
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
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

    }
}