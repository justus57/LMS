using DATECS.Class;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace DATECS
{
    class Program
    {
        private static string pin;

        static void Main(string[] args)
        {
            
            var statusreponse =GetStatus();
            dynamic json = JObject.Parse(statusreponse);
            string status = json.isPinRequired;
            if (status == "true")
            {
                pin = GetPin("0000");
            }
            else 
            {                
               string value = Invoice("");
                //LastRequestID("");
                //RelevantNumber("");
                Invoice invoice = new Invoice()
                {
                    transactionType = 1,
                    ExemptionNumber = "",
                    invoiceType = 2,
                    relevantNumber = "",
                    TraderSystemInvoiceNumber = "",
                    
                    payment = new Payment[]
                    {
                        new Payment { amount = 23313, paymentType = 2133 }
                    },
                    buyer = new Buyer
                    {
                        buyerAddress = "",
                        buyerName = "",
                        buyerPhone ="",
                         pinOfBuyer =""
                    },
                    cashier  = "",
                    items = new Item[]
                    {
                        new Item{  description ="",
                        gtin
                        =}
                    }


                };
               var data = GetReports("");
            }

        }

        private static string GetReports(string jsonbody)
        {

            var client = new RestClient("http://62.8.73.35:8086/reports");
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
