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
        EditRequisition Edit = new EditRequisition();
        List<EditRequisition> requisitions = new List<EditRequisition>();
        public List<itemdetails> saveline { get; private set; }

        // GET: EditRequisition
        public ActionResult Index()
        {
            return View();
        }
      
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
                                    Functions.LoadDetails(Requisition);
                                    ViewBag.data = requisitions;
                                    break;
                                }
                        }
                        break;
                    }
            }
            return View(Edit);
        }
      
        public ActionResult Saveline(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string DocumentNo = param9;
            string Item = param1.Split(' ').First();
            string EmployeeID = System.Web.HttpContext.Current.Session["Username"].ToString();
            string EmployeeName = System.Web.HttpContext.Current.Session["Profile"].ToString();
            string RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
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
                saveline = Functions.GetitemTable(DocumentNo, type, EmployeeID, EmployeeName, "IMPORT", Item, Description, Quatity, unitofMeasure, Amount, DateofSelection);
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
            }

            return Json(JsonConvert.SerializeObject(saveline), JsonRequestBehavior.AllowGet);
        }
      
        public JsonResult DeleteOpenRequisition(string param1)
        {
            string status = "";
            string Message = "";
            string documentNo = param1;

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
            string response = null;
            string status = null;

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

    }
}