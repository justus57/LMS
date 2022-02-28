using LMS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Profile(Profile profile)
        {
            string user = null;
            string response = null;
            string GetUserInformationresponseString = null;
            string user1 = null;
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "active";
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
            string username = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            if (log.Equals("No"))
            {
                Response.Redirect("/Account/Login");
            }
            else
            {
                try
                {
                    // get session variable                                                                                

                    string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <GetEmployeeHomeData xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                                    <employeeNo>" + username + @"</employeeNo>
                                                </GetEmployeeHomeData>
                                            </Body>
                                        </Envelope>";

                    response = Assest.Utility.CallWebService(req);

                    GetUserInformationresponseString = Assest.Utility.GetJSONResponse(response);

                    dynamic json = JObject.Parse(GetUserInformationresponseString);

                    string Status = json.Status;

                    string EmployeeName = json.EmployeeName;
                    profile.txt_name = EmployeeName;
                    string PhoneNo = json.PhoneNo;
                    profile.txt_phone = PhoneNo;
                    string Email = json.Email;
                    profile.txt_email = Email;
                    string Bank = json.Bank;
                    profile.txt_bank = Bank;
                    string BankBranch = json.BankBranch;
                    profile.txt_bank_branch = BankBranch;
                    string BankAccountNo = json.BankAccountNo;
                    profile.txt_bankAcc = BankAccountNo;
                    string BirthDate = json.BirthDate;
                    profile.txt_dob = BirthDate;
                    string NationalIDNo = json.NationalIDNo;
                    profile.txt_idNumber = NationalIDNo;
                    string PassportNo = json.PassportNo;
                    profile.txt_pass = PassportNo;
                    string NHIFNo = json.NHIFNo;
                    profile.txt_nhif = NHIFNo;
                    string NSSFNo = json.NSSFNo;
                    profile.txt_nssf = NSSFNo;
                    string PINNo = json.PINNo;
                    profile.txt_krapin = PINNo;
                    string PayrollNo = json.PayrollNo;
                    profile.txt_payroll = PayrollNo;
                    string Title = json.Title;
                    profile.txt_jobtitle = Title;
                    string EmploymentDate = json.EmploymentDate;
                    profile.txt_epldate = EmploymentDate;
                    string Position = json.Position;
                    profile.txt_jobPost = Position;
                    string CompanyEmail = json.CompanyEmail;
                    profile.txt_comapnyMail = CompanyEmail;
                    string PersonNumber = json.PersonNumber;
                    profile.txt_PersonNumber = PersonNumber;
                    string SESANo = json.SESANo;
                    profile.txt_sesa = SESANo;
                    string ContractEndDate = json.ContractEndDate;
                    profile.txt_ContractEndDate = ContractEndDate;
                    string AsAt = json.AsAt;
                    profile.txt_payroll_cut = AsAt;
                    string HRMessage = json.HRMessage;
                    profile.txt_hrmsg = HRMessage;
                    string Department = json.Department;
                    profile.txt_Department = Department;
                    string SuperVisorName = json.SuperVisorName;
                    profile.txt_SuperVisorName = SuperVisorName;
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            return View(profile);
        }

    }
}