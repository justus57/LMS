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
            return View();
        }

        private string  LoadDetails(string Requisition)
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
                AdditemModules();
            }
            return json;

            void AdditemModules()
            {
                Edit.Amount = LineAmount;
                Edit.Description = Description;
                Edit.DocumentNo = DocumentNo;
                Edit.No = No;
                Edit.NOofItems = Quantity;
                Edit.cost = UnitCost;
                Edit.UnitOfMeasure = UoMCode;
            }
        }

        public JsonResult DeleteOpenRequisition(string param1)
        {
            string status = "";
            string Message = "";
            string documentNo = Functions.Base64Decode(param1);
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