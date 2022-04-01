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
    public class CreateStaffClaimsController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string _ShortcutDimCode3 = "";

        public bool IsPostBack { get; private set; }
        CreateStaffClaims createStaff = new CreateStaffClaims();

        // GET: CreateStaffClaims
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateStaffClaims()
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
            System.Web.HttpContext.Current.Session["Company"] = "KRCS GF Management Unit";

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
                        //GetDimensionCodes();
                    }

                    if (Request.QueryString["No"] != null)
                    {
                        //  AdvanceClaimRequestsHeader = Request.QueryString["No"].Trim();
                    }
                    else
                    {
                       // string DocumentNo = GenerateDocumentNo("");

                        //Session["StaffClaimNo"] = DocumentNo;

                        //Response.Redirect("CreateStaffClaims.aspx?No=" + DocumentNo + "");
                    }


                    //LoadStaffClaimLines(Session["StaffClaimNo"].ToString());
                    //LoadTableAttachments(Session["StaffClaimNo"].ToString());
                    //LoadAdvanceRequest(Session["StaffClaimNo"].ToString());
                }
            }
            return View();
        }
        private void LoadTableAttachments(string _AdvanceClaimRequestsHeader)
        {
            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(_AdvanceClaimRequestsHeader, folderPath, "AdvanceRequests", "Open", "StaffClaim");

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
            ViewBag.strText = strText;
            //placeholder1.Controls.Add(new Literal { Text = html.ToString() });
        }

        public void LoadAdvanceRequest(string AdvanceClaimRequestsHeader)
        {
            string AdvanceRequestData = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", AdvanceClaimRequestsHeader);
            dynamic json = JObject.Parse(AdvanceRequestData);

            //DateDue = json.DateDue;
            //DateOfRequest = json.DateOfRequest;
            //MissionSummary = json.MissionSummary;
            string GlobalDimCode1 = json.GlobalDimCode1;
            string GlobalDimCode2 = json.GlobalDimCode2;
            string ShortcutDimCode3 = json.ShortcutDimCode3;
            string ShortcutDimCode4 = json.ShortcutDimCode4;
            string ShortcutDimCode8 = json.ShortcutDimCode8;
            string _PreferredPaymentMethod = json.PreferredPaymentMethod;

            //DimCode1.Items.FindByValue(GlobalDimCode1).Selected = true;
            //DimCode2.Items.FindByValue(GlobalDimCode2).Selected = true;
            ////DimCode3.Items.FindByValue(ShortcutDimCode3).Selected = true; //GF
            //DimCode4.Items.FindByValue(ShortcutDimCode4).Selected = true;
            ////DimCode8.Items.FindByValue(ShortcutDimCode8).Selected = true; //GF
            //if (!string.IsNullOrEmpty(_PreferredPaymentMethod))
            //{
            //    PreferredPaymentMethod.Items.FindByValue(_PreferredPaymentMethod).Selected = true;
            //}
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
        
        public static string LoadBudgetLineCode()
        {
            return CreateAdvanceRequestXMLRequests.GetBudgetLineCode(_ShortcutDimCode3);
        }
        
        public static string GetRequestLineDetails(string param1)
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

            return JsonConvert.SerializeObject(_AdvanceRequestLines);
        }
        private void LoadStaffClaimLines(string AdvanceRequestHdrNo)
        {
            string strText = StaffClaims.GetAdvanceClaimLinesTable(AdvanceRequestHdrNo, "Open");
            ViewBag.placeholder = strText;
           // placeholder.Controls.Add(new Literal { Text = strText.ToString() });
        }
        private void GetDimensionCodes()
        {
            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();

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
                DropDownList DimCode1 = createStaff.DimCode1;
                //DimCode8Label.Text = "Region to be Paid From";
                LoadDimCodeValues(DimCode1, GlobalDimCode1);
                LoadDimCodeValues(createStaff.DimCode2, GlobalDimCode2);
                //LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                createStaff.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                createStaff.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                //DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(createStaff.DimCode1, GlobalDimCode1);
                LoadDimCodeValues(createStaff.DimCode2, GlobalDimCode2);
                LoadDimCodeValues(createStaff.DimCode4, ShortcutDimCode4);
                // LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                createStaff.DimCode1Label = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                createStaff.DimCode2Label = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                createStaff.DimCode4Label = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            }
        }
        private void LoadDimCodeValues(DropDownList _DropDownList, string Code)
        {
            //_DropDownList.Items.Clear();

            //WebRef.DimCodeValues _DimCodeValues = new WebRef.DimCodeValues();
            //WebserviceConfig.ObjNav.ValidateDimensionValueCode(Code, ref _DimCodeValues);
            var DimCodeValue = AdvanceRequestsXMLRequests.ValidateDimensionValueCode(Code);

            //foreach (var kvp in DimCodeValue)
            //{
            //    _DropDownList.Items.Insert(0, new ListItem(kvp.Code + " - " + kvp.Name, kvp.Code));
            //}
            // _DropDownList.Items.Insert(0, new ListItem(" ", ""));
            //ViewBag.data = _DropDownList.Items;
        }

        private void LoadPrefferedMethodOfPayment()
        {
            List<PreferredPaymentMethods> selectListItems = new List<PreferredPaymentMethods>()
            {
               new PreferredPaymentMethods(){Id="2",Name="Mpesa"},
               new PreferredPaymentMethods(){Id="1",Name="Cheque"},
               new PreferredPaymentMethods(){Id="0",Name=" "}

            };
            ViewBag.LoadPrefferedMethodOfPayment = selectListItems;
        }
        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }

        
        public static string LoadAdvanceRequestItem(string Code)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceType(Code);
        }
        
        public static string LoadUnitsOfMeasure()
        {
            return CreateAdvanceRequestXMLRequests.GetUnitOfMeasure();
        }
        
        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }

        private string GenerateDocumentNo(string RegionCode)
        {
            string DocumentNoXMLResponse = CreateAdvanceRequestXMLRequests.GetAdvanceRequestNewNo("StaffClaim", RegionCode);

            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);

            string _Status = jsonDocumentNo.Status;
            string DocumentNo = "";

            if (_Status == "000")
            {
                DocumentNo = jsonDocumentNo.DocumentNo;
            }
            return DocumentNo;
        }
        
        public static string DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "000";
            string _Message = "";
            string DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

            string DeleteAdvanceRequestLineXMLRequestsRssponse = CustomsClasses.StaffClaims.DeleteAdvanceRequestLine(DocumentNo, LineNo);
            dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(DeleteAdvanceRequestLineXMLRequestsRssponse);

            _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
            _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }

        
        public static string CreateAdvanceClaim(string param1, string param2, string param3, string param4, string param5, string param6,
           string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
           string param15, string param16, string param18, string param19, string param20)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            string DimCode3 = param1;
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
            string DimCode1 = "";
            string DimCode2 = "";
            string DimCode4 = "";
            string DimCode5 = "";
            string DimCode8 = "";
            string preferredPaymentMethod = param18;
            string Currency = "";
            string MissionSummary = param19;
            string Purpose = param20;

            string DocumentNo = "";


            //KRCS  uses DimCode1, DimCode2, DimCode4 and DimCode3 on the lines


            DimCode1 = param12;
            DimCode2 = param13;
            DimCode3 = param14;
            DimCode4 = param1;
            DimCode5 = "";
            DimCode8 = "";
            if (System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString() != "")
            {
                DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

                string UpdateAdvanceRequestResponse = CustomsClasses.StaffClaims.UpdateAdvanceRequest(DocumentNo, "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                string CreateAdvanceRequestXMLRequestsRssponse = CustomsClasses.StaffClaims.CreateAdvanceClaimLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

                string LineNo = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;

                _Status = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();// jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string UpdateAdvanceClaim(string param1, string param2, string param3, string param4, string param5, string param6,
            string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14,
            string param15, string param16, string param17, string param18, string param19, string param20)
        {

            string LineNo = param17;
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string _Status = "000";
            string _Message = "";

            // string DimCode8 = param1;
            string ActualAmount = param2;
            string Item = param3;
            string ItemDescription = param4;
            string UnitOfMeasure = param5;
            string NoOfUnits = param6;
            string UnitCost = param7;
            string Amount = param8;

            string DateOfRequest = param9;
            string DateDue = param10;
            string RequestToCompany = "";
            // string DimCode1 = param12;
            // string DimCode2 = param13;
            string DimCode5 = param14;
            // string DimCode3 = param15;
            string preferredPaymentMethod = param18;
            string Currency = "";
            string MissionSummary = param19;
            string Purpose = param20;

            //call upload file function here
            string DocumentNo = "";

            if (System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString() != "")
            {
                DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

                //get Dims
                string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", DocumentNo);
                dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

                string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
                string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
                string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
                string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
                string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

                string UpdateAdvanceRequestResponse = CustomsClasses.StaffClaims.UpdateAdvanceRequest(DocumentNo, "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, DocumentNo, preferredPaymentMethod, MissionSummary);

                string CreateAdvanceRequestXMLRequestsRssponse = CustomsClasses.StaffClaims.UpdatAdvanceClaimLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "", Purpose, DimCode1, DimCode2, DimCode1, DimCode2, DimCode3, DimCode4, "", "", "", "");
                dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

                string LineNo2 = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;

                //SaveAttachment(DocumentPath, FileName, DocumentNo, LineNo, LineNo2);

                _Status = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();// jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
                _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
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
            string Currency = "";
            string preferredPaymentMethod = param9;
            string MissionSummary = param10;

            if (System.Web.HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
            {
                DimCode1 = param4;
                DimCode2 = param5;
                DimCode3 = param6;
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
                if (System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString() != "")
                {
                    string DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();
                    string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "StaffClaim", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, "", DimCode4, "", "", "", "", Currency, DocumentNo, preferredPaymentMethod, MissionSummary);
                    dynamic jsonUpdateAdvanceRequestResponse = JObject.Parse(UpdateAdvanceRequestResponse);
                    _Status = jsonUpdateAdvanceRequestResponse.Status;
                    _Message = jsonUpdateAdvanceRequestResponse.Msg;

                    //submit
                    string SubmitAdvanceRequestXMLResponse = CustomsClasses.StaffClaims.SubmitAdvanceRequest(DocumentNo);
                    dynamic json = JObject.Parse(SubmitAdvanceRequestXMLResponse);

                    _Status = json.Status;
                    _Message = json.Msg;
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
        
        public static string SaveAttachment(string param1, string param2)
        {
            string _Status = "";
            string _Message = "";

            //string FileName = string.Format(@"{0}.txt", DateTime.Now.Ticks) + param1;
            //param1 = string.Format(@"{0}", DateTime.Now.Ticks) + param1;

            string FileName = param1;
            string AttachmentDescription = param2;

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string DocumentPath = folderPath + FileName;

            //call upload file function here
            string DocumentNo = "";

            if (System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString() != "")
            {
                DocumentNo = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

                bool IsUploaded =Convert.ToBoolean(CreateAdvanceRequestXMLRequests.UploadFile("1", DocumentNo, DocumentPath, AttachmentDescription));

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
            _Status = System.Web.HttpContext.Current.Session["StaffClaimNo"].ToString();

            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        public static void SaveAttachment(string Path, string FileName, string AdvanceSurrenderRequestsHeader, string lineNo, string lineNo2)
        {
            //FileName = string.Format(@"{0}", DateTime.Now.Ticks);
            string AttachmentDescription = FileName;

            object IsUploaded =Convert.ToString(StaffClaims.UploadFile("1", AdvanceSurrenderRequestsHeader, Path, AttachmentDescription, lineNo, lineNo2));

        }
        
        public static string ValidateDim3Code(string param1)
        {
            return CreateAdvanceRequestXMLRequests.ValidateShortcutDimCode3(param1);
        }
    }
}