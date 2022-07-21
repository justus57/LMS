using Newtonsoft.Json.Linq;
using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OshoPortal.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            var log1 = System.Web.HttpContext.Current.Session["logged"] = "yes";
            try
            {
                string GetUserInformationresponseString = null;
                if ((string)log1 == "No")
                {
                    Response.Redirect("/Account/Login");
                }
                else
                {
                    var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
                    Session["IsAdvanceActive"] = "";

                    if ((object)passRequired == "true")
                    {
                        Response.Redirect("/Account/OneTimePassword");
                    }
                    else
                    {
                        var FullName = System.Web.HttpContext.Current.User.Identity.Name;
                        var UserFullName = System.Web.HttpContext.Current.Session["Profile"];
                        string name = UserFullName.ToString();
                        string code = name.Split(' ').First();
                        ViewBag.user = code +"'s";
                        string name1 = UserFullName.ToString();
                        string code1 = name.Split(' ').Last();
                        //get user information
                        string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                        //GetUserInformationresponseString = LoginXMLRequests.GetUserInformation(PayrollNm.ToString());
                       // dynamic json = JObject.Parse(GetUserInformationresponseString);

                  //      string Status = json.Status;
                       

                     
                        

                        if (DateTime.Now.Hour < 12)
                        {
                            ViewBag.Greetings= " "+"Good Morning <br/>" + code +"!";
                        }
                        else if (DateTime.Now.Hour < 17)
                        {
                            ViewBag.Greetings = " " + "Good Afternoon <br/>" + UserFullName;
                        }
                        else
                        {
                            ViewBag.Greetings = " " + "Good Evening <br/>" + UserFullName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return View();
        }
    }
}