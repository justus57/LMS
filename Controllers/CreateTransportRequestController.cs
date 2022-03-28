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
    public class CreateTransportRequestController : Controller
    {
        CreateTransportRequest createTransport = new CreateTransportRequest();
        // GET: CreateTransportRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateTransportRequest()
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
                    GetDimensionCodes();
                }
            }
            return View();
        }
        private void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);

            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode6 = json.ShortcutDimension6Code;
            string ShortcutDimCode4 = json.ShortcutDimension4Code;
            System.Web.HttpContext.Current.Session["LCY"] = json.LCY;

            LoadDimCodeValues(createTransport.DimCode1, GlobalDimCode1);
            LoadDimCodeValues(createTransport.DimCode2, GlobalDimCode2);
            LoadDimCodeValues(createTransport.DimCode3, ShortcutDimCode3);
            LoadDimCodeValues(createTransport.DimCode4, ShortcutDimCode4);
            LoadDimCodeValues(createTransport.DimCode6, ShortcutDimCode6);

            LoadVehicleClass(createTransport.VehicleClass);

            createTransport.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
            createTransport.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
            createTransport.DimCode3Label = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            createTransport.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            createTransport.DimCode6Label = "Your Department";
            //load dynamic list

        }
        private void LoadVehicleClass(DropDownList _DropDownList)
        {
            _DropDownList.Items.Clear();

            WebRef.VehicleClass _VehicleClass = new WebRef.VehicleClass();
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

            foreach (var kvp in CreateAdvanceRequestXMLRequests.GetDimCode(Code))
            {
                _DropDownList.Items.Insert(0, new ListItem(kvp.Key + " - " + kvp.Value, kvp.Key));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
        
        public static string GetVehicleClassRate(string Code)
        {
            string rate = WebserviceConfig.ObjNav.GetVehicleClassRate(Code);
            rate = rate.Replace(",", "");
            dynamic obj = new JObject();
            obj.VehicleClass = Code;
            obj.Rate = rate;

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return jsonString;
        }
        
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11)
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
                DateTime date1 = Convert.ToDateTime(StartTime);
                DateTime date2 = Convert.ToDateTime(EndTime);
                int result = DateTime.Compare(date1, date2);
                string relationship;

                if (result < 0)
                    relationship = "is earlier than";
                else if (result == 0)
                    relationship = "is the same time as";
                else
                    relationship = "is later than";


                if (result < 0)
                {
                    string DocumentNo = "";

                    string DocumentNoXMLResponse = WebserviceConfig.ObjNav.GetTransportRequestNo();

                    dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

                    string Status = jsonDocumentNo.Status;

                    if (Status == "000")
                    {
                        DocumentNo = jsonDocumentNo.DocumentNo;

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
                }
                else
                {
                    _Status = "999";
                    _Message = "Start time not okay";
                }

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }

    }
}