﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace OshoPortal.WebPortal {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="webportal_Binding", Namespace="urn:microsoft-dynamics-schemas/codeunit/webportal")]
    public partial class webportal : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback DeleteDocumentOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetItemDetailsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetNewDocumentNoOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequisitionDetailOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequisitionListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetWebItemListOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public webportal() {
            this.Url = global::OshoPortal.Properties.Settings.Default.OshoPortal_WebPortal_webportal;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DeleteDocumentCompletedEventHandler DeleteDocumentCompleted;
        
        /// <remarks/>
        public event GetItemDetailsCompletedEventHandler GetItemDetailsCompleted;
        
        /// <remarks/>
        public event GetNewDocumentNoCompletedEventHandler GetNewDocumentNoCompleted;
        
        /// <remarks/>
        public event GetRequisitionDetailCompletedEventHandler GetRequisitionDetailCompleted;
        
        /// <remarks/>
        public event GetRequisitionListCompletedEventHandler GetRequisitionListCompleted;
        
        /// <remarks/>
        public event GetWebItemListCompletedEventHandler GetWebItemListCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:DeleteDocument", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="DeleteDocument_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public string DeleteDocument(string documentArea, string documentNo, string employee) {
            object[] results = this.Invoke("DeleteDocument", new object[] {
                        documentArea,
                        documentNo,
                        employee});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteDocumentAsync(string documentArea, string documentNo, string employee) {
            this.DeleteDocumentAsync(documentArea, documentNo, employee, null);
        }
        
        /// <remarks/>
        public void DeleteDocumentAsync(string documentArea, string documentNo, string employee, object userState) {
            if ((this.DeleteDocumentOperationCompleted == null)) {
                this.DeleteDocumentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteDocumentOperationCompleted);
            }
            this.InvokeAsync("DeleteDocument", new object[] {
                        documentArea,
                        documentNo,
                        employee}, this.DeleteDocumentOperationCompleted, userState);
        }
        
        private void OnDeleteDocumentOperationCompleted(object arg) {
            if ((this.DeleteDocumentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteDocumentCompleted(this, new DeleteDocumentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:GetItemDetails", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="GetItemDetails_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public string GetItemDetails(string itemNo) {
            object[] results = this.Invoke("GetItemDetails", new object[] {
                        itemNo});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetItemDetailsAsync(string itemNo) {
            this.GetItemDetailsAsync(itemNo, null);
        }
        
        /// <remarks/>
        public void GetItemDetailsAsync(string itemNo, object userState) {
            if ((this.GetItemDetailsOperationCompleted == null)) {
                this.GetItemDetailsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetItemDetailsOperationCompleted);
            }
            this.InvokeAsync("GetItemDetails", new object[] {
                        itemNo}, this.GetItemDetailsOperationCompleted, userState);
        }
        
        private void OnGetItemDetailsOperationCompleted(object arg) {
            if ((this.GetItemDetailsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetItemDetailsCompleted(this, new GetItemDetailsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:GetNewDocumentNo", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="GetNewDocumentNo_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public string GetNewDocumentNo(ref string documentNo, string employeeNo, bool foreignRequisition) {
            object[] results = this.Invoke("GetNewDocumentNo", new object[] {
                        documentNo,
                        employeeNo,
                        foreignRequisition});
            documentNo = ((string)(results[1]));
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetNewDocumentNoAsync(string documentNo, string employeeNo, bool foreignRequisition) {
            this.GetNewDocumentNoAsync(documentNo, employeeNo, foreignRequisition, null);
        }
        
        /// <remarks/>
        public void GetNewDocumentNoAsync(string documentNo, string employeeNo, bool foreignRequisition, object userState) {
            if ((this.GetNewDocumentNoOperationCompleted == null)) {
                this.GetNewDocumentNoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetNewDocumentNoOperationCompleted);
            }
            this.InvokeAsync("GetNewDocumentNo", new object[] {
                        documentNo,
                        employeeNo,
                        foreignRequisition}, this.GetNewDocumentNoOperationCompleted, userState);
        }
        
        private void OnGetNewDocumentNoOperationCompleted(object arg) {
            if ((this.GetNewDocumentNoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetNewDocumentNoCompleted(this, new GetNewDocumentNoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:GetRequisitionDetail", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="GetRequisitionDetail_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void GetRequisitionDetail(ref RequisitionHeaderList requisitionDetail, string documentNo, string employeeNo, string operation) {
            object[] results = this.Invoke("GetRequisitionDetail", new object[] {
                        requisitionDetail,
                        documentNo,
                        employeeNo,
                        operation});
            requisitionDetail = ((RequisitionHeaderList)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequisitionDetailAsync(RequisitionHeaderList requisitionDetail, string documentNo, string employeeNo, string operation) {
            this.GetRequisitionDetailAsync(requisitionDetail, documentNo, employeeNo, operation, null);
        }
        
        /// <remarks/>
        public void GetRequisitionDetailAsync(RequisitionHeaderList requisitionDetail, string documentNo, string employeeNo, string operation, object userState) {
            if ((this.GetRequisitionDetailOperationCompleted == null)) {
                this.GetRequisitionDetailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequisitionDetailOperationCompleted);
            }
            this.InvokeAsync("GetRequisitionDetail", new object[] {
                        requisitionDetail,
                        documentNo,
                        employeeNo,
                        operation}, this.GetRequisitionDetailOperationCompleted, userState);
        }
        
        private void OnGetRequisitionDetailOperationCompleted(object arg) {
            if ((this.GetRequisitionDetailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequisitionDetailCompleted(this, new GetRequisitionDetailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:GetRequisitionList", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="GetRequisitionList_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void GetRequisitionList(ref RequisitionHeaderList requisitionList, string employeeNo, string requestAs, string approvalStatus) {
            object[] results = this.Invoke("GetRequisitionList", new object[] {
                        requisitionList,
                        employeeNo,
                        requestAs,
                        approvalStatus});
            requisitionList = ((RequisitionHeaderList)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequisitionListAsync(RequisitionHeaderList requisitionList, string employeeNo, string requestAs, string approvalStatus) {
            this.GetRequisitionListAsync(requisitionList, employeeNo, requestAs, approvalStatus, null);
        }
        
        /// <remarks/>
        public void GetRequisitionListAsync(RequisitionHeaderList requisitionList, string employeeNo, string requestAs, string approvalStatus, object userState) {
            if ((this.GetRequisitionListOperationCompleted == null)) {
                this.GetRequisitionListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequisitionListOperationCompleted);
            }
            this.InvokeAsync("GetRequisitionList", new object[] {
                        requisitionList,
                        employeeNo,
                        requestAs,
                        approvalStatus}, this.GetRequisitionListOperationCompleted, userState);
        }
        
        private void OnGetRequisitionListOperationCompleted(object arg) {
            if ((this.GetRequisitionListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequisitionListCompleted(this, new GetRequisitionListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/webportal:GetWebItemList", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", ResponseElementName="GetWebItemList_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/webportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void GetWebItemList(ref ItemList itemList, string employeeNo) {
            object[] results = this.Invoke("GetWebItemList", new object[] {
                        itemList,
                        employeeNo});
            itemList = ((ItemList)(results[0]));
        }
        
        /// <remarks/>
        public void GetWebItemListAsync(ItemList itemList, string employeeNo) {
            this.GetWebItemListAsync(itemList, employeeNo, null);
        }
        
        /// <remarks/>
        public void GetWebItemListAsync(ItemList itemList, string employeeNo, object userState) {
            if ((this.GetWebItemListOperationCompleted == null)) {
                this.GetWebItemListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetWebItemListOperationCompleted);
            }
            this.InvokeAsync("GetWebItemList", new object[] {
                        itemList,
                        employeeNo}, this.GetWebItemListOperationCompleted, userState);
        }
        
        private void OnGetWebItemListOperationCompleted(object arg) {
            if ((this.GetWebItemListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetWebItemListCompleted(this, new GetWebItemListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-nav/xmlports/x51202")]
    public partial class RequisitionHeaderList {
        
        private RequisitionHeader[] requisitionHeaderField;
        
        private string[] textField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RequisitionHeader")]
        public RequisitionHeader[] RequisitionHeader {
            get {
                return this.requisitionHeaderField;
            }
            set {
                this.requisitionHeaderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-nav/xmlports/x51202")]
    public partial class RequisitionHeader {
        
        private string documentTypeField;
        
        private string documentNoField;
        
        private string requestByNoField;
        
        private string requestByNameField;
        
        private string currencyCodeField;
        
        private string shortcutDimCode1Field;
        
        private string shortcutDimCode2Field;
        
        private string requestDateField;
        
        private string validToDateField;
        
        private string requestedReceiptDateField;
        
        private string statusAsTextField;
        
        private string approverField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DocumentType {
            get {
                return this.documentTypeField;
            }
            set {
                this.documentTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DocumentNo {
            get {
                return this.documentNoField;
            }
            set {
                this.documentNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestByNo {
            get {
                return this.requestByNoField;
            }
            set {
                this.requestByNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestByName {
            get {
                return this.requestByNameField;
            }
            set {
                this.requestByNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurrencyCode {
            get {
                return this.currencyCodeField;
            }
            set {
                this.currencyCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ShortcutDimCode1 {
            get {
                return this.shortcutDimCode1Field;
            }
            set {
                this.shortcutDimCode1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ShortcutDimCode2 {
            get {
                return this.shortcutDimCode2Field;
            }
            set {
                this.shortcutDimCode2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestDate {
            get {
                return this.requestDateField;
            }
            set {
                this.requestDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ValidToDate {
            get {
                return this.validToDateField;
            }
            set {
                this.validToDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RequestedReceiptDate {
            get {
                return this.requestedReceiptDateField;
            }
            set {
                this.requestedReceiptDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StatusAsText {
            get {
                return this.statusAsTextField;
            }
            set {
                this.statusAsTextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Approver {
            get {
                return this.approverField;
            }
            set {
                this.approverField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-nav/xmlports/x51200")]
    public partial class Item {
        
        private string itemNoField;
        
        private string itemDescriptionField;
        
        /// <remarks/>
        public string ItemNo {
            get {
                return this.itemNoField;
            }
            set {
                this.itemNoField = value;
            }
        }
        
        /// <remarks/>
        public string ItemDescription {
            get {
                return this.itemDescriptionField;
            }
            set {
                this.itemDescriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-nav/xmlports/x51200")]
    public partial class ItemList {
        
        private Item[] itemField;
        
        private string[] textField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public Item[] Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void DeleteDocumentCompletedEventHandler(object sender, DeleteDocumentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteDocumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteDocumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetItemDetailsCompletedEventHandler(object sender, GetItemDetailsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetItemDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetItemDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetNewDocumentNoCompletedEventHandler(object sender, GetNewDocumentNoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetNewDocumentNoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetNewDocumentNoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string documentNo {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetRequisitionDetailCompletedEventHandler(object sender, GetRequisitionDetailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequisitionDetailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequisitionDetailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RequisitionHeaderList requisitionDetail {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RequisitionHeaderList)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetRequisitionListCompletedEventHandler(object sender, GetRequisitionListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequisitionListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequisitionListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RequisitionHeaderList requisitionList {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RequisitionHeaderList)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetWebItemListCompletedEventHandler(object sender, GetWebItemListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetWebItemListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetWebItemListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ItemList itemList {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ItemList)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591