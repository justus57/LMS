using DATECS.Class;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace DATECS
{
    class Program
    {
        private static string pin;

        public static Invoice invoice = new Invoice();
        Item item = new Item();
       public static string json = new JavaScriptSerializer().Serialize(invoice);
        public static Invoice data = new JavaScriptSerializer().Deserialize<Invoice>(json);
        static void Main(string[] args)
        {
            //var statusreponse =GetStatus();
            //dynamic json = JObject.Parse(statusreponse);
            //string status = json.isPinRequired;
             CreateInvoiceHeader("0", "0","");
            BuyersInfo("wer","wer","iwnrk","ewe");
            CreateItemLines("2323", "name", 3, 2,"0");
           
            CreateItemLines("2323", "name", 3, 2, "0");
            var json2 = JsonConvert.SerializeObject(imageArrayq, Formatting.Indented);
            
           
            data.items = imageArrayq.ToArray();

            //invoice.items = (Item[])item;
            //var data = JObject.Parse(item.ToString());


            //invoice.items = imageArrayq;

            data.lines = new Line[]
              {
                      new Line {
                          alignment = "LEFT",
                          format = "TEXT",
                          lineType = "BOLD",
                          value = "500"
                      }
              };
          
            ;

            json = new JavaScriptSerializer().Serialize(data);






            //if (status == "true")
            //{
            //    pin = GetPin("0000");
            //}
            //else
            //{
            //    //string value = Invoice("");
            //    //LastRequestID("");
            //    //RelevantNumber("");
            //    var invoicejsonbody = invoiceBody();
            //    string value = Invoice(invoicejsonbody);
            //    var data = GetReports("");
            //}
        }

        public static void CreateInvoiceHeader(string InvoiceNumber , string TransactionType,string NumberOfLines)
        {
            var data = new Invoice
            {
                transactionType =Convert.ToInt32(TransactionType),
                relevantNumber = InvoiceNumber,
                 NoofLines =NumberOfLines
            };          
        }
        public static void BuyersInfo(string BuyerAddress, string BuyerName, string BuyerPhone,string PinOfBuyer)
        {
            data.buyer = new Buyer
            {
                buyerAddress = BuyerAddress,
                buyerName = BuyerName,
                buyerPhone = BuyerPhone,
                pinOfBuyer = PinOfBuyer,                
            };

           
        }
        public static List<Item> imageArrayq = new List<Item>();
        

        public static void CreateItemLines(string HsCode, string ItemName, int Quantity, int UnitPrice,string CreditMemoAmount)
        {
            
            var data = new Item
            {
                hsCode = HsCode,
                name = ItemName,
                quantity = Quantity,
                unitPrice = UnitPrice,
                totalAmount = Convert.ToInt32(CreditMemoAmount)
            };
            var stringjson = JsonConvert.SerializeObject(data);

            imageArrayq.Add(data);
            
            }

        private static string invoiceBody()
        {
            var Item = "";

            foreach (var item in Item)
            {
                for (var i = 0; i < item; i++)
                {
                    var hsCode = "0011.11.00";
                    var name = "salt";
                    var quantity = 1;
                    var totalAmount = 1;
                    var unitPrice = 1F;
                }
            }
            Invoice invoice = new Invoice()
            {
                transactionType = 0,               
                invoiceType = 0,
                relevantNumber = "0000000000000000003",               
                payment = new Payment[]
                {
                        new Payment { 
                            amount = 2, 
                            paymentType = "CASH" }
                },
                buyer = new Buyer
                {
                    buyerAddress = "pobox0100",
                    buyerName = "MEJA Pharmaceuticals limited",
                    buyerPhone = "+254748108689",
                    pinOfBuyer = "P051819591D"
                },
                cashier = "",
                items = new Item[]
                {

                        new Item {
                            hsCode = "0011.11.00",
                            name = "salt",
                            quantity = 1,
                            totalAmount = 1,
                            unitPrice = 1F,

                        },
                        new Item {
                            hsCode = "0011.11.00",
                            name = "Milk",
                            quantity = 1,
                            totalAmount = 1,
                            unitPrice = 1F,

                        }
                },
                lines = new Line[]
                {
                        new Line{
                        alignment ="LEFT",
                        format ="TEXT",
                        lineType ="BOLD",
                        value ="500"
                        }
                }  
            };
            string stringjson = JsonConvert.SerializeObject(invoice);
            return stringjson;
        }

        private static string GetReports(string jsonbody)
        {

            var client = new RestClient("http://192.168.100.21:8086/api/v3/reports");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = jsonbody;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var GetReports = response.Content;
            Console.WriteLine(response.Content);
            return GetReports;
        }

        private static void RelevantNumber(string relevantNumber)
        {
            string RelevantNumberUrl = "http://192.168.100.29:8086/api/v3/transactions/" + relevantNumber;
            var client = new RestClient(RelevantNumberUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        private static void LastRequestID(string requestId)
        {
            string resquestidUrl = "http://192.168.100.29:8086/api/v3/invoices/" + requestId;
            var client = new RestClient(resquestidUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        private static string Invoice(string Invoicebody)
        {
            var client = new RestClient("http://192.168.100.21:8086/api/v3/invoices");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = Invoicebody;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var Invoice = response.Content;
            Console.WriteLine(response.Content);
            return Invoice;
        }

        private static string GetPin(string password)
        {
            var pinreponse = string.Empty;
            var client = new RestClient("http://192.168.100.21:8086/api/v3/pin");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/plain");
            var body = password;
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            pinreponse = response.Content;
            Console.WriteLine(response.Content);
            return pinreponse;
        }

        public static string GetStatus()
        {
            var statusreponse = string.Empty;
            var client = new RestClient("http://192.168.100.21:8086/api/v3/status");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            statusreponse = response.Content;
            return statusreponse;
        }
    }
}
