using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class ViewCreatedAppraisalController : Controller
    {
        static string employeeNumber = "";
        // GET: ViewCreatedAppraisal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewCreatedAppraisal()
        {
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

            if (Session["Logged"].Equals("No"))
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
                    string i = Request.QueryString["id"].Trim();

                    if (i == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }

                    else
                    {
                        string AppraisalHeader = AppFunctions.Base64Decode(i);

                        LoadAppraisal(AppraisalHeader);

                    }
                }
            }
            return View();
        }

        private void LoadAppraisal(string AppraisalHeaderNumber)
        {
            string EmployeeComment = ""; //HR text area comment
            string RadioButtonDisabled = "";
            string TextFieldDisabled = "";

            EmployeeComment = "disabled = 'true'";
            RadioButtonDisabled = "disabled = 'true'";
            TextFieldDisabled = "disabled = 'true'";

            StringBuilder html = new StringBuilder();

            html.Append("<form id='needs-validation' novalidate autocomplete='off' method ='post'>");
            html.Append("<div class='card-body'>");
            html.Append("<fieldset>");//" + FieldsEditable + "

            foreach (var AppraisalSection in CreateAppraisalXMLREquests.AppraisalSection("ViewAppraisal"))
            {
                html.Append("<ol>");

                html.Append("<H5> " + AppraisalSection.Value + "</H5>");

                string quenss = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportAll", AppraisalSection.Key);

                dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

                foreach (var QuestionsToFill in stuff)
                {
                    string QuestnNumber = QuestionsToFill.QuestionNumber;
                    //get question responses

                    string questionsresponse = AppraisalsXMLRequests.GetResponse(QuestnNumber, AppraisalHeaderNumber, employeeNumber);
                    dynamic responseObject = JObject.Parse(questionsresponse);

                    ///Question Lines
                    string comment = responseObject.CommentSubmitted;
                    string supervisorComment = responseObject.SupervisorComent;
                    string weightedScore = responseObject.WeightedScore;

                    if (QuestionsToFill.QuestionDescription != "")
                    {
                        html.Append("<li><b> " + QuestionsToFill.QuestionDescription + "</b></li>");

                        html.Append("<ul class=''>");

                        ///Question Lines


                        foreach (var QuestionObjectives in AppraisalsXMLRequests.GetQuestionObjectives(AppraisalHeaderNumber, QuestnNumber))
                        {
                            if (QuestionObjectives.Value.Length > 1)
                            {
                                html.Append("<li class=''>" + QuestionObjectives.Value + "</li>");
                            }
                        }
                        html.Append("</ul>");
                        html.Append("<br>");

                        if (QuestionsToFill.PerformanceMeasurementType == "2")
                        {
                            html.Append("<label>Your response</label>");
                            html.Append("<br>");

                            string userResponse = responseObject.ChoiceSelected; //user choices for choice questiomn

                            foreach (var PML in AppraisalsXMLRequests.GetPerformanceMeasurementLevels())
                            {
                                string RadioText = PML.Key;
                                string PMLNumber = PML.Value;
                                html.Append("<div class='custom-control custom-radio'>");

                                if (userResponse == RadioText)
                                {
                                    html.Append("<input type='radio' class='custom-control-input' id='MVC1_OptionNumber_" + QuestnNumber + "_" + PMLNumber + "' name='radio_" + QuestnNumber + "' value ='" + RadioText + "' checked=''  required " + RadioButtonDisabled + ">");
                                }
                                else
                                {
                                    html.Append("<input type='radio' class='custom-control-input' id='MVC1_OptionNumber_" + QuestnNumber + "_" + PMLNumber + "' name='radio_" + QuestnNumber + "' value ='" + RadioText + "'  required " + RadioButtonDisabled + " >");
                                }

                                html.Append("<label class='custom-control-label' for='MVC1_OptionNumber_" + QuestnNumber + "_" + PMLNumber + "'>" + RadioText + "</label>");
                                html.Append("<div class='invalid-feedback'>Choose the performance measurement level</div> ");
                                html.Append("</div>");
                                html.Append("<br>");
                            }

                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250 " + EmployeeComment + ">" + comment + "</textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");


                            html.Append("<br>");
                            html.Append("</li>");
                        }
                        else if (QuestionsToFill.PerformanceMeasurementType == "3")
                        {
                            string WeightedScore = QuestionsToFill.WeightScoreValue;
                            //Weighted Score
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Weighted score value</label>");
                            html.Append("<input type ='text' class='form-control' id='text_" + QuestnNumber + "' name='weighttxt_" + QuestnNumber + "' value ='" + Convert.ToInt16(Math.Floor(Convert.ToDouble(WeightedScore))) + "' disabled = 'true'> ");
                            html.Append("<div class='invalid-feedback'>You must provide a score for this target</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");

                            //your score 

                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Your score value</label>");
                            html.Append("<input type ='number' class='form-control' id='numb_" + QuestnNumber + "' name='text_" + QuestnNumber + "' value ='" + Convert.ToInt16(Math.Floor(Convert.ToDouble(weightedScore))) + "' " + TextFieldDisabled + " min='1' max='" + Convert.ToInt16(Math.Floor(Convert.ToDouble(WeightedScore))) + "'> ");
                            html.Append("<div class='invalid-feedback'>You must provide a score for this target</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                            //desc
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250  " + EmployeeComment + ">" + comment + "</textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");


                            html.Append("<br>");
                            html.Append("</li>");
                        }
                        else
                        {
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250  " + EmployeeComment + ">" + comment + "</textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");



                            html.Append("<br>");
                            html.Append("</li>");
                        }
                    }
                }

                html.Append("</ol>");
                //comment on sections

                string HRCommentResponse = CreateAppraisalXMLREquests.GetAppraisalSectionComment(AppraisalSection.Key, AppraisalHeaderNumber, employeeNumber);
                dynamic HRCommentResponseOject = JsonConvert.DeserializeObject(HRCommentResponse);
                string SectionComment = HRCommentResponseOject.HRAppraisalSectionComment;
            }
            html.Append("</fieldset>");

            //efrdfd
            string HRAppraisalCommentResponse = AppraisalsXMLRequests.GetAppraisalHRComment(employeeNumber, AppraisalHeaderNumber);
            dynamic HRAppraisalCommentResponseOject = JsonConvert.DeserializeObject(HRAppraisalCommentResponse);

            string HRComment = HRAppraisalCommentResponseOject.HRComment;

            html.Append("</div>");

            html.Append("</form> ");

            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
          //  placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }
    }
}