using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.UI.WebControls;
using System.Xml;

namespace LMS.CustomsClasses
{
    public class ApprovalEntiesXMLRequests
    {
        public static DataTable GetLeavePageData(string status)
        {
            string LeaveStatus = null;
            string AppliedAs = "Supervisor";

            if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "Leave", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                    string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeEmployeeID = xmlSoapRequest.GetElementsByTagName("EmployeeID")[count];
                    string EmployeeID = NodeEmployeeID.InnerText;
                    
                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Pending")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-danger btn-xs reject_leave' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Application'><span class = 'fa fa-times'> </span></a>  <a class = 'btn btn-success btn-xs approve_leave' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Application'><span class = 'fa fa-check'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&Emp=" + EmployeeID + "&parent=Leaves" + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&Emp=" + EmployeeID + "&parent=Leaves" + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'></span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&Emp=" + EmployeeID + "&parent=Leaves" + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'></span></a>");
                    }

                    count++;
                }
            }

            return table;
        }
        public static string GetOpenLeaveApprovals()
        {
            string LeaveStatus = "PendingApproval";
            string AppliedAs = "Supervisor";

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "Leave", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    if (HeaderNo != "")
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }
        public static DataTable GetLeaveRecallPageData(string status)
        {
            string LeaveStatus = null;
            string AppliedAs = "Supervisor";


            if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "LeaveRecall", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                    string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Pending")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-danger btn-xs reject_leaverecall' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)'><span class = 'fa fa-times'> </span></a>  <a class = 'btn btn-success btn-xs approve_leaverecall' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)'><span class = 'fa fa-check'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&parent=LeaveRecalls" + "&status=Pending" + "><span class = 'fa fa-eye'> </span></a>");

                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&parent=LeaveRecalls" + "&status=Approved" + "><span class = 'fa fa-eye'></span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewApprovalEntry.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&parent=LeaveRecalls" + "&status=Rejected" + "><span class = 'fa fa-eye'></span></a>");
                    }
                    count++;
                }
            }

            return table;
        }
        public static DataTable GetTrainingPageData(string status)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            int count = 0;
            string TrainingStatus = null;
            string AppliedAs = "Supervisor";

            if (status == "Pending")
            {
                TrainingStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                TrainingStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                TrainingStatus = "Rejected";
            }

            DataTable table = new DataTable();


            table.Columns.Add("Training No.", typeof(string));
            table.Columns.Add("Planned Start Date", typeof(string));
            table.Columns.Add("Planned End Date", typeof(string));
            table.Columns.Add("Training Description", typeof(string));
            table.Columns.Add("Course Name", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.GetTrainingList("", username, TrainingStatus, AppliedAs));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Training"))
                {
                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Description = NodeDescription.InnerText;

                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeCourseDescription = xmlSoapRequest.GetElementsByTagName("CourseDescription")[count];
                    string CourseDescription = NodeCourseDescription.InnerText;

                    XmlNode NodePlannedStartDate = xmlSoapRequest.GetElementsByTagName("PlannedStartDate")[count];
                    string PlannedStartDate = NodePlannedStartDate.InnerText;

                    XmlNode NodePlannedEndDate = xmlSoapRequest.GetElementsByTagName("PlannedEndDate")[count];
                    string PlannedEndDate = NodePlannedEndDate.InnerText;

                    if (No != "")
                    {
                        if (status == "Pending")
                        {
                            table.Rows.Add(No, PlannedStartDate, PlannedEndDate, Description, CourseDescription, status,
                               "<a class = 'btn btn-danger btn-xs reject_training_request' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Training Request'><span class = 'fa fa-times' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs approve_training_request' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Training Request'><span class = 'fa fa-check' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs' href = " + "ViewTraining.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(No) + "&status=Open><span class = 'fa fa-pencil-alt' > </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, PlannedStartDate, PlannedEndDate, Description, CourseDescription, status,
                            "<a class = 'btn btn-secondary btn-xs' href = " + "ViewTraining.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(No) + "&status=Pending><span class = 'fa fa-eye' > </span></a> ");
                        }
                    }

                    count++;
                }


            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = count.ToString();
            }

            return table;
        }
        public static DataTable GetFilledAppraisal(string status, string employeeNumber, string requestAs)
        {

            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetFilledAppraisals xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <requestAs>" + requestAs + @"</requestAs>
                                    <appraisalHeaderNumber>[string]</appraisalHeaderNumber>
                                    <status>" + status + @"</status>
                                    <filledAppraisals>
                                        <FilledAppraisal xmlns=""urn:microsoft-dynamics-nav/xmlports/x50083"">
                                            <AppraisalHeaderNumber>[string]</AppraisalHeaderNumber>
                                            <Descrption>[string]</Descrption>
                                            <ValidFrom>[date]</ValidFrom>
                                            <ValidTo>[date]</ValidTo>
                                            <Status>[string]</Status>
                                            <Supervisor>[string]</Supervisor>
                                            <EmployeeName>[string]</EmployeeName>
                                            <HRMComment>[string]</HRMComment>
                                            <EmployeeNo>[string]</EmployeeNo>
                                            <EmployeeAppraisalHeaderNo>[string]</EmployeeAppraisalHeaderNo>
                                        </FilledAppraisal>
                                    </filledAppraisals>
                                </GetFilledAppraisals>
                            </Body>
                        </Envelope>";

            string tabledata = AppFunctions.CallWebService(req);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Employee Appraisal Header No", typeof(string));
            table.Columns.Add("Appraisal", typeof(string));
            table.Columns.Add("Validity Period", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));

            if (requestAs == "HRManager")
            {
                table.Columns.Add("Supervisor", typeof(string));
            }
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Action", typeof(string));


            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("FilledAppraisal"))
            {
                XmlNode NodeEmployeeAppraisalHeaderNo = xmlSoapRequest.GetElementsByTagName("EmployeeAppraisalHeaderNo")[count];
                string EmployeeAppraisalHeaderNo = NodeEmployeeAppraisalHeaderNo.InnerText;

                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Descrption")[count];
                string AppraisalName = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ValidFrom")[count];
                string ValidFrom = NodeValidFrom.InnerText;

                XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("ValidTo")[count];
                string ValidTo = NodeValidTo.InnerText;

                XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalHeaderNumber")[count];
                string AppraisalCode = NodeAppraisalCode.InnerText;

                XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                string EmployeeName = NodeEmployeeName.InnerText;

                XmlNode NodeEmployeeNo = xmlSoapRequest.GetElementsByTagName("EmployeeNo")[count];
                string EmployeeNo = NodeEmployeeNo.InnerText;

                XmlNode NodeHRComment = xmlSoapRequest.GetElementsByTagName("HRMComment")[count];
                string HRComment = NodeHRComment.InnerText;

                XmlNode NodeSupervisor = xmlSoapRequest.GetElementsByTagName("Supervisor")[count];
                string Supervisor = NodeSupervisor.InnerText;

                if (EmployeeAppraisalHeaderNo != "")
                {
                    string Validity = AppFunctions.Convert_AppraisalDetails(ValidFrom, ValidTo);

                    if (requestAs == "HRManager")
                    {
                        string SupervisorName = TrainingsXMLRequests.GetSupervisorFullName(Supervisor);

                        table.Rows.Add(EmployeeAppraisalHeaderNo, AppraisalName, Validity, EmployeeName, SupervisorName, status, "<a class = 'btn btn-primary btn-xs' href = " + "ViewAppraisal.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "&status=" + status + "&emp=" + AppFunctions.Base64Encode(EmployeeNo) + "&viewer=" + AppFunctions.Base64Encode(requestAs) + "&HRC=" + AppFunctions.Base64Encode(HRComment) + "&EmployeeAppraisalHeaderNo=" + AppFunctions.Base64Encode(EmployeeAppraisalHeaderNo) + "><span class = 'fa fa-eye'> </span></a>");
                    }
                    else
                    {
                        table.Rows.Add(EmployeeAppraisalHeaderNo, AppraisalName, Validity, EmployeeName, status, "<a class = 'btn btn-primary btn-xs' href = " + "ViewAppraisal.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "&status=" + status + "&emp=" + AppFunctions.Base64Encode(EmployeeNo) + "&viewer=" + AppFunctions.Base64Encode(requestAs) + "&HRC=" + AppFunctions.Base64Encode(HRComment) + "&EmployeeAppraisalHeaderNo=" + AppFunctions.Base64Encode(EmployeeAppraisalHeaderNo) + "><span class = 'fa fa-eye'> </span></a>");
                    }
                }

                count++;
            }

            return table;
        }
        public static DataTable GetApprovalEntries(string DocumentArea, string status)
        {
            string WorkflowApprovalUserName = HttpContext.Current.Session["username"].ToString();
            int count = 0;
            string AdvanceRequestStatus = null;

            if (status == "Pending")
            {
                AdvanceRequestStatus = "1";
            }
            else if (status == "Approved")
            {
                AdvanceRequestStatus = "4";
            }
            else if (status == "Rejected")
            {
                AdvanceRequestStatus = "3";
            }

            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();

            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string Status = json.Status;

            string GlobalDimCode1 = json.GlobalDimension1Code;
            string GlobalDimCode2 = json.GlobalDimension2Code;
            string ShortcutDimCode3 = json.ShortcutDimension3Code;
            string ShortcutDimCode8 = json.ShortcutDimension8Code;

            DataTable table = new DataTable();

            table.Columns.Add("Advance No.", typeof(string));
           // table.Columns.Add("Document Type", typeof(string));
            table.Columns.Add("Date submitted", typeof(string));
           // table.Columns.Add("Employee Name", typeof(string));

            table.Columns.Add(SetFirstLetterToUpper(GlobalDimCode1.ToLower()), typeof(string));
            table.Columns.Add(SetFirstLetterToUpper(GlobalDimCode2.ToLower()), typeof(string));
           


            if (DocumentArea != "TransportRequests")
            {

                if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                {
                    table.Columns.Add(SetFirstLetterToUpper(ShortcutDimCode3.ToLower()), typeof(string));
                    table.Columns.Add("Reg/Sr", typeof(string));
                }
                else
                {
                   // table.Columns.Add(SetFirstLetterToUpper(ShortcutDimCode8.ToLower()), typeof(string));
                }
                table.Columns.Add("Requester Name", typeof(string));
                table.Columns.Add("Mission Summary", typeof(string));
                table.Columns.Add("Total Amount", typeof(string));
            }
            else
            {
                table.Columns.Add(SetFirstLetterToUpper(ShortcutDimCode8.ToLower()), typeof(string));
                table.Columns.Add("Requester Name", typeof(string));
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Estimated cost of trip", typeof(string));
            }
            table.Columns.Add("Action", typeof(string));

            try
            {
                WebRef.ApprovalEntries _ApprovalEntry = new WebRef.ApprovalEntries();
                WebserviceConfig.ObjNav.GetApprovalEntries(ref _ApprovalEntry, DocumentArea, Convert.ToInt16(AdvanceRequestStatus), WorkflowApprovalUserName);
                int recordcount = _ApprovalEntry.ApprovalEntry.Count();

                if (recordcount > 0)
                {
                    foreach (
                        
                        var item 
                        
                        in 
                        
                        _ApprovalEntry.ApprovalEntry
                        
                        )
                    {
                        string EntryNo = item.EntryNo.ToString();
                        string TableID = item.TableId.ToString();
                        string DocumentNo = item.DocumentNo[0];
                        string DateTimeSentForApproval = item.DateTimeSentForApproval[0];
                        string EmployeeName = item.EmployeeName[0];
                        string ShortcutDimCode1 = item.ShortcutDimCode1[0];
                        string ShortcutDimCode2 = item.ShortcutDimCode2[0];
                        string ShortcutDimCode3Text = item.ShortcutDimCode3[0];
                        string TotalAmount = item.TotalAmount[0];
                        string MissionSummary = item.MissionSummary[0];
                        //for region
                        string ShortcutDimCode8Text = item.ShortcutDimCode8[0];
                        string DocumentType = item.DocumentType[0].ToString();
                        string StaffAdvanceHeaderNumber = item.StaffAdvanceHeaderNumber[0];
                        //transport request
                        string RequesterName = item.TRRequesterName[0];
                        string Destination = item.TRDestination[0];
                        string EstimatedCostofTrip = item.TREstimatedCostofTrip[0];




                        if (DocumentNo != "")
                        {

                            if (DocumentType == "0")
                            {
                                if (status == "Pending")
                                {
                                    if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                         "<a class = 'btn btn-success btn-xs approve_staffadvance' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Advance Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                         "<a class = 'btn btn-danger btn-xs reject_staffadvance' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Advance Request'><span class = 'fa fa-times' > </span></a> " +
                                         "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    else
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                        "<a class = 'btn btn-success btn-xs approve_staffadvance' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Advance Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                        "<a class = 'btn btn-danger btn-xs reject_staffadvance' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Advance Request'><span class = 'fa fa-times' > </span></a> " +
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }                                    
                                }
                                else
                                {
                                    if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    else
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                   
                                }
                            }
                            else if (DocumentType == "1")
                            {
                                //separate Transport Req and Adv Req

                                if (TableID == "52018772")
                                {
                                    if (status == "Pending")
                                    {
                                        if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                        {
                                            table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                              "<a class = 'btn btn-success btn-xs approve_staffclaim' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Staff Claim Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                               "<a class = 'btn btn-danger btn-xs reject_staffclaim' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Staff Claim Request'><span class = 'fa fa-times' > </span></a> " +
                                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Claim Request'><span class = 'fa fa-eye' > </span></a> ");
                                        }
                                        else
                                        {
                                            table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                              "<a class = 'btn btn-success btn-xs approve_staffclaim' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Staff Claim Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                               "<a class = 'btn btn-danger btn-xs reject_staffclaim' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Staff Claim Request'><span class = 'fa fa-times' > </span></a> " +
                                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Claim Request'><span class = 'fa fa-eye' > </span></a> ");
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                        {
                                            table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                            "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Staff Claim Request'><span class = 'fa fa-eye' > </span></a> ");
                                        }
                                        else
                                        {
                                            table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                            "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Staff Claim Request'><span class = 'fa fa-eye' > </span></a> ");
                                        }
                                        
                                    }
                                }
                                else if (TableID == "52018777")
                                {
                                    if (status == "Pending")
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, RequesterName, Destination, EstimatedCostofTrip,
                                          "<a class = 'btn btn-success btn-xs approve_transportrequest' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Transport Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                           "<a class = 'btn btn-danger btn-xs reject_transportrequest' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Transport Request'><span class = 'fa fa-times' > </span></a> " +
                                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Transport Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    else
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, RequesterName, Destination, EstimatedCostofTrip,
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewTransportRequest.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Transport Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                }
                            }
                            else if (DocumentType == "2")
                            {
                                if (status == "Pending")
                                {
                                    if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                      "<a class = 'btn btn-success btn-xs approve_staffsurrender' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Advance Surrender Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                       "<a class = 'btn btn-danger btn-xs reject_staffsurrender' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Advance Surrender Request'><span class = 'fa fa-times' > </span></a> " +
                                       "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNumber) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Surrender Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    else
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                          "<a class = 'btn btn-success btn-xs approve_staffsurrender' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Approve Advance Surrender Request'><span class = 'fa fa-paper-plane' > </span></a> " +
                                           "<a class = 'btn btn-danger btn-xs reject_staffsurrender' data-id=" + AppFunctions.Base64Encode(EntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Reject Advance Surrender Request'><span class = 'fa fa-times' > </span></a> " +
                                           "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNumber) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Surrender Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    
                                }
                                else
                                {
                                    if (HttpContext.Current.Session["Company"].ToString() == "KRCS GF Management Unit")
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, ShortcutDimCode3Text, ShortcutDimCode8Text, MissionSummary, TotalAmount,
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNumber) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Surrender Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    else
                                    {
                                        table.Rows.Add(DocumentNo, DateTimeSentForApproval, ShortcutDimCode1, ShortcutDimCode2, EmployeeName, MissionSummary, TotalAmount,
                                        "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(DocumentNo) + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNumber) + "&status=" + status + "&parent=Approver&ApprovalEntryNo=" + AppFunctions.Base64Encode(EntryNo) + " data-toggle='tooltip' title='View Advance Surrender Request'><span class = 'fa fa-eye' > </span></a> ");
                                    }
                                    
                                }
                            }

                        }

                        count++;
                    }

                }

            }catch(Exception es)
            {
                AppFunctions.WriteLog(es.Message);
            }
            return table;
        }       
        public static string GetOpenLeaveRecalls()
        {
            string LeaveStatus = "PendingApproval";

            string AppliedAs = "Supervisor";

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "LeaveRecall", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {
                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (HeaderNoLink != "")
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }
        public static string SendApprovalRequest(string LeaveHeaderNo, string username)
        {
            return WebService.ApproveApprovalRequest(LeaveHeaderNo, username, "Absence");
        }
        public static string RejectApprovalRequest(string LeaveHeaderNo, string username, string DocumentArea)
        {
            return WebService.RejectApprovalRequest(LeaveHeaderNo, username, DocumentArea);
        }
        public static string SaveRejectionComment(string DocumentNo, string Username, string RejectionComment, string DocumentArea)
        {
            return WebService.SaveRejectionComment(DocumentNo, Username, RejectionComment, DocumentArea);
        }
        public static string SetFirstLetterToUpper(string inString)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            return cultInfo.ToTitleCase(inString);
        }
    }
    public class LeaveApplicationXMLRequests
    {
        public static string GetUserLeaves(string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveLookups xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <lookupType>CauseOfAbsenceCode</lookupType> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                 </ReturnLeaveLookups> 
                             </Body>
                         </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetSelectedLeaveDetails(string username, string leaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveCode>" + leaveCode + @"</leaveCode>
                                </ReturnLeaveCodeDetails>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveQuantityAndReturnDate(string employeeNo, string causeofAbsenceCode, string startDate, string endDate)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <GetQuantityAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                            <employeeNo>" + employeeNo + @"</employeeNo>
                            <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                            <startDate>" + startDate + @"</startDate>
                            <endDate>" + endDate + @"</endDate>
                        </GetQuantityAndReturnDate>
                    </Body>
                </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveEndDateAndReturnDate(string employeeNo, string causeofAbsenceCode, string startDate, string qty)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetEndDateAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                                    <startDate>" + startDate + @"</startDate>
                                    <qty>" + qty + @"</qty>
                                </GetEndDateAndReturnDate>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string SaveLeaveApplication(string DocumentNo, string EmployeeID, string EmployeeName,
            string RequestDate, string DateCreated, string AccountId, string LeaveCode, string Description,
            string StartDate, string EndDate, string LeaveDays, string ReturnDate)
        {
            string request = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <leaveDetail>
                                            <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                                                <HeaderDocumentType>1</HeaderDocumentType>
                                                <HeaderNo>" + DocumentNo + @"</HeaderNo>
                                                <EmployeeID>" + EmployeeID + @"</EmployeeID>
                                                <EmployeeName>" + EmployeeName + @"</EmployeeName>
                                                <RequestDate>" + RequestDate + @"</RequestDate>
                                                <ApprovalStatus>0</ApprovalStatus>
                                                <DateCreated>" + DateCreated + @"</DateCreated>
                                                <ApproverID></ApproverID>
                                                <ApproverName></ApproverName>
                                                <LeaveSubType>Leave</LeaveSubType>
                                                <RejectionComment></RejectionComment>
                                                <AppliedBy>" + EmployeeID + @"</AppliedBy>
                                                <HasAttachment></HasAttachment>
                                                <AttachmentName></AttachmentName>
                                                <LeaveLine>
                                                    <LineDocumentNo>" + DocumentNo + @"</LineDocumentNo>
                                                    <LineDocumentType>1</LineDocumentType>
                                                    <LineNo>10000</LineNo>
                                                    <LeaveCode>" + LeaveCode + @"</LeaveCode>
                                                    <ExternalDocNo></ExternalDocNo>
                                                    <Description>" + Description + @"</Description>
                                                    <UnitOfMeasure>DAY</UnitOfMeasure>
                                                    <StartDate>" + StartDate + @"</StartDate>
                                                    <EndDate>" + EndDate + @"</EndDate>
                                                    <LeaveDays>" + LeaveDays + @"</LeaveDays>
                                                    <ReturnDate>" + ReturnDate + @"</ReturnDate>
                                                    <ApprovedStartDate>" + StartDate + @"</ApprovedStartDate>
                                                    <ApprovedEndDate>" + EndDate + @"</ApprovedEndDate>
                                                    <ApprovedQty>" + LeaveDays + @"</ApprovedQty>
                                                    <ApprovedReturnDate>" + ReturnDate + @"</ApprovedReturnDate>
                                                </LeaveLine>
                                            </LeaveHeader>
                                        </leaveDetail>
                                        <documentNo>" + DocumentNo + @"</documentNo>
                                        <employeeNo>" + EmployeeID + @"</employeeNo>
                                        <operation>Import</operation>
                                    </GetLeaveDetail>
                                </Body>
                            </Envelope>";
            
            string str = Assest.Utility.CallWebService(request);
            string resp = "";
            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                resp = "success";
            } else
            {
                resp = str;
            }

            return resp;
        }
        public static string GetDocumentNumber(string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo></documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveSubType>Leave</leaveSubType>
                                </GetLeaveNewNo>
                            </Body>
                        </Envelope>";

            
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str); 
        }
        public static void UploadFile(string documentNo, string fromPath)
        {
            WebService.AttachFileToRecord("Absence", documentNo, fromPath, "");
        }
    }
    public class LeaveForOtherXMLRequests
    {
        public static IDictionary<string, string> GetEmpoyeeList()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetEmployees xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                            <employeeList>
                                                <Employee xmlns=""urn:microsoft-dynamics-nav/xmlports/Employees"">
                                                    <EmpCode></EmpCode>
                                                    <EmployeeName></EmployeeName>
                                                </Employee>
                                            </employeeList>
                                            <hRArea></hRArea>
                                            <fieldName></fieldName>
                                        </GetEmployees>
                                    </Body>
                                </Envelope>";


            string strText = Assest.Utility.CallWebService(req);
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);
            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Employee"))
            {
                XmlNode NodeEmpCode = xmlSoapRequest.GetElementsByTagName("EmpCode")[count];
                string EmployeeCode = NodeEmpCode.InnerText;

                XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                string EmployeeName = NodeEmployeeName.InnerText;

                dictionary.Add(EmployeeCode, EmployeeName);

                count++;
            }
            return dictionary;
        }
        public static string GetUserLeaves(string employeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveLookups xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <lookupType>CauseOfAbsenceCode</lookupType>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                </ReturnLeaveLookups>
                            </Body>
                        </Envelope>";


            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveCodeDetails(string leaveCode, string employeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <leaveCode>" + leaveCode + @"</leaveCode>
                                </ReturnLeaveCodeDetails>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveState(string employeeNo, string causeofAbsenceCode, string startDate, string endDate)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <GetQuantityAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                            <employeeNo>" + employeeNo + @"</employeeNo>
                            <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                            <startDate>" + startDate + @"</startDate>
                            <endDate>" + endDate + @"</endDate>
                        </GetQuantityAndReturnDate>
                    </Body>
                </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveEndDateAndReturnDate(string employeeNo, string causeofAbsenceCode, string startDate, string qty)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetEndDateAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                                    <startDate>" + startDate + @"</startDate>
                                    <qty>" + qty + @"</qty>
                                </GetEndDateAndReturnDate>
                            </Body>
                        </Envelope>";


            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string SaveLeaveApplicationForOther(string DocumentNo, string EmployeeID, string EmployeeName, string RequestDate, string DateCreated,
            string username, string LeaveCode, string Description, string StartDate, string EndDate, string LeaveDays, string ReturnDate)
        {
            string request = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <leaveDetail>
                                        <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                                            <HeaderDocumentType>1</HeaderDocumentType>
                                            <HeaderNo>" + DocumentNo + @"</HeaderNo>
                                            <EmployeeID>" + EmployeeID + @"</EmployeeID>
                                            <EmployeeName>" + EmployeeName + @"</EmployeeName>
                                            <RequestDate>" + RequestDate + @"</RequestDate>
                                            <ApprovalStatus>0</ApprovalStatus>
                                            <DateCreated>" + DateCreated + @"</DateCreated>
                                            <ApproverID></ApproverID>
                                            <ApproverName></ApproverName>
                                            <LeaveSubType>Leave</LeaveSubType>
                                            <RejectionComment></RejectionComment>
                                            <AppliedBy>" + username + @"</AppliedBy>
                                            <HasAttachment></HasAttachment>
                                            <AttachmentName></AttachmentName>
                                            <LeaveLine>
                                                <LineDocumentNo>" + DocumentNo + @"</LineDocumentNo>
                                                <LineDocumentType>1</LineDocumentType>
                                                <LineNo>10000</LineNo>
                                                <LeaveCode>" + LeaveCode + @"</LeaveCode>
                                                <ExternalDocNo></ExternalDocNo>
                                                <Description>" + Description + @"</Description>
                                                <UnitOfMeasure>DAY</UnitOfMeasure>
                                                <StartDate>" + StartDate + @"</StartDate>
                                                <EndDate>" + EndDate + @"</EndDate>
                                                <LeaveDays>" + LeaveDays + @"</LeaveDays>
                                                <ReturnDate>" + ReturnDate + @"</ReturnDate>
                                                <ApprovedStartDate>" + StartDate + @"</ApprovedStartDate>
                                                <ApprovedEndDate>" + EndDate + @"</ApprovedEndDate>
                                                <ApprovedQty>" + LeaveDays + @"</ApprovedQty>
                                                <ApprovedReturnDate>" + ReturnDate + @"</ApprovedReturnDate>
                                            </LeaveLine>
                                        </LeaveHeader>
                                    </leaveDetail>
                                    <documentNo>" + DocumentNo + @"</documentNo>
                                    <employeeNo>" + EmployeeID + @"</employeeNo>
                                    <operation>Import</operation>
                                </GetLeaveDetail>
                            </Body>
                        </Envelope>";
            //return request;
            return Assest.Utility.CallWebService(request);
        }
        public static string SendApprovalRequest(string DocumentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <SendApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <documentArea>Absence</documentArea>
                                        <documentNo>" + DocumentNo + @"</documentNo>
                                    </SendApprovalRequest>
                                </Body>
                            </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetDocumentNumber(string EmployeeNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo></documentNo>
                                    <employeeNo>" + EmployeeNumber + @"</employeeNo>
                                    <leaveSubType>Leave</leaveSubType>
                                </GetLeaveNewNo>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
    }
    public class RecallApplicationXMLRequests
    {
        public static string GetLeaves(string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveLookups xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <lookupType>CauseOfAbsenceCode</lookupType> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                 </ReturnLeaveLookups> 
                             </Body>
                         </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveDetails(string username, string leaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveCode>" + leaveCode + @"</leaveCode>
                                </ReturnLeaveCodeDetails>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetApprovedLeaves(string username, string leaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                             <Body>
                                 <ReturnLastApprovedLeaves xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                     <employeeNo>" + username + @"</employeeNo>
                                     <leaveCode>" + leaveCode + @"</leaveCode>
                                 </ReturnLastApprovedLeaves>
                             </Body>
                         </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str); ;
        }
        public static string GetApprovedLeaveDetails(string username, string leaveNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <ReturnLeaveNoDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <employeeNo>" + username + @"</employeeNo>
                                        <leaveNo>" + leaveNo + @"</leaveNo>
                                    </ReturnLeaveNoDetail>
                                </Body>
                            </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static void SaveLeaveRecallApplication(string DocumentNo, string EmployeeID, string EmployeeName, string RequestDate, string DateCreated,
            string AccountId, string LeaveCode, string Description, string StartDate, string EndDate, string LeaveDays, string ReturnDate)
        {
            string request = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <leaveDetail>
                                        <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                                            <HeaderDocumentType>1</HeaderDocumentType>
                                            <HeaderNo>" + DocumentNo + @"</HeaderNo>
                                            <EmployeeID>" + EmployeeID + @"</EmployeeID>
                                            <EmployeeName>" + EmployeeName + @"</EmployeeName>
                                            <RequestDate>" + RequestDate + @"</RequestDate>
                                            <ApprovalStatus>0</ApprovalStatus>
                                            <DateCreated>" + DateCreated + @"</DateCreated>
                                            <ApproverID></ApproverID>
                                            <ApproverName></ApproverName>
                                            <LeaveSubType>LeaveRecall</LeaveSubType>
                                            <RejectionComment></RejectionComment>
                                            <AppliedBy>" + AccountId + @"</AppliedBy>
                                            <HasAttachment></HasAttachment>
                                            <AttachmentName></AttachmentName>
                                            <LeaveLine>
                                                <LineDocumentNo>" + DocumentNo + @"</LineDocumentNo>
                                                <LineDocumentType>1</LineDocumentType>
                                                <LineNo>10000</LineNo>
                                                <LeaveCode>" + LeaveCode + @"</LeaveCode>
                                                <ExternalDocNo></ExternalDocNo>
                                                <Description>" + Description + @"</Description>
                                                <UnitOfMeasure>DAY</UnitOfMeasure>
                                                <StartDate>" + StartDate + @"</StartDate>
                                                <EndDate>" + EndDate + @"</EndDate>
                                                <LeaveDays>" + LeaveDays + @"</LeaveDays>
                                                <ReturnDate>" + ReturnDate + @"</ReturnDate>
                                                <ApprovedStartDate>" + StartDate + @"</ApprovedStartDate>
                                                <ApprovedEndDate>" + EndDate + @"</ApprovedEndDate>
                                                <ApprovedQty>" + LeaveDays + @"</ApprovedQty>
                                                <ApprovedReturnDate>" + ReturnDate + @"</ApprovedReturnDate>
                                            </LeaveLine>
                                        </LeaveHeader>
                                    </leaveDetail>
                                    <documentNo>" + DocumentNo + @"</documentNo>
                                    <employeeNo>" + EmployeeID + @"</employeeNo>
                                    <operation>Import</operation>
                                </GetLeaveDetail>
                            </Body>
                        </Envelope>";

            string LeaveApplied = Assest.Utility.CallWebService(request);
        }
        public static string GetDocumentNumber(string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo></documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveSubType>LeaveRecall</leaveSubType>
                                </GetLeaveNewNo>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string SendApprovalRequest(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <SendApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <documentArea>Absence</documentArea>
                                        <documentNo>" + documentNo + @"</documentNo>
                                    </SendApprovalRequest>
                                </Body>
                            </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
    }
    public class LeaveRecallForOtherXMLRequests
    {
        public static string GetDocumentNumber(string username)
        {
            string req =
                @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo></documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <leaveSubType>Leave</leaveSubType>
                                </GetLeaveNewNo>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetApprovedLeaves(string employeeNo, string leaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                             <Body>
                                 <ReturnLastApprovedLeaves xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                     <employeeNo>" + employeeNo + @"</employeeNo>
                                     <leaveCode>" + leaveCode + @"</leaveCode>
                                 </ReturnLastApprovedLeaves>
                             </Body>
                         </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string SendApprovalRequest(string DocumentNo)
        {
            return WebserviceConfig.ObjNav.SendApprovalRequest("Absence", DocumentNo); 
        }
        public static string GetUserLeaves(string employeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveLookups xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <lookupType>CauseOfAbsenceCode</lookupType>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                </ReturnLeaveLookups>
                            </Body>
                        </Envelope>";


            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static IDictionary<string, string> GetEmpoyeeList()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetEmployees xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                            <employeeList>
                                                <Employee xmlns=""urn:microsoft-dynamics-nav/xmlports/Employees"">
                                                    <EmpCode></EmpCode>
                                                    <EmployeeName></EmployeeName>
                                                </Employee>
                                            </employeeList>
                                            <hRArea></hRArea>
                                            <fieldName></fieldName>
                                        </GetEmployees>
                                    </Body>
                                </Envelope>";
            string strText = Assest.Utility.CallWebService(req);
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);


            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Employee"))
            {
                XmlNode NodeEmpCode = xmlSoapRequest.GetElementsByTagName("EmpCode")[count];
                string EmployeeCode = NodeEmpCode.InnerText;

                XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                string EmployeeName = NodeEmployeeName.InnerText;

                dictionary.Add(EmployeeCode, EmployeeName);

                count++;
            }
            return dictionary;
        }
        public static string GetLeaveState(string employeeNo, string causeofAbsenceCode, string startDate, string endDate)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <GetQuantityAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                            <employeeNo>" + employeeNo + @"</employeeNo>
                            <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                            <startDate>" + startDate + @"</startDate>
                            <endDate>" + endDate + @"</endDate>
                        </GetQuantityAndReturnDate>
                    </Body>
                </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveDetails(string leaveCode, string employeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <leaveCode>" + leaveCode + @"</leaveCode>
                                </ReturnLeaveCodeDetails>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string GetLeaveEndDateAndReturnDate(string employeeNo, string causeofAbsenceCode, string startDate, string qty)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetEndDateAndReturnDate xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <causeofAbsenceCode>" + causeofAbsenceCode + @"</causeofAbsenceCode>
                                    <startDate>" + startDate + @"</startDate>
                                    <qty>" + qty + @"</qty>
                                </GetEndDateAndReturnDate>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static void SaveLeaveRecallForOther(string DocumentNo, string EmployeeID, string username, string EmployeeName, string RequestDate, string DateCreated,
            string AccountId, string LeaveCode, string Description, string StartDate, string EndDate, string LeaveDays, string ReturnDate)
        {
            string request = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <leaveDetail>
                                        <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                                            <HeaderDocumentType>1</HeaderDocumentType>
                                            <HeaderNo>" + DocumentNo + @"</HeaderNo>
                                            <EmployeeID>" + EmployeeID + @"</EmployeeID>
                                            <EmployeeName>" + EmployeeName + @"</EmployeeName>
                                            <RequestDate>" + RequestDate + @"</RequestDate>
                                            <ApprovalStatus>0</ApprovalStatus>
                                            <DateCreated>" + DateCreated + @"</DateCreated>
                                            <ApproverID></ApproverID>
                                            <ApproverName></ApproverName>
                                            <LeaveSubType>LeaveRecall</LeaveSubType>
                                            <RejectionComment></RejectionComment>
                                            <AppliedBy>" + username + @"</AppliedBy>
                                            <HasAttachment></HasAttachment>
                                            <AttachmentName></AttachmentName>
                                            <LeaveLine>
                                                <LineDocumentNo>" + DocumentNo + @"</LineDocumentNo>
                                                <LineDocumentType>1</LineDocumentType>
                                                <LineNo>10000</LineNo>
                                                <LeaveCode>" + LeaveCode + @"</LeaveCode>
                                                <ExternalDocNo></ExternalDocNo>
                                                <Description>" + Description + @"</Description>
                                                <UnitOfMeasure>DAY</UnitOfMeasure>
                                                <StartDate>" + StartDate + @"</StartDate>
                                                <EndDate>" + EndDate + @"</EndDate>
                                                <LeaveDays>" + LeaveDays + @"</LeaveDays>
                                                <ReturnDate>" + ReturnDate + @"</ReturnDate>
                                                <ApprovedStartDate>" + StartDate + @"</ApprovedStartDate>
                                                <ApprovedEndDate>" + EndDate + @"</ApprovedEndDate>
                                                <ApprovedQty>" + LeaveDays + @"</ApprovedQty>
                                                <ApprovedReturnDate>" + ReturnDate + @"</ApprovedReturnDate>
                                            </LeaveLine>
                                        </LeaveHeader>
                                    </leaveDetail>
                                    <documentNo>" + DocumentNo + @"</documentNo>
                                    <employeeNo>" + EmployeeID + @"</employeeNo>
                                    <operation>Import</operation>
                                </GetLeaveDetail>
                            </Body>
                        </Envelope>";
            string LeaveApplied = Assest.Utility.CallWebService(request);
        }
    }
    public class LeaverecallsXMLRequests
    {
        public static DataTable GetOthersPageData(string status, string owner)
        {
            string LeaveStatus = null;
            string AppliedAs = null;

            if (owner == "self")
            {
                AppliedAs = "Employee";
            }
            else if (owner == "others")
            {
                AppliedAs = "AppliedForAnother";
            }

            if (status == "Open")
            {
                LeaveStatus = "Open";
            }
            else if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "LeaveRecall", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                    string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Open")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = btn btn-succes btn-xs 'submit_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Application'><span class = 'fa fa-paper-plane' > </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " href = 'javascript:void(0)'><span class = 'fa fa-trash' data-toggle='tooltip' title='Delete Application'> </span></a> <a href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)'><span class = 'fa fa-times' data-toggle='tooltip' title='Cancel Application'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }

                    count++;
                }
            }

            return table;
        }
        public static DataTable GetSelfPageData(string status, string owner)
        {
            string LeaveStatus = null;
            string AppliedAs = null;

            if (owner == "self")
            {
                AppliedAs = "Employee";
            }
            else if (owner == "others")
            {
                AppliedAs = "AppliedForAnother";
            }

            if (status == "Open")
            {
                LeaveStatus = "Open";
            }
            else if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "LeaveRecall", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            // table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    //XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                    //string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Open")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-success btn-xs submit_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Application'><span class = 'fa fa-paper-plane'> </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete Application'><span class = 'fa fa-trash' > </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel Application'><span class = 'fa fa-times'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(CustomsClasses.AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, CustomsClasses.AppFunctions.ConvertTime(StartDate), CustomsClasses.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeaveRecall.aspx?id=" + CustomsClasses.AppFunctions.Base64Encode(HeaderNoLink) + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }

                    // table.Rows.Add(classes.AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, classes.AppFunctions.ConvertTime(StartDate), classes.AppFunctions.ConvertTime(EndDate), LeaveDays, "<a href = " + "ViewApprovedLeave.aspx?id=" + classes.AppFunctions.Base64Encode(HeaderNoLink) + "><span class = 'fa fa-eye'> View Record</span></a>");

                    count++;
                }
            }

            return table;
        }
        public static string SubmitOpenLeaveRecall(string DocumentNo)
        {
            return WebserviceConfig.ObjNav.SendApprovalRequest("Absence", DocumentNo);
        }
        public static string DeleteOpenLeaveRecall(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <DeleteHRActivityHeader xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <documentArea>Absence</documentArea>
                                <documentNo>" + documentNo + @"</documentNo>
                            </DeleteHRActivityHeader>
                        </Body>
                    </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string CancelOpenLeaveRecall(string documentNo)
        {
            return WebService.CancelApprovalRequest(documentNo, "Absence");
        }

    }
    public class LeavesXMLRequests
    {
        public static DataTable GetOthersPageData(string status, string owner)
        {
            string LeaveStatus = null;
            string AppliedAs = null;

            if (owner == "self")
            {
                AppliedAs = "Employee";
            }
            else if (owner == "others")
            {
                AppliedAs = "AppliedForAnother";
            }

            if (status == "Open")
            {
                LeaveStatus = "Open";
            }
            else if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();
    
            string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus, "Leave", "500");

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText) > 0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                    string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Open")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-secondary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " data-toggle='tooltip' title='Edit Application'><span class = 'fa fa-edit'> </span></a>  <a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " data-date=" + AppFunctions.ConvertTime(StartDate) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Application'><span class = 'fa fa-paper-plane'> </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete Application'><span class = 'fa fa-trash'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel Application'><span class = 'fa fa-times' > </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), EmployeeName, LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }

                    count++;
                }
            }

            return table;
        }
        public static DataTable GetSelfPageData(string status, string owner)
        {
            string LeaveStatus = null;
            string AppliedAs = null;

            if (owner == "self")
            {
                AppliedAs = "Employee";
            }
            else if (owner == "others")
            {
                AppliedAs = "AppliedForAnother";
            }

            if (status == "Open")
            {
                LeaveStatus = "Open";
            }
            else if (status == "Pending")
            {
                LeaveStatus = "PendingApproval";
            }
            else if (status == "Approved")
            {
                LeaveStatus = "Approved";
            }
            else if (status == "Rejected")
            {
                LeaveStatus = "Rejected";
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            //string tabledata = WebService.GetLeaveList("0", "10", username, AppliedAs, LeaveStatus,"Leave","500");
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <GetLeaveList xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                 <leaveList>
                                     <LeaveHeader xmlns = ""urn:microsoft-dynamics-nav/xmlports/GetLeaveList"">
                                          <HeaderDocumentType></HeaderDocumentType>
                                          <HeaderNo></HeaderNo>
                                          <EmployeeID></EmployeeID>
                                          <EmployeeName></EmployeeName>
                                          <RequestDate></RequestDate>
                                          <ApprovalStatus></ApprovalStatus>
                                          <DateCreated></DateCreated>
                                          <LeaveCode></LeaveCode>
                                          <Description></Description>
                                          <UnitOfMeasure></UnitOfMeasure>
                                          <StartDate></StartDate>
                                          <EndDate></EndDate>
                                          <LeaveDays></LeaveDays>
                                          <ReturnDate></ReturnDate>
                                          <ApprovedStartDate></ApprovedStartDate>
                                          <ApprovedEndDate></ApprovedEndDate>
                                          <ApprovedQty></ApprovedQty>
                                          <ApprovedReturnDate></ApprovedReturnDate>
                                          <LeaveSubType></LeaveSubType>
                                      </LeaveHeader>
                                  </leaveList>
                                  <startRecord>" + 0 + @"</startRecord>
                                  <noOfRecords>" + 10 + @"</noOfRecords>
                                  <employeeNo>" + username + @"</employeeNo>
                                  <requestAs>" + AppliedAs + @"</requestAs>
                                  <approvalStatus>" + LeaveStatus + @"</approvalStatus>
                                  <leaveSubType>" +"Leave" + @"</leaveSubType>
                                  <totalRecords>" + 500 + @"</totalRecords>
                              </GetLeaveList>
                          </Body>
                      </Envelope>";
             Assest.Utility.CallWebService(req);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(req);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("Leave Type", typeof(string));
            table.Columns.Add("Leave Number", typeof(string));
            table.Columns.Add("Start Date", typeof(string));
            table.Columns.Add("End Date", typeof(string));
            table.Columns.Add("Leave Days", typeof(string));
            table.Columns.Add("View", typeof(string));

            if (Convert.ToInt16(xmlSoapRequest.GetElementsByTagName("totalRecords")[count].InnerText)>0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("LeaveHeader"))
                {

                    XmlNode NodeDateCreated = xmlSoapRequest.GetElementsByTagName("DateCreated")[count];
                    string DateCreated = NodeDateCreated.InnerText;

                    XmlNode NodeLeaveCode = xmlSoapRequest.GetElementsByTagName("LeaveCode")[count];
                    string LeaveCode = NodeLeaveCode.InnerText;

                    XmlNode NodeHeaderNo = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNo = NodeHeaderNo.InnerText;

                    XmlNode NodeStartDate = xmlSoapRequest.GetElementsByTagName("StartDate")[count];
                    string StartDate = NodeStartDate.InnerText;

                    XmlNode NodeEndDate = xmlSoapRequest.GetElementsByTagName("EndDate")[count];
                    string EndDate = NodeEndDate.InnerText;

                    XmlNode NodeLeaveDays = xmlSoapRequest.GetElementsByTagName("LeaveDays")[count];
                    string LeaveDays = NodeLeaveDays.InnerText;

                    XmlNode NodeHeaderNoLink = xmlSoapRequest.GetElementsByTagName("HeaderNo")[count];
                    string HeaderNoLink = NodeHeaderNoLink.InnerText;

                    if (status == "Open")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-secondary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " data-toggle='tooltip' title='Edit Application'><span class = 'fa fa-edit'> </span></a>                                               <a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " data-date=" + AppFunctions.ConvertTime(StartDate) + " href = 'javascript:void(0)'><span class = 'fa fa-paper-plane' data-toggle='tooltip' title='Submit Application'> </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)'><span class = 'fa fa-trash' data-toggle='tooltip' title='Delete Application'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Open" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-success btn-xs delegate_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delegate Application'><span class = 'fa fa-fighter-jet'> </span></a>                             <a class = 'btn btn-danger btn-xs cancel_record' data-id=" + AppFunctions.Base64Encode(HeaderNoLink) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel Application'><span class = 'fa fa-times'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(AppFunctions.ConvertTime(DateCreated), LeaveCode, HeaderNo, AppFunctions.ConvertTime(StartDate), AppFunctions.ConvertTime(EndDate), LeaveDays, "<a class = 'btn btn-primary btn-xs' href = " + "ViewLeave.aspx?id=" + AppFunctions.Base64Encode(HeaderNoLink) + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }

                    count++;
                }
            }

            return table;
        }
        public static string SubmitOpenLeave(string DocumentNo)
        {
            return WebserviceConfig.ObjNav.SendApprovalRequest("Absence", DocumentNo);
        }
        public static string DeleteOpenLeave(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <DeleteHRActivityHeader xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <documentArea>Absence</documentArea>
                                <documentNo>" + documentNo + @"</documentNo>
                            </DeleteHRActivityHeader>
                        </Body>
                    </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string CancelOpenLeave(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CancelApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentArea>Absence</documentArea>
                                    <documentNo>" + documentNo + @"</documentNo>
                                </CancelApprovalRequest>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }

        public static string DelegateApprovalRequest(string LeaveHeaderNo, string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DelegateApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <calledFrom>Document</calledFrom>
                                    <documentArea>Absence</documentArea>
                                    <documentNo>" + LeaveHeaderNo + @"</documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <sequenceNo>0</sequenceNo>
                                </DelegateApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }

    }
    public class LoginXMLRequests
    {
        public static string UserLogin(string username, string password)
        {
            return WebService.ConfirmEmployeePassword(username, password);
        }
    }
    public class OneTimePassXMLRequests
    {
        public static string ChangePassword(string username, string oldpass, string newpass)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <ChangeEmployeePassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <empNo>" + username + @"</empNo>
                                <oldPassword>" + oldpass + @"</oldPassword>
                                <newPassword>" + newpass + @"</newPassword>
                            </ChangeEmployeePassword>
                        </Body>
                    </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
    }
    public class ProfileXMLRequests
    {
        public static string GetUserInformation(string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetEmployeeHomeData xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                     <employeeNo>" + username + @"</employeeNo>
                                 </GetEmployeeHomeData>
                             </Body>
                         </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
    }
    public class ViewLeaveRecallXMLRequests
    {
        public static string LoadLeaveRecallData(string LeaveID, string username)
        {
            return WebService.GetLeaveDetail(LeaveID, username, "Export", "");
        }
        public static string LoadLeaveDetails(string username, string LeaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                     <leaveCode>" + LeaveCode + @"</leaveCode> 
                                 </ReturnLeaveCodeDetails> 
                             </Body>
                         </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string SubmitOpenLeaveRecall(string DocumentNo)
        {
            return WebserviceConfig.ObjNav.SendApprovalRequest("Absence", DocumentNo);
        }
        public static string DeleteOpenLeaveRecall(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <DeleteHRActivityHeader xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <documentArea>Absence</documentArea>
                                <documentNo>" + documentNo + @"</documentNo>
                            </DeleteHRActivityHeader>
                        </Body>
                    </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CancelSubmittedeaveRecall(string documentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CancelApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentArea>Absence</documentArea>
                                    <documentNo>" + documentNo + @"</documentNo>
                                </CancelApprovalRequest>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
    }
    public class ViewApprovalEntryXMLRequests
    {
        public static string GetLeaveData(string LeaveID, string username, string Parent)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                            <leaveDetail>
                                <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                                    <HeaderDocumentType></HeaderDocumentType>
                                    <HeaderNo></HeaderNo>
                                    <EmployeeID></EmployeeID>
                                    <EmployeeName></EmployeeName>
                                    <RequestDate></RequestDate>
                                    <ApprovalStatus></ApprovalStatus>
                                    <DateCreated></DateCreated>
                                    <ApproverID></ApproverID>
                                    <ApproverName></ApproverName>
                                    <LeaveSubType>" + Parent + @"</LeaveSubType>
                                    <RejectionComment></RejectionComment>
                                    <AppliedBy></AppliedBy>
                                    <LeaveLine>
                                        <LineDocumentNo></LineDocumentNo>
                                        <LineDocumentType></LineDocumentType>
                                        <LineNo></LineNo>
                                        <LeaveCode></LeaveCode>
                                        <ExternalDocNo></ExternalDocNo>
                                        <Description></Description>
                                        <UnitOfMeasure></UnitOfMeasure>
                                        <StartDate></StartDate>
                                        <EndDate></EndDate>
                                        <LeaveDays></LeaveDays>
                                        <ReturnDate></ReturnDate>
                                        <ApprovedStartDate></ApprovedStartDate>
                                        <ApprovedEndDate></ApprovedEndDate>
                                        <ApprovedQty></ApprovedQty>
                                        <ApprovedReturnDate></ApprovedReturnDate>
                                    </LeaveLine>
                                </LeaveHeader>
                            </leaveDetail>
                            <documentNo>" + LeaveID + @"</documentNo>
                            <employeeNo>" + username + @"</employeeNo>
                            <operation>Export</operation>
                        </GetLeaveDetail>
                    </Body>
                </Envelope>";
            return AppFunctions.CallWebService(req);

        }
        public static string GetLeaveDetails(string username, string LeaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                     <leaveCode>" + LeaveCode + @"</leaveCode> 
                                 </ReturnLeaveCodeDetails> 
                             </Body>
                         </Envelope>";


            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string RejectApprovalRequest(string LeaveHeaderNo, string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <RejectApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                                    <calledFrom>Document</calledFrom>
                                                    <documentArea>Absence</documentArea>
                                                    <documentNo>" + LeaveHeaderNo + @"</documentNo>
                                                    <employeeNo>" + username + @"</employeeNo>
                                                    <sequenceNo></sequenceNo>
                                                </RejectApprovalRequest>
                                            </Body>
                                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static void SaveRejectionComment(string LeaveHeaderNo, string username, string rejectionComment)
        {
            string SaveRejectionComment = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <SaveRejectionComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                                    <calledFrom>Document</calledFrom>
                                                    <documentArea>Absence</documentArea>
                                                    <documentNo>" + LeaveHeaderNo + @"</documentNo>
                                                    <employeeNo>" + username + @"</employeeNo>
                                                    <sequenceNo></sequenceNo>
                                                    <rejectionComment>" + rejectionComment + @"</rejectionComment>
                                                </SaveRejectionComment>
                                            </Body>
                                        </Envelope>";

            string SaveRejectionCommentResponse = AppFunctions.CallWebService(SaveRejectionComment);

        }
        public static string ApproveApprovalRequest(string LeaveHeaderNo, string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ApproveApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <calledFrom>Document</calledFrom>
                                    <documentArea>Absence</documentArea>
                                    <documentNo>" + LeaveHeaderNo + @"</documentNo>
                                    <employeeNo>" + username + @"</employeeNo>
                                    <sequenceNo>0</sequenceNo>
                                </ApproveApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }

    }
    public class ViewLeaveXMLRequest
    {
        public static void DeleteAttachment(string documentNo)
        {
            string delete = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <DeleteAttachmentFromRecord xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <documentNo>" + documentNo + @"</documentNo>
                                        <documentArea>Absence</documentArea>
                                    </DeleteAttachmentFromRecord>
                                </Body>
                            </Envelope>";
            AppFunctions.CallWebService(delete);
        }
        public static string GetLeaveData(string LeaveID, string username)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
            <Body>
                <GetLeaveDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                    <leaveDetail>
                        <LeaveHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/GetLeaveDetail"">
                            <HeaderDocumentType></HeaderDocumentType>
                            <HeaderNo></HeaderNo>
                            <EmployeeID></EmployeeID>
                            <EmployeeName></EmployeeName>
                            <RequestDate></RequestDate>
                            <ApprovalStatus></ApprovalStatus>
                            <DateCreated></DateCreated>
                            <ApproverID></ApproverID>
                            <ApproverName></ApproverName>
                            <LeaveSubType></LeaveSubType>
                            <RejectionComment></RejectionComment>
                            <AppliedBy></AppliedBy>
                            <HasAttachment></HasAttachment>
                            <AttachmentName></AttachmentName>
                            <LeaveLine>
                                <LineDocumentNo></LineDocumentNo>
                                <LineDocumentType></LineDocumentType>
                                <LineNo></LineNo>
                                <LeaveCode></LeaveCode>
                                <ExternalDocNo></ExternalDocNo>
                                <Description></Description>
                                <UnitOfMeasure></UnitOfMeasure>
                                <StartDate></StartDate>
                                <EndDate></EndDate>
                                <LeaveDays></LeaveDays>
                                <ReturnDate></ReturnDate>
                                <ApprovedStartDate></ApprovedStartDate>
                                <ApprovedEndDate></ApprovedEndDate>
                                <ApprovedQty></ApprovedQty>
                                <ApprovedReturnDate></ApprovedReturnDate>
                            </LeaveLine>
                        </LeaveHeader>
                    </leaveDetail>
                    <documentNo>" + LeaveID + @"</documentNo>
                    <employeeNo>" + username + @"</employeeNo>
                    <operation>Export</operation>
                </GetLeaveDetail>
            </Body>
        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetLeaveDetails(string username, string LeaveCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReturnLeaveCodeDetails xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""> 
                                     <employeeNo>" + username + @"</employeeNo> 
                                     <leaveCode>" + LeaveCode + @"</leaveCode> 
                                 </ReturnLeaveCodeDetails> 
                             </Body>
                         </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }

    }
    public class AppraisalsXMLRequests
    {
        public static string GetDocumentNo(string AppraisalDocument)
        {
            return WebService.GetAppraisalNewNo(AppraisalDocument);
        }
        public static string CreateEmployeeAppraisalHeader(string appraisalHeaderNumber, string employeeNumber, string employeeAppraisalHeaderNo)
        {
            return WebService.CreateEmployeeAppraisalHeader(appraisalHeaderNumber, employeeNumber, employeeAppraisalHeaderNo);
        }
        public static DataTable GetAppraisalsToFill(string employeeNo)
        {
            string username = HttpContext.Current.Session["Username"].ToString();

            string str = AppFunctions.CallWebService(WebService.GetNewAppraisals("New", employeeNo, ""));

            XmlDocument xmlSoapRequest = new XmlDocument();
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("Appraisal", typeof(string));
            table.Columns.Add("Validity Period", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Action", typeof(string));

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Appraisal"))
                {
                    XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string AppraisalName = NodeAppraisalName.InnerText;

                    XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ValidFrom")[count];
                    string ValidFrom = NodeValidFrom.InnerText;

                    XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("ValidTo")[count];
                    string ValidTo = NodeValidTo.InnerText;

                    XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalNo")[count];
                    string AppraisalCode = NodeAppraisalCode.InnerText;

                    string Validity = AppFunctions.ConvertAppraisalDetails(ValidFrom, ValidTo);

                    if (AppraisalCode != "")
                    {
                        table.Rows.Add(AppraisalName, Validity, "Open", "<a class = 'btn btn-secondary btn-xs' href = " + "AppraisalApplication.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "><span class = 'fa fa-edit'> </span></a> ");
                    }

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static DataTable GetFilledAppraisal(string status, string employeeNumber, string requestAs)
        {
            string tabledata = WebService.GetFilledAppraisals(status, employeeNumber, requestAs);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Employee Appraisal Header No", typeof(string));
            table.Columns.Add("Appraisal", typeof(string));
            table.Columns.Add("Validity Period", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Action", typeof(string));


            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("FilledAppraisal"))
            {
                XmlNode NodeEmployeeAppraisalHeaderNo = xmlSoapRequest.GetElementsByTagName("EmployeeAppraisalHeaderNo")[count];
                string EmployeeAppraisalHeaderNo = NodeEmployeeAppraisalHeaderNo.InnerText;

                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Descrption")[count];
                string AppraisalName = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ValidFrom")[count];
                string ValidFrom = NodeValidFrom.InnerText;

                XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("ValidTo")[count];
                string ValidTo = NodeValidTo.InnerText;

                XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalHeaderNumber")[count];
                string AppraisalCode = NodeAppraisalCode.InnerText;

                XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("EmployeeName")[count];
                string EmployeeName = NodeEmployeeName.InnerText;

                XmlNode NodeEmployeeNo = xmlSoapRequest.GetElementsByTagName("EmployeeNo")[count];
                string EmployeeNo = NodeEmployeeNo.InnerText;

                XmlNode NodeHRComment = xmlSoapRequest.GetElementsByTagName("HRMComment")[count];
                string HRComment = NodeHRComment.InnerText;

                if (EmployeeAppraisalHeaderNo != "")
                {
                    string Validity = AppFunctions.Convert_AppraisalDetails(ValidFrom, ValidTo);
                    table.Rows.Add(EmployeeAppraisalHeaderNo, AppraisalName, Validity, EmployeeName, status, "<a class = 'btn btn-primary btn-xs' href = " + "ViewAppraisal.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "&status=" + status + "&emp=" + AppFunctions.Base64Encode(EmployeeNo) + "&viewer=" + AppFunctions.Base64Encode(requestAs) + "&HRC=" + AppFunctions.Base64Encode(HRComment) + "&EmployeeAppraisalHeaderNo=" + AppFunctions.Base64Encode(EmployeeAppraisalHeaderNo) + "><span class = 'fa fa-eye'> </span></a>");
                }

                count++;
            }

            return table;
        }
        public static string GetAppraisalHRComment(string employeeNumber, string appraisalHeaderNumber)
        {
            Appraisal fields;
            string HRMComment = "";
            string AppraisalCode = "";

            string tabledata = WebService.GetFilledAppraisals(employeeNumber, appraisalHeaderNumber);           

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("FilledAppraisal"))
            {
                XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalHeaderNumber")[count];
                AppraisalCode = NodeAppraisalCode.InnerText;

                if (AppraisalCode != "")
                {
                    XmlNode NodeHRMComment = xmlSoapRequest.GetElementsByTagName("HRMComment")[count];
                    HRMComment = NodeHRMComment.InnerText;
                }

                count++;
            }

            fields = new Appraisal
            {
                AppraisalHeaderNumber = AppraisalCode,
                HRComment = HRMComment
            };

            return JsonConvert.SerializeObject(fields);
        }
        public static IDictionary<string, string> GetQuestionObjectives(string appraisalHeaderNumber, string appraisalTargetNumber)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string tabledata = WebService.GetAppraisalTargetObjectives(appraisalHeaderNumber, appraisalTargetNumber);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalTargetObjective"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("TargetObjectiveNo")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ObjectiveDescription")[count];
                string Description = NodeValidFrom.InnerText;

                dictionary.Add(No, Description);

                count++;
            }

            return dictionary;
        }
        public static IDictionary<string, string> GetPerformanceMeasurementLevels()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string tabledata = WebService.GetPerformanceMeasurementLevels();

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;


            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("PerformanceMeasurementLevel"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeValidFrom.InnerText;

                dictionary.Add(No, Description);

                count++;
            }

            return dictionary;
        }
        public static string GetQuestionsToFillJson(string appraisalHeaderNumber, string appraisalTargetNo, string operation, string appraisalSection)
        {
            List<Question> products = new List<Question>();

            string tabledata = WebService.GetAppraisalTarget(appraisalHeaderNumber, appraisalTargetNo, operation, appraisalSection);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            Question qn;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalTarget"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("AppraisalTargetCode")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("TargetDescription")[count];
                string Description = NodeValidFrom.InnerText;

                XmlNode NodePerformanceMeasurementType = xmlSoapRequest.GetElementsByTagName("PerformanceMeasurementType")[count];
                string PerformanceMeasurementType = NodePerformanceMeasurementType.InnerText;

                XmlNode NodeWeightedScore = xmlSoapRequest.GetElementsByTagName("WeightedScore")[count];
                string WeightedScore = NodeWeightedScore.InnerText;

                XmlNode NodeAppraisalSection = xmlSoapRequest.GetElementsByTagName("AppraisalSection")[count];
                string AppraisalSection = NodeAppraisalSection.InnerText;

                qn = new Question
                {
                    QuestionNumber = No,
                    QuestionDescription = Description,
                    PerformanceMeasurementType = PerformanceMeasurementType,
                    WeightScoreValue = WeightedScore,
                    AppraisalSection = AppraisalSection
                };
                products.Add(qn);

                count++;
            }
            return JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);
        }
        public static string RecordResponses(string emloyeeAppraisalHeaderNo, string employeeNumber, string appraisalHeaderNumber, string TargetNumber, string Choice, string Description, string WeightedScore)
        {
            return WebService.AppraisalResponses(emloyeeAppraisalHeaderNo, employeeNumber, appraisalHeaderNumber, TargetNumber, Choice, Description, WeightedScore);
        }
        public static string GetResponse(string TargetNumber, string appraisalHeaderNumber, string employeeNumber)
        {           
            string x = WebService.AppraisalResponses(TargetNumber, appraisalHeaderNumber, employeeNumber);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(x);

            XmlNode NodeQuestionNumber = xmlSoapRequest.GetElementsByTagName("QuestionNumber")[0];
            string QuestionNumber = NodeQuestionNumber.InnerText;
            XmlNode NodeChoice = xmlSoapRequest.GetElementsByTagName("Choice")[0];
            string Choice = NodeChoice.InnerText;
            XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[0];
            string Description = NodeDescription.InnerText;
            XmlNode NoDeNo = xmlSoapRequest.GetElementsByTagName("SupervisorComment")[0];
            string SupervisorComent = NoDeNo.InnerText;
            XmlNode NoDeWeightedScore = xmlSoapRequest.GetElementsByTagName("WeightedScore")[0];
            string WeightedScore = NoDeWeightedScore.InnerText;

            var QuestionDetails = new QuestionResponse
            {
                QuestionNumber = QuestionNumber,
                ChoiceSelected = Choice,
                CommentSubmitted = Description,
                SupervisorComent = SupervisorComent,
                WeightedScore = WeightedScore
            };

            return JsonConvert.SerializeObject(QuestionDetails);
        }
        public static string EmployeeSubmitAppraisal(string AppraisalHeaderNumber, string EmployeeAppraisalHeaderNo, string EmployeeNumber)
        {
            return  WebserviceConfig.ObjNav.SendApprovalRequest("Appraisal", EmployeeAppraisalHeaderNo);
        }
        public static string SubmitComment(string QuestionNumber, string Comment, string employeeNumber, string appraisalHeaderNumber)
        {            
            return WebService.SubmitSupervisorComment(QuestionNumber, Comment, employeeNumber, appraisalHeaderNumber);
        }
        public static string UpdateAppraisalResponses(string apraisalHeaderNumber, string questionNumber, string employeeNo, string choice, string description, string weightedscore)
        {            
            return WebService.UpdateAppraisalResponses(apraisalHeaderNumber, questionNumber, employeeNo, choice, description, weightedscore);
        }
        public static string SaveHRComment(string appraisalHeaderNumber, string employeeNumber, string comment, string action)
        {
            return WebService.SaveHRComment(appraisalHeaderNumber, employeeNumber, comment, action);
        }
        public static string SendAppraisalToHR(string empNo, string appraisalHdrNo)
        {            
            return WebService.SendAppraisalToHR(empNo, appraisalHdrNo);
        }
        public static string CloseAppraisal(string empNo, string appraisalHdrNo, string hREmpNo)
        {
            return WebService.CloseAppraisal(empNo, appraisalHdrNo, hREmpNo);
        }
        public static string CreateAppraisalSectionComment(string sectionCommentNo, string appraisalHeaderNo, string employeeNo, string hREmployeeNo, string appraisalSectionNo, string hRComment)
        {            
            return WebService.CreateAppraisalSectionComment(sectionCommentNo, appraisalHeaderNo, employeeNo, hREmployeeNo, appraisalSectionNo, hRComment);
        }
        public static IDictionary<string, string> GetAppraisalMemberList(string appraisalHeaderNo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string tabledata = WebService.GetAppraisalMemberList(appraisalHeaderNo);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalMember"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("AppraisalListNo")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ApplicableToPerson")[count];
                string ApplicableToPersons = NodeValidFrom.InnerText;

                dictionary.Add(No, ApplicableToPersons);

                count++;
            }

            return dictionary;
        }
        public static string SendAppraisalBackToEmployee(string AppraisalHeaderNumber, string Username)
        {
            return WebService.RejectApprovalRequest(AppraisalHeaderNumber, Username, "Appraisal");
        }
    }
    public class CreateAppraisalXMLREquests
    {
        public static string DeleteAppraisal(string AppraisalHeaderNumber)
        {
            return WebService.DeleteAppraisal(AppraisalHeaderNumber);
        }
        public static string ReleaseAppraisal(string AppraisalHeaderNumber)
        {
            return WebService.ReleaseAppraisal(AppraisalHeaderNumber);
        }
        public static DataTable GetAppraisalsToFill(string employeeNo)
        {
            string username = HttpContext.Current.Session["Username"].ToString();

            string str = AppFunctions.CallWebService(WebService.GetNewAppraisals("CreatedBy", employeeNo, ""));

            XmlDocument xmlSoapRequest = new XmlDocument();

            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("Appraisal", typeof(string));
            table.Columns.Add("Validity Period", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Action", typeof(string));

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Appraisal"))
                {
                    XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string AppraisalName = NodeAppraisalName.InnerText;

                    XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ValidFrom")[count];
                    string ValidFrom = NodeValidFrom.InnerText;

                    XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("ValidTo")[count];
                    string ValidTo = NodeValidTo.InnerText;

                    XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalNo")[count];
                    string AppraisalCode = NodeAppraisalCode.InnerText;

                    XmlNode NodeStatus = xmlSoapRequest.GetElementsByTagName("Status")[count];
                    string Status = NodeStatus.InnerText;

                    string Validity = AppFunctions.ConvertAppraisalDetails(ValidFrom, ValidTo);

                    if (AppraisalCode != "")
                    {
                        if (Status == "0")
                        {
                            table.Rows.Add(AppraisalName, Validity, "Open",
                            "<a class = 'btn btn-success btn-xs ReleaseAppraisal' data-id=" + AppraisalCode + " href = 'javascript:void(0)'><span class = 'fa fa-location-arrow' data-toggle='tooltip' title='Release Appraisal To Appraisees'> </span></a> " +
                            "<a class = 'btn btn-secondary btn-xs' data-id=" + AppraisalCode + " href = " + "EditAppraisal.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "><span class = 'fa fa-pencil-alt' data-toggle='tooltip' title='Edit Appraisal Header'> </span></a> " +
                            "<a class = 'btn btn-primary btn-xs' href = " + "CreateAppraisalQuestions.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "> <span class = 'fa fa-edit' data-toggle='tooltip' title='Add Appraisal Questions'> </span></a> " +
                            "<a class = 'btn btn-danger btn-xs DeleteAppraisal' data-id=" + AppraisalCode + " href = 'javascript:void(0)'><span class = 'fa fa-trash-alt' data-toggle='tooltip' title='Delete Appraisal'> </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(AppraisalName, Validity, "Posted",
                            "<a class = 'btn btn-primary btn-xs' href = " + "ViewCreatedAppraisal.aspx?id=" + AppFunctions.Base64Encode(AppraisalCode) + "&code=" + AppFunctions.Base64Encode(AppraisalName) + "> <span class = 'fa fa-eye' data-toggle='tooltip' title='View Appraisal'> </span></a>");
                        }

                    }

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static IDictionary<string, string> AppraisalList(string employeeNo, string AppraisalHeaderNumber)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string strText = AppFunctions.CallWebService(WebService.GetNewAppraisals("", employeeNo, ""));

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);


            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Appraisal"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("AppraisalNo")[count];
                string Code = NodeCode.InnerText;

                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                if (Code != AppraisalHeaderNumber)
                {
                    dictionary.Add(Code, Description);
                }

                count++;
            }
            return dictionary;
        }
        public static IDictionary<string, string> GetOrgUnitList()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string strText = WebService.GetOrgUnits();
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);

            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("OrgUnit"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;

                XmlNode NodeName = xmlSoapRequest.GetElementsByTagName("Name")[count];
                string Name = NodeName.InnerText;

                dictionary.Add(Code, Name);

                count++;
            }
            return dictionary;
        }
        public static IDictionary<string, string> HRPositionList()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string strText = WebService.GetHRPositions();
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);


            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("HRPosition"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;

                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                dictionary.Add(Code, Description);

                count++;
            }
            return dictionary;
        }
        public static string CreateAppraisal(string headerNo, string username, string name, string applicableTo, string startDate, string endDate)
        {
            return WebService.CreateAppraisal(headerNo, username, name, applicableTo, startDate, endDate);
        }
        public static string GetAppraisalDetails(string AppraisalHeaderNo, string username)
        {
            string AppraisalHeaderNumber = "";
            string AppraisalDescription = "";
            string AppraisalStartDate = "";
            string AppraisalEndDate = "";
            string AppraisalType = "";
            string AppraisalApplicableTo = "";

            string tabledata = AppFunctions.CallWebService(WebService.GetNewAppraisals("Single", username, AppraisalHeaderNo));

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;
            string AppraisalName = "";
            string ValidFrom = "";
            string ValidTo = "";
            string AppraisalCode = "";
            string Appraisal_Type = "";
            string ApplicableTo = "";

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Appraisal"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("Description")[count];
                AppraisalName = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ValidFrom")[count];
                ValidFrom = NodeValidFrom.InnerText;

                XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("ValidTo")[count];
                ValidTo = NodeValidTo.InnerText;

                XmlNode NodeAppraisalCode = xmlSoapRequest.GetElementsByTagName("AppraisalNo")[count];
                AppraisalCode = NodeAppraisalCode.InnerText;

                XmlNode NodeApplicableTo = xmlSoapRequest.GetElementsByTagName("ApplicableTo")[count];
                ApplicableTo = NodeApplicableTo.InnerText;

                if (AppraisalCode != "")
                {
                    AppraisalHeaderNumber = AppraisalCode;
                    AppraisalDescription = AppraisalName;
                    AppraisalStartDate = ValidFrom;
                    AppraisalEndDate = ValidTo;
                    AppraisalType = Appraisal_Type;
                    AppraisalApplicableTo = ApplicableTo;
                }
                count++;
            }

            var AppraisalDetails = new Appraisal
            {
                AppraisalHeaderNumber = AppraisalHeaderNumber,
                AppraisalDescription = AppraisalDescription,
                AppraisalStartDate = AppraisalStartDate,
                AppraisalEndDate = AppraisalEndDate,
                AppraisalType = AppraisalType,
                AppraisalApplicableTo = AppraisalApplicableTo
            };

            return JsonConvert.SerializeObject(AppraisalDetails);
        }
        public static IDictionary<string, string> AppraisalSection(string Action)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string strText = WebService.GetAppraisalSections(Action);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);


            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalSection"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;

                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                dictionary.Add(Code, Description);

                count++;
            }
            return dictionary;
        }
        public static string UpdateAppraisal(string headerNo, string username, string name, string applicableTo, string startDate, string endDate)
        {            
            return WebService.UpdateAppraisal(headerNo, username, name, applicableTo, startDate, endDate);
        }
        public static string CreateAppraisalMembersList(string appraisalNo, string applicableToPersons)
        {
            return WebService.CreateAppraisalMembersList(appraisalNo, applicableToPersons);
        }
        public static string GetAppraisalSectionComment(string appraisalSection, string appraisalHeaderNo, string employeeNo)
        {
            QuestionResponse fields;
            
            string strText = WebService.GetAppraisalSectionComment(appraisalSection, appraisalHeaderNo, employeeNo);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(strText);


            string Code = "";
            string HRComment = "";

            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("SectionComment"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("SectionCommentNo")[count];
                Code = NodeCode.InnerText;

                XmlNode NodeHRComment = xmlSoapRequest.GetElementsByTagName("HRComment")[count];
                HRComment = NodeHRComment.InnerText;

                count++;
            }

            fields = new QuestionResponse
            {
                QuestionNumber = Code,
                HRAppraisalSectionComment = HRComment
            };

            return JsonConvert.SerializeObject(fields);
        }
    }
    public class CreateAppraisalQuestionXMLRequests
    {
        public static string UpdateAppraisalTarget(string appraisalTargetNo, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightedScore, string appraisalSection, string createdBy)
        {
            return WebService.UpdateAppraisalTarget(appraisalTargetNo, appraisalHeaderNumber, description, performanceMeasurementType, weightedScore, appraisalSection, createdBy);
        }
        public static string DeleteAppraisalTarget(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            return WebService.DeleteAppraisalTarget(AppraisalHeaderNo, AppraisalTargetNo);
        }
        public static string DeleteAppraisalTargetObjective(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            return WebService.DeleteAppraisalTargetObjectives(AppraisalHeaderNo, AppraisalTargetNo);
        }
        public static string CreateNewAppraisalQuestion(string no, string createdBy, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightScoreValue, string appraisalSection)
        {
            return WebService.CreateAppraisalTarget(no, createdBy, appraisalHeaderNumber, description, performanceMeasurementType, weightScoreValue, appraisalSection);
        }
        public static string CreateAppraisalQuestionObjective(string createdBy, string appraisalHeaderNo, string no, string appraisalTargetNumber, string description)
        {            
            return WebService.CreateAppraisalTargetObjective(createdBy, appraisalHeaderNo, no, appraisalTargetNumber, description);
        }
    }    
    public class DefineAppraisalSectionsXMLRequests
    {
        public static string DeleteAppraisalTarget(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            return WebService.DeleteHRAppraisalTarget(AppraisalHeaderNo, AppraisalTargetNo);
        }
        public static string DeleteAppraisalTargetObjectives(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            return WebService.DeleteHRAppraisalTargetObjectives(AppraisalHeaderNo, AppraisalTargetNo);
        }
        public static string CreateNewAppraisalQuestion(string no, string username, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightScoreValue, string appraisalSection)
        {
            return WebService.CreateHRAppraisalTarget(no, username, appraisalHeaderNumber, description, performanceMeasurementType, weightScoreValue, appraisalSection);
        }
        public static string CreateAppraisalQuestionObjective(string username, string AppraisalHeaderNumber, string no, string appraisalTargetNumber, string description)
        {
            return WebService.CreateHRAppraisalTargetObjective(username, AppraisalHeaderNumber, no, appraisalTargetNumber, description);
        }
        public static DataTable GetAppraisalSections()
        {
            string str = WebService.GetAppraisalSections("", "");

            XmlDocument xmlSoapRequest = new XmlDocument();

            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("Code", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Defined By", typeof(string));
            table.Columns.Add("Action", typeof(string));

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalSection"))
                {
                    XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                    string Code = NodeCode.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Description = NodeDescription.InnerText;

                    XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("IsHRDefined")[count];
                    string IsHRDefined = NodeValidTo.InnerText;

                    if (Code != "")
                    {
                        if (IsHRDefined == "true")
                        {
                            table.Rows.Add(Code, Description, "HR",
                                "<a class = 'btn btn-warning btn-xs SetSupervisorDefined' data-id=" + Code + " href = 'javascript:void(0)'><span class = 'fa fa-location-arrow' data-toggle='tooltip' title='Set to Supervisor Defined'> </span></a> " +
                                "<a class = 'btn btn-secondary btn-xs' data-id=" + Code + " href = " + "EditAppraisalSection.aspx?id=" + AppFunctions.Base64Encode(Code) + "&code=" + AppFunctions.Base64Encode(Code) + "><span class = 'fa fa-pencil-alt' data-toggle='tooltip' title='Edit Appraisal Section'> </span></a> " +
                                "<a class = 'btn btn-primary btn-xs' href = " + "CreateSectionTargets.aspx?id=" + AppFunctions.Base64Encode(Code) + "&code=" + AppFunctions.Base64Encode(Code) + "> <span class = 'fa fa-edit' data-toggle='tooltip' title='Define Section Targets'> </span></a> " +
                                "<a class = 'btn btn-danger btn-xs DeleteSection' data-id=" + Code + " href = 'javascript:void(0)'><span class = 'fa fa-trash-alt' data-toggle='tooltip' title='Delete Section'> </span></a> ");
                        }
                        else if (IsHRDefined == "false")
                        {
                            table.Rows.Add(Code, Description, "Supervisor",
                                "<a class = 'btn btn-success btn-xs SetHRDefined' data-id=" + Code + " href = 'javascript:void(0)'><span class = 'fa fa-location-arrow' data-toggle='tooltip' title='Set to HR Defined'> </span></a> " +
                                 "<a class = 'btn btn-secondary btn-xs' data-id=" + Code + " href = " + "EditAppraisalSection.aspx?id=" + AppFunctions.Base64Encode(Code) + "&code=" + AppFunctions.Base64Encode(Code) + "><span class = 'fa fa-pencil-alt' data-toggle='tooltip' title='Edit Appraisal Section'> </span></a> " +
                                 "<a class = 'btn btn-danger btn-xs DeleteSection' data-id=" + Code + " href = 'javascript:void(0)'><span class = 'fa fa-trash-alt' data-toggle='tooltip' title='Delete Section'> </span></a> ");

                        }
                    }

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static string CreateSection(string SectionName, string WhoDefines, string sectionNo)
        {
            return WebService.CreateAppraisalSection(SectionName, WhoDefines, sectionNo);
        }
        public static string DeleteAppraisalSection(string SectionNo)
        {
            return WebService.DeleteAppraisalSection(SectionNo);
        }
        public static string SetWhoDefines(string SectionNo, string WhoDefines)
        {
            return WebService.SetWhoDefinesSection(SectionNo, WhoDefines);
        }
        public static string GetQuestionsToFillJson(string appraisalHeaderNumber, string appraisalTargetNo, string operation, string appraisalSection)
        {
            List<Question> products = new List<Question>();

            string tabledata = WebService.GetHRAppraisalTarget(appraisalHeaderNumber, appraisalTargetNo, operation, appraisalSection);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            Question qn;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("HRAppraisalTarget"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("AppraisalTargetCode")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("TargetDescription")[count];
                string Description = NodeValidFrom.InnerText;

                XmlNode NodePerformanceMeasurementType = xmlSoapRequest.GetElementsByTagName("PerformanceMeasurementType")[count];
                string PerformanceMeasurementType = NodePerformanceMeasurementType.InnerText;

                XmlNode NodeWeightedScore = xmlSoapRequest.GetElementsByTagName("WeightedScore")[count];
                string WeightedScore = NodeWeightedScore.InnerText;
                XmlNode NodeAppraisalSection = xmlSoapRequest.GetElementsByTagName("AppraisalSection")[count];
                string AppraisalSection = NodeAppraisalSection.InnerText;

                qn = new Question
                {
                    QuestionNumber = No,
                    QuestionDescription = Description,
                    PerformanceMeasurementType = PerformanceMeasurementType,
                    WeightScoreValue = WeightedScore,
                    AppraisalSection = AppraisalSection
                };
                products.Add(qn);

                count++;
            }
            return JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);
        }
        public static IDictionary<string, string> GetQuestionObjectives(string appraisalHeaderNumber, string appraisalTargetNumber)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = WebService.GetHRAppraisalTargetObjectives(appraisalHeaderNumber, appraisalTargetNumber);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("HRTargetObjective"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("TargetObjectiveNo")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ObjectiveDescription")[count];
                string Description = NodeValidFrom.InnerText;

                dictionary.Add(No, Description);

                count++;
            }

            return dictionary;
        }
        public static DataTable GetAppraisalPMLs()
        {
            string username = HttpContext.Current.Session["Username"].ToString();
           
            string str = WebService.GetPerformanceMeasurementLevels("");

            XmlDocument xmlSoapRequest = new XmlDocument();

            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("Code", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Action", typeof(string));

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("PerformanceMeasurementLevel"))
                {
                    XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeAppraisalName.InnerText;

                    XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                    string Code = NodeCode.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Description = NodeDescription.InnerText;

                    if (Code != "")
                    {
                        table.Rows.Add(Code, Description,
                           "<a class = 'btn btn-secondary btn-xs' data-id=" + No + " href = " + "EditPML.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(No) + "><span class = 'fa fa-pencil-alt' data-toggle='tooltip' title='Edit PML'> </span></a> " +
                            "<a class = 'btn btn-danger btn-xs DeletePML' data-id=" + No + " href = 'javascript:void(0)'><span class = 'fa fa-trash-alt' data-toggle='tooltip' title='Delete PML'> </span></a> ");
                    }

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static string DeletePML(string pMLCode)
        {
            return WebService.DeletePML(pMLCode);
        }
        public static string CreatePML(string code, string description, string pMLNo)
        {
            return WebService.CreatePML(code, description, pMLNo);
        }
        public static string GetAppraisalSectionDetails(string AppraisalSectionNumber, string Action)
        {

            string str = WebService.GetAppraisalSections(Action, AppraisalSectionNumber);

            XmlDocument xmlSoapRequest = new XmlDocument();

            int count = 0;

            string Code = "";
            string Description = "";
            string IsHRDefined = "";


            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AppraisalSection"))
                {
                    XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                    Code = NodeCode.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    Description = NodeDescription.InnerText;

                    XmlNode NodeValidTo = xmlSoapRequest.GetElementsByTagName("IsHRDefined")[count];
                    IsHRDefined = NodeValidTo.InnerText;

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            var _AppraisalSection = new AppraisalSection
            {
                Code = Code,
                Description = Description,
                IsHRDefined = IsHRDefined
            };

            return JsonConvert.SerializeObject(_AppraisalSection);
        }

        public static string GetAppraisalPMLDetails(string _PMLNumber)
        {
            string str = WebService.GetPerformanceMeasurementLevels(_PMLNumber);

            XmlDocument xmlSoapRequest = new XmlDocument();

            int count = 0;

            string No = "";
            string Code = "";
            string Description = "";

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("PerformanceMeasurementLevel"))
                {
                    XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("No")[count];
                    No = NodeAppraisalName.InnerText;

                    XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                    Code = NodeCode.InnerText;

                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    Description = NodeDescription.InnerText;

                    count++;
                }
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            var _AppraisalPML = new AppraisalPML
            {
                Code = Code,
                Description = Description,
                No = No
            };
            return JsonConvert.SerializeObject(_AppraisalPML);
        }
    }
    public class TrainingsXMLRequests
    {
        public static string GetDocumentNo(string Document)
        {
            return WebService.GetTrainingNewNo(Document);
        }
        public static string GetSupervisorFullName(string EmployeeNo)
        {
            string GetNameFromSESAnoResponse = WebService.GetNameFromSESAno(EmployeeNo);
            dynamic json = JObject.Parse(GetNameFromSESAnoResponse);

            string name = json.Msg;

            return name;
        }
        public static DataTable GetTrainings(string status, string AppliedAs)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("Training No.", typeof(string));
            table.Columns.Add("Planned Start Date", typeof(string));
            table.Columns.Add("Planned End Date", typeof(string));
            table.Columns.Add("Training Description", typeof(string));
            table.Columns.Add("Course Name", typeof(string));
            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.GetTrainingList("", username, status, AppliedAs));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Training"))
                {
                    XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Description = NodeDescription.InnerText;

                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeCourseDescription = xmlSoapRequest.GetElementsByTagName("CourseDescription")[count];
                    string CourseDescription = NodeCourseDescription.InnerText;

                    XmlNode NodePlannedStartDate = xmlSoapRequest.GetElementsByTagName("PlannedStartDate")[count];
                    string PlannedStartDate = NodePlannedStartDate.InnerText;

                    XmlNode NodePlannedEndDate = xmlSoapRequest.GetElementsByTagName("PlannedEndDate")[count];
                    string PlannedEndDate = NodePlannedEndDate.InnerText;


                    if (No != "")
                    {
                        if (status == "Open")
                        {
                            table.Rows.Add(No, PlannedStartDate, PlannedEndDate, Description, CourseDescription,
                               "<a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete Training'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Training'><span class = 'fa fa-paper-plane' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs' href = " + "EditTrainingRequest.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(status) + "><span class = 'fa fa-pencil-alt' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewTraining.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(status) + "><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, PlannedStartDate, PlannedEndDate, Description, CourseDescription,
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewTraining.aspx?id=" + AppFunctions.Base64Encode(No) + "&code=" + AppFunctions.Base64Encode(status) + "><span class = 'fa fa-eye' > </span></a> ");
                        }

                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static string GetTrainingDetail(string TrainingNo)
        {
            string tabledata = AppFunctions.CallWebService(WebService.GetTrainingList(TrainingNo, "", "", ""));

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            string _AbsenceType = "";
            string _No = "";
            string _Description = "";
            string _PlannedStartDate = "";
            string _PlannedStartTime = "";
            string _PlannedEndDate = "";
            string _PlannedEndTime = "";
            string _TotalCost = "";
            string _NoSeries = "";
            string _CourseCode = "";
            string _CourseDescription = "";
            string _Trainer = "";
            string _TrainerName = "";
            string _Venue = "";
            string _Room = "";
            string _TrainingInstitution = "";
            string _ScheduledStartDate = "";
            string _ScheduledStartTime = "";
            string _ScheduledEndDate = "";
            string _ScheduledEndTime = "";
            string _ActualStartDate = "";
            string _ActualStartTime = "";
            string _ActualEndDate = "";
            string _ActualEndTime = "";
            string _CancellationCompletionDate = "";
            string _ProgressStatus = "";
            string _LPONo = "";
            string _TrainingPlanNo = "";
            string _Archived = "";
            string _CancellationReason = "";
            string _ActualCost = "";
            string _ApplicableTo = "";
            string _Approver = "";
            string _RequirementOfTrainingRequest = "";

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Training"))
            {
                XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                _No = NodeNo.InnerText;
                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                _Description = NodeDescription.InnerText;

                XmlNode NodePlannedStartDate = xmlSoapRequest.GetElementsByTagName("PlannedStartDate")[count];
                _PlannedStartDate = NodePlannedStartDate.InnerText;

                XmlNode NodePlannedStartTime = xmlSoapRequest.GetElementsByTagName("PlannedStartTime")[count];
                _PlannedStartTime = NodePlannedStartTime.InnerText;

                XmlNode NodePlannedEndDate = xmlSoapRequest.GetElementsByTagName("PlannedEndDate")[count];
                _PlannedEndDate = NodePlannedEndDate.InnerText;

                XmlNode NodePlannedEndTime = xmlSoapRequest.GetElementsByTagName("PlannedEndTime")[count];
                _PlannedEndTime = NodePlannedEndTime.InnerText;

                XmlNode NodeTotalCost = xmlSoapRequest.GetElementsByTagName("TotalCost")[count];
                _TotalCost = NodeTotalCost.InnerText;

                XmlNode NodeNoSeries = xmlSoapRequest.GetElementsByTagName("NoSeries")[count];
                _NoSeries = NodeNoSeries.InnerText;

                XmlNode NodeCourseCode = xmlSoapRequest.GetElementsByTagName("CourseCode")[count];
                _CourseCode = NodeCourseCode.InnerText;

                XmlNode NodeCourseDescription = xmlSoapRequest.GetElementsByTagName("CourseDescription")[count];
                _CourseDescription = NodeCourseDescription.InnerText;

                XmlNode NodeTrainer = xmlSoapRequest.GetElementsByTagName("Trainer")[count];
                _Trainer = NodeTrainer.InnerText;

                XmlNode NodeTrainerName = xmlSoapRequest.GetElementsByTagName("TrainerName")[count];
                _TrainerName = NodeTrainerName.InnerText;

                XmlNode NodeVenue = xmlSoapRequest.GetElementsByTagName("Venue")[count];
                _Venue = NodeVenue.InnerText;

                XmlNode NodeRoom = xmlSoapRequest.GetElementsByTagName("Room")[count];
                _Room = NodeRoom.InnerText;

                XmlNode NodeTrainingInstitution = xmlSoapRequest.GetElementsByTagName("TrainingInstitution")[count];
                _TrainingInstitution = NodeTrainingInstitution.InnerText;

                XmlNode NodeScheduledStartDate = xmlSoapRequest.GetElementsByTagName("ScheduledStartDate")[count];
                _ScheduledStartDate = NodeScheduledStartDate.InnerText;

                XmlNode NodeScheduledStartTime = xmlSoapRequest.GetElementsByTagName("ScheduledStartTime")[count];
                _ScheduledStartTime = NodeScheduledStartTime.InnerText;

                XmlNode NodeScheduledEndDate = xmlSoapRequest.GetElementsByTagName("ScheduledEndDate")[count];
                _ScheduledEndDate = NodeScheduledEndDate.InnerText;

                XmlNode NodeScheduledEndTime = xmlSoapRequest.GetElementsByTagName("ScheduledEndTime")[count];
                _ScheduledEndTime = NodeScheduledEndTime.InnerText;

                XmlNode NodeActualStartDate = xmlSoapRequest.GetElementsByTagName("ActualStartDate")[count];
                _ActualStartDate = NodeActualStartDate.InnerText;

                XmlNode NodeActualStartTime = xmlSoapRequest.GetElementsByTagName("ActualStartTime")[count];
                _ActualStartTime = NodeActualStartTime.InnerText;

                XmlNode NodeActualEndDate = xmlSoapRequest.GetElementsByTagName("ActualEndDate")[count];
                _ActualEndDate = NodeActualEndDate.InnerText;

                XmlNode NodeActualEndTime = xmlSoapRequest.GetElementsByTagName("ActualEndTime")[count];
                _ActualEndTime = NodeActualEndTime.InnerText;

                XmlNode NodeCancellationCompletionDate = xmlSoapRequest.GetElementsByTagName("CancellationCompletionDate")[count];
                _CancellationCompletionDate = NodeCancellationCompletionDate.InnerText;

                XmlNode NodeProgressStatus = xmlSoapRequest.GetElementsByTagName("ProgressStatus")[count];
                _ProgressStatus = NodeProgressStatus.InnerText;

                XmlNode NodeLPONo = xmlSoapRequest.GetElementsByTagName("LPONo")[count];
                _LPONo = NodeLPONo.InnerText;

                XmlNode NodeArchived = xmlSoapRequest.GetElementsByTagName("Archived")[count];
                _Archived = NodeArchived.InnerText;

                XmlNode NodeCancellationReason = xmlSoapRequest.GetElementsByTagName("CancellationReason")[count];
                _CancellationReason = NodeCancellationReason.InnerText;

                XmlNode NodeActualCost = xmlSoapRequest.GetElementsByTagName("ActualCost")[count];
                _ActualCost = NodeActualCost.InnerText;

                XmlNode NodeApplicableTo = xmlSoapRequest.GetElementsByTagName("ApplicableTo")[count];
                _ApplicableTo = NodeApplicableTo.InnerText;

                XmlNode NodeApprover = xmlSoapRequest.GetElementsByTagName("Approver")[count];
                _Approver = NodeApprover.InnerText;

                XmlNode NodeSourceOfTraining = xmlSoapRequest.GetElementsByTagName("SourceOfTraining")[count];
                _RequirementOfTrainingRequest = NodeSourceOfTraining.InnerText;

                count++;
            }

            var TrainingDetails = new Training
            {
                AbsenceType = _AbsenceType,
                No = _No,
                Description = _Description,
                PlannedStartDate = _PlannedStartDate,
                PlannedStartTime = _PlannedStartTime,
                PlannedEndDate = _PlannedEndDate,
                PlannedEndTime = _PlannedEndTime,
                TotalCost = _TotalCost,
                NoSeries = _NoSeries,
                CourseCode = _CourseCode,
                CourseDescription = _CourseDescription,
                Trainer = _Trainer,
                TrainerName = _TrainerName,
                Venue = _Venue,
                Room = _Room,
                TrainingInstitution = _TrainingInstitution,
                ScheduledStartDate = _ScheduledStartDate,
                ScheduledStartTime = _ScheduledStartTime,
                ScheduledEndDate = _ScheduledEndDate,
                ScheduledEndTime = _ScheduledEndTime,
                ActualStartDate = _ActualStartDate,
                ActualStartTime = _ActualStartTime,
                ActualEndDate = _ActualEndDate,
                ActualEndTime = _ActualEndTime,
                CancellationCompletionDate = _CancellationCompletionDate,
                ProgressStatus = _ProgressStatus,
                LPONo = _LPONo,
                TrainingPlanNo = _TrainingPlanNo,
                Archived = _Archived,
                CancellationReason = _CancellationReason,
                ActualCost = _ActualCost,
                ApplicableTo = _ApplicableTo,
                Approver = _Approver,
                RequirementOfTrainingRequest = _RequirementOfTrainingRequest
            };

            return JsonConvert.SerializeObject(TrainingDetails);
        }
        public static string CreateTraining(string No, string Description, string PlannedStartDate, string PlannedStartTime,
            string PlannedEndDate, string PlannedEndTime, string TotalCost, string NoSeries, string CourseCode, string CourseDescription,
            string Trainer, string TrainerName, string Venue,
        string Room, string TrainingInstitution, string ProgressStatus, string LPONo, string Archived, string CancellationReason,
        string ActualCost, string CreatedBy, string ApplicableToPersons, string RequirementOfTraining)
        {
            return WebService.CreateTraining(No, Description, PlannedStartDate, PlannedStartTime, PlannedEndDate, PlannedEndTime, TotalCost, NoSeries, CourseCode, CourseDescription, Trainer, TrainerName, Venue, Room, TrainingInstitution, ProgressStatus, LPONo, Archived, CancellationReason, ActualCost, CreatedBy, ApplicableToPersons, RequirementOfTraining);
        }
        public static string DeleteTraining(string TrainingNo)
        {
            return WebService.DeleteTraining(TrainingNo);
        }
        public static string SubmitTraining(string TrainingNo)
        {
            return WebserviceConfig.ObjNav.SendApprovalRequest("Training", TrainingNo);
        }
        public static string ApproveTrainingRequest(string TrainingNo, string Username)
        {
            return WebService.ApproveApprovalRequest(TrainingNo, Username, "Training");
        }
        public static string RejectTrainingRequest(string TrainingNo, string Username)
        {
            return WebService.RejectApprovalRequest(TrainingNo, Username, "Training");
        }

        public static string CreateTrainingMemberList(string appraisalMembersListNo, string TrainingNumber, string applicableToPersons)
        {
            return WebService.CreateTrainingMembersList(appraisalMembersListNo, TrainingNumber, applicableToPersons);
        }
        public static IDictionary<string, string> GetTrainingMemberList(string TrainingNo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string tabledata = WebService.GetTrainingMembersList(TrainingNo);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;


            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("TrainingMember"))
            {
                XmlNode NodeAppraisalName = xmlSoapRequest.GetElementsByTagName("TrainingListNo")[count];
                string No = NodeAppraisalName.InnerText;

                XmlNode NodeValidFrom = xmlSoapRequest.GetElementsByTagName("ApplicableToPersons")[count];
                string ApplicableToPersons = NodeValidFrom.InnerText;

                dictionary.Add(No, ApplicableToPersons);

                count++;
            }

            return dictionary;
        }
    }
    public class GenerateReports
    {
        public static string GeneratePaySlip(string EmployeeNo, string Path, string Date)
        {
            return WebService.ExportPayslip(EmployeeNo, Path, Date);
        }
        public static string GenerateP9Form(string EmployeeNo, string Path, string Year)
        {
            return WebService.ExportP9(EmployeeNo, Path, Year);
        }
        public static string GenerateStaffAdvance(string EmployeeNo, string Path)
        {
            return WebService.ExportStaffAdvance(EmployeeNo, Path);
        }
    }
    public class CreateAdvanceRequestXMLRequests
    {
        public static string GetAdvanceType(string Code)
        {
            string TargetResponse = WebService.GetAdvanceRequestType(Code);

            dynamic json = JObject.Parse(TargetResponse);
            string Currency = json.Currency;
            string Description = json.Description;
            string UnitCost = json.UnitCost;
            string UnitOfMeasure = json.UnitOfMeasure;
            string ExchangeRate = json.ExchangeRate;

            UnitCost = UnitCost.Replace(",", ""); 

            var _AdvanceRequestType = new AdvanceRequestType
            {
                Code = Code,
                Currency = Currency,
                Description = Description,
                UnitCost = UnitCost,
                UnitOfMeasure = UnitOfMeasure,
                ExchangeRate = ExchangeRate
            };

            return JsonConvert.SerializeObject(_AdvanceRequestType);
        }
        public static IDictionary<string, string> GetCompanies()
        {
            Dictionary<string, string> companies = new Dictionary<string, string>();

            string companydata = AppFunctions.CallWebService(WebService.GetCompanies());

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(companydata);

            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Company"))
            {
                XmlNode NodeName = xmlSoapRequest.GetElementsByTagName("Name")[count];
                string Name = NodeName.InnerText;

                companies.Add(count.ToString(), Name);

                count++;
            }

            return companies;
        }
        public static IDictionary<string, string> GetCurrencies()
        {
            Dictionary<string, string> currencies = new Dictionary<string, string>();

            string currencydata = AppFunctions.CallWebService(WebService.GetCurrencies());

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(currencydata);

            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Currency"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;
                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                currencies.Add(Code, Description);

                count++;
            }
            //var sortedDict = from entry in currencies orderby entry.Value ascending select entry;
            // return sortedDict.ToDictionary(x => x.Key, x => x.ToString());
            return currencies;
        }         
        public static string GetAdvanceRequestNewNo(string documentType, string RegionCode)
        {
            return WebService.GetAdvanceRequestNewNo(documentType, RegionCode);
        }
        public static string GetDimensionCodes()
        {
            return WebService.GetDimensionCodes();
        }
        public static string GetAdvanceTypes()
        {
            List<AdvanceRequestTypes> AdvanceRequestTypesList = new List<AdvanceRequestTypes>();

            string Advancetypedata = WebService.ExportAdvanceTypes();

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(Advancetypedata);

            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceType"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;
                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                AdvanceRequestTypesList.Add(new AdvanceRequestTypes { Code = Code, Description = Description });

                count++;
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        public static string GetUnitOfMeasure()
        {
            List<AdvanceRequestTypes> AdvanceRequestTypesList = new List<AdvanceRequestTypes>();

            string Advancetypedata = WebService.ExportUnitOfMeasure();

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(Advancetypedata);

            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Unit"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;
                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                string Description = NodeDescription.InnerText;

                AdvanceRequestTypesList.Add(new AdvanceRequestTypes { Code = Code, Description = Code });

                count++;
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        public static string GetAdvanceRequestLines(string AdvanceRequestHdrNo)
        {
            List<AdvanceRequestLines> AdvanceRequestTypesList = new List<AdvanceRequestLines>();

            string AdvanceRequestJSON = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    AdvanceRequestTypesList.Add(new AdvanceRequestLines { No = item.No, ItemDescription = item.ItemDescription });
                }
            }

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        public static string GetBudgetLineCode(string Code)
        {
            List<DimensionCode> AdvanceRequestTypesList = new List<DimensionCode>();          
          

            foreach (var item in CreateAdvanceRequestXMLRequests.GetDimCode(Code))
            {
                if (item.Key != "")
                {
                    AdvanceRequestTypesList.Add(new DimensionCode { Code = item.Key, Description = item.Value });
                }
            }            

            return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        public static string[] GetShortcutDimCodeArrayString(string Code)
        {
            List<string> list = new List<string>();

            foreach (var item in CreateAdvanceRequestXMLRequests.GetDimCode(Code))
            {
                if (item.Key != "")
                {
                    list.Add(item.Value);
                }
            }

            return list.ToArray();
        }
        public static string ValidateShortcutDimCode3(string code)
        {
            string Response = WebService.ValidateDimensionValueCode(code);

            dynamic jsonValidateDimensionValueCodeResponse = JObject.Parse(Response);

            string _Status = jsonValidateDimensionValueCodeResponse.Status;
            string Msg = jsonValidateDimensionValueCodeResponse.Msg;
            var response = new
            {
                Status = _Status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(response);
        }
        public static string GetShortcutDim3Code(string Code)
        {
            DataTable datatable = getData();

            //DataRow[] filteredRows =
            //            datatable.Select(string.Format("{0} LIKE '%{1}%'", "UserId", "Satinder"));



            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach (DataRow dr in datatable.Select(string.Format("{0} LIKE '%{1}%'", "text", Code)))
            {
                row = new Dictionary<string, object>();

                foreach (DataColumn col in datatable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            string tet = serializer.Serialize(rows);

            return tet;



            //List<ShortcutDimensionCode3> AdvanceRequestTypesList = new List<ShortcutDimensionCode3>();


            //foreach (var item in CreateAdvanceRequestXMLRequests.GetDimCode(Code))
            //{
            //    if (item.Key != "")
            //    {
            //        AdvanceRequestTypesList.Add(new ShortcutDimensionCode3 { id = item.Key, text = item.Value });
            //    }
            //}

            //return JsonConvert.SerializeObject(AdvanceRequestTypesList);
        }
        public static DataTable getData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("text", typeof(string));
            dt.Rows.Add(1, "Satinder Singh");
            dt.Rows.Add(2, "Amit Sarna");
            dt.Rows.Add(3, "Andrea Ely");
            dt.Rows.Add(4, "Leslie Mac");
            dt.Rows.Add(5, "Vaibhav Adhyapak");
            return dt;
        }
        public static IDictionary<string, string> GetDimCode(string DimCode)
        {
            Dictionary<string, string> currencies = new Dictionary<string, string>();

            string currencydata = AppFunctions.CallWebService(WebService.ExportDimensionCodeValues(DimCode));

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(currencydata);

            int count = 0;

            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("DimCodeValue"))
            {
                XmlNode NodeCode = xmlSoapRequest.GetElementsByTagName("Code")[count];
                string Code = NodeCode.InnerText;
                XmlNode NodeDescription = xmlSoapRequest.GetElementsByTagName("Name")[count];
                string Description = NodeDescription.InnerText;

                currencies.Add(Code, Description.TrimEnd());

                count++;
            }

            return currencies;
        } 
        public static string UpdateAdvanceRequest(string AdvanceRequestHdrNo,string documentType, string DateOfRequest, string DateDue, string Requester, string RequestBy, string RequestToCompany,
          string GlobalDimCode1, string GlobalDimCode2, string ShortCutDimCode1, string ShortCutDimCode2, string ShortCutDimCode3, string ShortCutDimCode4, string ShortCutDimCode5, string ShortCutDimCode6, string ShortCutDimCode7, string ShortCutDimCode8, string Currency, string staffAdvanceHeaderNo, string preferredPaymentMethod, string MissionSummary)
        {
            return WebService.UpdateAdvanceRequest(AdvanceRequestHdrNo, documentType, DateOfRequest, DateDue, Requester, RequestBy, RequestToCompany,
                                                   GlobalDimCode1, GlobalDimCode2, ShortCutDimCode1, ShortCutDimCode2, ShortCutDimCode3, ShortCutDimCode4, ShortCutDimCode5, ShortCutDimCode6, ShortCutDimCode7, ShortCutDimCode8, Currency, staffAdvanceHeaderNo, preferredPaymentMethod, MissionSummary);
        }
        public static string CreateAdvanceRequestLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortcutDimCode1, string shortcutDimCode2, string shortcutDimCode3, string shortcutDimCode4, string shortcutDimCode5, string shortcutDimCode6, string shortcutDimCode7, string shortcutDimCode8)
        {
            return WebService.CreateAdvanceRequestLine(AdvanceRequestHdrNo,"0", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, "0",  "", Purpose, globalDimCode1, globalDimCode2, shortcutDimCode1, shortcutDimCode2, shortcutDimCode3, shortcutDimCode4, shortcutDimCode5, shortcutDimCode6, shortcutDimCode7, shortcutDimCode8);
        }
        public static string UpdateAdvanceRequestLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string LineNo, string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortCutDimCode1, string shortCutDimCode2, string shortCutDimCode3, string shortCutDimCode4, string shortCutDimCode5, string shortCutDimCode6, string shortCutDimCode7, string shortCutDimCode8)
        {
            return WebService.UpdateAdvanceRequestLine(AdvanceRequestHdrNo, "0", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "0", Remarks, Purpose, globalDimCode1, globalDimCode2, shortCutDimCode1, shortCutDimCode2, shortCutDimCode3, shortCutDimCode4, shortCutDimCode5, shortCutDimCode6, shortCutDimCode7, shortCutDimCode8);
        }
        public static void UploadFile(string documentType, string documentNo, string fromPath, string description)
        {
            WebService.AttachAttachmentToRecord("AdvanceRequest", documentType, documentNo, fromPath, description, "0","0","0");
        }
    }
    public class AdvanceRequestsXMLRequests
    {
        public static int GetNumberOfAttachments(string AdvanceRequestHdrNo)
        {
            int count = 0;

            string str = AppFunctions.CallWebService(WebService.ExportAttachments(AdvanceRequestHdrNo, "StaffAdvance"));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Attachment"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("EntryNo")[count];
                    string No = NodeNo.InnerText;

                    if (No != "0")
                    {
                        count = count + 1;
                    }
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return count;
        }
        public static DataTable GetAdvanceRequestsList(string status, string CreatedBy)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            string AdvanceRequestHdrNo = "";
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Expected mission start date", typeof(string));
            table.Columns.Add("Expected mission end date", typeof(string));

            if (status == "Open")
            {
                table.Columns.Add("Rejection Comment", typeof(string));
            }

            if (status == "Pending")
            {
                table.Columns.Add("Approver", typeof(string));
            }
           
            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests("StaffAdvance", status, AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    string DateOfRequest = NodeDateOfRequest.InnerText;

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    string DateDue = NodeDateDue.InnerText;

                    XmlNode NodeApprover = xmlSoapRequest.GetElementsByTagName("Approver")[count];
                    string Approver = NodeApprover.InnerText;

                    XmlNode NodeApprovalEntryNo = xmlSoapRequest.GetElementsByTagName("ApprovalEntryNo")[count];
                    string ApprovalEntryNo = NodeApprovalEntryNo.InnerText;

                    XmlNode NodeRejectionComment = xmlSoapRequest.GetElementsByTagName("RejectionComment")[count];
                    string RejectionComment = NodeRejectionComment.InnerText;

                    if (No != "")
                    {
                        if (status == "Open")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, RejectionComment,
                               "<a class = 'btn btn-secondary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + " data-toggle='tooltip' title='Edit advance request'><span class = 'fa fa-pencil-alt' > </span></a> " +
                               "<a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance request'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit advance request'><span class = 'fa fa-paper-plane' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + " data-toggle='tooltip' title='View advance request'><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else if(status == "Pending")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, Approver,
                               "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel advance request'><span class = 'fa fa-times' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs delegate_record' data-id=" + AppFunctions.Base64Encode(ApprovalEntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delegate advance request'><span class = 'fa fa-fighter-jet' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs print_staffadvance' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Advance Request'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + " data-toggle='tooltip' title='View advance request'><span class = 'fa fa-eye' > </span></a> ");
                        }

                        else
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue,
                               "<a class = 'btn btn-secondary btn-xs print_staffadvance' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Advance Request'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceRequest.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "><span class = 'fa fa-eye' > </span></a> ");
                        }
                    }
                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static string GetAdvanceRequests(string DocumentType, string _AdvanceRequestHdrNo)
        {
            List<AdvanceRequestLines> AdvanceRequestLinesList = new List<AdvanceRequestLines>();

            string username = HttpContext.Current.Session["Username"].ToString();
            int count = 0;
            int countInnerNodes = 0;
            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests(DocumentType, "ExportSingle", _AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            string AdvanceRequestHdrNo = "";
            string Requester = "";
            string DateOfRequest = "";
            string DateDue = "";
            string GlobalDimCode1 = "";
            string GlobalDimCode2 = "";
            string ShortcutDimCode1 = "";
            string ShortcutDimCode2 = "";
            string ShortcutDimCode3 = "";
            string ShortcutDimCode4 = "";
            string ShortcutDimCode5 = "";
            string ShortcutDimCode6 = "";
            string ShortcutDimCode7 = "";
            string ShortcutDimCode8 = "";
            string PreferredPaymentMethod = "";
            string Currency = "";
            string Balance = "";
            string StaffAdvanceHeaderNo = "";
            string MissionSummary = "";
            string RejectionComment = "";

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {

                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    AdvanceRequestHdrNo = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    DateOfRequest = NodeDateOfRequest.InnerText;

                    if(DateOfRequest != "")
                    {
                        DateOfRequest = Convert.ToDateTime(DateOfRequest).ToString("dd/MM/yyyy"); 
                    }                    

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    DateDue = NodeDateDue.InnerText;

                    if (DateDue != "")
                    {
                        DateDue = Convert.ToDateTime(DateDue).ToString("dd/MM/yyyy");
                    }

                    XmlNode NodeRequester = xmlSoapRequest.GetElementsByTagName("Requester")[count];
                    Requester = NodeRequester.InnerText;

                    XmlNode NodeGlobalDimCode1 = xmlSoapRequest.GetElementsByTagName("GlobalDimCode1")[count];
                    GlobalDimCode1 = NodeGlobalDimCode1.InnerText;

                    XmlNode NodeGlobalDimCode2 = xmlSoapRequest.GetElementsByTagName("GlobalDimCode2")[count];
                    GlobalDimCode2 = NodeGlobalDimCode2.InnerText;

                    XmlNode NodeShortcutDimCode1 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode1")[count];
                    ShortcutDimCode1 = NodeShortcutDimCode1.InnerText;

                    XmlNode NodeShortcutDimCode2 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode2")[count];
                    ShortcutDimCode2 = NodeShortcutDimCode2.InnerText;

                    XmlNode NodeShortcutDimCode3 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode3")[count];
                    ShortcutDimCode3 = NodeShortcutDimCode3.InnerText;

                    XmlNode NodeShortcutDimCode4 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode4")[count];
                    ShortcutDimCode4 = NodeShortcutDimCode4.InnerText;

                    XmlNode NodeShortcutDimCode5 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode5")[count];
                    ShortcutDimCode5 = NodeShortcutDimCode5.InnerText;

                    XmlNode NodeShortcutDimCode6 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode6")[count];
                    ShortcutDimCode6 = NodeShortcutDimCode6.InnerText;

                    XmlNode NodeShortcutDimCode7 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode7")[count];
                    ShortcutDimCode7 = NodeShortcutDimCode7.InnerText;

                    XmlNode NodeShortcutDimCode8 = xmlSoapRequest.GetElementsByTagName("ShortcutDimCode8")[count];
                    ShortcutDimCode8 = NodeShortcutDimCode8.InnerText;

                    XmlNode NodePreferredPaymentMethod = xmlSoapRequest.GetElementsByTagName("PreferredPaymentMethod")[count];
                    PreferredPaymentMethod = NodePreferredPaymentMethod.InnerText;

                    XmlNode NodeBalance = xmlSoapRequest.GetElementsByTagName("Balance")[count];
                    Balance = NodeBalance.InnerText;

                    XmlNode NodeStaffAdvanceHeaderNo = xmlSoapRequest.GetElementsByTagName("StaffAdvanceHeaderNo")[count];
                    StaffAdvanceHeaderNo = NodeStaffAdvanceHeaderNo.InnerText;

                    XmlNode NodeMissionSummary = xmlSoapRequest.GetElementsByTagName("MissionSummary")[count];
                    MissionSummary = NodeMissionSummary.InnerText;

                    XmlNode NodeRejectionComment = xmlSoapRequest.GetElementsByTagName("RejectionComment")[count];
                    RejectionComment = NodeRejectionComment.InnerText;

                    if (Currency == "")
                    {
                        Currency = HttpContext.Current.Session["LCY"].ToString();
                    }

                    foreach (XmlNode innerxmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequestLines"))
                    {
                        XmlNode _NodeNo = xmlSoapRequest.GetElementsByTagName("AdvanceRequestLineNo")[countInnerNodes];
                        XmlNode _NodeItem = xmlSoapRequest.GetElementsByTagName("Item")[countInnerNodes];
                        XmlNode _NodePurpose = xmlSoapRequest.GetElementsByTagName("Purpose")[countInnerNodes];
                        XmlNode _NodeItemDescription = xmlSoapRequest.GetElementsByTagName("ItemDescription")[countInnerNodes];
                        XmlNode _UnitOfMeasure = xmlSoapRequest.GetElementsByTagName("UnitOfMeasure")[countInnerNodes];
                        XmlNode _NodeNoOfUnits = xmlSoapRequest.GetElementsByTagName("NoOfUnits")[countInnerNodes];
                        XmlNode _NodeUnitCost = xmlSoapRequest.GetElementsByTagName("UnitCost")[countInnerNodes];                      
                        XmlNode _NodeCurrency = xmlSoapRequest.GetElementsByTagName("Currency")[countInnerNodes];
                        XmlNode _NodeAttachmentName = xmlSoapRequest.GetElementsByTagName("AttachmentName")[countInnerNodes];
                        XmlNode _NodeAttachmentId = xmlSoapRequest.GetElementsByTagName("AttachmentId")[countInnerNodes];

                        XmlNode _NodeAdvancedAmount = xmlSoapRequest.GetElementsByTagName("AdvancedAmount")[countInnerNodes];
                        XmlNode _NodeAdvancedAmountLCY = xmlSoapRequest.GetElementsByTagName("AdvancedAmountLCY")[countInnerNodes];
                        XmlNode _NodeClaimedAmount = xmlSoapRequest.GetElementsByTagName("ClaimedAmount")[countInnerNodes];
                        XmlNode _NodeClaimedAmountLCY = xmlSoapRequest.GetElementsByTagName("ClaimedAmountLCY")[countInnerNodes];
                        XmlNode _NodeSurrenderedAmount = xmlSoapRequest.GetElementsByTagName("SurrenderedAmount")[countInnerNodes];
                        XmlNode _NodeSurrenderedAmountLCY = xmlSoapRequest.GetElementsByTagName("SurrenderedAmountLCY")[countInnerNodes];
                        XmlNode _LineShortcutDimCode3 = xmlSoapRequest.GetElementsByTagName("LineShortcutDimCode3")[countInnerNodes];
                        XmlNode _LineShortcutDimCode5 = xmlSoapRequest.GetElementsByTagName("LineShortcutDimCode5")[countInnerNodes];

                        string Item = _NodeItem.InnerText;
                        string LineNo = _NodeNo.InnerText;
                        string Purpose = _NodePurpose.InnerText;
                        string UnitOfMeasure = _UnitOfMeasure.InnerText;
                        string NoOfUnits = _NodeNoOfUnits.InnerText;
                        string UnitCost = _NodeUnitCost.InnerText;
                        string _Currency = _NodeCurrency.InnerText;
                        string AdvancedAmount = _NodeAdvancedAmount.InnerText;
                        string AdvancedAmountLCY = _NodeAdvancedAmountLCY.InnerText;
                        string ClaimedAmount = _NodeClaimedAmount.InnerText;
                        string ClaimedAmountLCY = _NodeClaimedAmountLCY.InnerText;
                        string SurrenderedAmount = _NodeSurrenderedAmount.InnerText;
                        string SurrenderedAmountLCY = _NodeSurrenderedAmountLCY.InnerText;
                        string AttachmentName = _NodeAttachmentName.InnerText;
                        string AttachmentId = _NodeAttachmentId.InnerText;
                        string LineShortcutDimCode3 = _LineShortcutDimCode3.InnerText;
                        string LineShortcutDimCode5 = _LineShortcutDimCode5.InnerText;
                        string ItemDescription = _NodeItemDescription.InnerText;

                        if (LineNo != "")
                        {
                            AdvanceRequestLinesList.Add(

                            new AdvanceRequestLines
                            {
                                No = LineNo,
                                Item = Item,
                                ItemDescription = ItemDescription,
                                Purpose = Purpose,
                                UnitOfMeasure = UnitOfMeasure,
                                NoOfUnits = NoOfUnits,
                                UnitCost = UnitCost,
                                Currency = _Currency,
                                AttachmentId = AttachmentId,
                                AttachmentName = AttachmentName,
                                AdvancedAmount = AdvancedAmount,
                                AdvancedAmountLCY = AdvancedAmountLCY,
                                ClaimedAmount = ClaimedAmount,
                                ClaimedAmountLCY = ClaimedAmountLCY,
                                SurrenderedAmount = SurrenderedAmount,
                                SurrenderedAmountLCY = SurrenderedAmountLCY,
                                ShortcutDimCode3 = LineShortcutDimCode3,
                                ShortcutDimCode5 = LineShortcutDimCode5

                            });
                        }

                        countInnerNodes++;
                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }


            var _AdvanceRequestHeader = new AdvanceRequestHeader
            {
                No = AdvanceRequestHdrNo,
                DateOfRequest = DateOfRequest,
                DateDue = DateDue,
                Requester = Requester,
                GlobalDimCode1 = GlobalDimCode1,
                GlobalDimCode2 = GlobalDimCode2,
                ShortcutDimCode1 = ShortcutDimCode1,
                ShortcutDimCode2 = ShortcutDimCode2,
                ShortcutDimCode3 = ShortcutDimCode3,
                ShortcutDimCode4 = ShortcutDimCode4,
                ShortcutDimCode5 = ShortcutDimCode5,
                ShortcutDimCode6 = ShortcutDimCode6,
                ShortcutDimCode7 = ShortcutDimCode7,
                ShortcutDimCode8 = ShortcutDimCode8,
                PreferredPaymentMethod = PreferredPaymentMethod,
                MissionSummary = MissionSummary,
                RejectionComment = RejectionComment,
                Balance = Balance,
                AdvanceRequestHdrNo = StaffAdvanceHeaderNo,
                _AdvanceRequestLines = AdvanceRequestLinesList,


            };
            string aa = JsonConvert.SerializeObject(_AdvanceRequestHeader);

            return aa;
        }

        public static string SubmitAdvanceRequest(string AdvanceRequestHdrNo)
        {
            string response = "";

            int NumberOfAttachments = GetNumberOfAttachments(AdvanceRequestHdrNo);

            if (NumberOfAttachments > 0)
            {
                double CustomerBalance = Convert.ToDouble(GetBalance(AdvanceRequestHdrNo));

                if (CustomerBalance <= 0)
                {
                    response = WebService.SubmitAdvanceRequest("0", AdvanceRequestHdrNo);
                }
                else
                {
                    var _RequestResponse = new
                    {
                        Status = "999",
                        Msg = "The Advance request header number " + AdvanceRequestHdrNo + " cannot be processed because you have a balance"
                    };
                    response = JsonConvert.SerializeObject(_RequestResponse);
                }                
            }
            else
            {
                var _RequestResponse = new 
                {
                    Status = "999",
                    Msg = "The Advance request header number " + AdvanceRequestHdrNo + " has no attatchment. Kindly add an attachment before proceeding"
                };                               

                response = JsonConvert.SerializeObject(_RequestResponse);
            }
            return response;
        }
        public static double GetBalance(string AdvanceRequestHdrNo)
        {
            double Balance = 0;
            string AdvanceRequestData = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffAdvance", AdvanceRequestHdrNo);
            dynamic json = JObject.Parse(AdvanceRequestData);
            string customerbalance = json.Balance;

            if(customerbalance != "")
            {
                Balance = Convert.ToDouble(customerbalance);
            }

            return Balance;
        }
        public static string DeleteAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DeleteAdvanceRequest("0", AdvanceRequestHdrNo);
        }
        public static string DelegateAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DelegateWorkflowApprovalRequest(AdvanceRequestHdrNo);
        }
        public static string CancelAdvanceRequest(string AdvanceRequestHdrNo, string DocumentType)
        {
            return WebService.CancelWorkflowApprovalRequest(AdvanceRequestHdrNo, DocumentType);
        }
        public static string DeleteAdvanceRequestLine(string AdvanceRequestHdrNo, string LineNo)
        {
            return WebService.DeleteAdvanceRequestLine("0", AdvanceRequestHdrNo, LineNo);
        }
        
        public static string GetAdvanceRequestsLinesTable(string AdvanceRequestHdrNo, string status)
        {
            string AdvanceRequestJSON = GetAdvanceRequests("StaffAdvance", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);

            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();
            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string ShortcutDimCode3 = json.ShortcutDimension3Code;

            double sum = 0;

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    string itemamount = item.AdvancedAmountLCY;

                    itemamount = itemamount.Replace(",", "");

                    double itemAmount = 0;

                    if (itemamount != "")
                    {
                        itemAmount = Convert.ToDouble(itemamount);
                    }

                    sum = sum + itemAmount;
                }
            }

            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
                html.Append("<th>"+ ApprovalEntiesXMLRequests.SetFirstLetterToUpper(ShortcutDimCode3.ToLower()) + "</th>");
                //html.Append("<th>Item</th>");
                html.Append("<th>Item Description</th>");
                html.Append("<th>Purpose</th>");
                html.Append("<th>Unit of Measure</th>");
                html.Append("<th>Unit Cost</th>");
                html.Append("<th>No. Of Units</th>");
                html.Append("<th>Currency</th>");
                html.Append("<th>Amount(LCY)</th>");
                html.Append("<th>Action</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
                html.Append("<th></th>");
                //html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th></th>");
                html.Append("<th>"+ string.Format("{0:N2}", sum) + "</th>");
                html.Append("<th></th>");
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");
            
            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    html.Append("<tr>");

                    if (status == "Open")
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>"+ item.Item + "</td>");
                        html.Append("<td>"+ item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>"+ item.UnitOfMeasure + "</td>");
                        html.Append("<td>"+ item.UnitCost + "</td>");
                        html.Append("<td>"+ item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");
                        html.Append("<td>"+ item.AdvancedAmountLCY + "</td>");
                        html.Append("<td><a class = 'btn btn-danger btn-xs delete_advanceRequestLines' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance request line'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs EditAdvanceReqLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Edit advance request line'><span class = 'fa fa-pencil-alt' > </span></a></td>");

                    }
                    else
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>" + item.Item + "</td>");
                        html.Append("<td>" + item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>" + item.UnitOfMeasure + "</td>");
                        html.Append("<td>" + item.UnitCost + "</td>");
                        html.Append("<td>" + item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");
                        html.Append("<td>" + item.AdvancedAmountLCY + "</td>");
                        html.Append("<td></td>");
                    }

                    html.Append("</tr>");
                }
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();

            return strText;
        }
        public static DataTable GetAttachments(string AdvanceRequestHdrNo, string Uploadspath, string DocumentArea, string status, string DocumentType)
        {
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("File Name", typeof(string));
            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.ExportAttachments(AdvanceRequestHdrNo, DocumentType));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Attachment"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("EntryNo")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeFileName = xmlSoapRequest.GetElementsByTagName("FileName")[count];
                    string FileName = NodeFileName.InnerText;

                    XmlNode NodeDesription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Desription = NodeDesription.InnerText;

                    //DownloadFile

                    string exportToPath = Uploadspath + FileName;

                    WebService.ExportAttachmentsToFile(DocumentType, AdvanceRequestHdrNo, "0", exportToPath);

                    if (Desription != "")
                    {
                        if(status == "Open")
                        {
                            table.Rows.Add(No, Desription, FileName,
                               "<a class = 'btn btn-danger btn-xs delete_attachment' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete attachment'><span class = 'fa fa-trash-alt' > </span></a>"+
                               "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a> ");
                        }
                        else if (status != "Open")
                        {
                            table.Rows.Add(No, Desription, FileName,
                               "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a> ");
                        }                       
                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static DataTable GetAttachments2(string AdvanceRequestHdrNo, string Uploadspath, string DocumentArea, string status, string DocumentType)
        {
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("File Name", typeof(string));
            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.ExportAttachments(AdvanceRequestHdrNo, DocumentType));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Attachment"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("EntryNo")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeFileName = xmlSoapRequest.GetElementsByTagName("FileName")[count];
                    string FileName = NodeFileName.InnerText;

                    XmlNode NodeDesription = xmlSoapRequest.GetElementsByTagName("Description")[count];
                    string Desription = NodeDesription.InnerText;

                    //DownloadFile

                    string exportToPath = Uploadspath + FileName;

                    WebService.ExportAttachmentsToFile(DocumentType, AdvanceRequestHdrNo, "0", exportToPath);

                    if (Desription != "")
                    {
                        if (status == "Open")
                        {
                            table.Rows.Add(No, Desription, FileName,
                               "<a class = 'btn btn-danger btn-xs delete_attachment2' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete attachment'><span class = 'fa fa-trash-alt' > </span></a>" +
                               "<a class = 'btn btn-primary btn-xs downloadfile2' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a> ");
                        }
                        else if (status != "Open")
                        {
                            table.Rows.Add(No, Desription, FileName,
                               "<a class = 'btn btn-primary btn-xs downloadfile2' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a> ");
                        }
                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }

        public static string GetAdvanceRequestLine(string AdvanceRequestLineNo)
        {
            return WebService.GetAdvanceRequestLine("0", AdvanceRequestLineNo);
        }
        public static string DeleteAttachment(string DocumentNo)
        {
            return WebService.DeleteAttachment(DocumentNo);
        }
    }
    public class AdvanceSurrender
    {
        public static string GetAdvanceSurrenderinesTable(string AdvanceRequestHdrNo, string status)
        {
            string Uploadspath = HttpContext.Current.Server.MapPath("~/Uploads/");

            string AdvanceRequestJSON = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);


            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();
            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string ShortcutDimCode3 = json.ShortcutDimension3Code;

            double sum = 0;
            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    string itemamount = item.SurrenderedAmountLCY;

                    itemamount = itemamount.Replace(",", "");

                    double itemAmount = 0;

                    if (itemamount != "")
                    {
                        itemAmount = Convert.ToDouble(itemamount);
                    }

                    sum = sum + itemAmount;
                }
            }

            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th>" + ApprovalEntiesXMLRequests.SetFirstLetterToUpper(ShortcutDimCode3.ToLower()) + "</th>");
            //html.Append("<th>Item</th>");
            html.Append("<th>Item Description</th>");
            html.Append("<th>Purpose</th>");
            html.Append("<th>Unit of Measure</th>"); 
            html.Append("<th>Unit Cost</th>");
            html.Append("<th>No. Of Units</th>");
            html.Append("<th>Currency</th>");           
            //html.Append("<th>Attachment</th>");
            html.Append("<th>Advanced Amount</th>");
            html.Append("<th>Amount(LCY)</th>");
            html.Append("<th>Action</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
            html.Append("<th></th>");
            //html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            //html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th>" + string.Format("{0:N2}", sum) + "</th>");
            html.Append("<th></th>");
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    html.Append("<tr>");

                    if (status == "Open")
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>" + item.Item + "</td>");
                        html.Append("<td>" + item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>" + item.UnitOfMeasure + "</td>");
                        html.Append("<td>" + item.UnitCost + "</td>");
                        html.Append("<td>" + item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");

                        //if (item.AttachmentName != "")
                        //{
                        //    string FileName = item.AttachmentName;

                        //    string exportToPath = Uploadspath + FileName;

                        //    WebService.ExportAttachmentsToFile("StaffSurrender", AdvanceRequestHdrNo, item.No, exportToPath);

                        //    html.Append("<td>" + item.AttachmentName + " <a class = 'btn btn-danger btn-xs delete_advanceSurrenderLineAttachment' data-id=" + item.AttachmentId + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete line attachment'><span class = 'fa fa-trash' > </span></a>" +
                        //        "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a></td>");
                        //}
                        //else
                        //{
                        //    html.Append("<td></td>");
                        //}
                        html.Append("<td>" + item.AdvancedAmount + "</td>");
                        html.Append("<td>" + item.SurrenderedAmountLCY + "</td>");
                        html.Append("<td><a class = 'btn btn-secondary btn-xs EditAdvanceSurrenderLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Edit advance request line'><span class = 'fa fa-pencil-alt' > </span></a> "+
                                        "<a class = 'btn btn-danger btn-xs delete_advanceSurrenderLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete staff surrender line'><span class = 'fa fa-trash' > </span></a></td>");

                    }
                    else
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>" + item.Item + "</td>");
                        html.Append("<td>" + item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>" + item.UnitOfMeasure + "</td>");
                        html.Append("<td>" + item.UnitCost + "</td>");
                        html.Append("<td>" + item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");

                        //if (item.AttachmentName != "")
                        //{
                        //    string FileName = item.AttachmentName;

                        //    string exportToPath = Uploadspath + FileName;

                        //    WebService.ExportAttachmentsToFile("StaffSurrender", AdvanceRequestHdrNo, item.No, exportToPath);

                        //    html.Append("<td>" + item.AttachmentName + " <a class = 'btn btn-danger btn-xs delete_advanceSurrenderLineAttachment' data-id=" + item.AttachmentId + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete line attachment'><span class = 'fa fa-trash' > </span></a>" +
                        //        "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a></td>");
                        //}
                        //else
                        //{
                        //    html.Append("<td></td>");
                        //}
                        html.Append("<td>" + item.AdvancedAmountLCY + "</td>");
                        html.Append("<td>" + item.SurrenderedAmountLCY + "</td>");
                        html.Append("<td></td>");
                    }

                    html.Append("</tr>");
                }
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();

            return strText;
        }
        public static string GetAdvanceSurrenderLines(string AdvanceRequestHdrNo, string status)
        {
            string response = "";

            string AdvanceRequestJSON = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffSurrender", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);


            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();
            dynamic json = JObject.Parse(GetDimensionCodesresponseString);

            double sumSurrenderedAmountLCY = 0;
            double sumAdvancedAmountLCY = 0;

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    string itemamount = item.SurrenderedAmountLCY;
                    string itemAdvanceAmt = item.AdvancedAmountLCY;
                    itemamount = itemamount.Replace(",", "");
                    itemAdvanceAmt = itemAdvanceAmt.Replace(",", "");

                    double itemAmount = 0;
                    double itemAdvAmount = 0;

                    if (itemamount != "")
                    {
                        itemAmount = Convert.ToDouble(itemamount);
                    }
                    if(itemAdvanceAmt != "")
                    {
                        itemAdvAmount = Convert.ToDouble(itemAdvanceAmt);
                    }

                    sumSurrenderedAmountLCY = sumSurrenderedAmountLCY + itemAmount;

                    sumAdvancedAmountLCY = sumAdvancedAmountLCY + itemAdvAmount;
                }
            }

            if (sumAdvancedAmountLCY < sumSurrenderedAmountLCY)
            {
                double amt = (sumAdvancedAmountLCY - sumSurrenderedAmountLCY);

                response = "<label>Balance to be refunded <b>" + string.Format("{0:N2}", amt) + "</b></label>";
            }
            else if (sumAdvancedAmountLCY > sumSurrenderedAmountLCY)
            {
                double amt = sumAdvancedAmountLCY - sumSurrenderedAmountLCY;

                response = "<label>Balance for staff to pay <b>" + string.Format("{0:N2}", amt) + "</b></label>";
            }

            return response;
        }
        public static DataTable GetAdvanceSurrenders(string status, string CreatedBy)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            string AdvanceRequestHdrNo = "";
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Expected mission start date", typeof(string));
            table.Columns.Add("Expected mission end date", typeof(string));

            if (status == "Open")
            {
                table.Columns.Add("Rejection Comment", typeof(string));
            }

            if (status == "Pending")
            {
                table.Columns.Add("Approver", typeof(string));
            }

            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests("StaffSurrender", status, AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    string DateOfRequest = NodeDateOfRequest.InnerText;

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    string DateDue = NodeDateDue.InnerText;

                    XmlNode NodeApprover = xmlSoapRequest.GetElementsByTagName("Approver")[count];
                    string Approver = NodeApprover.InnerText;

                    XmlNode NodeApprovalEntryNo = xmlSoapRequest.GetElementsByTagName("ApprovalEntryNo")[count];
                    string ApprovalEntryNo = NodeApprovalEntryNo.InnerText;

                    XmlNode NodeStaffAdvanceHeaderNo = xmlSoapRequest.GetElementsByTagName("StaffAdvanceHeaderNo")[count];
                    string StaffAdvanceHeaderNo = NodeStaffAdvanceHeaderNo.InnerText;

                    XmlNode NodeRejectionComment = xmlSoapRequest.GetElementsByTagName("RejectionComment")[count];
                    string RejectionComment = NodeRejectionComment.InnerText;

                    if (No != "")
                    {
                        if (status == "Open")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, RejectionComment,
                               "<a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance surrender'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit advance surrender'><span class = 'fa fa-paper-plane' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + " data-toggle='tooltip' title='View advance surrender'><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else if (status == "Pending")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, Approver,
                               "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel advance surrender'><span class = 'fa fa-times' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs delegate_record' data-id=" + AppFunctions.Base64Encode(ApprovalEntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delegate advance surrender'><span class = 'fa fa-fighter-jet' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs print_advancesurrender' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Advance Surrender'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + "><span class = 'fa fa-eye' data-toggle='tooltip' title='View advance surrender'> </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue,
                                "<a class = 'btn btn-secondary btn-xs print_advancesurrender' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Advance Surrender'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceSurrender.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + " data-toggle='tooltip' title='View advance surrender'><span class = 'fa fa-eye' > </span></a> ");
                        }

                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static int GetNumberOfAttachments(string AdvanceRequestHdrNo)
        {
            int count = 0;

            string str = AppFunctions.CallWebService(WebService.ExportAttachments(AdvanceRequestHdrNo, "StaffSurrender"));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Attachment"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("EntryNo")[count];
                    string No = NodeNo.InnerText;

                    if (No != "0")
                    {
                        count = count + 1;
                    }
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return count;
        }
        public static IDictionary<string, string> GetAdvanceRequestList(string status, string CreatedBy)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            string AdvanceRequestHdrNo = "";
            int count = 0;
            Dictionary<string, string> AdvanceRequestList = new Dictionary<string, string>();

            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests("StaffAdvance", status, AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    string DateOfRequest = NodeDateOfRequest.InnerText;

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    string DateDue = NodeDateDue.InnerText;

                    XmlNode NodeGlobalDimCode1 = xmlSoapRequest.GetElementsByTagName("GlobalDimCode1")[count];
                    string GlobalDimCode1 = NodeGlobalDimCode1.InnerText;

                    if (DateOfRequest != "")
                    {
                        AdvanceRequestList.Add(No, No +" -"+ DateOfRequest );                    

                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return AdvanceRequestList;
        }
        public static string CreateAdvanceSurrenderLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string ActualAmount,  string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortcutDimCode1, string shortcutDimCode2, string shortcutDimCode3, string shortcutDimCode4, string shortcutDimCode5, string shortcutDimCode6, string shortcutDimCode7, string shortcutDimCode8)
        {
            return WebService.CreateAdvanceRequestLine(AdvanceRequestHdrNo, "2", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, Remarks, Purpose, globalDimCode1, globalDimCode2, shortcutDimCode1, shortcutDimCode2, shortcutDimCode3, shortcutDimCode4, shortcutDimCode5, shortcutDimCode6, shortcutDimCode7, shortcutDimCode8);
        }
        public static string UpdateAdvanceSurrenderLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount,string ActualAmount, string LineNo, string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortCutDimCode1, string shortCutDimCode2, string shortCutDimCode3, string shortCutDimCode4, string shortCutDimCode5, string shortCutDimCode6, string shortCutDimCode7, string shortCutDimCode8)
        {
            return WebService.UpdateAdvanceRequestLine(AdvanceRequestHdrNo, "2", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, ActualAmount, Remarks, Purpose, globalDimCode1, globalDimCode2, shortCutDimCode1, shortCutDimCode2, shortCutDimCode3, shortCutDimCode4, shortCutDimCode5, shortCutDimCode6, shortCutDimCode7, shortCutDimCode8);
        }
        public static string SubmitAdvanceRequest(string AdvanceRequestHdrNo)
        {         
            string response = "";

            int NumberOfAttachments = GetNumberOfAttachments(AdvanceRequestHdrNo);

            if (NumberOfAttachments > 0)
            {
                response = WebService.SubmitAdvanceRequest("2", AdvanceRequestHdrNo);
            }
            else
            {
                var _RequestResponse = new
                {
                    Status = "999",
                    Msg = "The staff surrender header number " + AdvanceRequestHdrNo + " has no attatchment. Kindly add an attachment before proceeding"
                };

                response = JsonConvert.SerializeObject(_RequestResponse);
            }
            return response;
        }
        public static string DeleteAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DeleteAdvanceRequest("2", AdvanceRequestHdrNo);
        }
        public static string DeleteAdvanceRequestLine(string AdvanceRequestHdrNo, string LineNo)
        {
            return WebService.DeleteAdvanceRequestLine("2", AdvanceRequestHdrNo, LineNo);
        }
        public static string CancelAdvanceRequest(string AdvanceRequestHdrNo, string DocumentType)
        {
            return WebService.CancelWorkflowApprovalRequest(AdvanceRequestHdrNo, DocumentType);
        }
        public static string DelegateAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DelegateWorkflowApprovalRequest(AdvanceRequestHdrNo);
        }
        public static string DeleteAttachment(string DocumentNo)
        {
            return WebService.DeleteAttachment(DocumentNo);
        }
        public static string GetAdvanceRequestLine(string AdvanceRequestLineNo)
        {
            return WebService.GetAdvanceRequestLine("0", AdvanceRequestLineNo);
        }
        public static string GetSurrenderRequestLine(string AdvanceRequestLineNo)
        { 
            return WebService.GetAdvanceRequestLine("2", AdvanceRequestLineNo);
        }
        public static void UploadFile(string documentType, string documentNo, string fromPath, string description, string lineNo)
        {
            WebService.AttachAttachmentToRecord("AdvanceRequestLines", documentType, documentNo, fromPath, description, "0", lineNo, "0");
        }
    }
    public class StaffClaims
    {
        public static DataTable GetStaffClaimsLines(string AdvanceRequestHdrNo, string status)
        {
            string AdvanceRequestJSON = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);
            DataTable table = new DataTable();

            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Purpose", typeof(string));
            table.Columns.Add("Unit of Measure", typeof(string));
            table.Columns.Add("Unit Cost", typeof(string));
            table.Columns.Add("No. Of Units", typeof(string));
            table.Columns.Add("Amount", typeof(string));
            //table.Columns.Add("Actual Amount", typeof(string));
            table.Columns.Add("Action", typeof(string));

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    if (status == "Open")
                    {
                        table.Rows.Add(item.Item, item.ItemDescription, item.UnitOfMeasure, item.UnitCost, item.NoOfUnits, item.ClaimedAmountLCY, 
                               "<a class = 'btn btn-danger btn-xs delete_advanceRequestLines' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance request line'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs EditAdvanceReqLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Edit advance request line'><span class = 'fa fa-pencil-alt' > </span></a>");
                    }
                    else
                    {
                        table.Rows.Add(item.Item, item.ItemDescription, item.UnitOfMeasure, item.UnitCost, item.NoOfUnits, item.ClaimedAmountLCY, "");
                    }

                }
            }

            return table;
        }
        public static string GetAdvanceClaimLinesTable(string AdvanceRequestHdrNo, string status)
        {
            string Uploadspath = HttpContext.Current.Server.MapPath("~/Uploads/");

            string AdvanceRequestJSON = AdvanceRequestsXMLRequests.GetAdvanceRequests("StaffClaim", AdvanceRequestHdrNo);
            AdvanceRequestHeader bsObj = JsonConvert.DeserializeObject<AdvanceRequestHeader>(AdvanceRequestJSON);


            string GetDimensionCodesresponseString = CreateAdvanceRequestXMLRequests.GetDimensionCodes();
            dynamic json = JObject.Parse(GetDimensionCodesresponseString);
            string ShortcutDimCode3 = json.ShortcutDimension3Code;

            double sum = 0;
            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    string itemamount = item.ClaimedAmountLCY;
                    itemamount = itemamount.Replace(",", "");

                    double itemAmount = 0;

                    if (itemamount != "")
                    {
                        itemAmount = Convert.ToDouble(itemamount);
                    }

                    sum = sum + itemAmount;
                }
            }

            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
            //Building the Header row.
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th>" + ApprovalEntiesXMLRequests.SetFirstLetterToUpper(ShortcutDimCode3.ToLower()) + "</th>");
            //html.Append("<th>Item</th>");
            html.Append("<th>Item Description</th>");
            html.Append("<th>Purpose</th>");
            html.Append("<th>Unit of Measure</th>");
            html.Append("<th>Unit Cost</th>");
            html.Append("<th>No. Of Units</th>");
            html.Append("<th>Currency</th>");
            //html.Append("<th>Attachment</th>");
            html.Append("<th>Amount(LCY)</th>");
            html.Append("<th>Action</th>");
            html.Append("</tr>"); 
            html.Append("</thead>");

            html.Append("<tfoot>");
            html.Append("<tr>");
            html.Append("<th></th>");
            //html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            html.Append("<th></th>");
            //html.Append("<th></th>");
            html.Append("<th>" + string.Format("{0:N2}", sum) + "</th>");
            html.Append("<th></th>");
            html.Append("</tr>");
            html.Append("</tfoot>");

            //Building the Data rows.
            html.Append("<tbody>");

            foreach (var item in bsObj._AdvanceRequestLines)
            {
                if (item.Item != "")
                {
                    html.Append("<tr>");

                    if (status == "Open")
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>" + item.Item + "</td>");
                        html.Append("<td>" + item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>" + item.UnitOfMeasure + "</td>");
                        html.Append("<td>" + item.UnitCost + "</td>");
                        html.Append("<td>" + item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");
                        //if (item.AttachmentName != "")
                        //{
                        //    string FileName = item.AttachmentName;

                        //    string exportToPath = Uploadspath + FileName;

                        //    WebService.ExportAttachmentsToFile("StaffClaim", AdvanceRequestHdrNo, item.No, exportToPath);

                        //    html.Append("<td>" + item.AttachmentName + " <a class = 'btn btn-danger btn-xs delete_advanceClaimLineAttachment' data-id=" + item.AttachmentId + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete line attachment'><span class = 'fa fa-trash' > </span></a>" +
                        //        "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a></td>");
                        //}
                        //else
                        //{
                        //    html.Append("<td></td>");
                        //}
                        html.Append("<td>" + item.ClaimedAmountLCY + "</td>");
                        html.Append("<td><a class = 'btn btn-danger btn-xs delete_advanceRequestLines' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete advance request line'><span class = 'fa fa-trash-alt' > </span></a> " +
                                        "<a class = 'btn btn-secondary btn-xs EditAdvanceReqLine' data-id=" + item.No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Edit advance request line'><span class = 'fa fa-pencil-alt' > </span></a></td>");

                    }
                    else
                    {
                        html.Append("<td>" + item.ShortcutDimCode3 + "</td>");
                        //html.Append("<td>" + item.Item + "</td>");
                        html.Append("<td>" + item.ItemDescription + "</td>");
                        html.Append("<td>" + item.Purpose + "</td>");
                        html.Append("<td>" + item.UnitOfMeasure + "</td>");
                        html.Append("<td>" + item.UnitCost + "</td>");
                        html.Append("<td>" + item.NoOfUnits + "</td>");
                        html.Append("<td>" + item.Currency + "</td>");
                        //if (item.AttachmentName != "")
                        //{
                        //    string FileName = item.AttachmentName;

                        //    string exportToPath = Uploadspath + FileName;

                        //    WebService.ExportAttachmentsToFile("StaffClaim", AdvanceRequestHdrNo, item.No, exportToPath);

                        //    html.Append("<td>" + item.AttachmentName + " <a class = 'btn btn-danger btn-xs delete_advanceSurrenderLineAttachment' data-id=" + item.AttachmentId + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete line attachment'><span class = 'fa fa-trash' > </span></a>" +
                        //        "<a class = 'btn btn-primary btn-xs downloadfile' href = " + Uri.EscapeUriString(exportToPath) + " data-id=" + Uri.EscapeUriString(FileName) + " download><span class = 'fa fa-download' > </span></a></td>");
                        //}
                        //else
                        //{
                        //    html.Append("<td></td>");
                        //}
                        html.Append("<td>" + item.ClaimedAmountLCY + "</td>");
                        html.Append("<td></td>");
                    }

                    html.Append("</tr>");
                }
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            string strText = html.ToString();

            return strText;
        }
        public static DataTable GetStaffClaims(string status, string CreatedBy)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            string AdvanceRequestHdrNo = "";
            int count = 0;

            DataTable table = new DataTable();

            table.Columns.Add("No.", typeof(string));
            table.Columns.Add("Actual mission start date", typeof(string));
            table.Columns.Add("Actual date of return from mission", typeof(string));

            if (status == "Open")
            {
                table.Columns.Add("Rejection Comment", typeof(string));
            }

            if (status == "Pending")
            {
                table.Columns.Add("Approver", typeof(string));
            }
            if (status == "Approved")
            {
                table.Columns.Add("Date Of Payment", typeof(string));
                table.Columns.Add("Total Amount Approved", typeof(string));
            }

            table.Columns.Add("Action", typeof(string));

            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests("StaffClaim", status, AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    string DateOfRequest = NodeDateOfRequest.InnerText;

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    string DateDue = NodeDateDue.InnerText;

                    XmlNode NodeApprover = xmlSoapRequest.GetElementsByTagName("Approver")[count];
                    string Approver = NodeApprover.InnerText;

                    XmlNode NodeApprovalEntryNo = xmlSoapRequest.GetElementsByTagName("ApprovalEntryNo")[count];
                    string ApprovalEntryNo = NodeApprovalEntryNo.InnerText;

                    XmlNode NodeStaffAdvanceHeaderNo = xmlSoapRequest.GetElementsByTagName("StaffAdvanceHeaderNo")[count];
                    string StaffAdvanceHeaderNo = NodeStaffAdvanceHeaderNo.InnerText;

                    XmlNode NodeDateOfPayment = xmlSoapRequest.GetElementsByTagName("DateOfPayment")[count];
                    string DateOfPayment = NodeDateOfPayment.InnerText;

                    XmlNode NodeTotalAmountApproved = xmlSoapRequest.GetElementsByTagName("TotalAmountApproved")[count];
                    string TotalAmountApproved = NodeTotalAmountApproved.InnerText;

                    XmlNode NodeRejectionComment = xmlSoapRequest.GetElementsByTagName("RejectionComment")[count];
                    string RejectionComment = NodeRejectionComment.InnerText;

                    if (No != "")
                    {
                        if (status == "Open")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, RejectionComment,
                               "<a class = 'btn btn-danger btn-xs delete_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete Staff Claim'><span class = 'fa fa-trash-alt' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs submit_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Staff Claim'><span class = 'fa fa-paper-plane' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + " data-toggle='tooltip' title='View Staff Claim'><span class = 'fa fa-eye' > </span></a> ");
                        }
                        else if (status == "Pending")
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, Approver,
                               "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + AppFunctions.Base64Encode(No) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel Staff Claim'><span class = 'fa fa-times' > </span></a> " +
                               "<a class = 'btn btn-success btn-xs delegate_record' data-id=" + AppFunctions.Base64Encode(ApprovalEntryNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delegate Staff Claim'><span class = 'fa fa-fighter-jet' > </span></a> " +
                               "<a class = 'btn btn-secondary btn-xs print_advanceclaim' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Staff Claim'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + " data-toggle='tooltip' title='View Staff Claim'><span class = 'fa fa-eye' data-toggle='tooltip' title='View Staff Claim'> </span></a> ");
                        }
                        else
                        {
                            table.Rows.Add(No, DateOfRequest, DateDue, DateOfPayment, TotalAmountApproved,
                                "<a class = 'btn btn-secondary btn-xs print_advanceclaim' data-id=" + No + " href = 'javascript:void(0)' data-toggle='tooltip' title='Print Staff Claim'><span class = 'fa fa-print' > </span></a> " +
                               "<a class = 'btn btn-primary btn-xs' href = " + "ViewAdvanceClaim.aspx?id=" + AppFunctions.Base64Encode(No) + "&status=" + status + "&code=" + AppFunctions.Base64Encode(StaffAdvanceHeaderNo) + " data-toggle='tooltip' title='View Staff Claim'><span class = 'fa fa-eye' > </span></a> ");
                        }
                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return table;
        }
        public static int GetNumberOfAttachments(string AdvanceRequestHdrNo)
        {
            int count = 0;

            string str = AppFunctions.CallWebService(WebService.ExportAttachments(AdvanceRequestHdrNo, "StaffClaim"));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Attachment"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("EntryNo")[count];
                    string No = NodeNo.InnerText;

                    if (No != "0")
                    {
                        count = count + 1;
                    }
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return count;
        }
        public static IDictionary<string, string> GetAdvanceRequestList(string status, string CreatedBy)
        {
            string username = HttpContext.Current.Session["Username"].ToString();
            string AdvanceRequestHdrNo = "";
            int count = 0;
            Dictionary<string, string> AdvanceRequestList = new Dictionary<string, string>();

            string str = AppFunctions.CallWebService(WebService.GetAdvanceRequests("StaffClaim", status, AdvanceRequestHdrNo, username));

            XmlDocument xmlSoapRequest = new XmlDocument();

            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                xmlSoapRequest.LoadXml(str);

                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("AdvanceRequest"))
                {
                    XmlNode NodeNo = xmlSoapRequest.GetElementsByTagName("No")[count];
                    string No = NodeNo.InnerText;

                    XmlNode NodeDateOfRequest = xmlSoapRequest.GetElementsByTagName("DateOfRequest")[count];
                    string DateOfRequest = NodeDateOfRequest.InnerText;

                    XmlNode NodeDateDue = xmlSoapRequest.GetElementsByTagName("DateDue")[count];
                    string DateDue = NodeDateDue.InnerText;

                    XmlNode NodeGlobalDimCode1 = xmlSoapRequest.GetElementsByTagName("GlobalDimCode1")[count];
                    string GlobalDimCode1 = NodeGlobalDimCode1.InnerText;

                    if (DateOfRequest != "")
                    {
                        AdvanceRequestList.Add(No, No + " -" + DateOfRequest);

                    }

                    count++;
                }

            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = str;
            }

            return AdvanceRequestList;
        }
        public static string UpdateAdvanceRequest(string AdvanceRequestHdrNo, string documentType, string DateOfRequest, string DateDue, string Requester, string RequestBy, string RequestToCompany,
          string GlobalDimCode1, string GlobalDimCode2, string ShortCutDimCode1, string ShortCutDimCode2, string ShortCutDimCode3, string ShortCutDimCode4, string ShortCutDimCode5, string ShortCutDimCode6, string ShortCutDimCode7, string ShortCutDimCode8, string Currency, string staffAdvanceHeaderNo, string preferredPaymentMethod, string MissionSummary)
        {
            return WebService.UpdateAdvanceRequest(AdvanceRequestHdrNo, documentType, DateOfRequest, DateDue, Requester, RequestBy, RequestToCompany,
                                                   GlobalDimCode1, GlobalDimCode2, ShortCutDimCode1, ShortCutDimCode2, ShortCutDimCode3, ShortCutDimCode4, ShortCutDimCode5, ShortCutDimCode6, ShortCutDimCode7, ShortCutDimCode8, Currency, staffAdvanceHeaderNo, preferredPaymentMethod, MissionSummary);
        }        
        public static string CreateAdvanceClaimLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortcutDimCode1, string shortcutDimCode2, string shortcutDimCode3, string shortcutDimCode4, string shortcutDimCode5, string shortcutDimCode6, string shortcutDimCode7, string shortcutDimCode8)
        {
            return WebService.CreateAdvanceRequestLine(AdvanceRequestHdrNo, "1", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, "0", "", Purpose, globalDimCode1, globalDimCode2, shortcutDimCode1, shortcutDimCode2, shortcutDimCode3, shortcutDimCode4, shortcutDimCode5, shortcutDimCode6, shortcutDimCode7, shortcutDimCode8);
        }
        public static string UpdatAdvanceClaimLine(string AdvanceRequestHdrNo, string Item, string ItemDescription, string UnitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string LineNo, string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortCutDimCode1, string shortCutDimCode2, string shortCutDimCode3, string shortCutDimCode4, string shortCutDimCode5, string shortCutDimCode6, string shortCutDimCode7, string shortCutDimCode8)
        {
            return WebService.UpdateAdvanceRequestLine(AdvanceRequestHdrNo, "1", Item, ItemDescription, UnitOfMeasure, NoOfUnits, UnitCost, Amount, LineNo, "0", Remarks, Purpose, globalDimCode1, globalDimCode2, shortCutDimCode1, shortCutDimCode2, shortCutDimCode3, shortCutDimCode4, shortCutDimCode5, shortCutDimCode6, shortCutDimCode7, shortCutDimCode8);
        }
        public static string SubmitAdvanceRequest(string AdvanceRequestHdrNo)
        {
            string response = "";

            int NumberOfAttachments = GetNumberOfAttachments(AdvanceRequestHdrNo);

            if (NumberOfAttachments > 0)
            {
                response = WebService.SubmitAdvanceRequest("1", AdvanceRequestHdrNo);
            }
            else
            {
                var _RequestResponse = new
                {
                    Status = "999",
                    Msg = "The staff claim header number " + AdvanceRequestHdrNo + " has no attatchment. Kindly add an attachment before proceeding"
                };

                response = JsonConvert.SerializeObject(_RequestResponse);
            }
            return response;
        }
        public static string DeleteAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DeleteAdvanceRequest("1", AdvanceRequestHdrNo);
        }
        public static string DeleteAttachment(string DocumentNo)
        {
            return WebService.DeleteAttachment(DocumentNo);
        }
        public static string DeleteAdvanceRequestLine(string AdvanceRequestHdrNo, string LineNo)
        {
            return WebService.DeleteAdvanceRequestLine("1", AdvanceRequestHdrNo, LineNo);
        }
        public static string CancelAdvanceRequest(string AdvanceRequestHdrNo, string DocumentType)
        {
            return WebService.CancelWorkflowApprovalRequest(AdvanceRequestHdrNo, DocumentType);
        }
        public static string DelegateAdvanceRequest(string AdvanceRequestHdrNo)
        {
            return WebService.DelegateWorkflowApprovalRequest(AdvanceRequestHdrNo);
        }
        public static void UploadFile(string documentType, string documentNo, string fromPath, string description, string lineNo, string lineNo2)
        {
            WebService.AttachAttachmentToRecord("AdvanceRequestLines", documentType, documentNo, fromPath, description, "0", lineNo, lineNo2);
        }
        public static string GetAdvanceRequestLine(string AdvanceRequestLineNo)
        {
            return WebService.GetAdvanceRequestLine("1", AdvanceRequestLineNo);
        }
    }
}
