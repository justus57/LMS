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
    public class DefineAppraisalSectionsController : Controller
    {
        // GET: DefineAppraisalSections
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// justus kasyoki 4/06/2022
        /// 
        /// Calls all details for Defining AppraisalSections 
        /// </summary>
        /// <returns></returns>
        public ActionResult DefineAppraisalSections()
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "active";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
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
                if ((object)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    LoadTable();
                }
            }
            return View();
        }
        private void LoadTable()
        {

            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();

            DataTable dt = new DataTable();
            dt = DefineAppraisalSectionsXMLRequests.GetAppraisalSections();

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
            ViewBag.DefineAppraisal = strText;
            ////Append the HTML string to Placeholder.
            //placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }
        
        public JsonResult Submit(string param1, string param2)
        {
            string SectionName = param1;
            SectionName = AppFunctions.EscapeInvalidXMLCharacters(SectionName);
            string response = "";
            string status = "999";
            string WhoDefines = param2;

            if (param2 == "Yes")
            {
                WhoDefines = "HR";
            }
            else
            {
                WhoDefines = "Supervisor";
            }

            string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalSection");
            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);
            status = jsonDocumentNo.Status;
            string AppraisalSectionNumber = "";

            if (status == "000")
            {
                AppraisalSectionNumber = jsonDocumentNo.DocumentNo;

                string xmlresponse = DefineAppraisalSectionsXMLRequests.CreateSection(SectionName, WhoDefines, AppraisalSectionNumber);

                dynamic json = JObject.Parse(xmlresponse);

                response = json.Msg;
                status = json.Status;
            }
            else
            {
                response = "Appraisal section Not created. An error occured.";
            }


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SetHRDefined(string param1)
        {
            string SectionNo = param1;
            string status = "999";
            string response = "";

            string xmlresponse = DefineAppraisalSectionsXMLRequests.SetWhoDefines(SectionNo, "HR");

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SetSupervisorDefined(string param1)
        {
            string SectionNo = param1;
            string status = "999";
            string response = "";

            string xmlresponse = DefineAppraisalSectionsXMLRequests.SetWhoDefines(SectionNo, "Supervisor");

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };
            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteSection(string param1)
        {
            string SectionNo = param1;
            string status = "999";
            string response = "";

            string xmlresponse = DefineAppraisalSectionsXMLRequests.DeleteAppraisalSection(SectionNo);

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