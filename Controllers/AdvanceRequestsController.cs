using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class AdvanceRequestsController : Controller
    {
        // GET: AdvanceRequests
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdvanceRequests()
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "active";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";

            Session["ErrorMessage"] = "";

            if (Session["Logged"].Equals("No"))//set to No
            {
                Response.Redirect("Login.aspx");
            }
            else if (Session["Logged"].Equals("Yes"))
            {
                if (Session["RequirePasswordChange"].Equals("TRUE"))
                {
                    Response.Redirect("OneTimePass.aspx");
                }
                else
                {
                    string status = Request.QueryString["status"].Trim();


                    if (status == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        LoadTable(status);
                    }
                }
            }
            return View();
        }
        private void LoadTable(string status)
        {

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable dt = new DataTable();


            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAdvanceRequestsList(status, username);

            // DataTable 
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
            // placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }

        public static string DeleteRecord(string param1)
        {
            string AdvanceRequestHdrNo = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string DeleteAdvanceRequestXMLResponse = AdvanceRequestsXMLRequests.DeleteAdvanceRequest(AdvanceRequestHdrNo);

                dynamic json = JObject.Parse(DeleteAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string SubmitRecord(string param1)
        {
            string AdvanceRequestHdrNo = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string SubmitAdvanceRequestXMLResponse = AdvanceRequestsXMLRequests.SubmitAdvanceRequest(AdvanceRequestHdrNo);

                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
       
        public static string CancelRequest(string param1)
        {
            string AdvanceRequestHdrNo = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string DeleteAdvanceRequestXMLResponse = AdvanceRequestsXMLRequests.CancelAdvanceRequest(AdvanceRequestHdrNo, "0");

                dynamic json = JObject.Parse(DeleteAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string DelegateRequest(string param1)
        {
            string EntryNo = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string SubmitAdvanceRequestXMLResponse = WebserviceConfig.ObjNav.DelegateWorkflowApprovalRequest(Convert.ToInt32(EntryNo));

                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
       
        public static string PrintRequest(string param1)
        {
            string AdvanceRequestHdrNo = param1;

            string status = "000";
            string Msg = "Success";

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");

            string ExportToPath = folderPath + AdvanceRequestHdrNo + ".pdf";

            try
            {
                string SubmitAdvanceRequestXMLResponse = WebserviceConfig.ObjNav.ExportAdvanceRequestReport(0, AdvanceRequestHdrNo, ExportToPath);

                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
                AppFunctions.WriteLog(e.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}