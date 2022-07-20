using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal.Models.Classes
{
    public class DynamicsNAVResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
    }
    public class LoginResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public string RequirePassChange { get; set; }
        public string EmployeeName { get; set; }
    }
    public class RequestResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}