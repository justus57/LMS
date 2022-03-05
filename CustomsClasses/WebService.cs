using System;
using System.Linq;

namespace LMS.CustomsClasses
{
    class WebService
    {
        /**
        * Approval request functions
        * Functions return JSON response
        */
        /********************************************************** APPROVAL REQUESTS ***********************************************************/

        public static string ApproveApprovalRequest(string DocumentNo, string Username, string DocumentArea)
        {
            return WebserviceConfig.ObjNav.ApproveApprovalRequest("Document", DocumentArea, DocumentNo, Username, 0);
        }

        public static string RejectApprovalRequest(string DocumentNo, string Username, string DocumentArea)
        {
            return WebserviceConfig.ObjNav.RejectApprovalRequest("Document", DocumentArea, DocumentNo, Username, 0);
        }
        public static string CancelApprovalRequest(string DocumentNo, string DocumentArea)
        {
            return WebserviceConfig.ObjNav.CancelApprovalRequest(DocumentArea, DocumentNo);
        }
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/
        /********************************************************** LEAVES ***********************************************************/

        public static string GetLeaveList(string startRecord, string noOfRecords, string employeeNo, string requestAs, string approvalStatus, string leaveSubType, string totalRecords)
        {
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
                                  <startRecord>" + startRecord + @"</startRecord>
                                  <noOfRecords>" + noOfRecords + @"</noOfRecords>
                                  <employeeNo>" + employeeNo + @"</employeeNo>
                                  <requestAs>" + requestAs + @"</requestAs>
                                  <approvalStatus>" + approvalStatus + @"</approvalStatus>
                                  <leaveSubType>" + leaveSubType + @"</leaveSubType>
                                  <totalRecords>" + totalRecords + @"</totalRecords>
                              </GetLeaveList>
                          </Body>
                      </Envelope>";
            var str = Assest.Utility.CallWebService(req);
           
            return str;
        }
        public static string GetLeaveDetail(string documentNo, string employeeNo, string operation, string LeaveSubType)
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
                                    <LeaveSubType>" + LeaveSubType + @"</LeaveSubType>
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
                            <documentNo>" + documentNo + @"</documentNo>
                            <employeeNo>" + employeeNo + @"</employeeNo>
                            <operation>" + operation + @"</operation>
                        </GetLeaveDetail>
                    </Body>
                </Envelope>";
            return Assest.Utility.CallWebService(req);
        }

