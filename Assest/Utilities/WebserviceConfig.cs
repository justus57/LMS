using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace LMS
{
    public class WebserviceConfig
    {
        public static WebRef.HRWebPortal ObjNav
        {
            get
            {
                string URL_status = "http://btl-svr-01.btl.local:7047/BC180/WS/CRONUS%20International%20Ltd./Codeunit/HRWebPortal";

                var username = "BTL/Kasyoki.justus";
                var Password = "$BTL?@2021/2022/S#.&\\$";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var creds = new NetworkCredential(username, Password, "");
                //myCache.Add(new Uri(URL_status), "NTLM", );

                WebRef.HRWebPortal HR = new WebRef.HRWebPortal();

                HR.Url = URL_status;
                HR.Credentials = creds;
                HR.UseDefaultCredentials = true;
                HR.PreAuthenticate = true;

                return HR; 
            }    
        }

    }
}