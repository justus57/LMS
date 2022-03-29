using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class FillVehicleLogSheetController : Controller
    {
        static string DocumentNo = null;
        DropDownList VehicleAssigned ;
        DropDownList VehicleClass;
        FillVehicleLogSheet logSheet = new FillVehicleLogSheet(); 
        // GET: FillVehicleLogSheet
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FillVehicleLogSheet()
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
                        LoadVehicleClass(VehicleClass);
                        
                        LoadVehicleClassAssigned(VehicleAssigned);

                        LoadTransportRequest();
                        LoadLogsheet(DocumentNo, status);
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
            string Destination = TransportRequestResponseObj.Destination;
            string VehicleClassRequested = TransportRequestResponseObj.VehicleClassRequested;
            string VehicleAssigned = TransportRequestResponseObj.VehicleAssigned;

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
                VehicleAssigned = VehicleAssigned,
                DriverAssigned = DriverAssigned,
                RejectionComment = RejectionComment,
                Destination = Destination,
                VehicleClassRequested = VehicleClassRequested,
                EstimatedTripCost = TripCost

            };
            return JsonConvert.SerializeObject(_TransportRequest);
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
        private void LoadVehicleClassAssigned(DropDownList _DropDownList)
        {
            _DropDownList.Items.Clear();

            WebRef.ExportVehicles _ExportVehicles = new WebRef.ExportVehicles();
            WebserviceConfig.ObjNav.ExportVehiclesList(ref _ExportVehicles, "All");

            foreach (var vehicles in _ExportVehicles.ExportVehicle)
            {
                _DropDownList.Items.Insert(0, new ListItem(vehicles.RegNo + " - " + vehicles.VehicleClass, vehicles.RegNo));
            }
            _DropDownList.Items.Insert(0, new ListItem(" ", ""));
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

            LoadDimCodeValues(logSheet.DimCode1, GlobalDimCode1);
            LoadDimCodeValues(logSheet.DimCode2, GlobalDimCode2);
            LoadDimCodeValues(logSheet.DimCode3, ShortcutDimCode3);
            LoadDimCodeValues(logSheet.DimCode4, ShortcutDimCode4);
            LoadDimCodeValues(logSheet.DimCode6, ShortcutDimCode6);

            logSheet.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
            logSheet.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
            logSheet.DimCode3Label = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            logSheet.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            logSheet.DimCode6Label = "Your Department";
            //load dynamic list

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
        private void LoadLogsheet(string TransportRequestNo, string status)
        {
            string str = GetLogSheetsLines(TransportRequestNo, status);

           // placeholder.Controls.Add(new Literal {  = str.ToString() });
        }
        public static string GetLogSheetsLines(string TransportRequestNo, string status)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            WebRef.ExportVehicleLogs _ExportVehicleLogs = new WebRef.ExportVehicleLogs();
            WebserviceConfig.ObjNav.ExportVehicleLogSheet(ref _ExportVehicleLogs, TransportRequestNo, username, "");

            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th>Date</th>");
            html.Append("<th>Destination</th>");
            html.Append("<th>Trip Start Time</th>");
            html.Append("<th>Odometer Reading</th>");
            html.Append("<th>Trip End Time</th>");
            html.Append("<th>Odometer Reading</th>");

            html.Append("<th>Mileage</th>");
            html.Append("<th>Estimated Cost</th>");
            html.Append("<th>Notes</th>");
            html.Append("<th>Action</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            double SumMileageCovered = 0, MileageCovered = 0, SumEstimatedCost = 0, EstimatedCost = 0;

            foreach (var item in _ExportVehicleLogs.ExportVehicleLogSheets)
            {
                string Mileage = item.Mileage[0];
                string Cost = item.EstimatedCost[0];

                if (Mileage != "")
                {
                    MileageCovered = Convert.ToDouble(Mileage);
                }
                if (Cost != "")
                {
                    EstimatedCost = Convert.ToDouble(Cost);
                }

                SumMileageCovered = SumMileageCovered + MileageCovered;
                SumEstimatedCost = SumEstimatedCost + EstimatedCost;
            }

            //footer
            html.Append("<tfoot>");
            html.Append("<tr>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th>" + string.Format("{0:N2}", SumMileageCovered) + "</th>");

            html.Append("<th>" + string.Format("{0:N2}", SumEstimatedCost) + "</th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");


            foreach (var item in _ExportVehicleLogs.ExportVehicleLogSheets)
            {

                if (item.Date != "")
                {
                    html.Append("<tr>");


                    html.Append("<td>" + item.Date + "</td>");
                    html.Append("<td>" + item.Destination + "</td>");
                    html.Append("<td>" + item.StartTime[0] + "</td>");
                    html.Append("<td>" + item.StartTimeOdometerReading + "</td>");
                    html.Append("<td>" + item.EndTime[0] + "</td>");
                    html.Append("<td>" + item.EndTimeOdometerReading + "</td>");
                    html.Append("<td>" + item.Mileage[0] + "</td>");
                    html.Append("<td>" + item.EstimatedCost[0] + "</td>");
                    html.Append("<td>" + item.Notes + "</td>");

                    if (status == "Unclosed")
                    {
                        html.Append("<td><a class = 'btn btn-danger btn-xs delete_line' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance request line'><span class = 'fa fa-trash-alt' > </span></a> " +
                          "<a class = 'btn btn-secondary btn-xs EditLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Edit advance request line'><span class = 'fa fa-pencil-alt' > </span></a></td>");

                    }
                    else
                    {
                        html.Append("<td> </td>");

                    }

                    html.Append("</tr>");
                }
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string str = html.ToString();

            return str;
        }

        
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "900";
            string _Message = "";


            string DestinationRoute = param1;
            string StartDateTime = param2;
            string StartTimeOdometerReading = param3;
            string EndTime = param4;
            string EndTimeOdometerReading = param5;
            string Notes = param6;
            Notes = AppFunctions.EscapeInvalidXMLCharacters(Notes);

            string Date = AppFunctions.ConvertToNavDate(StartDateTime);
            string StartTime = AppFunctions.ConvertToNavTime(StartDateTime);

            EndTime = AppFunctions.ConvertToNavTime(EndTime);

            try
            {
                string response = WebserviceConfig.ObjNav.CreateLogSheet(DocumentNo, username, DestinationRoute, Date, StartTime, StartTimeOdometerReading, EndTime, EndTimeOdometerReading, Notes);

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
        
        public static string UpdateLog(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "900";
            string _Message = "";

            string DestinationRoute = param1;
            string StartDateTime = param2;
            string StartTimeOdometerReading = param3;
            string EndTime = param4;
            string EndTimeOdometerReading = param5;
            string Notes = param6;
            string LineNo = param7;

            string Date = AppFunctions.ConvertToNavDate(StartDateTime);
            string StartTime = AppFunctions.ConvertToNavTime(StartDateTime);

            EndTime = AppFunctions.ConvertToNavTime(EndTime);
            try
            {
                string response = WebserviceConfig.ObjNav.UpdateLogSheet(Convert.ToInt32(LineNo), username, DestinationRoute, Date, StartTime, StartTimeOdometerReading, EndTime, EndTimeOdometerReading, Notes);

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
        
        public static string DeleteLine(string param1)
        {
            string No = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.DeleteVehicleLogSheet(Convert.ToInt16(No));

                dynamic json = JObject.Parse(response);

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string GetRequestLineDetails(string param1)
        {
            string LogSheetLineResponse = WebserviceConfig.ObjNav.GetLogSheetLine(Convert.ToInt32(param1));
            LogSheetLine LogSheeteObj = JsonConvert.DeserializeObject<LogSheetLine>(LogSheetLineResponse);


            string No = LogSheeteObj.No;
            string DestinationRoute = LogSheeteObj.DestinationRoute;
            string EndTimeOdometerReading = LogSheeteObj.EndTimeOdometerReading;
            string StartTimeOdometerReading = LogSheeteObj.StartTimeOdometerReading;
            string TripEndDateTime = LogSheeteObj.TripEndDateTime;
            string TripStartDateTime = LogSheeteObj.TripStartDateTime;
            string Notes = LogSheeteObj.Notes;


            var _LogSheetLine = new LogSheetLine
            {
                No = No,
                DestinationRoute = DestinationRoute,
                EndTimeOdometerReading = EndTimeOdometerReading,
                StartTimeOdometerReading = StartTimeOdometerReading,
                TripEndDateTime = TripEndDateTime,
                TripStartDateTime = TripStartDateTime,
                Notes = Notes

            };
            return JsonConvert.SerializeObject(_LogSheetLine);
        }

        
        public static string CloseTrip()
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            try
            {
                string response = WebserviceConfig.ObjNav.EndTrip(DocumentNo, username);

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