using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class ViewTransportRequestController : Controller
    {
        static string DocumentNo = "";
        private readonly DropDownList VehicleClass;
        ViewTransportRequest  viewTransport = new ViewTransportRequest();
        // GET: ViewTransportRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewTransportRequest()
        {
            Session["ErrorMessage"] = "";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "active";

            if (Session["Logged"].Equals("No"))//set to No
            {
                Response.Redirect("Login.aspx");
            }
            else if (Session["Logged"].Equals("Yes"))
            {
                if (Session["RequirePasswordChange"].Equals("TRUE"))
                {
                    Response.Redirect("OneTimePass.aspx");
                }
                else
                {
                    string status = Request.QueryString["status"].Trim();

                    if (status == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        string id = Request.QueryString["id"].Trim();
                        DocumentNo = AppFunctions.Base64Decode(id);

                        GetDimensionCodes();
                        LoadTransportRequest();
                    }
                }
            }
            return View();
        }     
        public static string LoadTransportRequest()
        {
            double TripCost = 0;

            string TransportRequestResponse = WebserviceConfig.ObjNav.GetTransportRequest(DocumentNo);
            TransportRequest TransportRequestResponseObj = JsonConvert.DeserializeObject<TransportRequest>(TransportRequestResponse);

            string GlobalDimension1Code = TransportRequestResponseObj.GlobalDimension1Code;
            string GlobalDimension2Code = TransportRequestResponseObj.GlobalDimension2Code;
            string ShortcutDimension1Code = TransportRequestResponseObj.ShortcutDimension1Code;
            string ShortcutDimension2Code = TransportRequestResponseObj.ShortcutDimension2Code;
            string ShortcutDimension3Code = TransportRequestResponseObj.ShortcutDimension3Code;
            string ShortcutDimension4Code = TransportRequestResponseObj.ShortcutDimension4Code;
            string ShortcutDimension5Code = TransportRequestResponseObj.ShortcutDimension5Code;
            string ShortcutDimension6Code = TransportRequestResponseObj.ShortcutDimension6Code;
            string ShortcutDimension7Code = TransportRequestResponseObj.ShortcutDimension7Code;
            string ShortcutDimension8Code = TransportRequestResponseObj.ShortcutDimension8Code;

            //
            string No = TransportRequestResponseObj.No;
            string PurposeOfTrip = TransportRequestResponseObj.PurposeOfTrip;
            string VehicleClassAssigned = TransportRequestResponseObj.VehicleClassAssigned;
            string EstimatedDistance = TransportRequestResponseObj.EstimatedDistance;
            string DateVehicleAcquired = TransportRequestResponseObj.DateVehicleAcquired;
            string DriverAssigned = TransportRequestResponseObj.DriverAssigned;
            string RequestStartDateTime = TransportRequestResponseObj.RequestStartDateTime;
            string RequestEndDateTime = TransportRequestResponseObj.RequestEndDateTime;
            string Requester = TransportRequestResponseObj.Requester;
            string RejectionComment = TransportRequestResponseObj.RejectionComment;
            string VehicleClassRequested = TransportRequestResponseObj.VehicleClassRequested;
            string Destination = TransportRequestResponseObj.Destination;

            if (VehicleClassRequested != "")
            {
                string rate = WebserviceConfig.ObjNav.GetVehicleClassRate(VehicleClassRequested);
                rate = rate.Replace(",", "");

                TripCost = Convert.ToDouble(rate) * Convert.ToDouble(EstimatedDistance);
            }


            var _TransportRequest = new TransportRequest
            {
                No = No,
                Requester = Requester,
                RequestStartDateTime = RequestStartDateTime,
                RequestEndDateTime = RequestEndDateTime,
                GlobalDimension1Code = GlobalDimension1Code,
                GlobalDimension2Code = GlobalDimension2Code,
                ShortcutDimension1Code = ShortcutDimension1Code,
                ShortcutDimension2Code = ShortcutDimension2Code,
                ShortcutDimension3Code = ShortcutDimension3Code,
                ShortcutDimension4Code = ShortcutDimension4Code,
                ShortcutDimension5Code = ShortcutDimension5Code,
                ShortcutDimension6Code = ShortcutDimension6Code,
                ShortcutDimension7Code = ShortcutDimension7Code,
                ShortcutDimension8Code = ShortcutDimension8Code,
                DateVehicleAcquired = DateVehicleAcquired,
                EstimatedDistance = EstimatedDistance,
                PurposeOfTrip = PurposeOfTrip,
                VehicleClassRequested = VehicleClassRequested,
                VehicleClassAssigned = VehicleClassAssigned,
                DriverAssigned = DriverAssigned,
                RejectionComment = RejectionComment,
                Destination = Destination,
                EstimatedTripCost = TripCost

            };
            return JsonConvert.SerializeObject(_TransportRequest);
        }
        private void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CustomsClasses.CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode6 = json.ShortcutDimension6Code;
            string ShortcutDimCode4 = json.ShortcutDimension4Code;
            System.Web.HttpContext.Current.Session["LCY"] = json.LCY;

            LoadDimCodeValues(viewTransport.DimCode1, GlobalDimCode1);
            LoadDimCodeValues(viewTransport.DimCode2, GlobalDimCode2);
            LoadDimCodeValues(viewTransport.DimCode3, ShortcutDimCode3);
            LoadDimCodeValues(viewTransport.DimCode4, ShortcutDimCode4);
            LoadDimCodeValues(viewTransport.DimCode6, ShortcutDimCode6);

            LoadVehicleClass(VehicleClass);

            viewTransport.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
            viewTransport.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
            viewTransport.DimCode3Label = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            viewTransport.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            viewTransport.DimCode6Label = "Your Department";
            //load dynamic list

        }
        private void LoadVehicleClass(DropDownList _DropDownList)
        {
            _DropDownList.Items.Clear();

            LMS.WebRef.VehicleClass _VehicleClass = new LMS.WebRef.VehicleClass();
            WebserviceConfig.ObjNav.ExportVehicleClass(ref _VehicleClass);

            foreach (var vehicleclass in _VehicleClass.VehicleClass1)
            {
                _DropDownList.Items.Insert(0, new ListItem(vehicleclass.Code + " - " + vehicleclass.Name, vehicleclass.Code));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }
        private void LoadDimCodeValues(DropDownList _DropDownList, string Code)
        {
            _DropDownList.Items.Clear();

            LMS.WebRef.DimCodeValues _DimCodeValues = new LMS.WebRef.DimCodeValues();
            WebserviceConfig.ObjNav.ExportDimensionCodeValues(Code, ref _DimCodeValues);

            foreach (var kvp in _DimCodeValues.DimCodeValue)
            {
                _DropDownList.Items.Insert(0, new ListItem(kvp.Code + " - " + kvp.Name, kvp.Code));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
        /// <summary>
        /// Justus kasyoki c#Developer
        /// 
        /// this gets data from view using Json format
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        /// <returns></returns>
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string TransportStartDateTime = param1;
            string TransportEndDateTime = param2;
            string Purpose = param3;
            string DimCode1 = param4;
            string DimCode2 = param5;
            string DimCode3 = param6;
            string DimCode4 = param11;
            string DimCode6 = param7;
            string DimCode8 = "";
            string VehicleClass = param8;
            string EstimatedDistance = param9;
            string Destination = param10;


            Purpose = AppFunctions.EscapeInvalidXMLCharacters(Purpose);

            string StartDate = AppFunctions.ConvertToNavDate(TransportStartDateTime);
            string StartTime = AppFunctions.ConvertToNavTime(TransportStartDateTime);

            string EndDate = AppFunctions.ConvertToNavDate(TransportEndDateTime);
            string EndTime = AppFunctions.ConvertToNavTime(TransportEndDateTime);

            try
            {

                string response = WebserviceConfig.ObjNav.UpdateTransportRequest(DocumentNo, StartDate, StartTime, EndDate, EndTime, Purpose,
                       DimCode1, DimCode2,
                       DimCode1, DimCode2, DimCode3, DimCode4, "", DimCode6, "", DimCode8,
                       VehicleClass, Convert.ToDecimal(EstimatedDistance),
                       username,
                       Destination);

                dynamic json = JObject.Parse(response);

                _Status = json.Status;
                _Message = json.Msg;

                //submit

                string responsesubmit = WebserviceConfig.ObjNav.SendTransportRequestForApproval(DocumentNo);

                dynamic jsonsubmit = JObject.Parse(responsesubmit);

                _Status = jsonsubmit.Status;
                _Message = jsonsubmit.Msg;


            }
            catch (Exception es)
            {
                _Message = es.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }       
        public JsonResult PrintRequest(string param1)
        {
            string No = AppFunctions.Base64Decode(param1);

            string status = "000";
            string Msg = "Success";

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");

            string ExportToPath = folderPath + No + ".pdf";

            try
            {
                string SubmitAdvanceRequestXMLResponse = WebserviceConfig.ObjNav.ExportTransportRequestReport(No, ExportToPath);

                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        //Approver views      
        public JsonResult RejectApprovalRequest(string param1, string param2)
        {
            string ApprovalEntryNo = AppFunctions.Base64Decode(param1);
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string response = "";
            string status = "";

            //param 2 shouldnt be blank
            if (param2 != "")
            {
                string rejectionComment = null;
                //param2 trim to 200 chars
                try
                {
                    if (param2.Length > 200)
                    {
                        var truncated = param2.Substring(0, 200);

                        rejectionComment = truncated.ToString();
                    }
                    else
                    {
                        rejectionComment = AppFunctions.EscapeInvalidXMLCharacters(param2); //escape XML strings  

                        string RejectWorkflowApprovalRequestResponse = CustomsClasses.WebService.RejectWorkflowApprovalRequest(ApprovalEntryNo);
                        dynamic json = JObject.Parse(RejectWorkflowApprovalRequestResponse);

                        status = json.Status;
                        response = json.Msg;
                        WebserviceConfig.ObjNav.SaveApprovalRejectComment("TransportRequests", Convert.ToInt32(ApprovalEntryNo), 0, rejectionComment);

                    }
                }
                catch (Exception e)
                {
                    response = e.Message;
                }

            }
            else
            {
                response = "You must give the rejection comment";
                status = "999";
            }


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }      
        public JsonResult ApproveApprovalRequest(string param1)
        {
            string _Status = "";
            string _Message = "";

            string DocumentNo = AppFunctions.Base64Decode(param1);

            string ApproveWorkflowApprovalRequestResponse = WebService.ApproveWorkflowApprovalRequest(DocumentNo);

            dynamic json = JObject.Parse(ApproveWorkflowApprovalRequestResponse);

            _Status = json.Status;
            _Message = json.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }    
        public JsonResult GetVehicleClassRate(string Code)
        {
            string rate = WebserviceConfig.ObjNav.GetVehicleClassRate(Code);
            rate = rate.Replace(",", "");
            dynamic obj = new JObject();
            obj.VehicleClass = Code;
            obj.Rate = rate;

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return jsonString;
        }
    }
}