using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class DashboardController : Controller
    {
        public string PayrollCutoffDateLabel { get; private set; }
        public ActionResult Dashboard(Dashboard dashboard)
        {
            var log1 = System.Web.HttpContext.Current.Session["logged"] = "yes";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "active";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "yes";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestApprover"] = "";
            System.Web.HttpContext.Current.Session["TransportRequests"] = "";
            System.Web.HttpContext.Current.Session["username"] = "";
            System.Web.HttpContext.Current.Session["Appraisal"] = "";
            System.Web.HttpContext.Current.Session["IsAppraisalSupervisor"] = "";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "active";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingSupervisor"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["IsStaffAdvanceApprover"] = "";
            System.Web.HttpContext.Current.Session["AdvanceClaim"] = "";
            System.Web.HttpContext.Current.Session["AdvanceSurrender"] = "";
            System.Web.HttpContext.Current.Session["IsGF"] = "";

            var profileData = this.Session["UserProfile"] as Login;
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
                        var UserFullName = System.Web.HttpContext.Current.User.Identity.Name;
                        var PayrollNm = System.Web.HttpContext.Current.Session["PayrollNo"];
                        //get user information

                        GetUserInformationresponseString = ProfileXMLRequests.GetUserInformation(PayrollNm.ToString());
                        dynamic json = JObject.Parse(GetUserInformationresponseString);

                        string Status = json.Status;
                        dashboard.txt_name = json.EmployeeName;

                        string EmployeeName = json.EmployeeName;
                        System.Web.HttpContext.Current.Session["UserFullName"] = EmployeeName;
                        System.Web.HttpContext.Current.Session["username"] = EmployeeName;
                        dashboard.txt_name = EmployeeName;
                        string PhoneNo = json.PhoneNo;
                        dashboard.txt_phone = PhoneNo;
                        string Email = json.Email;
                        dashboard.txt_email = Email;
                        string Bank = json.Bank;
                        dashboard.txt_bank = Bank;
                        string BankBranch = json.BankBranch;
                        dashboard.txt_bank_branch = BankBranch;
                        string BankAccountNo = json.BankAccountNo;
                        dashboard.txt_bankAcc = BankAccountNo;
                        string BirthDate = json.BirthDate;
                        dashboard.txt_dob = BirthDate;
                        string NationalIDNo = json.NationalIDNo;
                        dashboard.txt_idNumber = NationalIDNo;
                        string PassportNo = json.PassportNo;
                        dashboard.txt_pass = PassportNo;
                        string NHIFNo = json.NHIFNo;
                        dashboard.txt_nhif = NHIFNo;
                        string NSSFNo = json.NSSFNo;
                        dashboard.txt_nssf = NSSFNo;
                        string PINNo = json.PINNo;
                        dashboard.txt_krapin = PINNo;
                        string PayrollNo = json.PayrollNo;
                        dashboard.txt_payroll = PayrollNo;
                        dashboard.PayrollNumberLabel = PayrollNo;
                        string Title = json.Title;
                        dashboard.txt_jobtitle = Title;
                        string EmploymentDate = json.EmploymentDate;
                        dashboard.txt_epldate = EmploymentDate;

                        if (EmploymentDate != "")
                        {
                            DateTime oDate = DateTime.ParseExact(EmploymentDate, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);
                            dashboard.EmploymentDateLabel = oDate.ToString("MMMM dd yyyy");
                        }

                        string Position = json.Position;
                        dashboard.txt_jobPost = Position;
                        string CompanyEmail = json.CompanyEmail;
                        dashboard.txt_comapnyMail = CompanyEmail;
                        string PersonNumber = json.PersonNumber;
                        dashboard.txt_PersonNumber = PersonNumber;
                        string SESANo = json.SESANo;
                        dashboard.txt_sesa = SESANo;
                        string ContractEndDate = json.ContractEndDate;
                        dashboard.txt_ContractEndDate = ContractEndDate;
                        string AsAt = json.AsAt;
                        dashboard.txt_payroll_cut = AsAt;
                        if (AsAt != "")
                        {
                            DateTime oDate = DateTime.ParseExact(AsAt, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);
                            PayrollCutoffDateLabel = oDate.ToString("MMMM dd yyyy");
                        }

                        string HRMessage = json.HRMessage;
                        dashboard.txt_hrmsg = HRMessage;
                        string Department = json.Department;
                        dashboard.txt_Department = Department;
                        string SuperVisorName = json.SuperVisorName;
                        dashboard.txt_SuperVisorName = SuperVisorName;
                        dashboard.txt_hrmsg = HRMessage;
                        //format name
                        string[] words = EmployeeName.Split(' ');
                        EmployeeName = words[0];

                        if (DateTime.Now.Hour < 12)
                        {
                            dashboard.lblGreeting = "Good Morning " + EmployeeName;
                        }
                        else if (DateTime.Now.Hour < 17)
                        {
                            dashboard.lblGreeting = "Good Afternoon " + EmployeeName;
                        }
                        else
                        {
                            dashboard.lblGreeting = "Good Evening " + EmployeeName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return View(dashboard);
        }

        public static string DrawCahrt()
        {
            return "";
        }
    }
}