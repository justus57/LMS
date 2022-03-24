﻿using LMS.CustomsClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class AppraisalsController : Controller
    {
        // GET: Appraisals
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Appraisals()
        {
            Session["ErrorMessage"] = "";

            System.Web.HttpContext.Current.Session["IsAdvanceActive"] = "";
            System.Web.HttpContext.Current.Session["IsDashboardActive"] = "";
            System.Web.HttpContext.Current.Session["IsClaimActive"] = "";
            System.Web.HttpContext.Current.Session["IsSurrenderActive"] = "";
            System.Web.HttpContext.Current.Session["IsAppriasalActive"] = "active";
            System.Web.HttpContext.Current.Session["IsApprovalEntriesActive"] = "";
            System.Web.HttpContext.Current.Session["IsLeavesActive"] = "";
            System.Web.HttpContext.Current.Session["IsRecallActive"] = "";
            System.Web.HttpContext.Current.Session["IsReportsActive"] = "";
            System.Web.HttpContext.Current.Session["IsTrainingActive"] = "";
            System.Web.HttpContext.Current.Session["IsProfileActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";
            System.Web.HttpContext.Current.Session["IsTransportRequestActive"] = "";

            if (Session["Logged"].Equals("No"))//set to No
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
                    string status = Request.QueryString["status"].Trim();


                    if (status == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        LoadTable(status);
                    }
                }
            }
            return View();
        }
        private void LoadTable(string status)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            DataTable dt = new DataTable();


            if (status == "New")
            {
                dt = AppraisalsXMLRequests.GetAppraisalsToFill(username);
            }
            else
            {
                string statusparm = "";
                string requestAs = "";
                string owner = Request.QueryString["owner"].Trim();

                if (owner == "Employee")
                {
                    if (status == "Open")
                    {
                        statusparm = "Open";
                        requestAs = "Employee";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Submitted")
                    {
                        statusparm = "Submitted";
                        requestAs = "Employee";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "HR")
                    {
                        statusparm = "SentToHR";
                        requestAs = "Employee";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Closed")
                    {
                        statusparm = "Closed";
                        requestAs = "Employee";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                }
                else if (owner == "Approver")
                {
                    if (status == "Open")
                    {
                        statusparm = "Open";
                        requestAs = "Supervisor";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Submitted")
                    {
                        statusparm = "Submitted";
                        requestAs = "Supervisor";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                }
                else if (owner == "HR")
                {
                    if (status == "Open")
                    {
                        statusparm = "Open";
                        requestAs = "HRManager";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                    if (status == "Closed")
                    {
                        statusparm = "Closed";
                        requestAs = "HRManager";
                        dt = AppraisalsXMLRequests.GetFilledAppraisal(statusparm, username, requestAs);
                    }
                }

            }
            // DataTable 
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
            ////Append the HTML string to Placeholder.
            //placeholder.Controls.Add(new Literal { Text = html.ToString() });
            ViewBag.Table = strText;
        }
        private void ClearTable(DataTable table)
        {
            try
            {
                table.Clear();
            }
            catch (DataException e)
            {
                // Process exception and return.
                Console.WriteLine("Exception of type {0} occurred.",
                    e.GetType());
            }

        }
    }
}