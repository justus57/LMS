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
        public string documentNo { get; private set; }

        // GET: EditRequisition
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditRequisition()
        {
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            //bool passRequired = (bool)System.Web.HttpContext.Current.Session["RequirePasswordChange"];
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                //if (passRequired == true)
                //{
                //    Response.Redirect("/Account/OneTimePassword");
                //}
                //else
                //{
                string s = Request.QueryString["id"].Trim();
                if (s == "")
                {
                    Response.Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    string Requisition = Functions.Base64Decode(s);
                    documentNo = Requisition;
                    ViewBag.WordHtml = Requisition;
                    string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                    var data = GetItemsList.Getitemdetail(username, Requisition, "self");
                }
                //}
            }
            return View();
        }
    }
}