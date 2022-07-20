using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace OshoPortal.WebService_Connection
{
    public class WSConnection
    {
        //making connection to web service
        private static string responseString = null;
        public static string CallWebService(string req)
        {
            string action = "";
            var _url = "http://btl-svr-01.btl.local:8047/BC180-1/WS/Osho%20Chemical%20Industries%20Ltd/Codeunit/PortalLogin";
            var _action = action;
            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(req);
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                try
                {
                    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                    // begin async call to web request.
                    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                    // suspend this thread until call is complete. You might want to
                    // do something usefull here like update your UI.
                    asyncResult.AsyncWaitHandle.WaitOne();

                    // get the response from the completed web request.
                    string soapResult;

                    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                        }
                        responseString = soapResult;
                    }
                }
                catch (WebException ex)
                {
                    StreamReader responseReader = null;

                    string exMessage = ex.Message;

                    if (ex.Response != null)
                    {
                        using (responseReader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            responseString = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
            {
                //code specifically for a WebException ProtocolError
                ex.Message.ToString();
            }
            //catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            //{
            //    //code specifically for a WebException NotFound
            //    responseString = ParseExceptionRespose(ex);
            //}
            //catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.InternalServerError)
            //{
            //    //code specifically for a WebException InternalServerError
            //    responseString = ParseExceptionRespose(ex);
            //}
            //catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            //{
            //    //code specifically for a WebException InternalServerError
            //    responseString = ParseExceptionRespose(ex);
            //}
            //finally
            //{
            //    //call this if exception occurs or not
            //    //wc?.Dispose();
            //}

            return responseString;
        }
        public static string CallWebServicePortal(string req)
        {
            string action = "";
            var _url = "http://btl-svr-01.btl.local:8047/BC180-1/WS/Osho%20Chemical%20Industries%20Ltd/Codeunit/webportal";
            var _action = action;
            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(req);
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                try
                {
                    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                    // begin async call to web request.
                    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                    // suspend this thread until call is complete. You might want to
                    // do something usefull here like update your UI.
                    asyncResult.AsyncWaitHandle.WaitOne();

                    // get the response from the completed web request.
                    string soapResult;

                    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                        }
                        responseString = soapResult;
                    }
                }
                catch (WebException ex)
                {
                    StreamReader responseReader = null;

                    string exMessage = ex.Message;

                    if (ex.Response != null)
                    {
                        using (responseReader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            responseString = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
            {
                //code specifically for a WebException ProtocolError
                ex.Message.ToString();
            }
            //catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            //{
            //    //code specifically for a WebException NotFound
            //    responseString = ParseExceptionRespose(ex);
            //}
            //catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.InternalServerError)
            //{
            //    //code specifically for a WebException InternalServerError
            //    responseString = ParseExceptionRespose(ex);
            //}
            //catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            //{
            //    //code specifically for a WebException InternalServerError
            //    responseString = ParseExceptionRespose(ex);
            //}
            //finally
            //{
            //    //call this if exception occurs or not
            //    //wc?.Dispose();
            //}

            return responseString;
        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            /////////////////webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Timeout = 10000; //time-out value in milliseconds
            NetworkCredential creds = new System.Net.NetworkCredential("BTL\\Kasyoki.justus", @"$BTL@2022&*");
            webRequest.Credentials = creds;
            webRequest.PreAuthenticate = true;
            webRequest.UseDefaultCredentials = true;
            return webRequest;
        }
        private static XmlDocument CreateSoapEnvelope(string req)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(req);
            return soapEnvelopeDocument;
        }
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
            }
            catch (Exception e)
            {
                responseString = e.ToString();
                SystemLogs.WriteLog(e.ToString());
            }
        }
        public static string GetJSONResponse(string str)
        {
            string resp = "";

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                XmlDocument xmlSoapRequest = new XmlDocument();
                xmlSoapRequest.LoadXml(str);

                XmlNode faultstringNode;
                XmlNode return_valueNode;
                XmlElement root = xmlSoapRequest.DocumentElement;

                // Selects all the title elements that have an attribute named lang
                faultstringNode = xmlSoapRequest.GetElementsByTagName("faultstring")[0];
                return_valueNode = xmlSoapRequest.GetElementsByTagName("return_value")[0];

                if (faultstringNode != null)
                {
                    // It was found, manipulate it.
                    resp = faultstringNode.InnerText;

                }
                if (return_valueNode != null)
                {
                    // It was found, manipulate it.
                    resp = return_valueNode.InnerText;
                }
            }
            else
            {}

            return resp;
        }


    }
}