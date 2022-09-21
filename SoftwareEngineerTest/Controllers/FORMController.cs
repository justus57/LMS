using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SoftwareEngineerTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SoftwareEngineerTest.Controllers
{
    public class FORMController : Controller
    {
        private string result;

        // GET: FORM
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult form()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult form(Home model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Home home = new Home()
                    {
                        FullName = model.FullName,
                        address = model.address,
                        Email = model.Email,
                        phone = model.phone
                    };
                    string stringjson = JsonConvert.SerializeObject(home);

                    var client = new RestClient("http://developers.gictsystems.com/api/dummy/submit/");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    var body = stringjson;
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    result = response.Content;
                }
                dynamic json = JObject.Parse(result);
                int statuscode = json.StatusCode;
                string Description = json.Message;

                if (statuscode != 1)
                {
                    TempData["status"] = "success";
                    TempData["Message"] = Description;
                }
                else
                {
                    TempData["status"] = "warning";
                    TempData["Message"] = Description;
                }
            }
            catch (Exception es)
            {
                TempData["Message"] = es;

            }
            return View();
        }

        public ActionResult Table()
        {
            Response.Headers.Add("Refresh", "10");
            var data = Itmes();
            var jsonArray = JArray.Parse(data);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Columns.Add("Age", typeof(string));
            dt.Columns.Add("ACTION", typeof(string));

            foreach (var item in jsonArray)
            {
                var myJsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Class1>(item.ToString());
                dt.Rows.Add(myJsonObject.ID, myJsonObject.Message, myJsonObject.Age, "<a class = 'btn btn-secondary btn-xs' data-toggle='tooltip' title='Edit Application'><span class = 'fa fa-edit'> </span></a> ");

            }
            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            //Table start.
            html.Append("<table class='table table-bordered' id='dataTable' width='100%' cellspacing='0'>");
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
            ViewBag.Table = strText;

            return View();
        }

        private static string Itmes()
        {
            var client = new RestClient("http://developers.gictsystems.com/api/dummy/items/");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer ALDJAK23423JKSLAJAF23423J23SAD3");
            IRestResponse response = client.Execute(request);
            var Resp = response.Content;
            return Resp;
        }
    }
}