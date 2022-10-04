using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace OshoPortal.WebService_Connection
{
    public class WSConnection
    {
        //making connection to web service
        private static string responseString = null;

        public static string username { get; private set; }
        public static string Password { get; private set; }
        public static string IsPasswordEncrypted { get; private set; }

        public static string CallWebService(string req)
        {
            string action = "";
            var _url = ConfigurationManager.AppSettings["OshoPortal_WebRef_PortalLogin"];
            
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

            return responseString;
        }
        public static string CallWebServicePortal(string req)
        {
            string action = "";
            var _url = ConfigurationManager.AppSettings["OshoPortal_WebPortal_webportal"];
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
            username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];

            IsPasswordEncrypted = ConfigurationManager.AppSettings["IsEncrypted"];

            if (IsPasswordEncrypted == "N")
            {
                string EncryptedPassword = EncryptDecrypt.Encrypt(Password, true);
                //updateConfig
                UpdateAppSettings("IsEncrypted", "Y");
                UpdateAppSettings("Password", EncryptedPassword);
            }
            else if (IsPasswordEncrypted == "Y")
            {
                Password = EncryptDecrypt.Decrypt(Password, true);
            }

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
            NetworkCredential creds = new System.Net.NetworkCredential(username, Password);
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
        private static void UpdateAppSettings(string key, string value)
        {
            System.Configuration.Configuration configFile = null;
            if (System.Web.HttpContext.Current != null)
            {
                configFile =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                configFile =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

    }
    class EncryptDecrypt
    {
        static string Password = "thusendhcgpadwuf"; // Key for Encrypting & Decrypting Password -> Should be kept private
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            string key = Password;// (string)settingsReader.GetValue("SecurityKey", typeof(String));

            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = Password;// (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}