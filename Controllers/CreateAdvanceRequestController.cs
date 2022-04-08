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
    public class CreateAdvanceRequestController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string CreatedAdvanceRequestsHeader = "";
        string _CreatedAdvanceRequestsHeader = "";
        public static string LineDimension = "";

        CreateAdvanceRequest cr = new CreateAdvanceRequest();

        // GET: CreateAdvanceRequest
     
       
        public ActionResult CreateAdvanceRequest(CreateAdvanceRequest create)
        {
            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "active";
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
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["Company"] = "Management Unit";
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
                    bool IsPostBack = false;
                    if (!IsPostBack )
                    {
                        LoadPrefferedMethodOfPayment();
                       GetDimensionCodes();
                    }

                    if (Request.QueryString["No"] != null)
                    {
                        _CreatedAdvanceRequestsHeader = Request.QueryString["No"].Trim();
                    }


                    if (_CreatedAdvanceRequestsHeader == "")
                    {
                        CreatedAdvanceRequestsHeader = _CreatedAdvanceRequestsHeader;

                        //string DocumentNo = GenerateDocumentNo("");

                        //Session["AdvanceRequestNo"] = DocumentNo;

                        //Response.Redirect("CreateAdvanceRequest.aspx?No=" + DocumentNo + "");
                    }
                    else
                    {
                        CreatedAdvanceRequestsHeader = _CreatedAdvanceRequestsHeader;

                        LoadTable(Session["AdvanceRequestNo"].ToString());
                        LoadTableAttachments(Session["AdvanceRequestNo"].ToString());
                    }

                }
            }
            return View();
        }

        private void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string Status = json.Status;

            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode4 = json.ShortcutDimension4Code;
            string ShortcutDimCode8 = json.ShortcutDimension8Code;


            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "Management Unit")
            {
                LineDimension = "INTERVENTION";

                // DimCode8Label.Text = "Region to be Paid From";
                LoadDimCodeValues(cr.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(cr.DimCode2, GlobalDimCode2);
                // LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                // LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                cr.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                cr.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                // DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                LineDimension = ShortcutDimCode3;

                // DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(cr.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(cr.DimCode2, GlobalDimCode2);
                LoadDimCodeValues(cr.DimCode4, ShortcutDimCode4);
                // LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                cr.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                cr.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                cr.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            }

            System.Web.HttpContext.Current.Session["LCY"] = json.LCY;
            //load dynamic list

        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
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

        public static string LoadAdvanceRequest()
        {
            return AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", CreatedAdvanceRequestsHeader);
        }

        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }

        public static string LoadInterventions()
        {
            List<AdvanceRequestTypes> AdvanceRequestTypesList = new List<AdvanceRequestTypes>();

            WebRef.DimCodeValues _DimCodeValues = new WebRef.DimCodeValues();
            WebserviceConfig.ObjNav.ExportDimensionCodeValues(LineDimension, ref _DimCodeValues);

            foreach (var kvp in _DimCodeValues.DimCodeValue)
            {
                AdvanceRequestTypesList.Add(new AdvanceRequestTypes { Code = kvp.Code, Description = kvp.Code + " - " + kvp.Name });
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }

        public static string LoadAdvanceRequestItem(string Code)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceType(Code);
        }

        public static string LoadUnitsOfMeasure()
        {
            return CreateAdvanceRequestXMLRequests.GetUnitOfMeasure();
        }
        private void LoadTable(string AdvanceRequestHdrNo)
        {
            string strText = AdvanceRequestsXMLRequests.GetAdvanceRequestsLinesTable(AdvanceRequestHdrNo, "Open");
            ViewBag.Table = strText;
        }
        private void LoadPrefferedMethodOfPayment()
        {
            List<PreferredPaymentMethod> list = new List<PreferredPaymentMethod>()
            {
                new PreferredPaymentMethod() {Id = 0, Name="" },
                new PreferredPaymentMethod() {Id = 1, Name="Mpesa" },
                new PreferredPaymentMethod() {Id = 2, Name="Cheque" },
            };
            ViewBag.PreferredPaymentMethod = list;
           
        }
        private void LoadTableAttachments(string AdvanceRequestHdrNo)
        {

            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(AdvanceRequestHdrNo, folderPath, "AdvanceRequests", "Open", "StaffAdvance");

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
            ViewBag.Table = strText;
            // placeholder1.Controls.Add(new Literal { Text = html.ToString() });
        }

        public static string LoadBudgetLineCode()
        {
            string[] result = CreateAdvanceRequestXMLRequests.GetShortcutDimCodeArrayString(LineDimension);
            return JsonConvert.SerializeObject(result);
        }

        public JsonResult GetRequestLineDetails(string param1)
        {
            string LineNo = param1;

            string GetAdvanceRequestLineResponse = AdvanceRequestsXMLRequests.GetAdvanceRequestLine(LineNo);
            dynamic jsonGetAdvanceRequestLineResponse = JObject.Parse(GetAdvanceRequestLineResponse);

            string Item = jsonGetAdvanceRequestLineResponse.Item;
            string Purpose = jsonGetAdvanceRequestLineResponse.Purpose;
            string UnitOfMeasure = jsonGetAdvanceRequestLineResponse.UnitOfMeasure;
            string NoOfUnits = jsonGetAdvanceRequestLineResponse.NoOfUnits;
            string UnitCost = jsonGetAdvanceRequestLineResponse.UnitCost;
            string AdvancedAmountLCY = jsonGetAdvanceRequestLineResponse.AdvancedAmountLCY;
            string ShortCutDimCode5 = jsonGetAdvanceRequestLineResponse.ShortCutDimCode5;
            string ItemDescription = jsonGetAdvanceRequestLineResponse.ItemDescription;

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
                ShortcutDimCode5 = ShortCutDimCode5,
            };

            return Json(JsonConvert.SerializeObject(_AdvanceRequestLines),JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateStaffImprestLines(string param1, string param2, string param3, string param4, string param5, string param6,
            string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
            string param15, string param16, string param17)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "hh";
            string DocumentNo = "";

            string Item = "";
            string ItemDescription = "";
            string NoOfUnits = "";
            string UnitOfMeasure = "";
            string UnitCost = "";
            string Amount = "";
            string DateOfRequest = "";
            string DateDue = "";
            string RequestToCompany = "";
            string DimCode1 = "";
            string DimCode2 = "";
            string DimCode3 = "";
            string DimCode4 = "";
            string DimCode5 = "";
            string DimCode8 = "";
            string preferredPaymentMethod = "";
            string MissionSummary = "";
            string Purpose = "";
            string Currency = "";
            Item = param1;
            ItemDescription = param2;
            NoOfUnits = param5;
            UnitOfMeasure = param3;
            UnitCost = param4;
            Amount = param6;
            DateOfRequest = param7;
            DateDue = param8;
            RequestToCompany = "";
            preferredPaymentMethod = param15;
            MissionSummary = param16;
            Purpose = param17;
            Currency = "";

            //KRCS  uses DimCode1, DimCode2, DimCode4 and DimCode3 on the lines
            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                DimCode1 = param10;
                DimCode2 = param11;
                DimCode3 = param13;
                DimCode4 = "";
                DimCode5 = param12;
                DimCode8 = param14;

                if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
                {
                    DocumentNo = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();

                    string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffAdvance", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", DimCode8, Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                    string CreateAdvanceRequestXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.CreateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, DimCode5, "", "", DimCode8);
                    dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);
                    _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;


                    _Status = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
                }
                else
                {
                    CreateAdvanceRequest cr = new CreateAdvanceRequest();
                    DocumentNo = GenerateDocumentNo(DimCode8).ToString();

                    //CreatedAdvanceRequestsHeader = DocumentNo;

                    if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
                    {
                        string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffAdvance", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", DimCode8, Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                        string CreateAdvanceRequestXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.CreateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, DimCode5, "", "", DimCode8);
                        dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);
                        _Status = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
                        _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
                    }
                    else
                    {
                        _Status = "999";
                        _Message = "The staff advance header number could not be created";
                    }
                }
            }
            else
            {
                //KRCS  uses DimCode1, DimCode2, DimCode4 and DimCode3 on the lines

                DimCode1 = param10;
                DimCode2 = param11;
                DimCode3 = param12;
                DimCode4 = param9;
                DimCode5 = "";
                DimCode8 = "";

                if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
                {
                    DocumentNo = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();

                    string UpdateAdvanceRequestResponse =
                       CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffAdvance", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                    string CreateAdvanceRequestXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.CreateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                    dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);
                    _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;


                    _Status = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();

                }
                else
                {
                    CreateAdvanceRequest cr = new CreateAdvanceRequest();
                    //DocumentNo = cr.GenerateDocumentNo(DimCode8);
                    DocumentNo = GenerateDocumentNo(DimCode8).ToString();

                    //CreatedAdvanceRequestsHeader = DocumentNo;

                    if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
                    {
                        string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffAdvance", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                        string CreateAdvanceRequestXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.CreateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                        dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);
                        _Status = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
                        _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
                    }
                    else
                    {
                        _Status = "999";
                        _Message = "The staff advance header number could not be created";
                    }
                }

            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateDocumentNo(string RegionCode)
        {
            string DocumentNo = "";
            string DocumentNoXMLResponse = CreateAdvanceRequestXMLRequests.GetAdvanceRequestNewNo("StaffAdvance", RegionCode);
            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

            string _Status = jsonDocumentNo.Status;

            if (_Status == "000")
            {
                DocumentNo = jsonDocumentNo.DocumentNo;

                // _CreatedAdvanceRequestsHeader = DocumentNo;

            }
            return Json(DocumentNo,JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStaffImprestLines(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string _Status = "";
            string _Message = "";

            string DocumentNo = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
            string Item = param1;
            string ItemDescription = param2;
            string NoOfUnits = param5;
            string UnitOfMeasure = param3;
            string UnitCost = param4;
            string Amount = param6;
            string LineNo = param7;
            string DimCode5 = param8;
            string Purpose = param9;

            //get Dims
            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", DocumentNo);
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

            //update staff imprest line

            if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
            {
                string UpdateAdvanceRequestLineXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "", Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, DimCode5, "", "", DimCode8);
                dynamic jsonUpdateAdvanceRequestLineXMLRequestsRssponse = JObject.Parse(UpdateAdvanceRequestLineXMLRequestsRssponse);

                _Status = jsonUpdateAdvanceRequestLineXMLRequestsRssponse.Status;
                _Message = jsonUpdateAdvanceRequestLineXMLRequestsRssponse.Msg;

            }
            else
            {
                _Status = "999";
                _Message = "The staff advance header number was not found";
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public static string DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "000";
            string _Message = "";


            if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
            {
                string DeleteAdvanceRequestLineXMLRequestsRssponse = AdvanceRequestsXMLRequests.DeleteAdvanceRequestLine(System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString(), LineNo);
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(DeleteAdvanceRequestLineXMLRequestsRssponse);

                _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            }
            else
            {
                _Status = "999";
                _Message = "The staff advance header number was not found";
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }

        public JsonResult DeleteAdvanceRequestAttachment(string param1)
        {
            string _Status = "000";
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

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
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

            if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
            {
                DocumentNo = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
                //   bool IsUploaded = CreateAdvanceRequestXMLRequests.UploadFile("0", DocumentNo, DocumentPath, AttachmentDescription);

                bool IsUploaded = false || true;
                if (IsUploaded)
                {
                    _Message = "Attachment has been uploaded successfully";
                }
                else
                {
                    _Message = "Attachment failed to upload";
                }
            }

            _Status = DocumentNo;


            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse),JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string _Status = "900";
            string _Message = "";
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string DateOfRequest = param1;
            string DateDue = param2;
            string RequestToCompany = "";
            string DimCode1 = "";
            string DimCode2 = "";
            string DimCode3 = "";
            string DimCode4 = "";
            string DimCode8 = "";
            string preferredPaymentMethod = param9;
            string Currency = "";
            string MissionSummary = param6;

            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                DimCode1 = param4;
                DimCode2 = param5;
                DimCode3 = param3;
                DimCode4 = "";
                DimCode8 = param7;
            }
            else
            {
                DimCode1 = param4;
                DimCode2 = param5;
                DimCode3 = "";
                DimCode4 = param3;
                DimCode8 = "";
            }

            try
            {
                string DocumentNo = "";

                if (System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString() != "")
                {
                    DocumentNo = System.Web.HttpContext.Current.Session["AdvanceRequestNo"].ToString();
                }
                //else
                //{
                //    CreateAdvanceRequest cr = new CreateAdvanceRequest();
                //    DocumentNo = cr.GenerateDocumentNo(DimCode8);
                //}

                if (DocumentNo != "")
                {
                    //Create Advance Request Header
                    string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffAdvance", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", DimCode8, Currency, DocumentNo, preferredPaymentMethod, MissionSummary);
                    dynamic jsonUpdateAdvanceRequestResponse = JObject.Parse(UpdateAdvanceRequestResponse);

                    _Status = jsonUpdateAdvanceRequestResponse.Status;
                    _Message = jsonUpdateAdvanceRequestResponse.Msg;

                    //submit here

                    string SubmitAdvanceRequestXMLResponse = AdvanceRequestsXMLRequests.SubmitAdvanceRequest(DocumentNo);

                    dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                    _Status = json.Status;
                    _Message = json.Msg;
                }
                else
                {
                    _Status = "999";
                    _Message = "The staff advance header number could not be created";
                }

            }
            catch (Exception es)
            {
                _Message = es.ToString();
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateDim3Code(string param1)
        {
            return Json(CreateAdvanceRequestXMLRequests.ValidateShortcutDimCode3(param1),JsonRequestBehavior.AllowGet);
        } 
    }
}
