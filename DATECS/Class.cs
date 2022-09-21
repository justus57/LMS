using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATECS.Class
{
    public class Invoice
    {
        public string cashier { get; set; }
        public int invoiceType { get; set; }
        public int transactionType { get; set; }
        public string TraderSystemInvoiceNumber { get; set; }
        public string ExemptionNumber { get; set; }
        public Payment[] payment { get; set; }
        public string relevantNumber { get; set; }
        public Options options { get; set; }
        public Buyer buyer { get; set; }
        public Item[] items { get; set; }
        public Line[] lines { get; set; }
    }

    public class Options
    {
        public string omitQRCodeGen { get; set; }
        public string omitTextualRepresentation { get; set; }
    }

    public class Buyer
    {
        public string pinOfBuyer { get; set; }
        public string buyerName { get; set; }
        public string buyerAddress { get; set; }
        public string buyerPhone { get; set; }
    }

    public class Payment
    {
        public float amount { get; set; }
        public string paymentType { get; set; }
    }

    public class Item
    {
        public string gtin { get; set; }
        public string name { get; set; }
        public float quantity { get; set; }
        public string hsCode { get; set; }
        public float unitPrice { get; set; }
        public Description[] description { get; set; }
        public float totalAmount { get; set; }
    }

    public class Description
    {
        public string value { get; set; }
    }

    public class Line
    {
        public string lineType { get; set; }
        public string alignment { get; set; }
        public string format { get; set; }
        public string value { get; set; }
    }

    public class Reports
    {
        public int reportType { get; set; }
    }

    public class InvoiceResponse
    {
        public DateTime DateTime { get; set; }
        public string invoiceExtension { get; set; }
        public string invoiceCounter { get; set; }
        public string mtn { get; set; }
        public string verificationUrl { get; set; }
        public string messages { get; set; }
        public float totalAmount { get; set; }
        public string msn { get; set; }
    }

}
