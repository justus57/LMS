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
        private string tableview;


        public HtmlString str { get; private set; }
        // GET: Leave
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Leave()
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
                    string status = Request.QueryString["status"];
                    string owner = Request.QueryString["owner"];
                    if (status == "" || owner == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        tableview = LoadTable(status, owner);
                    }
                }
            }

            return View();
        }
        private string LoadTable(string status, string owner)
        {
            DataTable dt;

            if (owner == "self")
            {
                dt = LeavesXMLRequests.GetSelfPageData(status, owner);
            }
            else if (owner == "others")
            {
                dt = LeavesXMLRequests.GetOthersPageData(status, owner);
            }
            else
            {
                dt = LeavesXMLRequests.GetSelfPageData(status, owner);
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
            //////Append the HTML string to Placeholder.
            //placeholder.control.add(new Literal { Text = html.ToString() });

            str = new HtmlString(html.ToString());

            return str.ToString(); ;
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

        [ValidateInput(false)]
        public ActionResult DataViewPartial()
        {

            var model = str;
            return PartialView("~/Views/Leave/_DataViewPartial.cshtml", model);
        }
    }
}