        public static string SaveRejectionComment(string DocumentNo, string Username, string RejectionComment, string DocumentArea)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <SaveRejectionComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                                    <calledFrom>Document</calledFrom>
                                                    <documentArea>" + DocumentArea + @"</documentArea>
                                                    <documentNo>" + DocumentNo + @"</documentNo>
                                                    <employeeNo>" + Username + @"</employeeNo>
                                                    <sequenceNo>0</sequenceNo>
                                                    <rejectionComment>" + RejectionComment + @"</rejectionComment>
                                                </SaveRejectionComment>
                                            </Body>
                                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }

        public static string SubmitAppraisal(string AppraisalHeaderNumber, string EmployeeNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SubmitAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalNo>" + AppraisalHeaderNumber + @"</appraisalNo>
                                    <employeeNo>" + EmployeeNumber + @"</employeeNo>
                                </SubmitAppraisal>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }

        public static string ChangeEmployeePassword(string username, string oldpass, string newpass)
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
        public static string GetNewAppraisals(string status, string employeeNo, string appraisalHeaderNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetNewAppraisals xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeader>
                                        <Appraisal xmlns=""urn:microsoft-dynamics-nav/xmlports/x50088"">
                                            <AppraisalNo>[string]</AppraisalNo>
                                            <Description>[string]</Description>
                                            <ValidFrom>[date]</ValidFrom>
                                            <ValidTo>[date]</ValidTo>
                                            <ApplicableTo>[string]</ApplicableTo>
                                            <Status>[string]</Status>
                                        </Appraisal>
                                    </appraisalHeader>
                                    <status>" + status + @"</status>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <appraisalHeaderNo>" + appraisalHeaderNo + @"</appraisalHeaderNo>
                                </GetNewAppraisals>
                            </Body>
                        </Envelope>";

            return req;
        }
        public static string GetCompanies()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetCompanies xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <companies>
                                        <Company xmlns=""urn:microsoft-dynamics-nav/xmlports/x50099"">
                                            <Name>[string]</Name>
                                        </Company>
                                    </companies>
                                </GetCompanies>
                            </Body>
                        </Envelope>";
            return req;
        }
        public static string GetCurrencies()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetCurrencies xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <currencies>
                                        <Currency xmlns=""urn:microsoft-dynamics-nav/xmlports/x50085"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </Currency>
                                    </currencies>
                                </GetCurrencies>
                            </Body>
                        </Envelope>";
            return req;
        }
        public static string GetAdvanceRequestNewNo(string documentType, string RegionCode)
        {
            return WebserviceConfig.ObjNav.GetAdvanceRequestNewNo(documentType, RegionCode);
        }
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        /********************************************************** REPORTS ***********************************************************/
        public static string ExportPayslip(string EmployeeNo, string Path, string Date)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportPayslip xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + EmployeeNo + @"</employeeNo>
                                    <period>" + Date + @"</period>
                                    <exportToPath>" + Path + @"</exportToPath>
                                </ExportPayslip>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string ExportP9(string EmployeeNo, string Path, string Year)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportP9 xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <year>" + Year + @"</year>
                                    <employeeNo>" + EmployeeNo + @"</employeeNo>
                                    <exportToPath>" + Path + @"</exportToPath>
                                </ExportP9>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string ExportStaffAdvance(string EmployeeNo, string Path)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportStaffAdvance xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <employeeNo>" + EmployeeNo + @"</employeeNo>
                                    <exportToPath>" + Path + @"</exportToPath>
                                </ExportStaffAdvance>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string ExportAdvanceRequestReport(string documentType, string documentNumber, string exportToPath)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportAdvanceRequestReport xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + documentType + @"</documentType>
                                    <documentNumber>" + documentNumber + @"</documentNumber>
                                    <exportToPath>" + exportToPath + @"</exportToPath>
                                </ExportAdvanceRequestReport>
                            </Body>
                        </Envelope>";
            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/
        /********************************************************** STAFF ADVANCE ***********************************************************/

        public static string GetDimensionCodes()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetDimensionCodes xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal""/>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string ExportDimensionCodeValues(string DimCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportDimensionCodeValues xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <dimCode>" + DimCode + @"</dimCode>
                                    <exportDimCodeValues>
                                        <DimCodeValue xmlns=""urn:microsoft-dynamics-nav/xmlports/x50086"">
                                            <Code>[string]</Code>
                                            <Name>[string]</Name>
                                        </DimCodeValue>
                                    </exportDimCodeValues>
                                </ExportDimensionCodeValues>
                            </Body>
                        </Envelope>";
            return req;
        }
        public static string UpdateAdvanceRequest(string AdvanceRequestHdrNo, string documentType, string DateOfRequest, string DateDue, string Requester, string RequestBy, string RequestToCompany,
            string GlobalDimCode1, string GlobalDimCode2, string ShortCutDimCode1, string ShortCutDimCode2, string ShortCutDimCode3, string ShortCutDimCode4, string ShortCutDimCode5, string ShortCutDimCode6, string ShortCutDimCode7, string ShortCutDimCode8, string Currency, string staffAdvanceHeaderNo, string preferredPaymentMethod, string MissionSummary)
        {
            return WebserviceConfig.ObjNav.UpdateAdvanceRequest(AdvanceRequestHdrNo, documentType, DateOfRequest, DateDue, Requester, RequestBy, RequestToCompany, GlobalDimCode1, GlobalDimCode2, ShortCutDimCode1, ShortCutDimCode2, ShortCutDimCode3, ShortCutDimCode4, ShortCutDimCode5, ShortCutDimCode6, ShortCutDimCode7, ShortCutDimCode8, Currency, staffAdvanceHeaderNo, Convert.ToInt32(preferredPaymentMethod), MissionSummary);
        }
        public static string CreateAdvanceRequestLine(string AdvanceRequestHdrNo, string DocumentType, string Item, string ItemDescription, string unitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string ActualAmount, string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortcutDimCode1, string shortcutDimCode2, string shortcutDimCode3, string shortcutDimCode4, string shortcutDimCode5, string shortcutDimCode6, string shortcutDimCode7, string shortcutDimCode8)
        {
            return WebserviceConfig.ObjNav.CreateAdvanceRequestLine(AdvanceRequestHdrNo, DocumentType, Item, Purpose, unitOfMeasure, NoOfUnits, UnitCost, Amount, ActualAmount, Remarks, ItemDescription,
                globalDimCode1, globalDimCode2, shortcutDimCode1, shortcutDimCode2, shortcutDimCode3, shortcutDimCode4, shortcutDimCode5, shortcutDimCode6, shortcutDimCode7, shortcutDimCode8);

        }
        public static void AttachFileToRecord(string documentArea, string documentNo, string fromPath, string description)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <AttachFileToRecord xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentArea>" + documentArea + @"</documentArea>
                                    <documentNo>" + documentNo + @"</documentNo>
                                    <fromPath>" + fromPath + @"</fromPath>
                                    <description>" + description + @"</description>
                                </AttachFileToRecord>
                            </Body>
                        </Envelope>";

            Assest.Utility.CallWebService(req);
        }
        public static string GetAdvanceRequests(string documentArea, string Status, string AdvanceRequestHdrNo, string Requester)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <GetAdvanceRequests xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <exportAdvanceRequests>
                                    <AdvanceRequest xmlns=""urn:microsoft-dynamics-nav/xmlports/x50081"">
                                        <No>[string]</No>
                                        <DateOfRequest>[date]</DateOfRequest>
                                        <Requester>[string]</Requester>
                                        <DateDue>[date]</DateDue>
                                        <GlobalDimCode1>[string]</GlobalDimCode1>
                                        <GlobalDimCode2>[string]</GlobalDimCode2>
                                        <ShortcutDimCode1>[string]</ShortcutDimCode1>
                                        <ShortcutDimCode2>[string]</ShortcutDimCode2>
                                        <ShortcutDimCode3>[string]</ShortcutDimCode3>
                                        <ShortcutDimCode4>[string]</ShortcutDimCode4>
                                        <ShortcutDimCode5>[string]</ShortcutDimCode5>
                                        <ShortcutDimCode6>[string]</ShortcutDimCode6>
                                        <ShortcutDimCode7>[string]</ShortcutDimCode7>
                                        <ShortcutDimCode8>[string]</ShortcutDimCode8>
                                        <StaffAdvanceHeaderNo>[string]</StaffAdvanceHeaderNo>
                                        <Balance>[string]</Balance>
                                        <Approver>[string]</Approver>
                                        <ApprovalEntryNo>[string]</ApprovalEntryNo>
                                        <DocumentType>[string]</DocumentType>
                                        <AdvanceRequestLines>
                                            <AdvanceRequestLineNo>[int]</AdvanceRequestLineNo>
                                            <AdvanceRequestHdrNo>[string]</AdvanceRequestHdrNo>
                                            <Item>[string]</Item>
                                            <Purpose>[string]</Purpose>
                                            <NoOfUnits>[decimal]</NoOfUnits>
                                            <UnitCost>[decimal]</UnitCost>
                                            <Amount>[decimal]</Amount>
                                            <ActualAmount>[decimal]</ActualAmount>
                                            <AmountLCY>[decimal]</AmountLCY>
                                            <Currency>[string]</Currency>
                                        </AdvanceRequestLines>
                                    </AdvanceRequest>
                                </exportAdvanceRequests>
                                <documentArea>" + documentArea + @"</documentArea>
                                <status>" + Status + @"</status>
                                <advanceRequestHdrNo>" + AdvanceRequestHdrNo + @"</advanceRequestHdrNo>
                                <requester>" + Requester + @"</requester>
                            </GetAdvanceRequests>
                        </Body>
                    </Envelope>";
            return req;
        }
        public static string DeleteAdvanceRequest(string documentType, string AdvanceRequestHdrNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAdvanceRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + documentType + @"</documentType>
                                    <advanceRequestHdrNo>" + AdvanceRequestHdrNo + @"</advanceRequestHdrNo>
                                </DeleteAdvanceRequest>
                            </Body>
                        </Envelope>";

            string str = Assest.Utility.CallWebService(req);
            return Assest.Utility.GetJSONResponse(str);
        }
        public static string DeleteAdvanceRequestLine(string documentType, string AdvanceRequestHdrNo, string LineNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAdvanceRequestLine xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + documentType + @"</documentType>
                                    <advanceRequestHdrNo>" + AdvanceRequestHdrNo + @"</advanceRequestHdrNo>
                                    <lineNo>" + LineNo + @"</lineNo>
                                </DeleteAdvanceRequestLine>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string SubmitAdvanceRequest(string documentType, string advanceRequestHdrNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SubmitAdvanceRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + documentType + @"</documentType>
                                    <advanceRequestHdrNo>" + advanceRequestHdrNo + @"</advanceRequestHdrNo>
                                </SubmitAdvanceRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string ExportAttachments(string documentNo, string documentArea)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportAttachments xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <exportAttachments>
                                        <Attachment xmlns=""urn:microsoft-dynamics-nav/xmlports/x50045"">
                                            <EntryNo>[int]</EntryNo>
                                            <FileName>[string]</FileName>
                                            <Desription>[string]</Desription>
                                        </Attachment>
                                    </exportAttachments>
                                    <documentNo>" + documentNo + @"</documentNo>
                                    <documentArea>" + documentArea + @"</documentArea>
                                    <lineNo>0</lineNo>
                                </ExportAttachments>
                            </Body>
                        </Envelope>";
            return req;
        }
        public static string GetAdvanceRequestLine(string documentType, string advanceRequestLineNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAdvanceRequestLine xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + documentType + @"</documentType>
                                    <advanceRequestLineNo>" + advanceRequestLineNo + @"</advanceRequestLineNo>
                                </GetAdvanceRequestLine>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string UpdateAdvanceRequestLine(string AdvanceRequestHdrNo, string DocumentType, string Item, string ItemDescription, string unitOfMeasure, string NoOfUnits, string UnitCost, string Amount, string lineNo, string ActualAmount, string Remarks, string Purpose,
            string globalDimCode1, string globalDimCode2, string shortCutDimCode1, string shortCutDimCode2, string shortCutDimCode3, string shortCutDimCode4, string shortCutDimCode5, string shortCutDimCode6, string shortCutDimCode7, string shortCutDimCode8
            )
        {
            //string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
            //                <Body>
            //                    <UpdateAdvanceRequestLine xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
            //                        <advanceRequestHdrNo>"+ AdvanceRequestHdrNo + @"</advanceRequestHdrNo>
            //                        <documentType>"+ DocumentType + @"</documentType>
            //                        <item>" + Item + @"</item>
            //                        <purpose>"+ Purpose + @"</purpose>
            //                        <unitOfMeasure>"+ unitOfMeasure + @"</unitOfMeasure>
            //                        <noOfUnits>"+ NoOfUnits + @"</noOfUnits>
            //                        <unitCost>"+ UnitCost + @"</unitCost>
            //                        <amount>"+ Amount + @"</amount>
            //                        <lineNo>"+ lineNo + @"</lineNo>
            //                        <actualAmount>"+ ActualAmount + @"</actualAmount>
            //                        <shortcutDimension3Code>"+ ShortcustDimensionCode3 + @"</shortcutDimension3Code>
            //                        <remarks>"+ Remarks + @"</remarks>
            //                        <itemDescription>"+ ItemDescription + @"</itemDescription>
            //                    </UpdateAdvanceRequestLine>
            //                </Body>
            //            </Envelope>";

            return WebserviceConfig.ObjNav.UpdateAdvanceRequestLine(AdvanceRequestHdrNo, DocumentType, Item, Purpose, unitOfMeasure, NoOfUnits, UnitCost, Amount, Convert.ToInt16(lineNo), ActualAmount
              , Remarks, ItemDescription, globalDimCode1, globalDimCode2, shortCutDimCode1, shortCutDimCode2, shortCutDimCode3, shortCutDimCode4, shortCutDimCode5, shortCutDimCode6, shortCutDimCode7, shortCutDimCode8);

        }
        public static string DeleteAttachment(string DocumentNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAttachment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentNo>" + DocumentNo + @"</documentNo>
                                </DeleteAttachment>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string ExportAdvanceTypes()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportAdvanceTypes xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <exportAdvanceTypesXMLPort>
                                        <AdvanceType xmlns=""urn:microsoft-dynamics-nav/xmlports/x50046"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </AdvanceType>
                                    </exportAdvanceTypesXMLPort>
                                </ExportAdvanceTypes>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static void AttachAttachmentToRecord(string documentArea, string documentType, string documentNo, string fromPath, string description,
            string tableId, string lineNo, string lineNo2)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <Body>
                            <AttachAttachmentToRecord xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                <documentArea>" + documentArea + @"</documentArea>
                                <documentType>" + documentType + @"</documentType>
                                <documentNo>" + documentNo + @"</documentNo>
                                <fromPath>" + fromPath + @"</fromPath>
                                <description>" + description + @"</description>
                                <tableId>" + tableId + @"</tableId>
                                <lineNo>" + lineNo + @"</lineNo>
                                <lineNo2>" + lineNo2 + @"</lineNo2>
                            </AttachAttachmentToRecord>
                        </Body>
                    </Envelope>";
            AppFunctions.CallWebService(req);
        }
        public static void ExportAttachmentsToFile(string documentArea, string documentNo, string lineNo, string exportToPath)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportAttachmentsToFile xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentArea>" + documentArea + @"</documentArea>
                                    <tableId>52018772</tableId>
                                    <documentType>0</documentType>
                                    <documentNo>" + documentNo + @"</documentNo>
                                    <lineNo>" + lineNo + @"</lineNo>
                                    <lineNo2>" + lineNo + @"</lineNo2>
                                    <exportToPath>" + exportToPath + @"</exportToPath>
                                </ExportAttachmentsToFile>
                            </Body>
                        </Envelope>";
            AppFunctions.CallWebService(req);
        }
        public static string GetApprovalEntries(string documentArea, string status, string WorkflowApprovalUserName)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetApprovalEntries xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <exportApprovalEntries>
                                        <ApprovalEntry xmlns=""urn:microsoft-dynamics-nav/xmlports/x50047"">
                                            <EntryNo>[int]</EntryNo>
                                            <TableId>[int]</TableId>
                                            <DocumentType>[string]</DocumentType>
                                            <Status>[string]</Status>
                                            <SenderId>[string]</SenderId>
                                            <DateTimeSentForApproval>[string]</DateTimeSentForApproval>
                                            <DueDate>[date]</DueDate>
                                            <Approver>[string]</Approver>
                                            <EmployeeName>[string]</EmployeeName>
                                            <TotalAmount>[string]</TotalAmount>
                                            <DocumentNo>[string]</DocumentNo>
                                            <ShortcutDimCode1>[string]</ShortcutDimCode1>
                                            <ShortcutDimCode2>[string]</ShortcutDimCode2>
                                            <ShortcutDimCode3>[string]</ShortcutDimCode3>
                                            <StaffAdvanceHeaderNumber>[string]</StaffAdvanceHeaderNumber>
                                        </ApprovalEntry>
                                    </exportApprovalEntries>
                                    <documentArea>" + documentArea + @"</documentArea>
                                    <status>" + status + @"</status>
                                    <employeeNo>" + WorkflowApprovalUserName + @"</employeeNo>
                                </GetApprovalEntries>
                            </Body>
                        </Envelope>";


            return AppFunctions.CallWebService(req);
        }
        public static string ApproveWorkflowApprovalRequest(string EntryNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ApproveWorkflowApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <approvalEntryNo>" + EntryNo + @"</approvalEntryNo>
                                </ApproveWorkflowApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string RejectWorkflowApprovalRequest(string EntryNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <RejectWorkflowApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <approvalEntryNo>" + EntryNo + @"</approvalEntryNo>
                                </RejectWorkflowApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DelegateWorkflowApprovalRequest(string EntryNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DelegateWorkflowApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <approvalEntryNo>" + EntryNo + @"</approvalEntryNo>
                                </DelegateWorkflowApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CancelWorkflowApprovalRequest(string EntryNo, string DocumentType)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CancelWorkflowApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + DocumentType + @"</documentType>
                                    <documentNumber>" + EntryNo + @"</documentNumber>
                                </CancelWorkflowApprovalRequest>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string ExportUnitOfMeasure()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ExportUnitOfMeasure xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <exportUnitsofMeasure>
                                        <Unit xmlns=""urn:microsoft-dynamics-nav/xmlports/x50062"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </Unit>
                                    </exportUnitsofMeasure>
                                </ExportUnitOfMeasure>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetAdvanceRequestType(string Code)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAdvanceRequestType xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <code>" + Code + @"</code>
                                </GetAdvanceRequestType>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string ConfirmEmployeePassword(string username, string password)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ConfirmEmployeePassword xmlns = ""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                     <sESANo>" + username + @"</sESANo>
                                     <prPassword>" + password + @"</prPassword>
                                 </ConfirmEmployeePassword >
                             </Body>
                         </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }

        public static string ValidateDimensionValueCode(string code)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ValidateDimensionValueCode xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <code>" + code + @"</code>
                                </ValidateDimensionValueCode>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }

        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        /********************************************************** APPRAISALS ***********************************************************/
        public static string CreateAppraisal(string headerNo, string username, string name, string applicableTo, string startDate, string endDate)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <headerNo>" + headerNo + @"</headerNo>
                                    <name>" + name + @"</name>
                                    <applicableTo>" + applicableTo + @"</applicableTo>
                                    <startDate>" + startDate + @"</startDate>
                                    <endDate>" + endDate + @"</endDate>
                                    <createdBy>" + username + @"</createdBy>
                                </CreateAppraisal>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetAppraisalSections(string action)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalSections xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalSections>
                                        <AppraisalSection xmlns=""urn:microsoft-dynamics-nav/xmlports/x50091"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                            <IsHRDefined>[boolean]</IsHRDefined>
                                        </AppraisalSection>
                                    </appraisalSections>
                                    <action>" + action + @"</action>
                                    <appraisalSectionNo>[string]</appraisalSectionNo>
                                </GetAppraisalSections>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string UpdateAppraisal(string headerNo, string username, string name, string applicableTo, string startDate, string endDate)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <UpdateAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <headerNo>" + headerNo + @"</headerNo>
                                    <name>" + name + @"</name>
                                    <applicableTo>" + applicableTo + @"</applicableTo>
                                    <startDate>" + startDate + @"</startDate>
                                    <endDate>" + endDate + @"</endDate>
                                    <createdBy>" + username + @"</createdBy>
                                </UpdateAppraisal>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateAppraisalMembersList(string appraisalNo, string applicableToPersons)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisalMembersList xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalNo>" + appraisalNo + @"</appraisalNo>
                                    <applicableToPersons>" + applicableToPersons + @"</applicableToPersons>
                                </CreateAppraisalMembersList>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetAppraisalSectionComment(string appraisalSection, string appraisalHeaderNo, string employeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalSectionComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                    <appraisalHeaderNo>" + appraisalHeaderNo + @"</appraisalHeaderNo>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <appraisalSectionComment>
                                        <SectionComment xmlns=""urn:microsoft-dynamics-nav/xmlports/x50094"">
                                            <SectionCommentNo>[string]</SectionCommentNo>
                                            <AppraisalHeaderNo>[string]</AppraisalHeaderNo>
                                            <SectionNo>[string]</SectionNo>
                                            <EmployeeNo>[string]</EmployeeNo>
                                            <HREmployeeNo>[string]</HREmployeeNo>
                                            <HRComment>[string]</HRComment>
                                        </SectionComment>
                                    </appraisalSectionComment>
                                </GetAppraisalSectionComment>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetAppraisalNewNo(string AppraisalDocument)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + AppraisalDocument + @"</documentType>
                                </GetAppraisalNewNo>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateEmployeeAppraisalHeader(string appraisalHeaderNumber, string employeeNumber, string employeeAppraisalHeaderNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateEmployeeAppraisalHeader xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <employeeAppraisalHeaderNo>" + employeeAppraisalHeaderNo + @"</employeeAppraisalHeaderNo>
                                </CreateEmployeeAppraisalHeader>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetFilledAppraisals(string status, string employeeNumber, string requestAs)
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

            return AppFunctions.CallWebService(req);
        }
        public static string GetFilledAppraisals(string employeeNumber, string appraisalHeaderNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetFilledAppraisals xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                        <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                        <requestAs>Viewer</requestAs>
                                        <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                        <status>Single</status>
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
                                            </FilledAppraisal>
                                        </filledAppraisals>
                                    </GetFilledAppraisals>
                                </Body>
                            </Envelope>";

            return AppFunctions.CallWebService(req);
        }
        public static string GetAppraisalTargetObjectives(string appraisalHeaderNumber, string appraisalTargetNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalTargetObjectives xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetObjectives>
                                        <AppraisalTargetObjective xmlns=""urn:microsoft-dynamics-nav/xmlports/x50080"">
                                            <AppraisalHeaderNo>[string]</AppraisalHeaderNo>
                                            <AppraisalTargetNo>[string]</AppraisalTargetNo>
                                            <TargetObjectiveNo>[string]</TargetObjectiveNo>
                                        </AppraisalTargetObjective>
                                    </appraisalTargetObjectives>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <appraisalTargetNumber>" + appraisalTargetNumber + @"</appraisalTargetNumber>
                                </GetAppraisalTargetObjectives>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetPerformanceMeasurementLevels()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetPerformanceMeasurementLevels xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <performnceMeasurementLevels>
                                        <PerformanceMeasurementLevel xmlns=""urn:microsoft-dynamics-nav/xmlports/x50098"">
                                            <No>[string]</No>
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </PerformanceMeasurementLevel>
                                    </performnceMeasurementLevels>
                                    <action>ExportAll</action>
                                    <pMLNo>[string]</pMLNo>
                                </GetPerformanceMeasurementLevels>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetAppraisalTarget(string appraisalHeaderNumber, string appraisalTargetNo, string operation, string appraisalSection)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTarget>
                                        <AppraisalTarget xmlns=""urn:microsoft-dynamics-nav/xmlports/x50087"">
                                            <AppraisalTargetCode>[string]</AppraisalTargetCode>
                                            <TargetDescription>[string]</TargetDescription>
                                            <PerformanceMeasurementType>[string]</PerformanceMeasurementType>
                                            <WeightedScore>[decimal]</WeightedScore>
                                        </AppraisalTarget>
                                    </appraisalTarget>
                                    <appraisalHeaderNo>" + appraisalHeaderNumber + @"</appraisalHeaderNo>
                                    <appraisalTargetNo>" + appraisalTargetNo + @"</appraisalTargetNo>
                                    <operation>" + operation + @"</operation>
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                </GetAppraisalTarget>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string AppraisalResponses(string emloyeeAppraisalHeaderNo, string employeeNumber, string appraisalHeaderNumber, string TargetNumber, string Choice, string Description, string WeightedScore)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <AppraisalResponses xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <operation>Import</operation>
                                    <appraisalResponses>
                                        <AppraisalResponse xmlns=""urn:microsoft-dynamics-nav/xmlports/x50082"">
                                            <ApraisalHeaderNumber>[string]</ApraisalHeaderNumber>
                                            <QuestionNumber>[string]</QuestionNumber>
                                            <Choice>[string]</Choice>
                                            <WeightedScore>[int]</WeightedScore>
                                            <Description>[string]</Description>
                                            <No>[int]</No>
                                            <EmployeeNumber>[string]</EmployeeNumber>
                                            <Supervisor>[string]</Supervisor>
                                            <SupervisorComment>[string]</SupervisorComment>
                                        </AppraisalResponse>
                                    </appraisalResponses>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <targetNumber>" + TargetNumber + @"</targetNumber>
                                    <choice>" + Choice + @"</choice>
                                    <weightedScore>" + WeightedScore + @"</weightedScore>
                                    <description>" + Description + @"</description>
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <supervisor></supervisor>
                                    <supervisorComment></supervisorComment>
                                    <emloyeeAppraisalHeaderNo>" + emloyeeAppraisalHeaderNo + @"</emloyeeAppraisalHeaderNo>
                                </AppraisalResponses>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string AppraisalResponses(string TargetNumber, string appraisalHeaderNumber, string employeeNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <AppraisalResponses xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <operation>Export</operation>
                                    <appraisalResponses>
                                        <AppraisalResponse xmlns=""urn:microsoft-dynamics-nav/xmlports/x50082"">
                                            <ApraisalHeaderNumber>[string]</ApraisalHeaderNumber>
                                            <QuestionNumber>[string]</QuestionNumber>
                                            <Choice>[string]</Choice>
                                            <WeightedScore>[int]</WeightedScore>
                                            <Description>[string]</Description>
                                            <No>[int]</No>
                                            <EmployeeNumber>[string]</EmployeeNumber>
                                            <Supervisor>[string]</Supervisor>
                                            <SupervisorComment>[string]</SupervisorComment>
                                        </AppraisalResponse>
                                    </appraisalResponses>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <targetNumber>" + TargetNumber + @"</targetNumber>
                                    <choice>[string]</choice>
                                    <weightedScore>[string]</weightedScore>
                                    <description>[string]</description>
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <supervisor>[string]</supervisor>
                                    <supervisorComment>[string]</supervisorComment>
                                    <emloyeeAppraisalHeaderNo>[string]</emloyeeAppraisalHeaderNo>
                                </AppraisalResponses>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string SubmitSupervisorComment(string QuestionNumber, string Comment, string employeeNumber, string appraisalHeaderNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SubmitSupervisorComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <questionNumber>" + QuestionNumber + @"</questionNumber>
                                    <comment>" + Comment + @"</comment>
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                </SubmitSupervisorComment>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string UpdateAppraisalResponses(string apraisalHeaderNumber, string questionNumber, string employeeNo, string choice, string description, string weightedscore)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <UpdateAppraisalResponses xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <apraisalHeaderNumber>" + apraisalHeaderNumber + @"</apraisalHeaderNumber>
                                    <questionNumber>" + questionNumber + @"</questionNumber>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <choice>" + choice + @"</choice>
                                    <description>" + description + @"</description>
                                    <weightedscore>" + weightedscore + @"</weightedscore>
                                </UpdateAppraisalResponses>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string SaveHRComment(string appraisalHeaderNumber, string employeeNumber, string comment, string action)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SaveHRComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <employeeNumber>" + employeeNumber + @"</employeeNumber>
                                    <comment>" + comment + @"</comment>
                                    <action>" + action + @"</action>
                                </SaveHRComment>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string SendAppraisalToHR(string empNo, string appraisalHdrNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SendAppraisalToHR xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <empNo>" + empNo + @"</empNo>
                                    <appraisalHdrNo>" + appraisalHdrNo + @"</appraisalHdrNo>
                                </SendAppraisalToHR>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CloseAppraisal(string empNo, string appraisalHdrNo, string hREmpNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CloseAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <empNo>" + empNo + @"</empNo>
                                    <appraisalHdrNo>" + appraisalHdrNo + @"</appraisalHdrNo>
                                    <hREmpNo>" + hREmpNo + @"</hREmpNo>
                                </CloseAppraisal>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateAppraisalSectionComment(string sectionCommentNo, string appraisalHeaderNo, string employeeNo, string hREmployeeNo, string appraisalSectionNo, string hRComment)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisalSectionComment xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeaderNo>" + appraisalHeaderNo + @"</appraisalHeaderNo>
                                    <employeeNo>" + employeeNo + @"</employeeNo>
                                    <hREmployeeNo>" + hREmployeeNo + @"</hREmployeeNo>
                                    <appraisalSectionNo>" + appraisalSectionNo + @"</appraisalSectionNo>
                                    <hRComment>" + hRComment + @"</hRComment>
                                    <sectionCommentNo>" + sectionCommentNo + @"</sectionCommentNo>
                                </CreateAppraisalSectionComment>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetAppraisalMemberList(string appraisalHeaderNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalMemberList xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalMember>
                                        <AppraisalMember xmlns=""urn:microsoft-dynamics-nav/xmlports/x50095"">
                                            <AppraisalListNo>[string]</AppraisalListNo>
                                            <AppraisalNo>[string]</AppraisalNo>
                                            <ApplicableToPerson>[string]</ApplicableToPerson>
                                        </AppraisalMember>
                                    </appraisalMember>
                                    <appraisalHeaderNo>" + appraisalHeaderNo + @"</appraisalHeaderNo>
                                </GetAppraisalMemberList>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }






        public static string GetHRPositions()
        {

            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetHRPositions xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <hRPosition>
                                        <HRPosition xmlns=""urn:microsoft-dynamics-nav/xmlports/x50089"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </HRPosition>
                                    </hRPosition>
                                </GetHRPositions>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string GetOrgUnits()
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetOrgUnits xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <xMLHROrgUnit>
                                        <OrgUnit xmlns=""urn:microsoft-dynamics-nav/xmlports/x50090"">
                                            <Code>[string]</Code>
                                            <Name>[string]</Name>
                                        </OrgUnit>
                                    </xMLHROrgUnit>
                                </GetOrgUnits>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string DeleteAppraisal(string AppraisalHeaderNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeaderNo>" + AppraisalHeaderNumber + @"</appraisalHeaderNo>
                                </DeleteAppraisal>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string ReleaseAppraisal(string AppraisalHeaderNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <ReleaseAppraisal xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalHeaderNo>" + AppraisalHeaderNumber + @"</appraisalHeaderNo>
                                </ReleaseAppraisal>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string UpdateAppraisalTarget(string appraisalTargetNo, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightedScore, string appraisalSection, string createdBy)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <UpdateAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetNo>" + appraisalTargetNo + @"</appraisalTargetNo>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <description>" + description + @"</description>
                                    <performanceMeasurementType>" + performanceMeasurementType + @"</performanceMeasurementType>
                                    <weightedScore>" + weightedScore + @"</weightedScore>
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                    <createdBy>" + createdBy + @"</createdBy>
                                </UpdateAppraisalTarget>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteAppraisalTargetObjectives(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAppraisalTargetObjectives xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetNo>" + AppraisalTargetNo + @"</appraisalTargetNo>
                                    <appraisalHeaderNumber>" + AppraisalHeaderNo + @"</appraisalHeaderNumber>
                                </DeleteAppraisalTargetObjectives>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateAppraisalTarget(string no, string createdBy, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightScoreValue, string appraisalSection)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <no>" + no + @"</no>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <description>" + description + @"</description>
                                    <performanceMeasurementType>" + performanceMeasurementType + @"</performanceMeasurementType>
                                    <weightedScore>" + weightScoreValue + @"</weightedScore>
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                    <createdBy>" + createdBy + @"</createdBy>
                                </CreateAppraisalTarget>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateAppraisalTargetObjective(string createdBy, string appraisalHeaderNo, string no, string appraisalTargetNumber, string description)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisalTargetObjective xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <no>" + no + @"</no>
                                    <appraisalTargetNumber>" + appraisalTargetNumber + @"</appraisalTargetNumber>
                                    <description>" + description + @"</description>
                                    <appraisalHeaderNo>" + appraisalHeaderNo + @"</appraisalHeaderNo>
                                    <createdBy>" + createdBy + @"</createdBy>
                                </CreateAppraisalTargetObjective>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteAppraisalTarget(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetNo>" + AppraisalTargetNo + @"</appraisalTargetNo>
                                    <appraisalHeaderNo>" + AppraisalHeaderNo + @"</appraisalHeaderNo>
                                </DeleteAppraisalTarget>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteHRAppraisalTarget(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteHRAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetNo>" + AppraisalTargetNo + @"</appraisalTargetNo>
                                    <appraisalHeaderNo>" + AppraisalHeaderNo + @"</appraisalHeaderNo>
                                </DeleteHRAppraisalTarget>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteHRAppraisalTargetObjectives(string AppraisalHeaderNo, string AppraisalTargetNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteHRAppraisalTargetObjectives xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetNo>" + AppraisalTargetNo + @"</appraisalTargetNo>
                                    <appraisalHeaderNumber>" + AppraisalHeaderNo + @"</appraisalHeaderNumber>
                                </DeleteHRAppraisalTargetObjectives>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateHRAppraisalTarget(string no, string username, string appraisalHeaderNumber, string description, string performanceMeasurementType, string weightScoreValue, string appraisalSection)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateHRAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <no>" + no + @"</no>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <description>" + description + @"</description>
                                    <performanceMeasurementType>" + performanceMeasurementType + @"</performanceMeasurementType>
                                    <weightedScore>" + weightScoreValue + @"</weightedScore>
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                    <createdBy>" + username + @"</createdBy>
                                </CreateHRAppraisalTarget>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateHRAppraisalTargetObjective(string username, string AppraisalHeaderNumber, string no, string appraisalTargetNumber, string description)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateHRAppraisalTargetObjective xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <no>" + no + @"</no>
                                    <appraisalTargetNumber>" + appraisalTargetNumber + @"</appraisalTargetNumber>
                                    <description>" + description + @"</description>
                                    <appraisalHeaderNo>" + AppraisalHeaderNumber + @"</appraisalHeaderNo>
                                    <createdBy>" + username + @"</createdBy>
                                </CreateHRAppraisalTargetObjective>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetAppraisalSections(string action, string appraisalSectionNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetAppraisalSections xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalSections>
                                        <AppraisalSection xmlns=""urn:microsoft-dynamics-nav/xmlports/x50091"">
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                            <IsHRDefined>[boolean]</IsHRDefined>
                                        </AppraisalSection>
                                    </appraisalSections>
                                    <action>" + action + @"</action>
                                    <appraisalSectionNo>" + appraisalSectionNo + @"</appraisalSectionNo>
                                </GetAppraisalSections>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string CreateAppraisalSection(string SectionName, string WhoDefines, string sectionNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateAppraisalSection xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <sectionName>" + SectionName + @"</sectionName>
                                    <whoDefines>" + WhoDefines + @"</whoDefines>
                                    <sectionNo>" + sectionNo + @"</sectionNo>
                                </CreateAppraisalSection>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteAppraisalSection(string SectionNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteAppraisalSection xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalSectionCode>" + SectionNo + @"</appraisalSectionCode>
                                </DeleteAppraisalSection>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetHRAppraisalTarget(string appraisalHeaderNumber, string appraisalTargetNo, string operation, string appraisalSection)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetHRAppraisalTarget xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTarget>
                                        <HRAppraisalTarget xmlns=""urn:microsoft-dynamics-nav/xmlports/x50096"">
                                            <AppraisalTargetCode>[string]</AppraisalTargetCode>
                                            <TargetDescription>[string]</TargetDescription>
                                            <PerformanceMeasurementType>[string]</PerformanceMeasurementType>
                                            <WeightedScore>[decimal]</WeightedScore>
                                        </HRAppraisalTarget>
                                    </appraisalTarget>
                                    <appraisalHeaderNo>" + appraisalHeaderNumber + @"</appraisalHeaderNo>
                                    <appraisalTargetNo>" + appraisalTargetNo + @"</appraisalTargetNo>
                                    <operation>" + operation + @"</operation>
                                    <appraisalSection>" + appraisalSection + @"</appraisalSection>
                                </GetHRAppraisalTarget>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string SetWhoDefinesSection(string SectionNo, string WhoDefines)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <SetWhoDefinesSection xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalSectionCode>" + SectionNo + @"</appraisalSectionCode>
                                    <whoDefines>" + WhoDefines + @"</whoDefines>
                                </SetWhoDefinesSection>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetHRAppraisalTargetObjectives(string appraisalHeaderNumber, string appraisalTargetNumber)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetHRAppraisalTargetObjectives xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <appraisalTargetObjectives>
                                        <HRTargetObjective xmlns=""urn:microsoft-dynamics-nav/xmlports/x50097"">
                                            <AppraisalHeaderNo>[string]</AppraisalHeaderNo>
                                            <AppraisalTargetNo>[string]</AppraisalTargetNo>
                                            <TargetObjectiveNo>[string]</TargetObjectiveNo>
                                            <ObjectiveDescription>[string]</ObjectiveDescription>
                                        </HRTargetObjective>
                                    </appraisalTargetObjectives>
                                    <appraisalHeaderNumber>" + appraisalHeaderNumber + @"</appraisalHeaderNumber>
                                    <appraisalTargetNumber>" + appraisalTargetNumber + @"</appraisalTargetNumber>
                                </GetHRAppraisalTargetObjectives>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
        public static string DeletePML(string pMLCode)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeletePML xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <pMLCode>" + pMLCode + @"</pMLCode>
                                </DeletePML>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreatePML(string code, string description, string pMLNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreatePML xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <code>" + code + @"</code>
                                    <description>" + description + @"</description>
                                    <pMLNo>" + pMLNo + @"</pMLNo>
                                </CreatePML>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetPerformanceMeasurementLevels(string pMLNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetPerformanceMeasurementLevels xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <performnceMeasurementLevels>
                                        <PerformanceMeasurementLevel xmlns=""urn:microsoft-dynamics-nav/xmlports/x50098"">
                                            <No>[string]</No>
                                            <Code>[string]</Code>
                                            <Description>[string]</Description>
                                        </PerformanceMeasurementLevel>
                                    </performnceMeasurementLevels>
                                    <action>ExportPMLDetails</action>
                                    <pMLNo>" + pMLNo + @"</pMLNo>
                                </GetPerformanceMeasurementLevels>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }

        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/
        /********************************************************** TRAINING ***********************************************************/

        public static string GetTrainingNewNo(string Document)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetTrainingNewNo xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <documentType>" + Document + @"</documentType>
                                </GetTrainingNewNo>
                            </Body>
                        </Envelope>";

            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetNameFromSESAno(string EmployeeNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetNameFromSESAno xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <sESANo>" + EmployeeNo + @"</sESANo>
                                </GetNameFromSESAno>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetTrainingList(string TrainingNo, string EmployeeNo, string status, string AppliedAs)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetTrainingList xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <exportTraingList>
                                        <Training xmlns=""urn:microsoft-dynamics-nav/xmlports/x50092"">
                                            <No>[string]</No>
                                            <Description>[string]</Description>
                                            <PlannedStartDate>[date]</PlannedStartDate>
                                            <PlannedStartTime>[time]</PlannedStartTime>
                                            <PlannedEndDate>[date]</PlannedEndDate>
                                            <PlannedEndTime>[time]</PlannedEndTime>
                                            <TotalCost>[decimal]</TotalCost>
                                            <NoSeries>[string]</NoSeries>
                                            <CourseCode>[string]</CourseCode>
                                            <CourseDescription>[string]</CourseDescription>
                                            <Trainer>[string]</Trainer>
                                            <TrainerName>[string]</TrainerName>
                                            <Venue>[string]</Venue>
                                            <Room>[string]</Room>
                                            <TrainingInstitution>[string]</TrainingInstitution>
                                            <ScheduledStartDate>[date]</ScheduledStartDate>
                                            <ScheduledStartTime>[time]</ScheduledStartTime>
                                            <ScheduledEndDate>[date]</ScheduledEndDate>
                                            <ScheduledEndTime>[time]</ScheduledEndTime>
                                            <ActualStartDate>[date]</ActualStartDate>
                                            <ActualStartTime>[time]</ActualStartTime>
                                            <ActualEndDate>[date]</ActualEndDate>
                                            <ActualEndTime>[time]</ActualEndTime>
                                            <CancellationCompletionDate>[date]</CancellationCompletionDate>
                                            <ProgressStatus>[string]</ProgressStatus>
                                            <LPONo>[string]</LPONo>
                                            <Archived>[boolean]</Archived>
                                            <CancellationReason>[string]</CancellationReason>
                                            <ActualCost>[decimal]</ActualCost>
                                            <ApplicableTo>[string]</ApplicableTo>
                                            <Approver>[string]</Approver>
                                            <SourceOfTraining>[string]</SourceOfTraining>
                                        </Training>
                                    </exportTraingList>
                                    <employeeNo>" + EmployeeNo + @"</employeeNo>
                                    <status>" + status + @"</status>
                                    <trainingNo>" + TrainingNo + @"</trainingNo>
                                    <appliedAs>" + AppliedAs + @"</appliedAs>
                                </GetTrainingList>
                            </Body>
                        </Envelope>";
            return req;
        }
        public static string CreateTraining(string No, string Description, string PlannedStartDate, string PlannedStartTime,
            string PlannedEndDate, string PlannedEndTime, string TotalCost, string NoSeries, string CourseCode, string CourseDescription,
            string Trainer, string TrainerName, string Venue,
        string Room, string TrainingInstitution, string ProgressStatus, string LPONo, string Archived, string CancellationReason,
        string ActualCost, string CreatedBy, string ApplicableToPersons, string RequirementOfTraining)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateTraining xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <no>" + No + @"</no>
                                    <description>" + Description + @"</description>
                                    <plannedStartDate>" + PlannedStartDate + @"</plannedStartDate>
                                    <plannedStartTime>" + PlannedStartTime + @"</plannedStartTime>
                                    <plannedEndDate>" + PlannedEndDate + @"</plannedEndDate>
                                    <plannedEndTime>" + PlannedEndTime + @"</plannedEndTime>
                                    <totalCost>" + TotalCost + @"</totalCost>
                                    <noSeries>" + NoSeries + @"</noSeries>
                                    <courseCode>" + CourseCode + @"</courseCode>
                                    <courseDescription>" + CourseDescription + @"</courseDescription>
                                    <trainer>" + Trainer + @"</trainer>
                                    <trainerName>" + TrainerName + @"</trainerName>
                                    <venue>" + Venue + @"</venue>
                                    <room>" + Room + @"</room>
                                    <trainingInstitution>" + TrainingInstitution + @"</trainingInstitution>
                                    <progressStatus>" + ProgressStatus + @"</progressStatus>
                                    <lPONo>" + LPONo + @"</lPONo>
                                    <archived>" + Archived + @"</archived>
                                    <cancellationReason>" + CancellationReason + @"</cancellationReason>
                                    <actualCost>5</actualCost>
                                    <createdBy>" + CreatedBy + @"</createdBy>
                                    <applicableToPersons>" + ApplicableToPersons + @"</applicableToPersons>
                                    <requirementOfTraining>" + RequirementOfTraining + @"</requirementOfTraining>
                                </CreateTraining>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string DeleteTraining(string TrainingNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <DeleteTraining xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <trainingNo>" + TrainingNo + @"</trainingNo>
                                </DeleteTraining>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string CreateTrainingMembersList(string appraisalMembersListNo, string TrainingNumber, string applicableToPersons)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <CreateTrainingMembersList xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <trainingNo>" + TrainingNumber + @"</trainingNo>
                                    <applicableToPersons>" + applicableToPersons + @"</applicableToPersons>
                                    <trainingMemberlistNo>" + appraisalMembersListNo + @"</trainingMemberlistNo>
                                </CreateTrainingMembersList>
                            </Body>
                        </Envelope>";
            string str = AppFunctions.CallWebService(req);
            return AppFunctions.GetJSONResponse(str);
        }
        public static string GetTrainingMembersList(string TrainingNo)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <Body>
                                <GetTrainingMembersList xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                    <trainingNo>" + TrainingNo + @"</trainingNo>
                                    <trainingMembersList>
                                        <TrainingMember xmlns=""urn:microsoft-dynamics-nav/xmlports/x50093"">
                                            <TrainingListNo>[string]</TrainingListNo>
                                            <TrainingNo>[string]</TrainingNo>
                                            <ApplicableToPersons>[string]</ApplicableToPersons>
                                        </TrainingMember>
                                    </trainingMembersList>
                                </GetTrainingMembersList>
                            </Body>
                        </Envelope>";
            return AppFunctions.CallWebService(req);
        }
    }
}
