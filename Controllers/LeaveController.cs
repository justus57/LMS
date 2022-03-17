using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class LeaveController : Controller
    {
        public HtmlString str { get; private set; }
        // GET: Leave
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Leave()
        {
            //get all sessions
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
            //check if user is logged
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
                    string status = Request.QueryString["status"];
                    ViewBag.WordHtml = status;
                    string owner = Request.QueryString["owner"];
                    string endpoint = Url.Action("ViewLeave", "ViewLeave", new {id= "" });
                    if (status == "" || owner == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        //load table data
                        LoadTable(status, owner, endpoint);
                    }
                }
            }
            return View();
        }

        private void LoadTable(string status, string owner,string endpoint)
        {
            DataTable dt;

            if (owner == "self")
            {
                dt = LeavesXMLRequests.GetSelfPageData(status, owner ,endpoint);
            }
            else if (owner == "others")
            {
                dt = LeavesXMLRequests.GetOthersPageData(status, owner, endpoint);
            }
            else
            {
                dt = LeavesXMLRequests.GetSelfPageData(status, owner, endpoint);
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
            ViewBag.Table = strText;
        }

        public JsonResult SubmitOpenLeave(string param1, string param2)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);

            DateTime LeaveStartDay = AppFunctions.GetDateTime(param2);
            string response = null;
            string status = null;

            //can user apply a backdated Leaave?
            string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();

            if (LeaveStartDay < DateTime.Today && CanApplyBackdatedLeave == "FALSE")
            {
                response = "You cannot apply a backdated leave";
                status = "999";
            }
            else
            {
                try
                {
                    string SubmitOpenLeaveresponseString = LeavesXMLRequests.SubmitOpenLeave(LeaveHeaderNo);

                    dynamic json = JObject.Parse(SubmitOpenLeaveresponseString);
                    response = json.Msg;
                    status = json.Status;
                }
                catch (Exception es)
                {
                    response = es.Message;
                    status = "999";
                    Console.Write(es);
                }
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOpenLeave(string param1)
        {
            string status = "";
            string Message = "";
            string documentNo = AppFunctions.Base64Decode(param1);
            //send XML req to delete record
            try
            {
                string DeleteOpenLeaveresponseString = LeavesXMLRequests.DeleteOpenLeave(documentNo);
                dynamic json = JObject.Parse(DeleteOpenLeaveresponseString);
                status = json.Status;
                Message = json.Message;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
            var _RequestResponse = new RequestResponse
            {
                Message = Message,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CancelOpenLeave(string param1)
        {
            string status = null;
            string Message = null;

            try
            {
                string documentNo = AppFunctions.Base64Decode(param1);
                //send XML req to delete record
                string CancelOpenLeaveresponseString = LeavesXMLRequests.CancelOpenLeave(documentNo);

                dynamic json = JObject.Parse(CancelOpenLeaveresponseString);

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

        public JsonResult DelegatePendingLeave(string param1)
        {
            string username = null;
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);

            //if (TempData.ContainsKey("mydata"))
            //    username = TempData["mydata"].ToString();

            string response = null;
            string status = null;

            string xmlresponse = LeavesXMLRequests.DelegateApprovalRequest(LeaveHeaderNo.Trim(), username);

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}
