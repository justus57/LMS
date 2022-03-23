using LMS.CustomsClasses;
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
    public class AssignTransportRequestController : Controller
    {
        static string DocumentNo = "";
        static string _VehicleClassRequested = "";
        public DropDownList AssignedDriver { get; private set; }
        public DropDownList AssignedVehicleClass { get; private set; }
        public DropDownList VehicleClassRequested { get; private set; }

        // GET: AssignTransportRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AssignTransportRequest()
        {
            Session["ErrorMessage"] = "";
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "active";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";

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
                        LoadVehicleClass(VehicleClassRequested);

                        LoadTransportRequest();
                        LoadEmployees(AssignedDriver);

                        LoadVehiclesToAssign(AssignedVehicleClass, _VehicleClassRequested);
                    }
                }
            }
            return View();
        }
        //
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
            string TransportManagerComment = TransportRequestResponseObj.TransportManagerComment;
            string VehicleClassRequested = TransportRequestResponseObj.VehicleClassRequested;
            string Destination = TransportRequestResponseObj.Destination;
            string VehicleAssigned = TransportRequestResponseObj.VehicleAssigned;

            _VehicleClassRequested = VehicleClassRequested;

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
                VehicleClassAssigned = VehicleClassAssigned,
                DriverAssigned = DriverAssigned,
                RejectionComment = RejectionComment,
                TransportManagerComment = TransportManagerComment,
                Destination = Destination,
                VehicleClassRequested = VehicleClassRequested,
                EstimatedTripCost = TripCost,
                VehicleAssigned = VehicleAssigned


            };
            return JsonConvert.SerializeObject(_TransportRequest);
        }
        public void LoadVehiclesToAssign(DropDownList _DropDownList, string VehicleClass)
        {
            _DropDownList.Items.Clear();

            WebRef.ExportVehicles _ExportVehicles = new WebRef.ExportVehicles();
            WebserviceConfig.ObjNav.ExportVehiclesList(ref _ExportVehicles, VehicleClass);

            foreach (var vehicles in _ExportVehicles.ExportVehicle)
            {
                _DropDownList.Items.Insert(0, new ListItem(vehicles.RegNo + " - " + vehicles.VehicleClass, vehicles.RegNo));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }
        public void LoadVehicleClass(DropDownList _DropDownList)
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
        public void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode6 = json.ShortcutDimension6Code;
            string ShortcutDimCode4 = json.ShortcutDimension4Code;
            System.Web.HttpContext.Current.Session["LCY"] = json.LCY;

            LoadDimCodeValues(DimCode1, GlobalDimCode1);
            LoadDimCodeValues(DimCode2, GlobalDimCode2);
            LoadDimCodeValues(DimCode3, ShortcutDimCode3);
            LoadDimCodeValues(DimCode4, ShortcutDimCode4);
            LoadDimCodeValues(DimCode6, ShortcutDimCode6);

            DimCode1Label.Text = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
            DimCode2Label.Text = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
            DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            DimCode4Label.Text = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            DimCode6Label.Text = "Your Department";
            //load dynamic list

        }
        public void LoadDimCodeValues(DropDownList _DropDownList, string Code)
        {
            _DropDownList.Items.Clear();

            foreach (var kvp in CreateAdvanceRequestXMLRequests.GetDimCode(Code))
            {
                _DropDownList.Items.Insert(0, new ListItem(kvp.Key + " - " + kvp.Value, kvp.Key));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }

        public void LoadEmployees(DropDownList _DropDownList)
        {
            _DropDownList.Items.Clear();

           WebRef.DriversList _DriversList = new WebRef.DriversList();
            WebserviceConfig.ObjNav.ExportDriversList(ref _DriversList);

            foreach (var driver in _DriversList.Driver)
            {
                _DropDownList.Items.Insert(0, new ListItem(driver.IdNumber + " - " + driver.Name, driver.IdNumber));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
       
        public static string Save(string param1, string param2, string param3)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string AssignedVehicle = param1;
            string AssignedDriver = param2;
            string Comments = param3;

            Comments = AppFunctions.EscapeInvalidXMLCharacters(Comments);

            try
            {
                string response = WebserviceConfig.ObjNav.AssignVehicle(DocumentNo, AssignedVehicle, AssignedDriver, Comments, username);

                dynamic json = JObject.Parse(response);

                _Status = json.Status;
                _Message = json.Msg;

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