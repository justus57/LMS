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
    public class TrainingsController : Controller
    {
        // GET: Trainings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Trainings()
        {
            
        
            Session["ErrorMessage"] = "";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "active";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            string status = "";

            if (Session["Logged"].Equals("No"))
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
                    if (Request.QueryString["status"] != null)
                    {
                        status = Request.QueryString["status"].Trim();
                    }

                    LoadTable(status);
                }
            }
            return View();
        }
        private void LoadTable(string status)
        {

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable dt = new DataTable();
            // dt = AppraisalsXMLRequests.GetFilledAppraisal(status, username, "Employee");

            dt.Clear();

            dt = TrainingsXMLRequests.GetTrainings(status, "Employee");
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
            ViewBag.Trainings = strText;
       
        }
        
        public static string SubmitTraining(string param1)
        {
            string status = "999";
            string Msg = "";

            string TrainingNo = AppFunctions.Base64Decode(param1);

            try
            {
                string submitTrainingXMLResponse = TrainingsXMLRequests.SubmitTraining(TrainingNo);

                //DynamicsNAVResponse response = JsonConvert.DeserializeObject<DynamicsNAVResponse>(submitTrainingXMLResponse);
                dynamic json = JObject.Parse(submitTrainingXMLResponse);

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
        
        public static string DeleteRecord(string param1)
        {
            string TrainingNo = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string deleteTrainingXMLResponse = TrainingsXMLRequests.DeleteTraining(TrainingNo);

                dynamic json = JObject.Parse(deleteTrainingXMLResponse);

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
    }
}