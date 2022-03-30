using LMS.CustomsClasses;
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
    public class ViewAdvanceRequestController : Controller
    {
        static string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        static string AdvanceRequestsHeader = "";
        static string _ShortcutDimCode3 = "";
        // GET: ViewAdvanceRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAdvanceRequest()
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
                    string i = Request.QueryString["id"].Trim();
                    string status = Request.QueryString["status"].Trim();
                    string _AdvanceRequestHdrNo = AppFunctions.Base64Decode(i);

                    AdvanceRequestsHeader = _AdvanceRequestHdrNo;
                    bool IsPostBack = false||true;
                    if (!IsPostBack)
                    {
                        LoadPrefferedMethodOfPayment();
                        GetDimensionCodes();
                    }
                    LoadTable(AdvanceRequestsHeader, status);
                    LoadTableAttachments(AdvanceRequestsHeader, status);
                }
            }
            return View();
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
                LoadDimCodeValues(DimCode1, GlobalDimCode1);
                LoadDimCodeValues(DimCode2, GlobalDimCode2);
                //LoadDimCodeValues(DimCode3, ShortcutDimCode3);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                DimCode1Label.Text = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                DimCode2Label.Text = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                //DimCode3Label.Text = SetFirstLetterToUpper(ShortcutDimCode3.ToLower());
            }
            else
            {
                //DimCode8Label.Text = SetFirstLetterToUpper(ShortcutDimCode8.ToLower());
                LoadDimCodeValues(DimCode1, GlobalDimCode1);
                LoadDimCodeValues(DimCode2, GlobalDimCode2);
                LoadDimCodeValues(DimCode4, ShortcutDimCode4);
                //LoadDimCodeValues(DimCode8, ShortcutDimCode8);
                DimCode1Label.Text = SetFirstLetterToUpper(GlobalDimCode1.ToLower());
                DimCode2Label.Text = SetFirstLetterToUpper(GlobalDimCode2.ToLower());
                DimCode4Label.Text = SetFirstLetterToUpper(ShortcutDimCode4.ToLower());
            }
        }
        
        public static string LoadAdvanceTypes()
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceTypes();
        }
        
        public static string LoadAdvanceRequestItem(string Code)
        {
            return CreateAdvanceRequestXMLRequests.GetAdvanceType(Code);
        }
        
        public static string LoadUnitsOfMeasure()
        {
            return CreateAdvanceRequestXMLRequests.GetUnitOfMeasure();
        }
        
        public static string LoadAdvanceRequest()
        {
            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", AdvanceRequestsHeader);
            return AdvanceRequestFields;
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
        private void LoadPrefferedMethodOfPayment()
        {
            //PreferredPaymentMethod.Items.Clear();
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Mpesa", "2"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem("Cheque ", "1"));
            //PreferredPaymentMethod.Items.Insert(0, new ListItem(" ", "0"));
        }
        private void LoadTable(string AdvanceRequestHdrNo, string status)
        {
            string strText = AdvanceRequestsXMLRequests.GetAdvanceRequestsLinesTable(AdvanceRequestHdrNo, status);

           // placeholder.Controls.Add(new Literal { Text = strText.ToString() });
        }
        private void LoadTableAttachments(string AdvanceRequestHdrNo, string status)
        {
            DataTable dt = new DataTable();

            dt.Clear();

            dt = AdvanceRequestsXMLRequests.GetAttachments(AdvanceRequestHdrNo, folderPath, "AdvanceRequests", status, "StaffAdvance");

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

            //placeholder1.Controls.Add(new Literal { Text = html.ToString() });
        }

        public string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
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

        
        public static string LoadBudgetLineCode()
        {
            return CreateAdvanceRequestXMLRequests.GetBudgetLineCode(_ShortcutDimCode3);
        }
        
        public static string GetRequestLineDetails(string param1)
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
                AdvancedAmountLCY = AdvancedAmountLCY,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode5 = ShortcutDimCode5
            };

            return JsonConvert.SerializeObject(_AdvanceRequestLines);
        }
        
        public static string DeleteAdvanceRequestLines(string param1)
        {
            string LineNo = param1;
            string _Status = "";
            string _Message = "";

            string DeleteAdvanceRequestLineXMLRequestsRssponse = AdvanceRequestsXMLRequests.DeleteAdvanceRequestLine(AdvanceRequestsHeader, LineNo);
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
        
        public static string SaveAttachment(string param1, string param2)
        {
            string _Status = "000";
            string _Message = "sdsa";

            string FileName = param1;
            string AttachmentDescription = param2;

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            string DocumentPath = folderPath + FileName;

            //call upload file function here
            string DocumentNo = AdvanceRequestsHeader;

            bool IsUploaded =Convert.ToBoolean(CreateAdvanceRequestXMLRequests.UploadFile("0", DocumentNo, DocumentPath, AttachmentDescription));

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string CreateStaffImprestLines(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
        {
            string _Status = "";
            string _Message = "";

            string DocumentNo = AdvanceRequestsHeader;
            string Item = param1;
            string ItemDescription = param2;
            string NoOfUnits = param5;
            string UnitOfMeasure = param3;
            string UnitCost = param4;
            string Amount = param6;
            string Purpose = param7;
            string BudgetLineCode = param8;


            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", DocumentNo);
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            string DimCode8 = jsonAdvanceRequestFields.ShortcutDimCode8;

            string DimCode5 = param8;// BudgetLIne

            //create staff imprest line
            string CreateAdvanceRequestXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.CreateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, Purpose, DimCode1, DimCode2, DimCode1, DimCode2, BudgetLineCode, DimCode4, "", "", "", "");
            dynamic jsonCreateAdvanceRequestXMLRequestsRssponse = JObject.Parse(CreateAdvanceRequestXMLRequestsRssponse);

            _Status = jsonCreateAdvanceRequestXMLRequestsRssponse.Status;
            _Message = jsonCreateAdvanceRequestXMLRequestsRssponse.Msg;
            var _RequestResponse = new RequestResponse
            {
                Status = _Status,
                Message = _Message
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string UpdateStaffImprestLines(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
        {
            string _Status = "";
            string _Message = "";

            string DocumentNo = AdvanceRequestsHeader;
            string Item = param1;
            string ItemDescription = param2;
            string NoOfUnits = param5;
            string UnitOfMeasure = param3;
            string UnitCost = param4;
            string Amount = param6;
            string LineNo = param7;
            string BudgetLineCode = param8;

            string Purpose = param9;

            //get Dims
            string AdvanceRequestFields = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", DocumentNo);
            dynamic jsonAdvanceRequestFields = JObject.Parse(AdvanceRequestFields);

            string DimCode1 = jsonAdvanceRequestFields.GlobalDimCode1;
            string DimCode2 = jsonAdvanceRequestFields.GlobalDimCode2;
            string DimCode3 = jsonAdvanceRequestFields.ShortcutDimCode3;
            string DimCode4 = jsonAdvanceRequestFields.ShortcutDimCode4;
            //create staff imprest line
            string UpdateAdvanceRequestLineXMLRequestsRssponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequestLine(DocumentNo, Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "", Purpose, DimCode1, DimCode2, DimCode1, DimCode2, BudgetLineCode, DimCode4, "", "", "", "");
            dynamic jsonUpdateAdvanceRequestLineXMLRequestsRssponse = JObject.Parse(UpdateAdvanceRequestLineXMLRequestsRssponse);

            _Status = jsonUpdateAdvanceRequestLineXMLRequestsRssponse.Status;
            _Message = jsonUpdateAdvanceRequestLineXMLRequestsRssponse.Msg;

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
            string RequestToCompany = param3;
            string DimCode1 = param4;
            string DimCode2 = param5;
            string DimCode4 = param6;
            string DimCode8 = param7;
            string DimCode9 = param8;
            string Currency = "";
            string PreferredPaymentMethod = param9;
            string MissionSummary = param10;

            try
            {
                string DocumentNo = AdvanceRequestsHeader;

                //Create Advance Request Header
                string UpdateAdvanceRequestResponse = CreateAdvanceRequestXMLRequests.UpdateAdvanceRequest(DocumentNo, "AdvanceRequest", DateOfRequest, DateDue, username, username, RequestToCompany, DimCode1, DimCode2, DimCode1, DimCode2, DimCode9, DimCode4, "", "", "", "", Currency, "", PreferredPaymentMethod, MissionSummary);
                dynamic jsonUpdateAdvanceRequestResponse = JObject.Parse(UpdateAdvanceRequestResponse);

                _Status = jsonUpdateAdvanceRequestResponse.Status;
                _Message = jsonUpdateAdvanceRequestResponse.Msg;

                //submit here

                string SubmitAdvanceRequestXMLResponse = AdvanceRequestsXMLRequests.SubmitAdvanceRequest(AdvanceRequestsHeader);

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string RejectApplication(string param1, string param2)
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

                        WebserviceConfig.ObjNav.SaveApprovalRejectComment("AdvanceRequests", Convert.ToInt32(DocumentNo), 0, rejectionComment);

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string PrintRequest(string param1)
        {
            string AdvanceRequestHdrNo = AppFunctions.Base64Decode(param1);

            string status = "000";
            string Msg = "Success";

            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");

            string ExportToPath = folderPath + AdvanceRequestHdrNo + ".pdf";

            try
            {
                string SubmitAdvanceRequestXMLResponse = CustomsClasses.WebService.ExportAdvanceRequestReport("0", AdvanceRequestHdrNo, ExportToPath);

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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string ApproveStaffAdvance(string param1)
        {
            string _Status = "";
            string _Message = "";

            //string Username = System.Web.HttpContext.Current.Session["Username"].ToString();
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

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string ValidateDim3Code(string param1)
        {
            return CreateAdvanceRequestXMLRequests.ValidateShortcutDimCode3(param1);
        }
    }
}