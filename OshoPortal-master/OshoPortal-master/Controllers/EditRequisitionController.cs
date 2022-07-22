using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OshoPortal.Controllers
{
    public class EditRequisitionController : Controller
    {
        // GET: EditRequisition
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditRequisition()
        {
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                if ((string)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    string s = Request.QueryString["id"].Trim();
                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        string Requisition  = Functions.Base64Decode(s);
                        ViewBag.WordHtml = Requisition;
                    }
                }
            }
            return View();
        }
    }
}