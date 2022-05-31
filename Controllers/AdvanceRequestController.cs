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
    public class AdvanceRequestController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string fileforDownload = "";
        static string attachmentName = "";
        // GET: AdvanceRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdvanceRequest()
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
      
        public static string GenerateAdvanceRequest()
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string pdfName = "StaffAdvance" + username + ".pdf";
            string FileName = folderPath + pdfName;

            try
            {
                string XMLRequestResponse = WebserviceConfig.ObjNav.ExportStaffAdvance(username, FileName);

                attachmentName = pdfName;

                fileforDownload = FileName;
                dynamic json = JObject.Parse(XMLRequestResponse);

                _Status = json.Status;
                _Message = json.Msg;
            }
            catch (Exception es)
            {
                AppFunctions.WriteLog(es.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };
            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}