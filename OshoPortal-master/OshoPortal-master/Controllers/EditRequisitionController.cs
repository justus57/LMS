using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.Models;
using OshoPortal.Models.Classes;
using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace OshoPortal.Controllers
{
    public class EditRequisitionController : Controller
    {
        public string DocumentNo { get; private set; }
        public string LineAmount { get; private set; }
        public string Description { get; private set; }
        public string No { get; private set; }
        public string UoMCode { get; private set; }
        public string UnitCost { get; private set; }
        public string Quantity { get; private set; }
        public string RequestByName { get; private set; }
        public string RequestByNo { get; private set; }
        public string RequestD { get; private set; }
        public string ValidDate { get; private set; }
        public string ValidToDate { get; private set; }
        public string RequestDate { get; private set; }
        public List<itemdetails> saveline { get; private set; }

        // GET: EditRequisition
        public ActionResult Index()
        {
            return View();
        }
        EditRequisition Edit = new EditRequisition();
        List<EditRequisition> requisitions = new List<EditRequisition>();
        public ActionResult EditRequisition()
        {
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            switch (log)
            {
                case "No":
                    Response.Redirect("/Account/login");
                    break;
                case "yes":
                    {
                        string s = Request.QueryString["id"].Trim();
                        switch (s)
                        {
                            case "":
                                Response.Redirect(Request.UrlReferrer.ToString());
                                break;
                            default:
                                {
                                    string Requisition = Functions.Base64Decode(s);
                                    DocumentNo = Requisition;
                                    ViewBag.WordHtml = Requisition;
                                    LoadDetails(Requisition);
                                    ViewBag.data = requisitions;
                                    break;
                                }
                        }

                        break;
                    }
            }
            return View(Edit);
        }
        private string LoadDetails(string Requisition)
        {
            var json = "";
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
                    DocumentNo = NodeDocumentNo.InnerText;

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
                        DocumentNo = DocumentNo,
                        No = No,
                        NOofItems = Quantity,
                        cost = UnitCost,
                        UnitOfMeasure = UoMCode
                    });
                }
                RequestD = Functions.ConvertTime(RequestDate);
                ValidDate = Functions.ConvertTime(ValidToDate);
                AdditemModules();
            }
            return json;

            void AdditemModules()
            {
                Edit.EmployeeNo = RequestByNo;
                Edit.Requestedby = RequestByName;
                Edit.DocumentNo = DocumentNo;
                Edit.Datesubmitted = RequestD;
                Edit.ValidToDate = ValidDate;
                Edit.cost = UnitCost;
                Edit.UnitOfMeasure = UoMCode;
            }
        }
        public ActionResult Saveline(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = param9;
            string Item = param1.Split(' ').First();
            string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
            string EmployeeName = System.Web.HttpContext.Current.Session["Profile"].ToString();
            string RequestDate = DateTime.Now.ToString("dd-MM-yyyy");//d/m/Y
            string DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
            string AccountId = System.Web.HttpContext.Current.Session["Username"].ToString();
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
            try
            {
                saveline = GetitemTable(DocumentNo, type, EmployeeID, EmployeeName, "IMPORT", Item, Description, Quatity, unitofMeasure, Amount, DateofSelection);

            }
            catch (Exception)
            {

            }

            return Json(JsonConvert.SerializeObject(saveline), JsonRequestBehavior.AllowGet);
        }
        public static List<itemdetails> GetitemTable(string documentNo, string type, string EmpNo, string EmpName, string operation, string Item, string description, string quantity, string unitOfMeasure, string amount, string dateofSelection)
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
        public JsonResult DeleteOpenRequisition(string param1)
        {
            string status = "";
            string Message = "";
            string documentNo = param1;
            //send XML req to delete record
            try
            {
                string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                string DeleteOpenRequisitionresponseString = XMLRequest.DeleteDocument(documentNo, "", username);
                dynamic json = JObject.Parse(DeleteOpenRequisitionresponseString);
                status = json.Status;
                Message = json.Message;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }
            var _RequestResponse = new RequestResponse
            {
                Message = Message,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CancelOpenRequisition(string param1)
        {
            string status = null;
            string Message = null;

            try
            {
                string documentNo = Functions.Base64Decode(param1);
                ////send XML req to delete record
                string CancelresponseString = XMLRequest.CancelRequisition(documentNo);

                dynamic json = JObject.Parse(CancelresponseString);

                status = json.Status;
                Message = json.Message;
            }
            catch (Exception es)
            {
                Console.Write(es);
            }

            var _RequestResponse = new RequestResponse
            {
                Message = "Action sent successfully"
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelegatePendingRequisition(string param1)
        {
            string username = null;
            string HeaderNo = Functions.Base64Decode(param1);

            //if (TempData.ContainsKey("mydata"))
            //    username = TempData["mydata"].ToString();

            string response = null;
            string status = null;

            // string xmlresponse = XMLRequest.DelegateApprovalRequest(LeaveHeaderNo.Trim(), username);

            //dynamic json = JObject.Parse(xmlresponse);

            //response = json.Msg;
            //status = json.Status;

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

    }
}