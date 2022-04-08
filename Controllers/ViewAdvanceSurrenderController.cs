using LMS.CustomsClasses;
using LMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Controllers
{
    public class ViewAdvanceSurrenderController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string AdvanceRequestsHeader = "";
        static string _AdvanceSurrenderRequestsHeader = "";
        static string AdvanceRequestsHeaderNumber = "";
        static string _ShortcutDimCode3 = "";
        ViewAdvanceSurrender advanceSurrender = new ViewAdvanceSurrender();
  

        // GET: ViewAdvanceSurrender
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAdvanceSurrender()
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "active";
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
                }
                else
                {

                    string id = Request.QueryString["id"].Trim();
                    string code = Request.QueryString["code"].Trim();
                    string status = Request.QueryString["status"].Trim();
                    AdvanceRequestsHeaderNumber = AppFunctions.Base64Decode(code);
                    _AdvanceSurrenderRequestsHeader = AppFunctions.Base64Decode(id);

                    bool IsPostBack = false||true;
                    if (!IsPostBack)
                    {
                        GetDimensionCodes();
                        LoadPrefferedMethodOfPayment();
                    }

                    LoadTable(_AdvanceSurrenderRequestsHeader, status);
                    LoadAdvanceRequestList();
                    LoadTableAttachments(_AdvanceSurrenderRequestsHeader, status);
                    LoadTableAttachments2(AdvanceRequestsHeaderNumber);
                    LoadSurrenderCaption(_AdvanceSurrenderRequestsHeader, status);
                }
            }

            return View();
        }
        private void LoadTableAttachments(string AdvanceRequestHdrNo, string status)
        {
            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(AdvanceRequestHdrNo, folderPath, "AdvanceRequests", status, "StaffSurrender");

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
            ViewBag.ViewAdvanceSurrender = strText;
           // placeholder1.Controls.Add(new Literal { Text = html.ToString() });
        }
        private void LoadTableAttachments2(string AdvanceRequestHdrNo)
        {

            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments2(AdvanceRequestHdrNo, folderPath, "AdvanceRequests", "Approved", "StaffAdvance");

            // DataTable 
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTableAttachment2' width='100%' cellspacing='0'>");
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
            ViewBag.LoadTableAttachments2 = strText;
           // placeholder2.Controls.Add(new Literal { Text = html.ToString() });
        }
        private void LoadSurrenderCaption(string AdvanceSurrenderRequestsHeader, string status)
        {
            StringBuilder html = new StringBuilder();


            html.Append(AdvanceSurrender.GetAdvanceSurrenderLines(AdvanceSurrenderRequestsHeader, status));

            ViewBag.LoadSurrenderCaption = html;
           // placeholder3.Controls.Add(new Literal { Text = html.ToString() });
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

        
        public static string LoadAdvanceSurrender()
        {
            return AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", _AdvanceSurrenderRequestsHeader);
        }

        
        public static string LoadAdvanceRequest(string param1)
        {
            AdvanceRequestsHeader = param1;

            return AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceRequestsHeader);
        }
        
        public static string LoadAdvanceRequestLines()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceRequestLines(AdvanceRequestsHeaderNumber);
        }
        
        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }
        
        public static string LoadAdvanceRequestItem(string Code)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceType(Code);
        }
        
        public JsonResult GetRequestLineDetails(string param1)
        {
            string LineNo = param1;

            string GetAdvanceRequestLineResponse = AdvanceSurrender.GetSurrenderRequestLine(LineNo);
            dynamic jsonGetAdvanceRequestLineResponse = JObject.Parse(GetAdvanceRequestLineResponse);

            string Item = jsonGetAdvanceRequestLineResponse.Item;
            string ItemDescription = jsonGetAdvanceRequestLineResponse.ItemDescription;
            string UnitOfMeasure = jsonGetAdvanceRequestLineResponse.UnitOfMeasure;
            string NoOfUnits = jsonGetAdvanceRequestLineResponse.NoOfUnits;
            string UnitCost = jsonGetAdvanceRequestLineResponse.UnitCost;
            string AdvancedAmountLCY = jsonGetAdvanceRequestLineResponse.AdvancedAmountLCY;
            string SurrenderedAmountLCY = jsonGetAdvanceRequestLineResponse.SurrenderedAmountLCY;
            string ShortcutDimCode3 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode3;
            string ShortcutDimCode5 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode5;
            string Remarks = jsonGetAdvanceRequestLineResponse.Remarks;
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
                AdvancedAmountLCY = AdvancedAmountLCY,
                SurrenderedAmountLCY = SurrenderedAmountLCY,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode5 = ShortcutDimCode5,
                Remarks = Remarks
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetAdvanceRequestLineDetails(string param1)
        {
            string LineNo = param1;

            string GetAdvanceRequestLineResponse = AdvanceSurrender.GetAdvanceRequestLine(LineNo);
            dynamic jsonGetAdvanceRequestLineResponse = JObject.Parse(GetAdvanceRequestLineResponse);

            string Item = jsonGetAdvanceRequestLineResponse.Item;
            string ItemDescription = jsonGetAdvanceRequestLineResponse.ItemDescription;
            string UnitOfMeasure = jsonGetAdvanceRequestLineResponse.UnitOfMeasure;
            string NoOfUnits = jsonGetAdvanceRequestLineResponse.NoOfUnits;
            string UnitCost = jsonGetAdvanceRequestLineResponse.UnitCost;
            string AdvancedAmountLCY = jsonGetAdvanceRequestLineResponse.AdvancedAmountLCY;
            string SurrenderedAmountLCY = jsonGetAdvanceRequestLineResponse.SurrenderedAmountLCY;
            string ShortcutDimCode3 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode3;
            string ShortcutDimCode5 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode5;
            string Remarks = jsonGetAdvanceRequestLineResponse.Remarks;

            var _AdvanceRequestLines = new AdvanceRequestLines
            {
                No = LineNo,
                Item = Item,
                ItemDescription = ItemDescription,
                UnitOfMeasure = UnitOfMeasure,
                NoOfUnits = NoOfUnits,
                UnitCost = UnitCost,
                AdvancedAmountLCY = AdvancedAmountLCY,
                SurrenderedAmountLCY = SurrenderedAmountLCY,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode5 = ShortcutDimCode5,
                Remarks = Remarks
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines), JsonRequestBehavior.AllowGet);
        }
        private void LoadPrefferedMethodOfPayment()
        {
            //PreferredPaymentMethod.Items.Clear();
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Mpesa", "2"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Cheque ", "1"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem(" ", "0"));
            List<PreferredPaymentMethods> selectListItems = new List<PreferredPaymentMethods>()
            {
               new PreferredPaymentMethods(){Id="2",Name="Mpesa"},
               new PreferredPaymentMethods(){Id="1",Name="Cheque"},
               new PreferredPaymentMethods(){Id="0",Name=" "}

            };
            ViewBag.LoadPrefferedMethodOfPayment = selectListItems;
        }
        private void LoadTable(string AdvanceRequestHdrNo, string status)
        {
            string strText = AdvanceSurrender.GetAdvanceSurrenderinesTable(AdvanceRequestHdrNo, status);
            ViewBag.LoadTable = strText;
           // placeholder.Controls.Add(new Literal { Text = strText.ToString() });
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
                LoadDimCodeValues(advanceSurrender.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(advanceSurrender.DimCode2, GlobalDimCode2);
                //LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                advanceSurrender.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                advanceSurrender.DimCode2Label= SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                //DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(advanceSurrender.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(advanceSurrender.DimCode2, GlobalDimCode2);
                LoadDimCodeValues(advanceSurrender.DimCode4, ShortcutDimCode4);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                advanceSurrender.DimCode1Label= SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                advanceSurrender.DimCode2Label= SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                advanceSurrender.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
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
        private void LoadAdvanceRequestList()
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            //AdvanceRequestList.Items.Clear();
            List<string> vs = new List<string>();
            foreach (var kvp in AdvanceSurrender.GetAdvanceRequestList("Approved", username))//here change status
            {
                // AdvanceRequestList.Items.Insert(0, new ListItem(kvp.Value, kvp.Key));
                //vs.Add(kvp.Value, kvp.Key);
            }
           // AdvanceRequestList.Items.Insert(0, new ListItem(" ", ""));
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
                string SubmitAdvanceRequestXMLResponse = CustomsClasses.WebService.ExportAdvanceRequestReport("2", AdvanceRequestHdrNo, ExportToPath);

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
        
        public JsonResult DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "000";
            string _Message = "Delete staff surrender Lines";

            string DeleteAdvanceRequestLineXMLRequestsRssponse = AdvanceSurrender.DeleteAdvanceRequestLine(_AdvanceSurrenderRequestsHeader, LineNo);
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
        
        public JsonResult DeleteAdvanceRequestAttachment(string param1)
        {
            string _Status = "000";
            string _Message = "";

            string DeleteAttachmentResponse = AdvanceSurrender.DeleteAttachment(param1);
            dynamic jsonDeleteAttachmentResponse = JObject.Parse(DeleteAttachmentResponse);

            _Status = jsonDeleteAttachmentResponse.Status;
            _Message = jsonDeleteAttachmentResponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SaveSurrender(string param1, string param2, string param3, string param4, string param5, string param6,
           string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
           string param15, string param16, string param17, string param18, string param19)
        {

            AdvanceRequestsHeaderNumber = param17;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            string DimCode5 = "";
            // string DimCode3 = "";



            //  string DimCode5 = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string UnitCost = param6;
            string NoOfUnits = param7;
            string Amount = param8;

            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", _AdvanceSurrenderRequestsHeader);
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                DimCode5 = param1;
            }
            else
            {
                DimCode3 = param1;
            }

            string Purpose = param19; ;

            //call upload file function here
            string DocumentNo = "";

            if (_AdvanceSurrenderRequestsHeader != "")
            {
                DocumentNo = _AdvanceSurrenderRequestsHeader;

                string CreateAdvanceRequestXMLRequestsRssponse = AdvanceSurrender.CreateAdvanceSurrenderLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, "", Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, DimCode5, "", "", DimCode8);
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

                string CreatedLineNo = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                //SaveAttachment(DocumentPath, FileName, DocumentNo, CreatedLineNo);

                _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            }


            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult UpdateSurrender(string param1, string param2, string param3, string param4, string param5, string param6,
            string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
            string param15, string param16, string param17, string param18, string param19, string param20, string param21)
        {

            string AdvanceRequestLineNo = param17;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            //set for different companies - GF uses Dim Code 5 for Advance Line
            string DimCode5 = "";
            // string DimCode3 = "";            


            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string UnitCost = param6;
            string NoOfUnits = param7;
            string Amount = param8;

            string DateOfRequest = param9;
            string DateDue = param10;
            string RequestToCompany = param11;
            //string DimCode1 = param12;
            //string DimCode2 = param13;
            //
            //string DimCode8 = param15;
            string Currency = param16;
            string PreferredPaymentMethod = param18;
            string Remarks = param19;
            string MissionSummary = param20;
            string Purpose = param21;

            //call upload file function here
            string DocumentNo = "";

            if (_AdvanceSurrenderRequestsHeader != "")
            {
                DocumentNo = _AdvanceSurrenderRequestsHeader;

                //get Dims
                string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", DocumentNo);
                dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

                string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
                string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
                string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
                string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
                string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

                if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                {
                    DimCode5 = param1;
                }
                else
                {
                    DimCode3 = param1;
                }

                //  string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffSurrender", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, "", "", "", "", DimCode8, Currency, AdvanceRequestsHeaderNumber, PreferredPaymentMethod, MissionSummary);

                string CreateAdvanceRequestXMLRequestsRssponse = AdvanceSurrender.UpdateAdvanceSurrenderLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, AdvanceRequestLineNo, Remarks, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, DimCode5, "", "", "");
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

                _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            }


            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string DateOfRequest = param1;
            string DateDue = param2;
            string RequestToCompany = param3;
            string DimCode1 = param4;
            string DimCode2 = param5;
            string DimCode3 = param6;
            string DimCode8 = param7;
            string Currency = param8;
            string PreferredPaymentMethod = param9;
            try
            {


                //string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(_AdvanceSurrenderRequestsHeader, "AdvanceSurrender", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, "", "", "", "", DimCode8, Currency, AdvanceRequestsHeaderNumber, PreferredPaymentMethod, "");
                //dynamic jsonUpdateAdvanceRequestResponse = JObject.Parse(UpdateAdvanceRequestResponse);
                //_Status = jsonUpdateAdvanceRequestResponse.Status;
                //_Message = jsonUpdateAdvanceRequestResponse.Msg;

                //submit
                string SubmitAdvanceRequestXMLResponse = AdvanceSurrender.SubmitAdvanceRequest(_AdvanceSurrenderRequestsHeader);
                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                AdvanceRequestsHeader = "";
                AdvanceRequestsHeaderNumber = "";
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

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
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
            string DocumentNo = _AdvanceSurrenderRequestsHeader;

            bool IsUploaded =Convert.ToBoolean(CreateAdvanceRequestXMLRequests.UploadFile("2", DocumentNo, DocumentPath, AttachmentDescription));

            if (IsUploaded)
            {
                _Message = "Attachment has been uploaded successfully";
            }
            else
            {
                _Message = "Attachment failed to upload";
            }

            //if (File.Exists(DocumentPath))
            //{
            //    File.Delete(DocumentPath);
            //}

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
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

                        WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(DocumentNo), 2, rejectionComment);

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
        
        public JsonResult ApproveStaffSurrender(string param1)
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