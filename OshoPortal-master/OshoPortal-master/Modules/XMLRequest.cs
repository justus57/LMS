using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OshoPortal.WebService_Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace OshoPortal.Modules
{
    public class XMLRequest
    {
        public static string GetUserInformation(string EmpNo)
        {
            var req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetEmployeeHomeData xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <employeeNo>{EmpNo}</employeeNo>
                                        </GetEmployeeHomeData>
                                    </Body>
                                </Envelope>";
            var response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
        public static string SaveRequisition(string documentNo,string type, string EmpNo, string EmpName,string Item, string description,string quantity,string unitOfMeasure,string amount,string dateofSelection)
        {        
           
            var req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetSaveRequisitionDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <requisitionDetail>
                                                <RequisitionHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/x51202"">
                                                    <DocumentType>1</DocumentType>
                                                    <DocumentNo>{documentNo}</DocumentNo>
                                                    <RequestByNo>{EmpNo}</RequestByNo>
                                                    <RequestByName>{EmpName}</RequestByName>
                                                    <CurrencyCode></CurrencyCode>
                                                    <ShortcutDimCode1></ShortcutDimCode1>
                                                    <ShortcutDimCode2></ShortcutDimCode2>
                                                    <RequestDate>{dateofSelection}</RequestDate>
                                                    <ValidToDate></ValidToDate>
                                                    <RequestedReceiptDate>{dateofSelection}</RequestedReceiptDate>
                                                    <StatusAsText></StatusAsText>
                                                    <Approver></Approver>
                                                    <RequisitionHdrLine>
                                                        <Type>{type}</Type>
                                                        <No>{Item}</No>
                                                        <Description>{description}</Description>
                                                        <Quantity>{quantity}</Quantity>
                                                        <LocationCode></LocationCode>
                                                        <UnitofMeasureCode>{unitOfMeasure}</UnitofMeasureCode>
                                                        <UnitCostLCY>{amount}</UnitCostLCY>
                                                    </RequisitionHdrLine>
                                                </RequisitionHeader>
                                            </requisitionDetail>
                                            <documentNo>{documentNo}</documentNo>
                                            <employeeNo>{EmpNo}</employeeNo>
                                            <operation>IMPORT</operation>
                                        </GetSaveRequisitionDetail>
                                    </Body>
                                </Envelope>";
            string response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
        public static string SaveItemLine(string documentNo, string type, string EmpNo, string operation, string Item, string description, string quantity, string unitOfMeasure, string amount, string dateofSelection)
        {
            var req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetRequisitionLineDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <requisitionLineDetail>
                                            <!-- Optional -->
                                            <RequisitionHeaderLine xmlns=""urn:microsoft-dynamics-nav/xmlports/x51204"">
                                                <DocumentType>1</DocumentType>
                                                <DocumentNo>{documentNo}</DocumentNo>
                                                <LineNo>0</LineNo>
                                                <LineType>{type}</LineType>
                                                <No>{Item}</No>
                                                <Description>{description}</Description>
                                                <Description2>{description}</Description2>
                                                <Specification>{description}</Specification>
                                                <Quantity>{quantity}</Quantity>
                                                <UoMCode>{unitOfMeasure}</UoMCode>
                                                <UnitCost>{amount}</UnitCost>
                                                <LineAmount>{amount}</LineAmount>
                                            </RequisitionHeaderLine>
                                        </requisitionLineDetail>
                                        <documentNo></documentNo>
                                        <lineNo>0</lineNo>
                                        <employeeNo>{EmpNo}</employeeNo>
                                        <operation>{operation}</operation>
                                    </GetRequisitionLineDetail>
                                </Body>
                            </Envelope>";
            return WSConnection.CallWebServicePortal(req); ;
        }
        public static string DeleteDocument(string documentNo, string documentArea, string employee)
        {
            var req = $@" <Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <DeleteDocument xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <documentArea></documentArea>
                                            <documentNo>{documentNo}</documentNo>
                                            <employee>{employee}</employee>
                                        </DeleteDocument>
                                    </Body>
                                </Envelope>";
            string response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
        public static string GetRequisitionLine(string documentNo, string Operation, string employee)
        {
            var req =$@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetRequisitionLineDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <requisitionLineDetail>
                                            <!-- Optional -->
                                            <RequisitionHeaderLine xmlns=""urn:microsoft-dynamics-nav/xmlports/x51204"">
                                                <DocumentNo></DocumentNo>
                                                <LineNo></LineNo>
                                                <No></No>
                                                <LineType></LineType>
                                                <Description></Description>
                                                <Description2></Description2>
                                                <Specification></Specification>
                                                <Quantity></Quantity>
                                                <UoMCode></UoMCode>
                                                <UnitCost></UnitCost>
                                                <LineAmount></LineAmount>
                                            </RequisitionHeaderLine>
                                        </requisitionLineDetail>
                                        <documentNo>{documentNo}</documentNo>
                                        <lineNo></lineNo>
                                        <employeeNo>{employee}</employeeNo>
                                        <operation>{Operation}</operation>
                                    </GetRequisitionLineDetail>
                                </Body>
                            </Envelope>";
            return WSConnection.CallWebServicePortal(req); 
            
        }
        public static IDictionary<string, string> GetGLlist(string GLAccountname, string empNo)
        {
            var req = "";
            var node = "";
            var dictionary = new Dictionary<string, string>();

            switch (GLAccountname)
            {
                case "Item":
                    req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetWebItemList xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <itemList>
                                            <Item xmlns=""urn:microsoft-dynamics-nav/xmlports/x51200"">
                                                <ItemNo></ItemNo>
                                                <ItemDescription></ItemDescription>
                                            </Item>
                                        </itemList>
                                        <employeeNo>{empNo}</employeeNo>
                                    </GetWebItemList>
                                </Body>
                            </Envelope>";
                    break;
                default:
                    req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                        <Body>
                                            <GetWebGLList xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                <gLList>
                                                    <GLAccount xmlns=""urn:microsoft-dynamics-nav/xmlports/x51203"">
                                                        <GLAccountNo></GLAccountNo>
                                                        <GLAccountName>{GLAccountname}</GLAccountName>
                                                    </GLAccount>
                                                </gLList>
                                                <employeeNo></employeeNo>
                                            </GetWebGLList>
                                        </Body>
                                    </Envelope>";
                    break;
            }
            string response1 = WSConnection.CallWebServicePortal(req);
            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(response1);
            int count = 0;
            XmlNode xmlNode1 = xmlSoapRequest.GetElementsByTagName("Item")[count];
            switch (xmlNode1)
            {
                case null:
                    {
                        foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("GLAccount"))
                        {
                            XmlNode NodeEmpCode = xmlSoapRequest.GetElementsByTagName("GLAccountNo")[count];
                            string ItemNo = NodeEmpCode.InnerText;

                            XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("GLAccountName")[count];
                            string ItemDescription = NodeEmployeeName.InnerText;

                            dictionary.Add(ItemNo, ItemDescription);

                            count++;
                        }

                        break;
                    }

                default:
                    {
                        foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Item"))
                        {
                            XmlNode NodeEmpCode = xmlSoapRequest.GetElementsByTagName("ItemNo")[count];
                            string ItemNo = NodeEmpCode.InnerText;

                            XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("ItemDescription")[count];
                            string ItemDescription = NodeEmployeeName.InnerText;

                            dictionary.Add(ItemNo, ItemDescription);

                            count++;
                        }

                        break;
                    }
            }

            return dictionary;
        }
         public static string CancelRequisition(string document)
        {
          var req = $@" <Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <CancelRequisitionApprovalRequest xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <documentNo>{document}</documentNo>
                                    </CancelRequisitionApprovalRequest>
                                </Body>
                            </Envelope>";
            var response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
    }
   
    public class LoginXMLRequests
    {
        public static string UserLogin(string username, string password)
        {
            string req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <ConfirmEmployeePassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                        <employeeNo>{username}</employeeNo>
                                        <password>{password}</password>
                                    </ConfirmEmployeePassword>
                                </Body>
                            </Envelope>";
            string response = WSConnection.CallWebService(req);
            return WSConnection.GetJSONResponse(response);
        }
    }
    public class OneTimePassXMLRequests
    {
        public static string ChangePassword(string username, string oldpass, string newpass)
        {
            string req = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <ChangeEmployeePassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                            <employeeNo>{username}</employeeNo>
                                            <oldPassword>{oldpass}</oldPassword>
                                            <newPassword>{newpass}</newPassword>
                                        </ChangeEmployeePassword>
                                    </Body>
                                </Envelope>";
            string str = WSConnection.CallWebService(req);
            return WSConnection.GetJSONResponse(str);
        }
    }
    public class ForgotPasswordXmlRequest
    {
        public static string ForgotPassword(string username)
        {
            string ResetPassresponseString = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                    <Body>
                                                        <RecoverLostPassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                                            <employeeNo>{username}</employeeNo>
                                                        </RecoverLostPassword>
                                                    </Body>
                                                </Envelope>";
            string response = WSConnection.CallWebService(ResetPassresponseString);
            return WSConnection.GetJSONResponse(response);
        }
    }
    public class createRequisition
    {
        public static IDictionary<string, string> Requisition(string name)
        {
            var dictionary = new Dictionary<string, string>();
            string listdata = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetWebItemList xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <itemList>
                                            <Item xmlns=""urn:microsoft-dynamics-nav/xmlports/GetWebItemList"">
                                                <ItemNo></ItemNo>
                                                <ItemDescription></ItemDescription>
                                            </Item>
                                        </itemList>
                                        <employeeNo>{name}</employeeNo>
                                    </GetWebItemList>
                                </Body>
                            </Envelope>";
            string response1 = WSConnection.CallWebServicePortal(listdata);
             XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(response1);
            int count = 0;
            foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("Item"))
            {
                XmlNode NodeEmpCode = xmlSoapRequest.GetElementsByTagName("ItemNo")[count];
                string ItemNo = NodeEmpCode.InnerText;

                XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("ItemDescription")[count];
                string ItemDescription = NodeEmployeeName.InnerText;

                dictionary.Add(ItemNo, ItemDescription);

                count++;
            }
            return dictionary;
           
        }
        public static string GetitemDetails(string value, string type)
        {
            string details = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                <Body>
                                                    <GetItemDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                        <accNo>{value}</accNo>
                                                       <parameterType>{type}</parameterType>
                                                    </GetItemDetails>
                                                </Body>
                                            </Envelope>";
            string response = WSConnection.CallWebServicePortal(details);
            return WSConnection.GetJSONResponse(response);
        }
    }
    public class ProductsXMLRequests
    {
        public static DataTable GetPageData(string status, string RequestAS,string endpoint)
        {
            string Status = null;
            string AppliedAs = null;

            switch (status)
            {
                case "self":
                    AppliedAs = "Employee";
                    break;
                case "others":
                    AppliedAs = "AppliedForAnother";
                    break;
            }

            switch (status)
            {
                case "Open":
                    Status = "Open";
                    break;
                case "Pending":
                    Status = "PendingApproval";
                    break;
                case "Approved":
                    Status = "Approved";
                    break;
                case "Rejected":
                    Status = "Rejected";
                    break;
            }

            string username = HttpContext.Current.Session["Username"].ToString();

            string tabledata = GetItemsList.Getitemlist(username, RequestAS, status);

            XmlDocument xmlSoapRequest = new XmlDocument();
            xmlSoapRequest.LoadXml(tabledata);
            int count = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date Submitted", typeof(string));
            table.Columns.Add("DocumentNo", typeof(string));
            table.Columns.Add("Employee Name", typeof(string));
            table.Columns.Add("RequestByNo", typeof(string));           
            table.Columns.Add("ValidToDate", typeof(string));         
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Approver", typeof(string));
            table.Columns.Add("View", typeof(string));


            if (xmlSoapRequest.GetElementsByTagName("RequisitionHeader").Count >0)
            {
                foreach (XmlNode xmlNode in xmlSoapRequest.DocumentElement.GetElementsByTagName("RequisitionHeader"))
                {

                    XmlNode NodeRequestDate = xmlSoapRequest.GetElementsByTagName("RequestDate")[count];
                    string RequestDate = NodeRequestDate.InnerText;

                    XmlNode NodeEmployeeName = xmlSoapRequest.GetElementsByTagName("RequestByName")[count];
                    string EmployeeName = NodeEmployeeName.InnerText;

                    XmlNode NodeRequestByNo = xmlSoapRequest.GetElementsByTagName("RequestByNo")[count];
                    string RequestByNo = NodeRequestByNo.InnerText;

                    XmlNode NodeDocumentNo = xmlSoapRequest.GetElementsByTagName("DocumentNo")[count];
                    string DocumentNo = NodeDocumentNo.InnerText;

                    XmlNode NodeValidToDate = xmlSoapRequest.GetElementsByTagName("ValidToDate")[count];
                    string ValidToDate = NodeValidToDate.InnerText;

                    XmlNode NodeApprover = xmlSoapRequest.GetElementsByTagName("Approver")[count];
                    string Approver = NodeApprover.InnerText;

                    XmlNode NodeStatusAsText = xmlSoapRequest.GetElementsByTagName("StatusAsText")[count];
                    string StatusAsText = NodeStatusAsText.InnerText;

                    XmlNode NodeDocumentType = xmlSoapRequest.GetElementsByTagName("DocumentType")[count];
                    string DocumentType = NodeDocumentType.InnerText;

                    if (status == "Open")
                    {
                      //  table.Rows.Add(Functions.ConvertTime(RequestDate), EmployeeName, RequestByNo, DocumentNo, Functions.ConvertTime(ValidToDate), Approver, StatusAsText, "<a class = 'btn btn-secondary btn-xs' href = " + endpoint + Functions.Base64Encode(DocumentType) + "&status=Open" + " data-toggle='tooltip' title='Edit Application'><span class = 'fa fa-edit'> </span></a>  <a class = 'btn btn-success btn-xs submit_record' data-id=" + Functions.Base64Encode(DocumentType) + " data-date=" + Functions.ConvertTime(ValidToDate) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Submit Application'><span class = 'fa fa-paper-plane'> </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + Functions.Base64Encode(DocumentType) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Delete Application'><span class = 'fa fa-trash'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + endpoint + Functions.Base64Encode(DocumentType) + "&status=Open" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                        table.Rows.Add(Functions.ConvertTime(RequestDate), DocumentNo, EmployeeName, RequestByNo, Functions.ConvertTime(ValidToDate), StatusAsText, Approver, "<a class = 'btn btn-secondary btn-xs' href = " + endpoint + "?id=" + Functions.Base64Encode(DocumentNo) + "&status=Open" + " data-toggle='tooltip' title='Edit Application'><span class = 'fa fa-edit'> </span></a><a class = 'btn btn-success btn-xs submit_record' data-id=" + Functions.Base64Encode(DocumentNo) + " data-date=" + Functions.ConvertTime(ValidToDate) + " href = 'javascript:void(0)'><span class = 'fa fa-paper-plane' data-toggle='tooltip' title='Submit Application'> </span></a> <a class = 'btn btn-danger btn-xs delete_record' data-id=" + Functions.Base64Encode(DocumentNo) + " href = 'javascript:void(0)'><span class = 'fa fa-trash' data-toggle='tooltip' title='Delete Application'> </span></a> <a class = 'btn btn-primary btn-xs' href = " + endpoint + "?id=" +Functions.Base64Encode(DocumentNo) + "&status=Open" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Pending")
                    {
                        table.Rows.Add(Functions.ConvertTime(RequestDate), DocumentNo, EmployeeName, RequestByNo,  Functions.ConvertTime(ValidToDate), StatusAsText, Approver,  "<a class = 'btn btn-danger btn-xs cancel_record' data-id=" + Functions.Base64Encode(DocumentNo) + " href = 'javascript:void(0)' data-toggle='tooltip' title='Cancel Application'><span class = 'fa fa-times' > </span></a> <a class = 'btn btn-primary btn-xs' href = " +endpoint+ "?id=" + Functions.Base64Encode(DocumentNo) + "&status=Pending" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Approved")
                    {
                        table.Rows.Add(Functions.ConvertTime(RequestDate), DocumentNo, EmployeeName, RequestByNo,  Functions.ConvertTime(ValidToDate), StatusAsText, Approver, "<a class = 'btn btn-primary btn-xs' href = " + endpoint+"?id=" + Functions.Base64Encode(DocumentNo) + "&status=Approved" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }
                    else if (status == "Rejected")
                    {
                        table.Rows.Add(Functions.ConvertTime(RequestDate), DocumentNo, EmployeeName, RequestByNo,Functions.ConvertTime(ValidToDate), StatusAsText, Approver, "<a class = 'btn btn-primary btn-xs' href = " + endpoint+"?id=" + Functions.Base64Encode(DocumentNo) + "&status=Rejected" + " data-toggle='tooltip' title='View Application'><span class = 'fa fa-eye'> </span></a>");
                    }

                    count++;
                }
            }

            return table;
        }
    }
    public class GetItemsList
    {
        public static string Getitemlist(string EmpNo,string RequestAS,string status)
        {
            string reqitem = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetRequisitionList xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <requisitionList>
                                                <RequisitionHeader xmlns=""urn:microsoft-dynamics-nav/xmlports/x51202"">
                                                    <DocumentType></DocumentType>
                                                    <DocumentNo></DocumentNo>
                                                    <RequestByNo></RequestByNo>
                                                    <RequestByName></RequestByName>
                                                    <CurrencyCode></CurrencyCode>
                                                    <ShortcutDimCode1></ShortcutDimCode1>
                                                    <ShortcutDimCode2></ShortcutDimCode2>
                                                    <RequestDate></RequestDate>
                                                    <ValidToDate></ValidToDate>
                                                    <RequestedReceiptDate></RequestedReceiptDate>
                                                    <StatusAsText></StatusAsText>
                                                    <Approver></Approver>
                                                </RequisitionHeader>
                                            </requisitionList>
                                            <employeeNo> {EmpNo} </employeeNo>
                                            <requestAs > {RequestAS} </requestAs>
                                            <approvalStatus> {status} </approvalStatus>
                                        </GetRequisitionList>
                                    </Body>
                                </Envelope>";
            var response = WSConnection.CallWebServicePortal(reqitem);
            return response;
        }

        public static string Getitemdetail(string EmpNo, string DocumentNo, string status)
        {
            string reqitem = $@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <GetAppliedItemDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                    <documentNo>{DocumentNo}</documentNo>
                                                </GetAppliedItemDetail>
                                            </Body>
                                        </Envelope>";
            var response = WSConnection.CallWebServicePortal(reqitem);
            return WSConnection.GetJSONResponse(response);
        }
      
    }
}