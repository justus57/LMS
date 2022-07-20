using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class PayslipsController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string fileforDownload = "";
        static string attachmentName = "";
        // GET: Payslips
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Payslips()
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "active";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";

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
            }
            return View();
        }
        protected void btn_download_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
            // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
            Response.TransmitFile(fileforDownload);
            Response.End();
        }
        protected void View_Click(object sender, EventArgs e)
        {
            string pdfPath = fileforDownload;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
        }
        
        public JsonResult GeneratePaylip(string param1)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            //convert date to this 13/11/2019 07:00 foemat
            DateTime PayslipMonth = DateTime.Parse(param1);

            string Month = PayslipMonth.ToString("MMyyyy");

            string pdfName = "Payslip_" + username + "_" + Month + ".pdf";
            //
            string FileName = folderPath + pdfName;

            //delete Old Files

            attachmentName = pdfName;

            fileforDownload = FileName;

            string CurrentMonth = PayslipMonth.ToString("MM/dd/yyyy");

            //call generate functions
            // string attachmentName = FileName;

            string XMLRequestResponse = GenerateReports.GeneratePaySlip(username, FileName, CurrentMonth);

            dynamic json = JObject.Parse(XMLRequestResponse);

            _Status = json.Status;
            _Message = json.Msg;


            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

    }
}