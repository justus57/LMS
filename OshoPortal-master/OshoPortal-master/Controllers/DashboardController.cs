using Newtonsoft.Json.Linq;
using OshoPortal.Models;
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
        public ActionResult Dashboard(Dashboard dashboard)
        {
            string GetUserInformationresponseString = null;
            var log1 = System.Web.HttpContext.Current.Session["logged"] = "yes";
            System.Web.HttpContext.Current.Session["supervisor"] = "";
            try
            {
                switch (log1)
                {
                    case "No":
                        Response.Redirect("/Account/Login");
                        break;
                    default:
                        {
                            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
                            Session["IsAdvanceActive"] = "";

                            switch (passRequired)
                            {
                                case "true":
                                    Response.Redirect("/Account/OneTimePassword");
                                    break;
                                default:
                                    {
                                        var FullName = System.Web.HttpContext.Current.User.Identity.Name;
                                        var UserFullName = System.Web.HttpContext.Current.Session["Profile"].ToString();

                                        string name = UserFullName.Split(' ').First();                                       
                                        ViewBag.user = name + "'s";                                      
                                        string name1 = UserFullName.Split(' ').Last();

                                        //get user information
                                        string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                                        GetUserInformationresponseString = XMLRequest.GetUserInformation(username);
                                        dynamic json = JObject.Parse(GetUserInformationresponseString);

                                        //pass details to view
                                        string Status = json.Status;
                                        string EmployeeName = json.EmployeeName;
                                        dashboard.Username = json.EmployeeNo;
                                        dashboard.FirstName = name;
                                        dashboard.LastName = name1;
                                        dashboard.BankBranch = json.BankBranch;
                                        dashboard.BankAccountNo = json.BankAccountNo;
                                        dashboard.BirthDate = json.BirthDate;
                                        dashboard.email = json.Email;
                                        dashboard.Title = json.Title;
                                        var title = json.Title;

                                        System.Web.HttpContext.Current.Session["supervisor"] = title;
                                        string EmploymentDate = json.EmploymentDate;
                                        if (EmploymentDate != "")
                                        {
                                            DateTime oDate = DateTime.ParseExact(EmploymentDate, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);
                                            dashboard.EmploymentDate = oDate.ToString("MMMM dd yyyy");
                                        }
                                        if (DateTime.Now.Hour < 12)
                                        {
                                            ViewBag.Greetings = " " + "Good Morning <br/>" + name + "!";
                                        }
                                        else if (DateTime.Now.Hour < 17)
                                        {
                                            ViewBag.Greetings = " " + "Good Afternoon <br/>" + name;
                                        }
                                        else
                                        {
                                            ViewBag.Greetings = " " + "Good Evening <br/>" + name;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return View(dashboard);
        }
    }
}