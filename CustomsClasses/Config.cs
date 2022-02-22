using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LMS
{
    public class Config
    {
        public static string WebserviceURL()
        {
            return "http://btl-svr-01.btl.local:7047/BC180/WS/CRONUS%20International%20Ltd./Codeunit/HRWebPortal"; // portal                                                                                               
        }
        public static string User()
        {
            return @"BTL/Kasyoki.justus";
        }
        public static string Password()
        {
            return "$BTL?@2021/2022/S#.&\\$";
        }

    }
}