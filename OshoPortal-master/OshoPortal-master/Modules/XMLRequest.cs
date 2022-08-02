using OshoPortal.WebService_Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace OshoPortal.Modules
{
    public class XMLRequest
    {
        public static string GetUserInformation(string EmpNo)
        {
            var req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <GetEmployeeHomeData xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                            <employeeNo>" + EmpNo + @"</employeeNo>
                                        </GetEmployeeHomeData>
                                    </Body>
                                </Envelope>";
            string response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
        public static string SaveRequisition(string documentNo,string EmpNo,string EmpName,string Item, string description,string quantity,string unitOfMeasure,string amount,string dateofSelection)
        {
            string req =@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <SaveRequisitionDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                    <documentNo>"+ documentNo + @"</documentNo>
                                                    <employeeID>" + EmpNo + @"</employeeID>
                                                    <employeeName>" + EmpName + @"</employeeName>
                                                    <item>" + Item + @"</item>
                                                    <description>" + description + @"</description>
                                                    <quantity>" + quantity + @"</quantity>
                                                    <unitOfMeasure>" + unitOfMeasure + @"</unitOfMeasure>
                                                    <amount>" + amount + @"</amount>
                                                    <dateofSelection>" + dateofSelection + @"</dateofSelection>
                                                </SaveRequisitionDetails>
                                            </Body>
                                        </Envelope>";
            string response = WSConnection.CallWebServicePortal(req);
            return WSConnection.GetJSONResponse(response);
        }
    }
   
    public class LoginXMLRequests
    {
        public static string UserLogin(string username, string password)
        {
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <ConfirmEmployeePassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                        <employeeNo>" + username + @"</employeeNo>
                                        <password>" + password + @"</password>
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
            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <Body>
                                        <ChangeEmployeePassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                            <employeeNo>" + username + @"</employeeNo>
                                            <oldPassword>" + oldpass + @"</oldPassword>
                                            <newPassword>" + newpass + @"</newPassword>
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
            string ResetPassresponseString = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                    <Body>
                                                        <RecoverLostPassword xmlns=""urn:microsoft-dynamics-schemas/codeunit/PortalLogin"">
                                                            <employeeNo>" + username + @"</employeeNo>
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
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string listdata = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <Body>
                                    <GetWebItemList xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                        <itemList>
                                            <Item xmlns=""urn:microsoft-dynamics-nav/xmlports/GetWebItemList"">
                                                <ItemNo></ItemNo>
                                                <ItemDescription></ItemDescription>
                                            </Item>
                                        </itemList>
                                        <employeeNo>" + name + @"</employeeNo>
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
        public static string GetitemDetails(string value)
        {
            string details = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                <Body>
                                                    <GetItemDetails xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                        <itemNo>"+ value + @"</itemNo>
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

            if (status == "self")
            {
                AppliedAs = "Employee";
            }
            else if (status == "others")
            {
                AppliedAs = "AppliedForAnother";
            }

            if (status == "Open")
            {
               Status = "Open";
            }
            else if (status == "Pending")
            {
                Status = "PendingApproval";
            }
            else if (status == "Approved")
            {
                Status = "Approved";
            }
            else if (status == "Rejected")
            {
               Status = "Rejected";
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
            string reqitem = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
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
                                            <employeeNo> "+EmpNo+@" </employeeNo>
                                            <requestAs > "+RequestAS+@" </requestAs>
                                            <approvalStatus> "+status+@" </approvalStatus>
                                        </GetRequisitionList>
                                    </Body>
                                </Envelope>";
            var response = WSConnection.CallWebServicePortal(reqitem);
            return response;
        }

        public static string Getitemdetail(string EmpNo, string DocumentNo, string status)
        {
            string reqitem = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                            <Body>
                                                <GetAppliedItemDetail xmlns=""urn:microsoft-dynamics-schemas/codeunit/webportal"">
                                                    <documentNo>"+DocumentNo+@"</documentNo>
                                                </GetAppliedItemDetail>
                                            </Body>
                                        </Envelope>";
            var response = WSConnection.CallWebServicePortal(reqitem);
            return response;
        }
      
    }
}