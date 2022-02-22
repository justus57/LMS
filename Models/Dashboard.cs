using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Dashboard
    { 
        public string Password { get; set; }
        public string txt_name { get; set; }
        public string Status { get; set;}
        public string txt_idNumber { get; set;}
        public string txt_dob { get; set;}
        public string txt_pass { get; set; }
        public string txt_email { get; set; }
        public string txt_phone { get; set; }
        public string txt_nhif { get; set;}
        public string txt_nssf { get; set; }
        public string txt_bank { get; set; }
        public string txt_bank_branch { get; set; }
        public string txt_bankAcc { get; set; }
        public string txt_krapin { get; set; }
        public string txt_jobtitle { get; set; }
        public string txt_payroll { get; set;}
        public string txt_sesa { get; set;} 
        public string txt_epldate { get; set;}
        public string txt_payroll_cut { get; set;}
        public string txt_jobPost { get; set;}
        public string txt_ContractEndDate { get; set;}
        public string txt_Department { get; set;}
        public string txt_SuperVisorName { get; set;}
        public string txt_comapnyMail { get; set;}  
        public string txt_PersonNumber { get; set;}
        public string txt_hrmsg { get; set;}
        public string PayrollNumberLabel { get; internal set; }
        public string ContractEndDateLabel { get; internal set; }
        public string lblGreeting { get; internal set; }
        public string EmploymentDateLabel { get; internal set; }
        public string LabelHRMessage { get;  set; }
    }
}