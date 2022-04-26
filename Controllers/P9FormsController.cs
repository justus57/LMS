using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class P9FormsController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string fileforDownload = "";
        static string attachmentName = "";
        // GET: P9Forms
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult P9Forms()
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
                    CreateYearsList();
                }
            }
            return View();
        }

        private void CreateYearsList()
        {
            //create Loop to Insert years in descending order

            int CurrentYear = DateTime.Now.Year;

            for (int year = (CurrentYear - 5); year <= CurrentYear; year++)
            {
                //P9Year.Items.Insert(0, new ListItem(year.ToString(), year.ToString()));
            }
            //P9Year.Items.Insert(0, new ListItem(" ", ""));
        }

        public ActionResult btn_download_Click()
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
            // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
            Response.TransmitFile(fileforDownload);
            Response.End();
            return View();
        }
        public ActionResult View_Click(object sender, EventArgs e)
        {
            string pdfPath = fileforDownload;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
            return View(buffer);
        }
        
        public JsonResult GenerateP9Form(string param1)
        {
            string _Status = "900";
            string _Message = "An error occured";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string Year = param1;
            //
            string pdfName = "P9form_" + username + "_" + Year + ".pdf";
            string FileName = folderPath + pdfName;

            //delete Old Files

            string XMLRequestResponse = GenerateReports.GenerateP9Form(username, FileName, Year);

            attachmentName = pdfName;

            fileforDownload = FileName;

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