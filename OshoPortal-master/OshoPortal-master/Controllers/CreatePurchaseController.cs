using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.Models;
using OshoPortal.Models.Classes;
using OshoPortal.Modules;
using OshoPortal.WebPortal;
using OshoPortal.WebService_Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace OshoPortal.Controllers
{
    public class CreatePurchaseController : Controller
    {
        private object data;

        // GET: CreatePurchase view
        public ActionResult Index()
        {
            return View();
        }        
        public ActionResult CreatePurchase()
        {
            var log1 = System.Web.HttpContext.Current.Session["logged"] = "yes";
            try
            {
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
                        try
                        {
                            var username = System.Web.HttpContext.Current.Session["Username"].ToString();

                            var productslist = createRequisition.Requisition(username);

                            var array = productslist.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                            var myList = new List<KeyValuePair<string, string>>(array);

                            Dictionary<string, string> dictionary = new Dictionary<string, string>(array);
                            dynamic DocumentNo = NewMethod1();
                            ViewBag.DocumentNo = DocumentNo;
                        }
                        catch (Exception es)
                        {
                            SystemLogs.WriteLog(es.Message);
                            return RedirectToAction("");
                        }
                    }
                }
            }
            catch (Exception es)
            {
                SystemLogs.WriteLog(es.Message);
                return RedirectToAction("");
            }
            return View();

        }

        private static dynamic NewMethod1()
        {
            string DocumentNoResponse = GetDocumentNumber();
            dynamic json = JObject.Parse(DocumentNoResponse);
            var DocumentNo = json.DocumentNo;
            return DocumentNo;
        }

        public JsonResult GetitemDetails(string param1 ,string param2)
        {
            string Status = NewMethod();
            string ItemNo = NewMethod();
            string Description = NewMethod();
            string UnitOfMeasure = NewMethod();
            string Cost = NewMethod();
            string Response = NewMethod();
            try
            {
                string name = param1;
                string code = name.Split(' ').First();

                dynamic json = JObject.Parse(createRequisition.GetitemDetails(code,param2));
                Status = json.Status;
                ItemNo = json.No;
                Description = json.Description;
                UnitOfMeasure = json.UnitOfMeasure;
                Cost = json.Cost;

            }
            catch (Exception e)
            {
                Response = e.Message;
            }
            var detail = new ItemDescription
            {
                Status = Status,
                ItemNo = ItemNo,
                Description = Description,
                UnitOfMeasure = UnitOfMeasure,
                Cost = Cost,
                Response = Response

            };
            return Json(JsonConvert.SerializeObject(detail), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GLaccount(string param1)
        {
            string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
            List<Models.Item> dropdown = new List<Models.Item>();
            var productslist = XMLRequest.GetGLlist(param1, EmployeeID);
                var array = productslist.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var myList = new List<KeyValuePair<string, string>>(array);

                Dictionary<string, string> dictionary = new Dictionary<string, string>(array);

                List<string> ValueList = (from KeyValuePair<string, string> item in dictionary
                                          let data = item.Key + "  " + item.Value
                                          select data).ToList();

            return Json(JsonConvert.SerializeObject(ValueList), JsonRequestBehavior.AllowGet); ;
        }

        private static string NewMethod()
        {
            return "";
        }

        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7,string param8, string param9)
        {
            string response = NewMethod();
            string status = "000";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = NewMethod();           
            string DocumentNoResponse = GetDocumentNumber();
            dynamic json = JObject.Parse(DocumentNoResponse);
            status = json.Status;
            string code = param1.Split(' ').First();
            switch (status)
            {
                case "000":
                    {

                        DocumentNo = json.DocumentNo;
                        string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
                        string EmployeeName = System.Web.HttpContext.Current.Session["Profile"].ToString();
                        string RequestDate = DateTime.Now.ToString("dd-MM-yyyy");//d/m/Y
                        string DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
                        string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
                        string Item = code;
                        string Description = param2;
                        string Quatity = param3;
                        string Amount = param4;
                        string DateofSelection = param5;
                        string Comment = param6;
                        string unitofMeasure = param7;
                        string type = "";
                        switch (param8)
                        {
                            case "GL Account":
                                type = "0";
                                break;
                            default:
                                type = "1";
                                break;
                        }
                        try
                        {
                            var saverequisition = XMLRequest.SaveRequisition(DocumentNo,type, EmployeeID, EmployeeName, Item, Description, Quatity, unitofMeasure, Amount, DateofSelection);
                        }
                        catch (Exception es)
                        {
                            response = es.ToString();
                            status = "999";
                        }

                        break;
                    }

                default:
                    response = json.Msg;
                    status = "999";
                    break;
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,

                Status = status
            };
            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Saveline(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8,string param9)
        {
            string response = NewMethod();
            string status = "000";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = param9;
            string code = param1.Split(' ').First();
            string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
            string EmployeeName = System.Web.HttpContext.Current.Session["Profile"].ToString();
            string RequestDate = DateTime.Now.ToString("dd-MM-yyyy");//d/m/Y
            string DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
            string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Item = code;
            string Description = param2;
            string Quatity = param3;
            string Amount = param4;
            string DateofSelection = param5;
            string Comment = param6;
            string unitofMeasure = param7;
            string type = "";

            switch (param8)
            {
                case "GL Account":
                    type = "GL Account";
                    break;
                default:
                    type = "Item";
                    break;
            }

            data = GetitemTable(DocumentNo, type, EmployeeID, "IMPORT", Item, Description, Quatity, unitofMeasure, Amount, DateofSelection);
            response = "Successful";
            status = "000";
            //PREQ00008463"
            return Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            //get Leave number 
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = NewMethod();
            string response = null;
            string status = null;

            DateTime LeaveStartDay = Functions.GetDateTime(param4);

            //can user apply a backdated Leaave?
            string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();

            DocumentNo = param9;
            string EmployeeID = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string EmployeeName = System.Web.HttpContext.Current.Session["Username"].ToString();
            string RequestDate = DateTime.Now.ToString("dd/MM/yyyy");//d/m/Y
            string DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
            string AccountId = System.Web.HttpContext.Current.Session["PayrollNo"].ToString();
            string ReturnDate = param1;
            string LeaveCode = param2;
            string Description = param3;
            Description = Functions.EscapeInvalidXMLCharacters(Description);
            string StartDate = param4;
            string EndDate = param5;
            string LeaveDays = param6;
            string uploadpath = param7;
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string documentpath = folderPath + param7;
            try
            {

            }
            catch (Exception es)
            {
                response = es.Message;
                status = "999";
                SystemLogs.WriteLog(es.Message);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        private static string GetDocumentNumber()
        {
            //get Leave number
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetNewDocumentNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <documentNo></documentNo>
                                            <employeeNo>"+username+@"</employeeNo>
                                            <foreignRequisition>false</foreignRequisition>
                                        </GetNewDocumentNo>
                                    </Body>
                                </Envelope>";
            string response = WSConnection.CallWebServicePortal(req);
            var GetDocumentNumber = WSConnection.GetJSONResponse(response);           
            return GetDocumentNumber;
        }
       public static List<itemdetails> GetitemTable(string documentNo, string type, string EmpNo, string operation, string Item, string description, string quantity, string unitOfMeasure, string amount, string dateofSelection)
        {
            //string Uploadspath = HttpContext.Current.Server.MapPath("~/Uploads/");

            string itemxml = XMLRequest.SaveItemLine(documentNo, type, EmpNo, "IMPORT", Item, description, quantity, unitOfMeasure, amount, dateofSelection);
            List<itemdetails> itemdetails = new List<itemdetails>();
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(itemxml);
            int count = 0;
            DataTable table = new DataTable();
            table.Columns.Add("No", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Quantity", typeof(string));
            table.Columns.Add("UoMCode", typeof(string));
            table.Columns.Add("UnitCost", typeof(string));
            table.Columns.Add("LineAmount", typeof(string));        
            table.Columns.Add("View", typeof(string));
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
            return itemdetails;
        }


    }
}