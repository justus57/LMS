using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using OshoPortal.Models.Classes;
using OshoPortal.Models;

namespace OshoPortal.Modules
{
   
    public class Functions
    {
        static string responseString;
        
        public static string LineAmount { get; private set; }
        public static string Description { get; private set; }
        public static  string No { get; private set; }
        public static string UoMCode { get; private set; }
        public static string UnitCost { get; private set; }
        public static string Quantity { get; private set; }
        public static string RequestByName { get; private set; }
        public static string RequestByNo { get; private set; }
        public static string RequestD { get; private set; }
        public static string ValidDate { get; private set; }
        public static string ValidToDate { get; private set; }
        public static string RequestDate { get; private set; }
        public static string documentNo { get; private set; }
        //delete old file
        public static bool DeleteFilesOlderThanDayOld(string dir)
        {
            bool status = false;
            string[] files = Directory.GetFiles(dir);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-1))//AddMonths
                    fi.Delete();
            }
            return status;
        }
        public static string EscapeInvalidXMLCharacters(string input)
        {
            return System.Security.SecurityElement.Escape(input);
        }
        public static string ConvertToNavDate(string _TrainingDateString)
        {
            string formattedDate = "";
            DateTime parsedDateTime;

            if (DateTime.TryParseExact(_TrainingDateString, "MM/dd/yyyy HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out parsedDateTime))
            {
                formattedDate = parsedDateTime.ToString("MM/dd/yyyy");
            }
            else
            {
                formattedDate = "Parsing failed";
            }
            return formattedDate;
        }
        public static string ConvertToTime(string _TrainingDateString)
        {
            DateTime mmddyyy = Convert.ToDateTime(DateTime.ParseExact(_TrainingDateString, "ddMMyyyy", CultureInfo.InvariantCulture));
            string dateddMMyyyy = mmddyyy.ToString("dd/MM/yyyy");
            return dateddMMyyyy;
        }
        public static string ConvertToNavTime(string _TrainingDateString)
        {
            string formattedTime = "";
            DateTime parsedDateTime;

            if (DateTime.TryParseExact(_TrainingDateString, "MM/dd/yyyy HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out parsedDateTime))
            {
                formattedTime = parsedDateTime.ToString("hh:mm tt");
            }
            else
            {
                Console.WriteLine("Parsing failed");
            }
            return formattedTime;
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
                //nodeToFind = root.SelectSingleNode("//title[@lang]");
                faultstringNode = xmlSoapRequest.GetElementsByTagName("faultstring")[0];
                return_valueNode = xmlSoapRequest.GetElementsByTagName("return_value")[0];

                if (faultstringNode != null)
                {
                    // It was found, manipulate it.
                    resp = faultstringNode.InnerText;

                    var _NavResponse = new DynamicsNAVResponse
                    {
                        Status = "888",
                        Msg = resp
                    };

                    resp = JsonConvert.SerializeObject(_NavResponse);
                }
                if (return_valueNode != null)
                {
                    // It was found, manipulate it.
                    resp = return_valueNode.InnerText;
                }
            }
            else
            {
                var PassChange = new DynamicsNAVResponse
                {
                    Status = "888",
                    Msg = str
                };

                resp = JsonConvert.SerializeObject(PassChange);
            }

            return resp;
        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //functions
        public static string Convert_AppraisalDetails(string ValidFrom, string ValidTo)
        {
            string ValidityData = null;
            try
            {
                DateTime FromDate = DateTime.ParseExact(ValidFrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ValidFrom = FromDate.ToString("dd/MM/yyyy");

                DateTime ToDate = DateTime.ParseExact(ValidTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ValidTo = ToDate.ToString("dd/MM/yyyy");

                DateTime CurrentDate = DateTime.Now;

                long DaysRemaining = TimeManager.DateDiff(TimeManager.DateInterval.Day, CurrentDate, ToDate);

                ValidityData = ValidFrom + " to " + ValidTo;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return ValidityData;
        }
        public static string ConvertAppraisalDetails(string ValidFrom, string ValidTo)
        {
            string ValidityData = null;
            try
            {
                DateTime FromDate = DateTime.ParseExact(ValidFrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ValidFrom = FromDate.ToString("dd/MM/yyyy");

                DateTime ToDate = DateTime.ParseExact(ValidTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ValidTo = ToDate.ToString("dd/MM/yyyy");

                DateTime CurrentDate = DateTime.Now;

                long DaysRemaining = TimeManager.DateDiff(TimeManager.DateInterval.Day, CurrentDate, ToDate);

                ValidityData = ValidFrom + " to " + ValidTo + " (" + DaysRemaining.ToString() + " day(s) left to create appraisal)";
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return ValidityData;
        }
        public static string ConvertTime(string timeString)
        {
            string date = null;

            try
            {
                DateTime oDate = DateTime.ParseExact(timeString, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                date = oDate.ToString("dd/MM/yyyy");
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return date;
        }
        public static string ConvertToNAVTime(string timeString)
        {
            string date = null;

            try
            {
                DateTime oDate = DateTime.ParseExact(timeString, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                date = oDate.ToString("MM/dd/yyyy");
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return date;
        }
        public static DateTime GetDateTime(string DateString)
        {
            string date = null;

            DateString = DateString.Replace("/", "");
            try
            {
                DateTime oDate = DateTime.ParseExact(DateString, "M/d/yyyy hh:mm", new System.Globalization.CultureInfo("pt-BR"));
                date = oDate.ToString();

            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return Convert.ToDateTime(date);
        }
        public static IDictionary<string, string> BreakDynamicJSON(string array)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                array = array.Substring(1, array.Length - 2);

                string[] resultArray = array.Split(',');

                foreach (var item in resultArray)
                {
                    string[] result = item.ToString().Split(':');
                    dictionary.Add(result[0].ToString().Trim('"'), result[1].ToString().Trim('"'));
                }
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            return dictionary;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            string Daata = null;
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                Daata = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
            return Daata;
        }
        public static List<itemdetails> GetitemTable(string documentNo, string type, string EmpNo, string EmpName,
                                                    string operation, string Item, string description, string quantity,
                                                    string unitOfMeasure, string amount, string dateofSelection)
        {

            string itemxml = XMLRequest.SaveItemLine(documentNo, type, EmpNo, EmpName, operation, Item, description, quantity, unitOfMeasure, amount, dateofSelection);
            List<itemdetails> itemdetails = new List<itemdetails>();
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(itemxml);
            int count = 0;

            if (xmlSoapRequest.GetElementsByTagName("RequisitionHeaderLine").Count > 0)
            {

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("RequisitionHeaderLine"))
                {

                    XmlNode NodeDocumentType = xmlSoapRequest.GetElementsByTagName("DocumentType")[count];
                    string DocumentType = NodeDocumentType.InnerText;

                    XmlNode NodeDocumentNo = xmlSoapRequest.GetElementsByTagName("DocumentNo")[count];
                    string DocumentNo = NodeDocumentNo.InnerText;

                    XmlNode NodeLineNo = xmlSoapRequest.GetElementsByTagName("LineNo")[count];
                    string LineNo = NodeLineNo.InnerText;

                    XmlNode NodeLineType = xmlSoapRequest.GetElementsByTagName("LineType")[count];
                    string LineType = NodeLineType.InnerText;

                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Description = NodeDescription.InnerText;

                    XmlNode NodeQuantity = xmlSoapRequest.GetElementsByTagName("Quantity")[count];
                    string Quantity = NodeQuantity.InnerText;

                    XmlNode NodeUoMCode = xmlSoapRequest.GetElementsByTagName("UoMCode")[count];
                    string UoMCode = NodeUoMCode.InnerText;

                    XmlNode NodeUnitCost = xmlSoapRequest.GetElementsByTagName("UnitCost")[count];
                    string UnitCost = NodeUnitCost.InnerText;

                    XmlNode NodeLineAmount = xmlSoapRequest.GetElementsByTagName("LineAmount")[count];
                    string LineAmount = NodeLineAmount.InnerText;

                    itemdetails.Add(new Models.itemdetails
                    {
                        Description = Description,
                        DocumentNo = DocumentNo,
                        DocumentType = DocumentType,
                        LineAmount = LineAmount,
                        LineNo = LineNo,
                        LineType = LineType,
                        No = No,
                        Quantity = Quantity,
                        UnitCost = UnitCost,
                        UoMCode = UoMCode
                    });
                }
            }
            else
            {
                XmlNode Nodefaultstring = xmlSoapRequest.GetElementsByTagName("faultstring")[count];
                string faultstring = Nodefaultstring.InnerText;
                itemdetails.Add(new Models.itemdetails
                {
                    Message = faultstring
                });

            }
            return itemdetails;
        }
        public static List<EditRequisition> LoadDetails(string Requisition)
        {
            EditRequisition Edit = new EditRequisition();
            List<EditRequisition> requisitions = new List<EditRequisition>();

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string EmpName = System.Web.HttpContext.Current.Session["Profile"].ToString();

            string operation = "Export";

            var RequisitionDetails = XMLRequest.ExportRequisition(username, Requisition, "self", EmpName, operation);
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(RequisitionDetails);
            int count = 0;

            if (xmlSoapRequest.GetElementsByTagName("RequisitionHeaderLine").Count > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("RequisitionHeaderLine"))
                {
                    XmlNode NodeDocumentType = xmlSoapRequest.GetElementsByTagName("DocumentType")[count];
                    string DocumentType = NodeDocumentType.InnerText;

                    XmlNode NodeDocumentNo = xmlSoapRequest.GetElementsByTagName("DocumentNo")[count];
                    documentNo = NodeDocumentNo.InnerText;

                    XmlNode NodeRequestByNo = xmlSoapRequest.GetElementsByTagName("RequestByNo")[count];
                    RequestByNo = NodeRequestByNo.InnerText;

                    XmlNode NodeRequestByName = xmlSoapRequest.GetElementsByTagName("RequestByName")[count];
                    RequestByName = NodeRequestByName.InnerText;

                    XmlNode NodeRequestDate = xmlSoapRequest.GetElementsByTagName("RequestDate")[count];
                    RequestDate = NodeRequestDate.InnerText;

                    XmlNode NodeValidToDate = xmlSoapRequest.GetElementsByTagName("ValidToDate")[count];
                    ValidToDate = NodeValidToDate.InnerText;

                    XmlNode NodeLineNo = xmlSoapRequest.GetElementsByTagName("LineNo")[count];
                    string LineNo = NodeLineNo.InnerText;

                    XmlNode NodeLineType = xmlSoapRequest.GetElementsByTagName("LineType")[count];
                    string LineType = NodeLineType.InnerText;

                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    No = NodeNo.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    Description = NodeDescription.InnerText;

                    XmlNode NodeQuantity = xmlSoapRequest.GetElementsByTagName("Quantity")[count];
                    Quantity = NodeQuantity.InnerText;

                    XmlNode NodeUoMCode = xmlSoapRequest.GetElementsByTagName("UoMCode")[count];
                    UoMCode = NodeUoMCode.InnerText;

                    XmlNode NodeUnitCost = xmlSoapRequest.GetElementsByTagName("UnitCost")[count];
                    UnitCost = NodeUnitCost.InnerText;

                    XmlNode NodeLineAmount = xmlSoapRequest.GetElementsByTagName("LineAmount")[count];
                    LineAmount = NodeLineAmount.InnerText;



                    requisitions.Add(new EditRequisition
                    {
                        Amount = LineAmount,
                        Description = Description,
                        DocumentNo = documentNo,
                        No = No,
                        NOofItems = Quantity,
                        cost = UnitCost,
                        UnitOfMeasure = UoMCode
                    });
                }
                RequestD = Functions.ConvertTime(RequestDate);
                ValidDate = Functions.ConvertTime(ValidToDate);
            
            }
            return requisitions;

        }
    }
    class TimeManager
    {
        public enum DateInterval
        {
            Year,
            Month,
            Weekday,
            Day,
            Hour,
            Minute,
            Second
        }
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {

            TimeSpan ts = date2 - date1;

            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return Fix(ts.TotalDays) / 7;
                case DateInterval.Day:
                    return Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }

        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }
    }
    internal class editdetails
    {
      
    }
}