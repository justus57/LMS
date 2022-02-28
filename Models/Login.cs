using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter Employee Number")]
        public string EmployeeNumber { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class LoginResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public string EmployeeName { get; set; }
        public string IsMedicalApprover { get; set; }
        public string IsLeaveApprover { get; set; }
        public string IsStaffAdvanceApprover { get; set; }
        public string IsEmployeeChangesApprover { get; set; }
        public string AppraisalAccessLevel { get; set; }
        public string IsGrievanceApprover { get; set; }
        public string IsDisciplinaryApprover { get; set; }
        public string IsTrainingSupervisor { get; set; }
        public string RequirePasswordChange { get; set; }
        public string IsTransportManager { get; set; }
        public string IsTransportRequestApprover { get; set; }
        public string IsDriver { get; set; }
        public string IsRecruiter { get; set; }
        public string PayrollNo { get; set; }
        public string CanApplyLeaveForAnother { get; set; }
        public string CanApplyBackdatedLeave { get; set; }
        public string SESANo { get; set; }
        public string WindowsUsername { get; set; }
        public string IsHRManager { get; set; }
        public string IsAppraisalSupervisor { get; set; }
    }

}