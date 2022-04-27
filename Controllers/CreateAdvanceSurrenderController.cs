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
    public class CreateAdvanceSurrenderController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string AdvanceSurrenderRequestsHeader = "";
        static string AdvanceRequestsHeaderNumber = "";
        static string _ShortcutDimCode3 = "";
        private bool IsPostBack;
        CreateAdvanceSurrender CreateAdvance =new CreateAdvanceSurrender();
        // GET: CreateAdvanceSurrender
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateAdvanceSurrender()
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

            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            //check if user is logged
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                if ((object)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadPrefferedMethodOfPayment();
                        GetDimensionCodes();
                    }

                    if (Request.QueryString["No"] != null)
                    {
                        AdvanceRequestsHeaderNumber = Request.QueryString["No"].Trim();

                        _ShortcutDimCode3 = "";
                    }
                    else
                    {
                        AdvanceRequestsHeaderNumber = "";
                        _ShortcutDimCode3 = "";
                    }

                    if (Request.QueryString["SA"] != null)
                    {

                        AdvanceSurrenderRequestsHeader = Request.QueryString["SA"].Trim(); // send surrender no as part of URL

                        _ShortcutDimCode3 = "";
                    }
                    else
                    {
                        AdvanceSurrenderRequestsHeader = "";
                        _ShortcutDimCode3 = "";
                    }


                    LoadAdvanceRequestList();
                    LoadStaffSurrenderLines(AdvanceSurrenderRequestsHeader);
                    LoadTableAttachments(AdvanceSurrenderRequestsHeader);
                    LoadSurrenderCaption(AdvanceSurrenderRequestsHeader, "Open");
                    LoadTableAttachments2(AdvanceRequestsHeaderNumber);

                    //if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                    //{
                    //    DimCode8Label.Visible = false;
                    //    DimCode8.Visible = false;
                    //}
                }
            }
            return View();
        }

        public static string LoadAdvanceSurrender()
        {
            return AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceSurrenderRequestsHeader);
        }
       
        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }
       
        public JsonResult LoadAdvanceRequestItem(string Code)
        {
            return Json(CreateAdvanceRequestXMLRequests.GetAdvanceType(Code),JsonRequestBehavior.AllowGet);
        }
        private void LoadTableAttachments(string SurrenderHdrNo)
        {

            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(SurrenderHdrNo, folderPath, "AdvanceRequests", "Open", "StaffSurrender");

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

           // placeholder2.Controls.Add(new Literal { Text = html.ToString() });
        }
        private void LoadSurrenderCaption(string AdvanceSurrenderRequestsHeader, string status)
        {
            StringBuilder html = new StringBuilder();

            html.Append(AdvanceSurrender.GetAdvanceSurrenderLines(AdvanceSurrenderRequestsHeader, status));


           // placeholder3.Controls.Add(new Literal { Text = html.ToString() });
        }
       
        public JsonResult DeleteAdvanceRequestAttachment(string param1)
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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult CreateStaffSurrender(string param1)
        {
            AdvanceRequestsHeaderNumber = param1;

            //don't create surrender if no advance request is selected
            if (AdvanceRequestsHeaderNumber != "")
            {
                //create staffsurrender Lines from here

                if (AdvanceSurrenderRequestsHeader == "")
                {
                    //get dim code 8
                    string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", AdvanceRequestsHeaderNumber);
                    dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);
                    string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

                    string DocumentNo = GenerateDocumentNo(DimCode8);

                    AdvanceSurrenderRequestsHeader = DocumentNo;

                    WebserviceConfig.ObjNav.CreateStaffSurrender(param1, AdvanceSurrenderRequestsHeader);

                    WebserviceConfig.ObjNav.CreateStaffSurrenderLines(param1, AdvanceSurrenderRequestsHeader);

                }
            }

            return Json(AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceSurrenderRequestsHeader),JsonRequestBehavior.AllowGet);
        }
       
        public static string LoadAdvanceRequestLines()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceRequestLines(AdvanceRequestsHeaderNumber);
        }
        
        public static string LoadInterventions()
        {
            List<AdvanceRequestTypes> AdvanceRequestTypesList = new List<AdvanceRequestTypes>();

            WebRef.DimCodeValues _DimCodeValues = new WebRef.DimCodeValues();
            WebserviceConfig.ObjNav.ExportDimensionCodeValues("INTERVENTION", ref _DimCodeValues);

            foreach (var kvp in _DimCodeValues.DimCodeValue)
            {
                AdvanceRequestTypesList.Add(new AdvanceRequestTypes { Code = kvp.Code, Description = kvp.Code + " - " + kvp.Name });
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        
        public JsonResult GetRequestLineDetails(string param1)
        {
            string LineNo = param1;

            string GetAdvanceRequestLineResponse = AdvanceRequestsXMLRequests.GetAdvanceRequestLine(LineNo);
            dynamic jsonGetAdvanceRequestLineResponse = JObject.Parse(GetAdvanceRequestLineResponse);

            string Item = jsonGetAdvanceRequestLineResponse.Item;
            string ItemDescription = jsonGetAdvanceRequestLineResponse.ItemDescription;
            string UnitOfMeasure = jsonGetAdvanceRequestLineResponse.UnitOfMeasure;
            string NoOfUnits = jsonGetAdvanceRequestLineResponse.NoOfUnits;
            string UnitCost = jsonGetAdvanceRequestLineResponse.UnitCost;
            string AdvancedAmountLCY = jsonGetAdvanceRequestLineResponse.AdvancedAmountLCY;
            string SurrenderedAmountLCY = jsonGetAdvanceRequestLineResponse.SurrenderedAmountLCY;
            string Remarks = jsonGetAdvanceRequestLineResponse.Remarks;
            string BudgetLineCode = jsonGetAdvanceRequestLineResponse.BudgetLineCode;
            string Purpose = jsonGetAdvanceRequestLineResponse.Purpose;

            var _AdvanceRequestLines = new AdvanceRequestLines
            {
                No = LineNo,
                Item = Item,
                ItemDescription = ItemDescription.Trim(),
                UnitOfMeasure = UnitOfMeasure,
                NoOfUnits = NoOfUnits,
                UnitCost = UnitCost,
                Purpose = Purpose.Trim(),
                SurrenderedAmountLCY = SurrenderedAmountLCY,
                AdvancedAmountLCY = AdvancedAmountLCY,
                Remarks = Remarks.Trim(),
                ShortcutDimCode5 = BudgetLineCode
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines), JsonRequestBehavior.AllowGet);
        }
        
        public void LoadPrefferedMethodOfPayment()
        {
            //PreferredPaymentMethod.Items.Clear();
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Mpesa", "2"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Cheque ", "1"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem(" ", "0"));
        }
        
        public JsonResult GetSurrenderLineDetails(string param1)
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
                UnitOfMeasure = UnitOfMeasure,
                NoOfUnits = NoOfUnits,
                UnitCost = UnitCost,
                Purpose = Purpose,
                AdvancedAmountLCY = AdvancedAmountLCY,
                SurrenderedAmountLCY = SurrenderedAmountLCY,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode5 = ShortcutDimCode5,
                Remarks = Remarks
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines), JsonRequestBehavior.AllowGet);
        }
        private void LoadStaffSurrenderLines(string AdvanceRequestHdrNo)
        {
            string strText = AdvanceSurrender.GetAdvanceSurrenderinesTable(AdvanceRequestHdrNo, "Open");

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
                LoadDimCodeValues(CreateAdvance.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(CreateAdvance.DimCode2, GlobalDimCode2);
                //LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                CreateAdvance.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                CreateAdvance.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                //DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(CreateAdvance.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(CreateAdvance.DimCode2, GlobalDimCode2);
                LoadDimCodeValues(CreateAdvance.DimCode4, ShortcutDimCode4);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                CreateAdvance.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                CreateAdvance.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                CreateAdvance.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            }
        }
        private void LoadDimCodeValues(DropDownList _DropDownList, string Code)
        {
            _DropDownList.Items.Clear();

            WebRef.DimCodeValues _DimCodeValues = new WebRef.DimCodeValues();
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

            //foreach (var kvp in AdvanceSurrender.GetAdvanceRequestList("ToSurrender", username))//here change status
            //{
            //    AdvanceRequestList.Items.Insert(0, new ListItem(kvp.Value, kvp.Key));
            //}
            //AdvanceRequestList.Items.Insert(0, new ListItem(" ", ""));
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
        private static string GenerateDocumentNo(string RegionCode)
        {
            string DocumentNoXMLResponse = CreateAdvanceRequestXMLRequests.GetAdvanceRequestNewNo("StaffSurrender", RegionCode);

            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

            string _Status = jsonDocumentNo.Status;

            if (_Status == "000")
            {
                string DocumentNo = jsonDocumentNo.DocumentNo;

                AdvanceRequestsHeaderNumber = DocumentNo;

            }
            return AdvanceRequestsHeaderNumber;
        }
        
        public JsonResult DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "000";
            string _Message = "";

            string DeleteAdvanceRequestLineXMLRequestsRssponse = AdvanceSurrender.DeleteAdvanceRequestLine(AdvanceRequestsHeaderNumber, LineNo);
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
        
        public JsonResult SaveSurrender(string param1, string param2, string param3, string param4, string param5, string param6,
            string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
            string param15, string param16, string param17, string param18, string param19, string param20, string param21)
        {
            string LineNo = param17;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";
            string DimCode5 = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string NoOfUnits = param6;
            string UnitCost = param7;
            string Amount = param8;
            string Remarks = param19;
            string Purpose = param21;

            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceSurrenderRequestsHeader);
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

            //call upload file function here
            string DocumentNo = "";

            if (AdvanceSurrenderRequestsHeader != "")
            {
                DocumentNo = AdvanceSurrenderRequestsHeader;

                string CreateAdvanceRequestXMLRequestsResponse = AdvanceSurrender.CreateAdvanceSurrenderLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, Remarks, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsResponse);

                string CreatedLineNo = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;

                _Status = AdvanceSurrenderRequestsHeader;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            }
            else
            {
                if (AdvanceSurrenderRequestsHeader != "")
                {
                    string CreateAdvanceRequestXMLRequestsResponse = AdvanceSurrender.CreateAdvanceSurrenderLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, Remarks, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                    dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsResponse);

                    ///find Record Line Number
                    /// 
                    string CreatedLineNo = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                    //SaveAttachment(DocumentPath, FileName, DocumentNo, CreatedLineNo);

                    _Status = AdvanceSurrenderRequestsHeader;
                    _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
                }
                else
                {
                    _Status = "999";
                    _Message = "The Advance surrender header number could not be created";
                }

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


            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            string DimCode5 = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string NoOfUnits = param6;
            string UnitCost = param7;
            string Amount = param8;
            string AdvanceRequestLineNo = param17;
            string Remarks = param19;
            string Purpose = param21;

            string DocumentNo = "";

            if (AdvanceSurrenderRequestsHeader != "")
            {
                DocumentNo = AdvanceSurrenderRequestsHeader;
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

                string CreateAdvanceRequestXMLRequestsRssponse = AdvanceSurrender.UpdateAdvanceSurrenderLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, AdvanceRequestLineNo, Remarks, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
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
        
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();


            try
            {
                //header is created from staff advance and shouldn't be modied

                //submit
                string SubmitAdvanceRequestXMLResponse = AdvanceSurrender.SubmitAdvanceRequest(AdvanceSurrenderRequestsHeader);
                dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                _Status = json.Status;
                _Message = json.Msg;

                AdvanceSurrenderRequestsHeader = "";
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

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SaveAttachment(string param1, string param2)
        {
            string _Status = "";
            string _Message = "";

            string FileName = param1;
            string AttachmentDescription = param2;

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string DocumentPath = folderPath + FileName;

            //call upload file function here

            string DocumentNo = "";

            if (AdvanceSurrenderRequestsHeader != "")
            {
                DocumentNo = AdvanceSurrenderRequestsHeader;

               bool IsUploaded = Convert.ToBoolean(CreateAdvanceRequestXMLRequests.UploadFile("2", DocumentNo, DocumentPath, AttachmentDescription));

                if (IsUploaded)
                {
                    _Message = "Attachment has been uploaded successfully";
                }
                else
                {
                    _Message = "Attachment failed to upload";
                }
            }

            //if uploaded delete file from uploads folder
            _Status = DocumentNo;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
        public static void SaveAttachment(string Path, string FileName, string AdvanceSurrenderRequestsHeader, string LineNo)
        {
            string AttachmentDescription = FileName;

            string Isuploaded = AdvanceSurrender.UploadFile("2", AdvanceSurrenderRequestsHeader, Path, AttachmentDescription, LineNo);

        }
       
        public static string ValidateDim3Code(string param1)
        {
            return CreateAdvanceRequestXMLRequests.ValidateShortcutDimCode3(param1);
        }
    }
}