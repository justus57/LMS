using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.Models;
using OshoPortal.Models.Classes;
using OshoPortal.Modules;
using OshoPortal.WebPortal;
using OshoPortal.WebService_Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OshoPortal.Controllers
{
    public class CreatePurchaseController : Controller
    {      
        // GET: CreatePurchase view
        public ActionResult Index()
        {
            return View();
        }        
        public ActionResult CreatePurchase()
        {          
            //first get itemlist to sent to view
            try
            {
                var username = System.Web.HttpContext.Current.Session["Username"].ToString();

                var productslist = createRequisition.Requisition(username);

                var array = productslist.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var myList = new List<KeyValuePair<string, string>>(array);

                Dictionary<string, string> dictionary = new Dictionary<string, string>(array);

                List<string> ValueList = (from KeyValuePair<string, string> item in dictionary
                                          let data = item.Key + "  " + item.Value
                                          select data).ToList();
                ViewBag.Leaves = ValueList;
            }
            catch(Exception es)
            {
                SystemLogs.WriteLog(es.Message);
                return RedirectToAction("");
            }
            return View();

        }
        public JsonResult GetitemDetails(string param1)
        {
            string Status = NewMethod();
            string ItemNo = NewMethod();
            string Description = NewMethod();
            string UnitOfMeasure = NewMethod();
            string Cost = NewMethod();
            try
            {
                string name = param1;
                string code = name.Split(' ').First();

                dynamic json = JObject.Parse(createRequisition.GetitemDetails(code));
                Status = json.Status;
                ItemNo = json.ItemNo;
                Description = json.Description;
                UnitOfMeasure = json.UnitOfMeasure;
                Cost = json.Cost;

            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            var detail = new Models.ItemDescription
            {
                Status = Status,
                ItemNo = ItemNo,
                Description = Description,
                UnitOfMeasure = UnitOfMeasure,
                Cost = Cost,

            };
            return Json(JsonConvert.SerializeObject(detail), JsonRequestBehavior.AllowGet);
        }

        private static string NewMethod()
        {
            return "";
        }

        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
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
                        try
                        {
                            var saverequisition = XMLRequest.SaveRequisition(DocumentNo, EmployeeID, EmployeeName, Item, Description, Quatity, unitofMeasure, Amount, DateofSelection);
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

        public JsonResult Submit(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            //get Leave number 
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = NewMethod();
            string response = null;
            string status = null;

            DateTime LeaveStartDay = Functions.GetDateTime(param4);

            //can user apply a backdated Leaave?
            string CanApplyBackdatedLeave = System.Web.HttpContext.Current.Session["CanApplyBackdatedLeave"].ToString();

            //if (/*LeaveStartDay < DateTime.Today && CanApplyBackdatedLeave == "FALSE"*/)
            //{
            //    //status = "999";
            //    //response = "Leave Start Date must be on or later than the current date";
            //}
            //else
            //{
                string DocumentNoResponse = GetDocumentNumber();
                dynamic json = JObject.Parse(DocumentNoResponse);
                status = json.Status;

            switch (status)
            {
                case "000":
                    {
                        DocumentNo = json.DocumentNo;
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

                        break;
                    }

                default:
                    response = json.Msg;
                    status = "999";
                    break;
            }
            //}
            //var _RequestResponse = new RequestResponse
            //{
            //    Message = response,
            //    Status = status
            //};

            return Json(JsonConvert.SerializeObject(""), JsonRequestBehavior.AllowGet);
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

    }
}