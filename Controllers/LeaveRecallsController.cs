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
    public class LeaveRecallsController : Controller
    {
        private HtmlString str;
        // GET: LeaveRecalls
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LeaveRecalls()
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
                    string status = Request.QueryString["status"].Trim();
                    ViewBag.WordHtml1 = status;
                    string owner = Request.QueryString["owner"].Trim();
                    string endpoint = Url.Action("ViewLeaveRecall", "ViewLeaveRecall", new { id = "" });
                    if (status == "" || owner == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        LoadTable(status, owner, endpoint);
                    }
                }
            }
        
    
            return View();
        }

        private void LoadTable(string status, string owner ,string endpoint)
        {
            DataTable dt;

            if (owner == "self")
            {
                dt = LeaverecallsXMLRequests.GetSelfPageData(status, owner, endpoint);
            }
            else if (owner == "others")
            {
                dt = LeaverecallsXMLRequests.GetOthersPageData(status, owner, endpoint);
            }
            else
            {
                dt = LeaverecallsXMLRequests.GetSelfPageData(status, owner, endpoint);
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

        public JsonResult SubmitOpenLeaveRecall(string param1)
        {
            string LeaveHeaderNo = AppFunctions.Base64Decode(param1);
            string response = null;
            string status = null;

            try
            {
                string SubmitOpenLeaveRecallresponseString = LeaverecallsXMLRequests.SubmitOpenLeaveRecall(LeaveHeaderNo);

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
            string documentNo = AppFunctions.Base64Decode(param1);
            //send XML req to delete record
            string DeleteOpenLeaveRecallResponseString = LeaverecallsXMLRequests.DeleteOpenLeaveRecall(documentNo);

            dynamic json = JObject.Parse(DeleteOpenLeaveRecallResponseString);

            string status = json.Status;
            string Message = json.Message;

            var _RequestResponse = new RequestResponse
            {
                Message = Message,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelOpenLeaveRecall(string param1)
        {
            string status = null;
            string Message = null;

            try
            {
                string documentNo = AppFunctions.Base64Decode(param1);
                //send XML req to delete record
                string CancelOpenLeaveRecallresponseString = LeaverecallsXMLRequests.CancelOpenLeaveRecall(documentNo);

                dynamic json = JObject.Parse(CancelOpenLeaveRecallresponseString);

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