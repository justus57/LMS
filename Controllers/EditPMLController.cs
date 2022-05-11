using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class EditPMLController : Controller
    {
        static string _PMLNumber = "";
        // GET: EditPML
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditPML()
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
                    string i = Request.QueryString["id"].Trim();

                    if (i == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        _PMLNumber = AppFunctions.Base64Decode(i);
                    }
                }
            }
            return View();
        }
        public static string LoadPML()
        {
            string GetAppraisalPMLDetailsResponse = DefineAppraisalSectionsXMLRequests.GetAppraisalPMLDetails(_PMLNumber);
            return GetAppraisalPMLDetailsResponse;
        }
        public static string UpdatePML(string param1, string param2)
        {
            string status = "";
            string response = "";
            string PMLCode = param1;
            string description = param2;
            string xmlresponse = DefineAppraisalSectionsXMLRequests.CreatePML(PMLCode, description, _PMLNumber);

            dynamic json = JObject.Parse(xmlresponse);
            response = json.Msg;
            status = json.Status;

            if (status == "000")
            {
                response = "The level of performance was successfully updated";
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };
            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}