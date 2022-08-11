using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.Models.Classes;
using OshoPortal.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OshoPortal.Controllers
{
    public class EditRequisitionController : Controller
    {
        public string documentNo { get; private set; }
        // GET: EditRequisition
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditRequisition()
        {
            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            //bool passRequired = (bool)System.Web.HttpContext.Current.Session["RequirePasswordChange"];
            switch (log)
            {
                case "No":
                    Response.Redirect("/Account/login");
                    break;
                case "yes":
                    {
                        //if (passRequired == true)
                        //{
                        //    Response.Redirect("/Account/OneTimePassword");
                        //}
                        //else
                        //{
                        string s = Request.QueryString["id"].Trim();
                        switch (s)
                        {
                            case "":
                                Response.Redirect(Request.UrlReferrer.ToString());
                                break;
                            default:
                                {
                                    string Requisition = Functions.Base64Decode(s);
                                    documentNo = Requisition;
                                    ViewBag.WordHtml = Requisition;
                                    string username = System.Web.HttpContext.Current.Session["Username"].ToString();
                                    var datavalues = GetItemsList.Getitemdetail(username, Requisition, "self");
                                    dynamic json = JObject.Parse(datavalues);
                                    break;
                                }
                        }

                        break;
                        //}
                    }
            }
            return View();
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
                //string CancelOpenLeaveresponseString = LeavesXMLRequests.CancelOpenLeave(documentNo);

                //dynamic json = JObject.Parse(CancelOpenLeaveresponseString);

                //status = json.Status;
                //Message = json.Message;
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