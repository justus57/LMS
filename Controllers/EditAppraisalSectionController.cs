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
    public class EditAppraisalSectionController : Controller
    {
        static string _AppraisalSectionNumber = "";
        // GET: EditAppraisalSection
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditAppraisalSection()
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
                    string i = Request.QueryString["id"].Trim();

                    if (i == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        _AppraisalSectionNumber = AppFunctions.Base64Decode(i);

                    }
                }
            }
            return View();
        }
        
        public static string LoadSectionDetails()
        {
            string RecordAppraisalResponse = DefineAppraisalSectionsXMLRequests.GetAppraisalSectionDetails(_AppraisalSectionNumber, "FetchAppraisalDetails");

            return RecordAppraisalResponse;
        }
        
        public static string UpdateAppraisalSection(string param1, string param2)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";
            string WhoDefines = "";

            if (param2 == "Yes")
            {
                WhoDefines = "HR";
            }
            else
            {
                WhoDefines = "Supervisor";
            }

            string AppraisalSectionDescription = param1;
            string IsHRDefined = param2;

            string submitAppraisalXMLResponse = DefineAppraisalSectionsXMLRequests.CreateSection(AppraisalSectionDescription, WhoDefines, _AppraisalSectionNumber);

            dynamic json = JObject.Parse(submitAppraisalXMLResponse);

            Status = json.Status;

            if (Status == "000")
            {
                Msg = "The appraisal section was successfully updated";
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}