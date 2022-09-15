using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.Models.Classes;
using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OshoPortal.Controllers
{
    public class ViewRequisitionsController : Controller
    {
        // GET: ViewRequisitions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewRequisitions()
        {
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            switch (log)
            {
                case "No":
                    Response.Redirect("/Account/login");
                    break;
                case "yes":
                    {
                        switch (passRequired)
                        {
                            case "true":
                                Response.Redirect("/Account/OneTimePassword");
                                break;
                            default:
                                {
                                    string status = Request.QueryString["status"];

                                    ViewBag.WordHtml = status;
                                    string owner = Request.QueryString["owner"];

                                    string endpoint = Url.Action("EditRequisition", "EditRequisition", new { id = "" });
                                    if (status == "" || owner == "")
                                    {
                                        Response.Redirect(Request.UrlReferrer.ToString());
                                    }
                                    else
                                    {
                                        LoadTable(status, owner, endpoint);
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
            return View();
        }
        private void LoadTable(string status, string owner,string endpoint)
        {
            DataTable dt;

            switch (owner)
            {
                case "Employee":
                    dt = ProductsXMLRequests.GetPageData(status, owner, endpoint);
                    break;
                case "others":
                    dt = ProductsXMLRequests.GetPageData(status, owner, endpoint);
                    break;
                default:
                    dt = ProductsXMLRequests.GetPageData(status, owner, endpoint);
                    break;
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
        public JsonResult SubmitOpenRequisition(string param1, string param2)
        {
            string HeaderNo = Functions.Base64Decode(param1);
            DateTime date = Functions.GetDateTime(param2);
            string response = string.Empty;
            string status = string.Empty;

            try
            {
                var approvalrequest = XMLRequest.SendforApproval(HeaderNo);
            }
            catch (Exception es)
            {
                response = es.Message;
                status = "999";
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteOpenRequisition(string param1)
        {
            string status = string.Empty;
            string Message =string.Empty;
            string documentNo = Functions.Base64Decode(param1);
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                string DeleteOpenRequisitionresponseString = XMLRequest.DeleteDocument(documentNo,"",username);
                dynamic json = JObject.Parse(DeleteOpenRequisitionresponseString);

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
        public JsonResult CancelOpenRequisition(string param1)
        {
            try
            {
                string documentNo = Functions.Base64Decode(param1);

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