﻿using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class TransportRequestsController : Controller
    {
        // GET: TransportRequests
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TransportRequests()
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
                        LoadTable(status);
                    }
                }
            }
            return View();
        }
        private void LoadTable(string status)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable dt = new DataTable();

            dt.Clear();

            dt = GetTransportRequests(status, username);

            // DataTable 
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();
            ViewBag.TransportRequests = strText;
            ////Append the HTML string to Placeholder.
          //  placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }
        public static DataTable GetTransportRequests(string status, string CreatedBy)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Request start date", typeof(string));
            table.Columns.Add("Request end date", typeof(string));


            //new approach
            int prstatus = 0;

            if (status == "Open")
            {
                prstatus = 0;
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Pending")
            {
                prstatus = 2;
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Approver", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Approved")
            {
                prstatus = 1;
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Unassigned")
            {
                prstatus = 3;
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Requester", typeof(string));
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Assigned")
            {
                prstatus = 4;
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Requester", typeof(string));
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Unclosed")
            {
                prstatus = 5;
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Requester", typeof(string));
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Closed")
            {
                prstatus = 6;
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Requester", typeof(string));
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }
            else if (status == "Ended")
            {
                prstatus = 7;
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Requester", typeof(string));
                table.Columns.Add("Vehicle Class Requested", typeof(string));
                table.Columns.Add("Driver Assigned", typeof(string));
                table.Columns.Add("Posted", typeof(string));
                table.Columns.Add("Action", typeof(string));
            }

            LMS.WebRef.TransportRequests _TransportRequest = new LMS.WebRef.TransportRequests();
            WebserviceConfig.ObjNav.ExportTransportRequests(ref _TransportRequest, username, prstatus);


            foreach (var transportrequest in _TransportRequest.TransportRequest)
            {
                string No = transportrequest.No;
                string ApprovalEntryNo = transportrequest.ApprovalEntryNo[0];
                string IsPosted = transportrequest.IsPosted[0];

                if (No != "")
                {
                    string IsEnded = "";

                    if (transportrequest.IsTripEnded == "No")
                    {
                        IsEnded = "<a class = 'btn btn-secondary btn-xs closeTrip' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Close transport request'><span class = 'fa fa-calendar-check' > </span></a> ";
                    }


                    if (status == "Open")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                           "<a class = 'btn btn-danger btn-xs delete_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete transport request'><span class = 'fa fa-trash-alt' > </span></a> " +
                           "<a class = 'btn btn-success btn-xs submit_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit transport request'><span class = 'fa fa-paper-plane' > </span></a> " +
                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0], transportrequest.Approver[0],
                           "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel transport request'><span class = 'fa fa-times' > </span></a> " +
                           "<a class = 'btn btn-success btn-xs delegate_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(ApprovalEntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delegate transport request'><span class = 'fa fa-fighter-jet' > </span></a> " +
                           //"<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' data-toggle='tooltip' title='View transport request'> </span></a> ");
                    }
                    else if (status == "Approved")
                    {
                        string driver = transportrequest.DriverAssigned[0];

                        if (driver == "")
                        {
                            table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                            "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                        }


                    }
                    else if (status == "Unassigned")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                            // "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                            "<a class = 'btn btn-success btn-xs' href = " + "AssignTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='Assign vehicle'><span class = 'fa fa-shuttle-van' data-toggle='tooltip' title='Assign vehicle'> </span></a> ");
                    }
                    else if (status == "Assigned")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                           "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                           "<a class = 'btn btn-primary btn-xs' href = " + "AssignTransportRequest.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                    }
                    else if (status == "Unclosed")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                           "<a class = 'btn btn-danger btn-xs EndTrip' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='End trip'><span class = 'fa fa-stop-circle' > </span></a> " +
                            "<a class = 'btn btn-success btn-xs' href = " + "FillVehicleLogSheet.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='Log sheets'><span class = 'fa fa-shuttle-van' data-toggle='tooltip' title='Log sheets'> </span></a>");
                    }
                    else if (status == "Closed")
                    {
                        if (transportrequest.IsTripEnded == "Yes")
                        {
                            table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                            "<a class = 'btn btn-success btn-xs openTrip' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reopen transport request'><span class = 'fa fa-folder-open' > </span></a> " +
                            "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                            "<a class = 'btn btn-primary btn-xs' href = " + "FillVehicleLogSheet.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0],
                           "<a class = 'btn btn-success btn-xs openTrip' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reopen transport request'><span class = 'fa fa-folder-open' > </span></a> " +
                           "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                           "<a class = 'btn btn-primary btn-xs' href = " + "FillVehicleLogSheet.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                        }

                    }
                    else if (status == "Ended")
                    {
                        table.Rows.Add(No, transportrequest.RequestStartDate, transportrequest.RequestEndDate, transportrequest.Destination, transportrequest.Requester[0], transportrequest.VehicleClassRequested, transportrequest.DriverAssigned[0], IsPosted,
                        "<a class = 'btn btn-danger btn-xs closeTrip' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Close transport request'><span class = 'fa fa-calendar-check' > </span></a> " +
                        "<a class = 'btn btn-secondary btn-xs print_tr' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print transport request'><span class = 'fa fa-print' > </span></a> " +
                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewClosedTrip.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + CustomsClasses.AppFunctions.Base64Encode(No) + " data-toggle='tooltip' title='View transport request'><span class = 'fa fa-eye' > </span></a> ");
                    }
                }
            }

            return table;
        }
        
        public static string DeleteRecord(string param1)
        {
            string No = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.DeleteTransportRequest(No);

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
        
        public static string SubmitRecord(string param1)
        {
            string No = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.SendTransportRequestForApproval(No);

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
        
        public static string CancelRequest(string param1)
        {
            string No = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.CancelTransportRequestApprovalRequest(No);

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
        
        public static string DelegateRequest(string param1)
        {
            string approvalEntry = AppFunctions.Base64Decode(param1);
            string status = "";
            string Msg = "";

            try
            {
                string SubmitAdvanceRequestXMLResponse = WebserviceConfig.ObjNav.DelegateWorkflowApprovalRequest(Convert.ToInt32(approvalEntry));

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string PrintRequest(string param1)
        {
            string No = param1;

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string CloseTrip(string param1)
        {
            string DocumentNo = param1;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.CloseTrip(DocumentNo);

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
        //
        
        public static string ReOpenTrip(string param1)
        {
            string DocumentNo = param1;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string status = "";
            string Msg = "";

            try
            {
                string response = WebserviceConfig.ObjNav.ReOpenTrip(DocumentNo, username);

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
        
        public static string EndTrip(string param1)
        {
            string _Status = "900";
            string _Message = "";
            string DocumentNo = param1;
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