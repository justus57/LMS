using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class AccountController : Controller
    {
        private static string hashpassword;

        [AllowAnonymous]//allow for anonymous inorder to use multiple view in one controller
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login EmployeeData)
        {
            var param1 = EmployeeData.EmployeeNumber;
            var param2 = EmployeeData.Password;
            string Msg = null;
            //perform user validation form Nav
            var Result = UserLogin(param1, param2);

            //Deserialize the result
            Models.LoginResponse json = JsonConvert.DeserializeObject<Models.LoginResponse>(Result);
            var Login = json.Status;

            try
            {
                Msg = Login + "You have been successfully authenticated";
                if (Login == "000")
                {
                    if (ModelState.IsValid)
                    {
                        var profileData = new Login
                        {
                            EmployeeNumber = EmployeeData.EmployeeNumber,
                        };
                        this.Session["UserProfile"] = profileData;
                        ModelState.AddModelError("", "Successful!" + Msg);
                        ViewBag.Message = Msg;
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
                else
                {
                    Msg = "Authentication failed. Wrong username or password. Kindly contact the administrator";
                    ViewBag.Message = Msg;
                    ModelState.AddModelError("", "Error! " + Msg);
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
               AppFunctions.WriteLog(ex.Message.ToString()+"Login Error");
            }

            return View();
        }
        //function for login
        public static string UserLogin(string param1, string param2)
        {
            string username = param1;
            string password = param2;
            string UserLoginresponseString = "";
            string Msg = null;
            string status = null;
            hashpassword = AppFunctions.ComputeSha256Hash(password);
            try
            {
                UserLoginresponseString = LoginXMLRequests.UserLogin(username,hashpassword);
                Models.LoginResponse json = JsonConvert.DeserializeObject<Models.LoginResponse>(UserLoginresponseString);
                status = json.Status;
                if (status == "000")
                {
                    Msg = status + "You have been successfully authenticated";
                    string UserFullName = json.EmployeeName;
                    string CanApplyBackdatedLeave = json.CanApplyBackdatedLeave;
                    string IsLeaveApprover = json.IsLeaveApprover;
                    string RequirePasswordChange = json.RequirePasswordChange;
                    string CanApplyLeaveForAnother = json.CanApplyLeaveForAnother;
                    string IsAppraisalSupervisor = json.IsAppraisalSupervisor;
                    string IsHRManager = json.IsHRManager;
                    string IsTransportManager = json.IsTransportManager;
                    string IsDriver = json.IsDriver;
                    string IsTransportRequestApprover = json.IsTransportRequestApprover;
                    string WorkflowApprovalUserName = json.WindowsUsername;

                    if (WorkflowApprovalUserName != "")
                    {
                        WorkflowApprovalUserName = WorkflowApprovalUserName.Replace("\\\\", "\\");
                    }
                    string IsMedicalApprover = json.IsMedicalApprover;
                    string IsEmployeeChangesApprover = json.IsEmployeeChangesApprover;
                    string AppraisalAccessLevel = json.AppraisalAccessLevel;
                    string IsGrievanceApprover = json.IsGrievanceApprover;
                    string IsDisciplinaryApprover = json.IsDisciplinaryApprover;
                    string IsTrainingSupervisor = json.IsTrainingSupervisor;
                    string IsRecruiter = json.IsRecruiter;
                    string PayrollNo = json.PayrollNo;
                    string IsStaffAdvanceApprover = json.IsStaffAdvanceApprover;
                    /// assign to session
                    System.Web.HttpContext.Current.Session["WorkflowApprovalUserName"] = WorkflowApprovalUserName;
                    System.Web.HttpContext.Current.Session["Logged"] = "Yes";
                    System.Web.HttpContext.Current.Session["PayrollNo"] = PayrollNo;
                    UserFullName = System.Web.HttpContext.Current.User.Identity.Name;
                    System.Web.HttpContext.Current.Session["Username"] = param1;
                    System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"] = CanApplyBackdatedLeave;
                    System.Web.HttpContext.Current.Session["IsLeaveApprover"] = IsLeaveApprover;
                    System.Web.HttpContext.Current.Session["RequirePasswordChange"] = RequirePasswordChange;
                    System.Web.HttpContext.Current.Session["CanApplyLeaveForAnother"] = CanApplyLeaveForAnother;
                    System.Web.HttpContext.Current.Session["IsStaffAdvanceApprover"] = IsStaffAdvanceApprover;
                    System.Web.HttpContext.Current.Session["IsAppraisalSupervisor"] = IsAppraisalSupervisor;
                    System.Web.HttpContext.Current.Session["IsTrainingSupervisor"] = IsTrainingSupervisor;
                    System.Web.HttpContext.Current.Session["IsHRManager"] = IsHRManager;
                    System.Web.HttpContext.Current.Session["IsTransportManager"] = IsTransportManager;
                    System.Web.HttpContext.Current.Session["IsDriver"] = IsDriver;
                    System.Web.HttpContext.Current.Session["IsTransportRequestApprover"] = IsTransportRequestApprover;
                    System.Web.HttpContext.Current.Session[" IsDashboardActive"] = "";
                    System.Web.HttpContext.Current.Session[" IsLeavesActive"] = "";
                    System.Web.HttpContext.Current.Session[" IsRecallActive"] = "";
                    System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
                    System.Web.HttpContext.Current.Session[" IsReportsActive"] = "";
                    System.Web.HttpContext.Current.Session["AdvanceRequests"] = "True" ;
                    System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
                    System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
                    System.Web.HttpContext.Current.Session["IsClaimActive"] = "True";
                    System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
                    System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "TRUE";
                    System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "active";
                    System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
                    System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
                    System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
                    System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
                    System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
                    System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
                }
                else if (status == "300")
                {
                    status = "999";
                    Msg = "Authentication failed. Wrong username or password.";
                }
                else
                {
                    Msg = json.Msg;
                }
            }
            catch (Exception es)
            {
                status = "999";
                Msg = "Authentication failed. Wrong username or password. Kindly contact the administrator";
                Msg = es.ToString();
               
                AppFunctions.WriteLog(es.Message);
            }
            var _RequestResponse = new RequestResponse
            {
                Status = status,

                Message = Msg,
            };
            return JsonConvert.SerializeObject(_RequestResponse);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Register register)
        {
            object v = System.Web.HttpContext.Current.Session["Logged"] = "Yes";
            string loging = v.ToString();
            if (loging.Equals("Yes"))
            {
                Response.Redirect("~/Dashboard/Dashboard");
            }
            return Redirect("Register");
        }
        //allow view for forgotpassword
        public ActionResult ForgotPassword() { return View(); }
        //function for getting password
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPassword forgot)
        {
            string EmployeeNo = forgot.EmployeeNumber;
            string Msg = null;
            string status = null;
            try
            {
                dynamic json = ForgotPasswordXmlRequest.ForgotPassword(EmployeeNo, "", "");
                status = json.Status;
                Msg = json.Msg;

                if (status == "000")
                {
                    status = "000";
                    Msg = json.Msg;
                }
                else
                {
                    status = json.Status;
                    Msg = json.Msg;
                }
                ViewBag.Message = Msg;
            }
            catch (Exception es)
            {
                status = "999";
                ViewBag.Message = es.Message;
                Msg = "An error occured. Kindly contact the administrator";
                ViewBag.Message= Msg;
                AppFunctions.WriteLog(es.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg,
            };

            return this.View();
        }
        public ActionResult OneTimePassword() { return View(); }
        [HttpPost]
        public ActionResult OneTimePassword(OneTimePassword password)
        {
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string oldpass = password.OldPassword;
            string newpass = password.Password;
            string Status = "0";
            string Msg = "";
            string Hashdstring = "";
            string OldPasswordHash = "";
            try
            {
                Hashdstring = AppFunctions.ComputeSha256Hash(newpass);
                OldPasswordHash = AppFunctions.ComputeSha256Hash(oldpass);
                string ChangePasswordresponseString = OneTimePassXMLRequests.ChangePassword(username, OldPasswordHash, Hashdstring);

                dynamic json = JObject.Parse(ChangePasswordresponseString);
                Status = json.Status;
                Msg = json.Msg;

                if (Status == "000")
                {
                    Msg = "Password has been changed succesfully";
                    Status = "000";
                    System.Web.HttpContext.Current.Session["RequirePasswordChange"] = "FALSE";
                }

            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Account/Login");
            return View();
        }
    }
}