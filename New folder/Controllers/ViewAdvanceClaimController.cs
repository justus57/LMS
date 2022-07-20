using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class ViewAdvanceClaimController : Controller
    {
        ViewAdvanceClaim AdvanceClaim = new ViewAdvanceClaim();
        // GET: ViewAdvanceClaim
        public ActionResult Index()
        {
            return View();
        }
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string fileforDownload = "";
        static string attachmentName = "";
        //  static string AdvanceClaimRequestsHeader = "";
        static string _ShortcutDimCode3 = "";
        public ActionResult ViewAdvanceClaim()
        {
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

            if (Session["Logged"].Equals("No"))
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

                    string id = Request.QueryString["id"].Trim();
                    //string code = Request.QueryString["code"].Trim();
                    string status = Request.QueryString["status"].Trim();
                    Session["StaffClaimNo"] = AppFunctions.Base64Decode(id);

                    bool IsPostBack = false||true;
                    if (!IsPostBack)
                    {
                        GetDimensionCodes();
                        LoadPrefferedMethodOfPayment();
                    }

                    LoadStaffClaimLines(Session["StaffClaimNo"].ToString(), status);
                    LoadAdvanceRequest(Session["StaffClaimNo"].ToString());
                    LoadTableAttachments(Session["StaffClaimNo"].ToString(), status);
                }
            }
            return View();
        }
        private void LoadTableAttachments(string AdvanceRequestHdrNo, string status)
        {
            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(AdvanceRequestHdrNo, folderPath, "AdvanceRequests", status, "StaffClaim");

            // DataTable 
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTableAttachment' width='100%' cellspacing='0'>");
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
            ////Append the HTML string to Placeholder.
            ViewBag.ViewAdvanceClaim = strText;
           // placeholder1.Controls.Add(new Literal { Text = html.ToString() });
        }
        
        public static string DeleteAdvanceRequestAttachment(string param1)
        {
            string _Status = "";
            string _Message = "";

            string DeleteAttachmentResponse = AdvanceRequestsXMLRequests.DeleteAttachment(param1);

            dynamic jsonDeleteAttachmentResponse = JObject.Parse(DeleteAttachmentResponse);

            _Status = jsonDeleteAttachmentResponse.Status;
            _Message = jsonDeleteAttachmentResponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        private void LoadPrefferedMethodOfPayment()
        {
            //PreferredPaymentMethod.Items.Clear();
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Mpesa", "2"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Cheque ", "1"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem(" ", "0"));
        }
        public void LoadAdvanceRequest(string AdvanceClaimRequestsHeader)
        {
            string AdvanceRequestData = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", AdvanceClaimRequestsHeader);
            dynamic json = JObject.Parse(AdvanceRequestData);

            AdvanceClaim.DateDue = json.DateDue;
            AdvanceClaim.DateOfRequest = json.DateOfRequest;
            AdvanceClaim.MissionSummary = json.MissionSummary;
            string GlobalDimCode1 = json.GlobalDimCode1;
            string GlobalDimCode2 = json.GlobalDimCode2;
            string ShortcutDimCode3 = json.ShortcutDimCode3;
            string ShortcutDimCode4 = json.ShortcutDimCode4;
            string ShortcutDimCode8 = json.ShortcutDimCode8;
            string _PreferredPaymentMethod = json.PreferredPaymentMethod;
            AdvanceClaim.Employee = json.Requester;


            //DimCode1.Items.FindByValue(GlobalDimCode1).Selected = true;
            //DimCode2.Items.FindByValue(GlobalDimCode2).Selected = true;
            ////DimCode3.Items.FindByValue(ShortcutDimCode3).Selected = true; //GF
            //DimCode4.Items.FindByValue(ShortcutDimCode4).Selected = true;
            //// DimCode8.Items.FindByValue(ShortcutDimCode8).Selected = true; //GF
            //PreferredPaymentMethod.Items.FindByValue(_PreferredPaymentMethod).Selected = true;
        }
        
        public static string LoadInterventions()
        {
            List<AdvanceRequestTypes> AdvanceRequestTypesList = new List<AdvanceRequestTypes>();

            LMS.WebRef.DimCodeValues _DimCodeValues = new LMS.WebRef.DimCodeValues();
            WebserviceConfig.ObjNav.ExportDimensionCodeValues("INTERVENTION", ref _DimCodeValues);

            foreach (var kvp in _DimCodeValues.DimCodeValue)
            {
                AdvanceRequestTypesList.Add(new AdvanceRequestTypes { Code = kvp.Code, Description = kvp.Code + " - " + kvp.Name });
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        
        public static string LoadUnitsOfMeasure()
        {
            return CreateAdvanceRequestXMLRequests.GetUnitOfMeasure();
        }
        
        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }

        
        public static string LoadAdvanceRequestLines(string param1)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceRequestLines(param1);
        }
        
        public static string LoadAdvanceRequestItem(string Code)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceType(Code);
        }
        
        public static string LoadBudgetLineCode()
        {
            return CreateAdvanceRequestXMLRequests.GetBudgetLineCode(_ShortcutDimCode3);
        }
        
        public JsonResult GetRequestLineDetails(string param1)
        {
            string LineNo = param1;

            string GetAdvanceRequestLineResponse = CustomsClasses.StaffClaims.GetAdvanceRequestLine(LineNo);
            dynamic jsonGetAdvanceRequestLineResponse = JObject.Parse(GetAdvanceRequestLineResponse);

            string Item = jsonGetAdvanceRequestLineResponse.Item;
            string ItemDescription = jsonGetAdvanceRequestLineResponse.ItemDescription;
            string UnitOfMeasure = jsonGetAdvanceRequestLineResponse.UnitOfMeasure;
            string NoOfUnits = jsonGetAdvanceRequestLineResponse.NoOfUnits;
            string UnitCost = jsonGetAdvanceRequestLineResponse.UnitCost;
            string ClaimedAmountLCY = jsonGetAdvanceRequestLineResponse.ClaimedAmountLCY;
            string ShortcutDimCode3 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode3;
            string ShortcutDimCode5 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode5;
            string Purpose = jsonGetAdvanceRequestLineResponse.Purpose;

            var _AdvanceRequestLines = new AdvanceRequestLines
            {
                No = LineNo,
                Item = Item,
                ItemDescription = ItemDescription,
                Purpose = Purpose,
                UnitOfMeasure = UnitOfMeasure,
                NoOfUnits = NoOfUnits,
                UnitCost = UnitCost,
                ClaimedAmountLCY = ClaimedAmountLCY,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode5 = ShortcutDimCode5
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines),JsonRequestBehavior.AllowGet);
        }
        private void LoadStaffClaimLines(string AdvanceRequestHdrNo, string status)
        {
            string strText = CustomsClasses.StaffClaims.GetAdvanceClaimLinesTable(AdvanceRequestHdrNo, status);

            //placeholder.Controls.Add(new Literal { Text = strText.ToString() });
        }
        private void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CustomsClasses.CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode4 = json.ShortcutDimension4Code;
            string ShortcutDimCode8 = json.ShortcutDimension8Code;
            _ShortcutDimCode3 = ShortcutDimCode3;
            System.Web.HttpContext.Current.Session["LCY"] = json.LCY;

            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                //DimCode8Label.Text = "Region to be Paid From";
                LoadDimCodeValues(AdvanceClaim.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(AdvanceClaim.DimCode2, GlobalDimCode2);
                // LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                AdvanceClaim.DimCode1Label  = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                AdvanceClaim.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                //DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(AdvanceClaim.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(AdvanceClaim.DimCode2, GlobalDimCode2);
                LoadDimCodeValues(AdvanceClaim.DimCode4, ShortcutDimCode4);

                AdvanceClaim.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                AdvanceClaim.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                AdvanceClaim.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            }
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

        
        public JsonResult PrintRequest(string param1)
        {
            string AdvanceRequestHdrNo = AppFunctions.Base64Decode(param1);

            string status = "000";
            string Msg = "Success";

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");

            string ExportToPath = folderPath + AdvanceRequestHdrNo + ".pdf";

            try
            {
                string SubmitAdvanceRequestXMLResponse = CustomsClasses.WebService.ExportAdvanceRequestReport("1", AdvanceRequestHdrNo, ExportToPath);

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
                Status = AdvanceRequestHdrNo,
                Message = Msg
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
        protected void btn_download_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentName + "");
            // Response.TransmitFile(Server.MapPath("~/doc/help.pdf"));
            Response.TransmitFile(fileforDownload);
            Response.End();
        }
        protected void View_Click(object sender, EventArgs e)
        {
            string pdfPath = fileforDownload;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
        }
        
        public JsonResult DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "000";
            string _Message = "";

            string DeleteAdvanceRequestLineXMLRequestsRssponse = CustomsClasses.StaffClaims.DeleteAdvanceRequestLine(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), LineNo);
            dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(DeleteAdvanceRequestLineXMLRequestsRssponse);

            _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
            _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult CreateAdvanceClaim(string param1, string param2, string param3, string param4, string param5, string param6,
          string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
          string param15, string param16, string param17, string param18, string param19, string param20)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            string BudgetLineCode = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string NoOfUnits = param7;
            string UnitCost = param6;
            string Amount = param8;

            string DateOfRequest = param9;
            string DateDue = param10;
            string RequestToCompany = "";
            // string DimCode1 = param12;
            //string DimCode2 = param13;
            //string DimCode8 = param14;
            //string DimCode3 = param15;
            string PreferredPaymentMethod = param18;
            string Currency = "";
            string MissionSummary = param19;
            string Purpose = param20;

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");

            //get Dims
            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString());
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;


            string UpdateAdvanceRequestResponse = CustomsClasses.StaffClaims.UpdateAdvanceRequest(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), PreferredPaymentMethod, MissionSummary);


            string CreateAdvanceRequestXMLRequestsRssponse = CustomsClasses.StaffClaims.CreateAdvanceClaimLine(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, BudgetLineCode, DimCode4, "", "", "", "");
            dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

            string LineNo = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;

            //SaveAttachment(DocumentPath, FileName, AdvanceClaimRequestsHeader, LineNo, "0");

            _Status = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();// jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
            _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult UpdateAdvanceClaim(string param1, string param2, string param3, string param4, string param5, string param6,
            string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
            string param15, string param16, string param17, string param18, string param19, string param20)
        {

            string LineNo = param17;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            string BudgetLineCode = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string UnitCost = param6;
            string NoOfUnits = param7;
            string Amount = param8;

            string DateOfRequest = param9;
            string DateDue = param10;
            string RequestToCompany = "";
            // string DimCode1 = param12;
            //string DimCode2 = param13;
            //string DimCode3 = param14;
            //string DimCode8 = param15;
            string PreferredPaymentMethod = param18;
            string MissionSummary = param19;
            string Purpose = param20;
            string Currency = "";

            //get Dims
            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString());
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

            string UpdateAdvanceRequestResponse = CustomsClasses.StaffClaims.UpdateAdvanceRequest(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), PreferredPaymentMethod, MissionSummary);

            string CreateAdvanceRequestXMLRequestsRssponse = CustomsClasses.StaffClaims.UpdatAdvanceClaimLine(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "", Purpose, DimCode1, DimCode2, DimCode1, DimCode2, BudgetLineCode, DimCode4, "", "", "", "");
            dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

            string LineNo2 = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;

            _Status = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();// jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
            _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string DateOfRequest = param1;
            string DateDue = param2;
            string RequestToCompany = param3;
            string DimCode1 = "";
            string DimCode2 = "";
            string DimCode3 = "";
            string DimCode4 = "";
            string DimCode5 = "";
            string DimCode8 = "";
            string Currency = param8;
            string PreferredPaymentMethod = param9;
            string MissionSummary = param10;

            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                DimCode1 = param4;
                DimCode2 = param5;
                DimCode3 = "";
                DimCode4 = param6;
                DimCode5 = "";
                DimCode8 = param7;
            }
            else
            {
                DimCode1 = param4;
                DimCode2 = param5;
                DimCode3 = "";
                DimCode4 = param6;
                DimCode5 = "";
                DimCode8 = "";
            }

            try
            {
                string UpdateAdvanceRequestResponse = CustomsClasses.StaffClaims.UpdateAdvanceRequest(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString(), PreferredPaymentMethod, MissionSummary);
                dynamic jsonUpdateAdvanceRequestResponse = JObject.Parse(UpdateAdvanceRequestResponse);
                _Status = jsonUpdateAdvanceRequestResponse.Status;
                _Message = jsonUpdateAdvanceRequestResponse.Msg;

                //submit
                string SubmitAdvanceRequestXMLResponse = CustomsClasses.StaffClaims.SubmitAdvanceRequest(System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString());
                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SaveAttachment(string param1, string param2)
        {
            string _Status = "000";
            string _Message = "";

            string FileName = param1;
            string AttachmentDescription = param2;

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string DocumentPath = folderPath + FileName;

            //call upload file function here
            string DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

            bool IsUploaded = Convert.ToBoolean(CreateAdvanceRequestXMLRequests.UploadFile("1", DocumentNo, DocumentPath, AttachmentDescription));

            if (IsUploaded)
            {
                _Message = "Attachment has been uploaded successfully";
            }
            else
            {
                _Message = "Attachment failed to upload";
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public static void SaveAttachment(string Path, string FileName, string _AdvanceClaimRequestsHeader, string lineNo, string lineNo2)
        {
            string AttachmentDescription = FileName;

            bool IsUploaded = Convert.ToBoolean(StaffClaims.UploadFile("1", _AdvanceClaimRequestsHeader, Path, AttachmentDescription, lineNo, lineNo2));

        }
        
        public JsonResult RejectApplication(string param1, string param2)
        {
            string DocumentNo = AppFunctions.Base64Decode(param1);
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

                        string RejectWorkflowApprovalRequestResponse = CustomsClasses.WebService.RejectWorkflowApprovalRequest(DocumentNo);
                        dynamic json = JObject.Parse(RejectWorkflowApprovalRequestResponse);

                        status = json.Status;
                        response = json.Msg;

                        WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(DocumentNo), 1, rejectionComment);

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
        
        public JsonResult ApproveStaffClaim(string param1)
        {
            string _Status = "";
            string _Message = "";

            string StaffAdvanceNo = AppFunctions.Base64Decode(param1);

            string ApproveWorkflowApprovalRequestResponse = CustomsClasses.WebService.ApproveWorkflowApprovalRequest(StaffAdvanceNo);
            dynamic json = JObject.Parse(ApproveWorkflowApprovalRequestResponse);

            _Status = json.Status;
            _Message = json.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ValidateDim3Code(string param1)
        {
            return Json(CreateAdvanceRequestXMLRequests.ValidateShortcutDimCode3(param1),JsonRequestBehavior.AllowGet);
        }
    }
}