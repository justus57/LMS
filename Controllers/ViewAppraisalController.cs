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
    public class ViewAppraisalController : Controller
    {
        static string AppraisalHeaderNumber = "";
        static string EmployeeAppraisalHeaderNo = "";
        static string employeeNumber = "";
        //static string username = "EMP001";
        // GET: ViewAppraisal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAppraisal()
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
                    string s = Request.QueryString["status"].Trim();
                    string empNo = Request.QueryString["emp"].Trim();
                    string Whoviewer = Request.QueryString["viewer"].Trim();
                    string _EmployeeAppraisalHeaderNo = Request.QueryString["EmployeeAppraisalHeaderNo"].Trim();
                    if (Whoviewer == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    if (i == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    if (_EmployeeAppraisalHeaderNo == "")
                    {

                    }

                    else
                    {
                        string AppraisalHeader = AppFunctions.Base64Decode(i);
                        //string status = AppFunctions.Base64Decode(s);
                        string viewer = AppFunctions.Base64Decode(Whoviewer);
                        AppraisalHeaderNumber = AppraisalHeader;
                        empNo = AppFunctions.Base64Decode(empNo);
                        employeeNumber = empNo;
                        EmployeeAppraisalHeaderNo = AppFunctions.Base64Decode(_EmployeeAppraisalHeaderNo);

                        LoadAppraisal(AppraisalHeader, s, empNo, viewer);

                    }
                }
            }
            return View();
        }

        private void LoadAppraisal(string AppraisalHeaderNumber, string status, string username, string viewer)
        {
            //set view permisions
            string SupervisorComment = ""; //supervisor text area comment
            string HumanResourceComment = ""; //HR text area comment
            string EmployeeComment = ""; //HR text area comment
            string RadioButtonDisabled = "";
            string TextFieldDisabled = "";

            if (status == "Closed")
            {
                // FieldsEditable = "disabled";
                SupervisorComment = "disabled = 'true'";
                HumanResourceComment = "disabled = 'true'";
                EmployeeComment = "disabled = 'true'";
                RadioButtonDisabled = "disabled = 'true'";
                TextFieldDisabled = "disabled = 'true'";

            }
            else
            {

                if (status == "Submitted")
                {
                    if (viewer == "Employee")
                    {
                        SupervisorComment = "disabled = 'true'";
                        HumanResourceComment = "disabled = 'true'";
                        EmployeeComment = "disabled = 'false'";
                        RadioButtonDisabled = "disabled = 'false'";
                        TextFieldDisabled = "disabled = 'false'";
                    }
                    if (viewer == "Supervisor")
                    {
                        SupervisorComment = "disabled = 'true'";
                        HumanResourceComment = "disabled = 'true'";
                        EmployeeComment = "disabled = 'true'";
                        RadioButtonDisabled = "disabled = 'true'";
                        TextFieldDisabled = "disabled = 'true'";
                    }
                    if (viewer == "HRManager")
                    {
                        SupervisorComment = "disabled = 'true'";
                        HumanResourceComment = "disabled = 'false'";
                        EmployeeComment = "disabled = 'true'";
                        RadioButtonDisabled = "disabled = 'true'";
                        TextFieldDisabled = "disabled = 'true'";
                    }

                }
                else if (status == "Open")
                {
                    if (viewer == "Employee")
                    {
                        SupervisorComment = "disabled = 'true'";
                        HumanResourceComment = "disabled = 'true'";
                        RadioButtonDisabled = "disabled = 'false'";
                        TextFieldDisabled = "disabled = 'false'";

                        // EmployeeComment = "disabled = 'false'";
                    }
                    if (viewer == "Supervisor")
                    {
                        // SupervisorComment = "disabled = 'false'";
                        HumanResourceComment = "disabled = 'true'";
                        // EmployeeComment = "disabled = 'true'";
                        // RadioButtonDisabled = "disabled = 'true'";
                        TextFieldDisabled = "disabled = 'true'";
                    }
                    if (viewer == "HRManager")
                    {
                        SupervisorComment = "disabled = 'true'";
                        //HumanResourceComment = "disabled = 'false'";
                        EmployeeComment = "disabled = 'true'";
                        RadioButtonDisabled = "disabled = 'true'";
                        TextFieldDisabled = "disabled = 'true'";
                    }

                }
            }

            StringBuilder html = new StringBuilder();

            html.Append("<form id='needs-validation' novalidate autocomplete='off' method ='post'>");
            html.Append("<div class='card-body'>");
            html.Append("<fieldset>");//" + FieldsEditable + "

            foreach (var AppraisalSection in CreateAppraisalXMLREquests.AppraisalSection("ViewAppraisal"))
            {
                html.Append("<ol>");

                html.Append("<H5> " + AppraisalSection.Value + "</H5>");//

                string quenss = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportAll", AppraisalSection.Key);

                dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

                foreach (var QuestionsToFill in stuff)
                {
                    string QuestnNumber = QuestionsToFill.QuestionNumber;
                    //get question responses

                    string questionsresponse = AppraisalsXMLRequests.GetResponse(QuestnNumber, AppraisalHeaderNumber, username);
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


                            //display supervisor comments

                            if (status == "Open")
                            {
                                if (viewer == "Employee")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                    }
                                }
                                if (viewer == "Supervisor")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                                if (viewer == "HRManager")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");

                                    }
                                }

                            }
                            else
                            {
                                if (supervisorComment != "")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                            }

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

                            //display supervisor comments

                            if (status == "Open")
                            {
                                if (viewer == "Employee")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");

                                    }
                                }
                                if (viewer == "Supervisor")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                                if (viewer == "HRManager")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");

                                    }
                                }

                            }
                            else
                            {
                                if (supervisorComment != "")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                            }

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


                            ////display this
                            if (status == "Open")
                            {
                                if (viewer == "Employee")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");

                                    }
                                }
                                if (viewer == "Supervisor")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                                if (viewer == "HRManager")
                                {
                                    if (supervisorComment != "")
                                    {
                                        html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                        html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                        html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                        html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");

                                    }
                                }

                            }
                            else
                            {
                                if (supervisorComment != "")
                                {
                                    html.Append("<label for='basic-url'>Supervisor Comments</label>");
                                    html.Append("<textarea class='form-control' id='supervisor_" + QuestnNumber + "' name='supervisor_" + QuestnNumber + "' rows='5' required data-limit=250 " + SupervisorComment + " > " + supervisorComment + "</textarea>");
                                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                                }
                            }

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
                //efrdfd
                if (status == "Open" && viewer == "HRManager" || status == "Closed" && viewer == "HRManager" || status == "Closed" && viewer == "Employee")
                {
                    html.Append("<hr>");
                    html.Append("<label for='HRManager_'>HR Manager Comments on section</label>");
                    html.Append("<textarea class='form-control' id='HRManagerSection_" + AppraisalSection.Key + "' name='HRManagerSection_" + AppraisalSection.Key + "' rows='5' required data-limit=250 " + HumanResourceComment + ">" + SectionComment + "</textarea>");
                    html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_'></p>");
                    html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
                }
            }
            html.Append("</fieldset>");

            //efrdfd
            string HRAppraisalCommentResponse = AppraisalsXMLRequests.GetAppraisalHRComment(employeeNumber, AppraisalHeaderNumber);
            dynamic HRAppraisalCommentResponseOject = JsonConvert.DeserializeObject(HRAppraisalCommentResponse);

            string HRComment = HRAppraisalCommentResponseOject.HRComment;

            if (status == "Open" && viewer == "HRManager" || status == "Closed" && viewer == "HRManager" || status == "Closed" && viewer == "Employee")
            {
                html.Append("<hr>");
                html.Append("<label for='HRManager_'>HR Manager Comments</label>");
                html.Append("<textarea class='form-control' id='HRManager_Comment' name='HRManager_" + AppraisalHeaderNumber + "' rows='5' required data-limit=250 " + HumanResourceComment + ">" + HRComment + "</textarea>");
                html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_'></p>");
                html.Append("<div class='invalid-feedback'>A comment must be supplied</div>");
            }


            html.Append("</div>");

            ///set nuttons visble or not 
            ///
            if (status == "Open" && viewer == "Employee")
            {
                html.Append("<div class='card-footer bg-light text-right'>");
                html.Append("<button id='EmployeeSave' type='submit' class='btn btn-primary'>Save Changes</button> ");
                html.Append("<button id='EmployeeSubmit' type='submit' class='btn btn-primary'>Submit Appraisal</button> ");
                html.Append("</div>");
            }
            if (status == "Open" && viewer == "HRManager")
            {
                html.Append("<div class='card-footer bg-light text-right'>");
                html.Append("<button id='HRSave' type='submit' class='btn btn-primary'>Save Comments</button> ");
                html.Append("<button id='HRSubmit' type='submit' class='btn btn-primary'>Close Appraisal</button> ");
                html.Append("</div>");
            }
            if (status == "Open" && viewer == "Supervisor")
            {
                html.Append("<div class='card-footer bg-light text-right'>");
                html.Append("<button id='SupervisorSave' type='submit' class='btn btn-primary'>Send Appraisal To Employee</button> ");
                html.Append("<button id='SupervisorSubmit' type='submit' class='btn btn-primary'>Submit Appraisal To HR</button> ");
                html.Append("</div>");
            }
            //if (status == "Closed")
            //{
            //    html.Append("<div class='card-footer bg-light text-right'>");
            //    html.Append("<button id='PrintAppraisal' type='submit' class='btn btn-primary'>Print</button> ");
            //    html.Append("</div>");
            //}

            html.Append("</form> ");

            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
            ViewBag.ViewAppraisal = strText;
           // placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }
        
        public static string HRSave(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string status = "";
            string Msg = "";
            string num = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string HRAppraisalSectionComment = "";
                    string HRAppraisalComment = "";

                    if (nv.name.StartsWith("HRManagerSection_"))
                    {
                        QuestionNumber = nv.name.Substring(17);
                        HRAppraisalSectionComment = nv.value;
                    }
                    else

                    if (nv.name.StartsWith("HRManager_"))
                    {
                        QuestionNumber = nv.name.Substring(10);
                        HRAppraisalComment = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        HRAppraisalSectionComment = HRAppraisalSectionComment,
                        HRAppraisalComment = HRAppraisalComment

                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses);
                string RecordAppraisalResponse = null;

                foreach (var QuestionsFilled in stuff)
                {
                    string QuestionNumber = QuestionsFilled.QuestionNumber;
                    string HRAppraisalSectionComment = QuestionsFilled.HRAppraisalSectionComment;
                    string HRAppraisalComment = QuestionsFilled.HRAppraisalComment;

                    if (QuestionNumber == AppraisalHeaderNumber)
                    {
                        RecordAppraisalResponse = AppraisalsXMLRequests.SaveHRComment(AppraisalHeaderNumber, employeeNumber, HRAppraisalComment, "Return");
                    }
                    else
                    {
                        //Fill sections
                        //get section comment number
                        string AppraisalSectionCommentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalSectionComment");
                        dynamic jsonAppraisalSectionCommentNo = JObject.Parse(AppraisalSectionCommentNoXMLResponse);
                        status = jsonAppraisalSectionCommentNo.Status;

                        if (status == "000")
                        {
                            string AppraisalSectionCommentNo = jsonAppraisalSectionCommentNo.DocumentNo;
                            RecordAppraisalResponse = AppraisalsXMLRequests.CreateAppraisalSectionComment(AppraisalSectionCommentNo, AppraisalHeaderNumber, employeeNumber, username, QuestionNumber, HRAppraisalSectionComment);
                        }

                    }

                }
                dynamic json = JObject.Parse(RecordAppraisalResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string HRSubmit(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string status = "";
            string Msg = "";
            string num = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string HRAppraisalSectionComment = "";
                    string HRAppraisalComment = "";

                    if (nv.name.StartsWith("HRManagerSection_"))
                    {
                        QuestionNumber = nv.name.Substring(17);
                        HRAppraisalSectionComment = nv.value;
                    }
                    else

                    if (nv.name.StartsWith("HRManager_"))
                    {
                        QuestionNumber = nv.name.Substring(10);
                        HRAppraisalComment = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        HRAppraisalSectionComment = HRAppraisalSectionComment,
                        HRAppraisalComment = HRAppraisalComment

                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses);
                string RecordAppraisalResponse = null;

                foreach (var QuestionsFilled in stuff)
                {
                    string QuestionNumber = QuestionsFilled.QuestionNumber;
                    string HRAppraisalSectionComment = QuestionsFilled.HRAppraisalSectionComment;
                    string HRAppraisalComment = QuestionsFilled.HRAppraisalComment;

                    if (QuestionNumber == AppraisalHeaderNumber)
                    {
                        RecordAppraisalResponse = AppraisalsXMLRequests.SaveHRComment(AppraisalHeaderNumber, employeeNumber, HRAppraisalComment, "Return");
                    }
                    else
                    {
                        //Fill sections
                        //get section comment number
                        string AppraisalSectionCommentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalSectionComment");
                        dynamic jsonAppraisalSectionCommentNo = JObject.Parse(AppraisalSectionCommentNoXMLResponse);
                        status = jsonAppraisalSectionCommentNo.Status;

                        if (status == "000")
                        {
                            string AppraisalSectionCommentNo = jsonAppraisalSectionCommentNo.DocumentNo;
                            RecordAppraisalResponse = AppraisalsXMLRequests.CreateAppraisalSectionComment(AppraisalSectionCommentNo, AppraisalHeaderNumber, employeeNumber, username, QuestionNumber, HRAppraisalSectionComment);
                        }
                    }

                }
                //close this appraisal buv
                RecordAppraisalResponse = AppraisalsXMLRequests.CloseAppraisal(employeeNumber, AppraisalHeaderNumber, username);
                dynamic json = JObject.Parse(RecordAppraisalResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string SupervisorSave(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string status = " 999";
            string Msg = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string SupervisorComent = "";

                    if (nv.name.StartsWith("supervisor_"))
                    {
                        QuestionNumber = nv.name.Substring(11);
                        SupervisorComent = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        SupervisorComent = SupervisorComent
                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses);
                string RecordAppraisalResponse = null;

                foreach (var QuestionsFilled in stuff)
                {
                    string QuestionNumber = QuestionsFilled.QuestionNumber;
                    string SupervisorComent = QuestionsFilled.SupervisorComent;
                    RecordAppraisalResponse = AppraisalsXMLRequests.SubmitComment(QuestionNumber, SupervisorComent, employeeNumber, AppraisalHeaderNumber);
                    //Msg = QuestionNumber + ":" + SupervisorComent + ":" + employeeNumber + ":" + AppraisalHeaderNumber;
                }

                dynamic json = JObject.Parse(RecordAppraisalResponse);
                status = json.Status;

                if (status == "000")
                {
                    //'reject' appraisal here
                    string SendAppraisalBackToEmployeeXMResponse = AppraisalsXMLRequests.SendAppraisalBackToEmployee(AppraisalHeaderNumber, username);
                    dynamic jsonSendAppraisalBackToEmployeeXMResponse = JObject.Parse(SendAppraisalBackToEmployeeXMResponse);
                    status = jsonSendAppraisalBackToEmployeeXMResponse.Status;

                    if (status == "000")
                    {
                        Msg = "Supervisor responses have been succesfully recorded and appraisal send back to the employee";
                    }
                    else
                    {
                        Msg = json.Msg;
                    }
                }
            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string SupervisorSubmit(NameValue[] formVars)
        {

            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            int Status = 000;
            string Msg = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string SupervisorComent = "";

                    if (nv.name.StartsWith("supervisor_"))
                    {
                        QuestionNumber = nv.name.Substring(11);
                        SupervisorComent = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        SupervisorComent = SupervisorComent
                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses);
                string RecordAppraisalResponse = null;

                foreach (var QuestionsFilled in stuff)
                {
                    string QuestionNumber = QuestionsFilled.QuestionNumber;
                    string SupervisorComent = QuestionsFilled.SupervisorComent;
                    RecordAppraisalResponse = AppraisalsXMLRequests.SubmitComment(QuestionNumber, SupervisorComent, employeeNumber, AppraisalHeaderNumber);
                    //Msg = QuestionNumber + ":" + SupervisorComent + ":" + employeeNumber + ":" + AppraisalHeaderNumber;
                }
                //send to HR
                RecordAppraisalResponse = AppraisalsXMLRequests.SendAppraisalToHR(employeeNumber, AppraisalHeaderNumber);

                //Msg = "Your Appraisal responses have been successfully saved.";
                dynamic json = JObject.Parse(RecordAppraisalResponse);
                Status = json.Status;
                Msg = json.Msg;
            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status.ToString(),
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string EmployeeSave(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            int Status = 000;
            string Msg = "";
            string num = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            List<QuestionResponse> products_to_save = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string Comment = "";
                    string ChoiceSelected = "";
                    string WeightScore = "0";

                    if (nv.name.StartsWith("radio_"))
                    {
                        QuestionNumber = nv.name.Substring(6);
                        ChoiceSelected = nv.value;
                        Comment = "";
                        WeightScore = "0";
                    }
                    else

                    if (nv.name.StartsWith("comment_"))
                    {
                        QuestionNumber = nv.name.Substring(8);
                        ChoiceSelected = "";
                        WeightScore = "0";
                        Comment = nv.value;
                    }
                    if (nv.name.StartsWith("text_"))
                    {
                        QuestionNumber = nv.name.Substring(5);
                        ChoiceSelected = "";
                        Comment = "";
                        WeightScore = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        ChoiceSelected = ChoiceSelected,
                        WeightedScore = WeightScore,
                        CommentSubmitted = Comment
                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses); //.Replace("\"", "'")

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                QuestionResponse field2;

                foreach (var QuestionsFilled in stuff)
                {
                    string questionNumber = QuestionsFilled.QuestionNumber;
                    string Choice = QuestionsFilled.ChoiceSelected;
                    string Description = QuestionsFilled.CommentSubmitted;
                    string WeightedScore = QuestionsFilled.WeightedScore;



                    string choices = "";

                    field2 = new QuestionResponse
                    {
                        ChoiceSelected = Choice,
                        WeightedScore = WeightedScore,
                        CommentSubmitted = Description,
                        QuestionNumber = questionNumber
                    };
                    choices = JsonConvert.SerializeObject(field2);

                    if (!dictionary.ContainsKey(questionNumber))
                    {
                        dictionary.Add(questionNumber, choices);
                    }
                    else
                    {
                        string v = dictionary[questionNumber];

                        dynamic choicecomment = JObject.Parse(v);
                        string myChoice = choicecomment.ChoiceSelected;
                        string myDescription = choicecomment.CommentSubmitted;
                        string myweight = choicecomment.WeightedScore;

                        if (myweight == "")
                        {
                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = myChoice,
                                WeightedScore = WeightedScore,
                                CommentSubmitted = Description,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        else if (myweight != "")
                        {
                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = myChoice,
                                WeightedScore = myweight,
                                CommentSubmitted = Description,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        else if (myChoice == "")
                        {

                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = Choice,
                                WeightedScore = myweight,
                                CommentSubmitted = myDescription,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        dictionary[questionNumber] = choices;
                    }
                }
                //sorted JSON string
                string RecordAppraisalResponse = null;

                foreach (var v in dictionary)
                {
                    string AppraisalTargetNumber = v.Key;
                    string jsoncommentchoices = v.Value;

                    dynamic choicecomment = JObject.Parse(jsoncommentchoices);

                    string _Choice = choicecomment.ChoiceSelected;
                    string _Description = choicecomment.CommentSubmitted;
                    string _WeightedScore = choicecomment.WeightedScore;

                    RecordAppraisalResponse = AppraisalsXMLRequests.UpdateAppraisalResponses(AppraisalHeaderNumber, AppraisalTargetNumber, employeeNumber, _Choice, _Description, _WeightedScore);
                }

                dynamic json = JObject.Parse(RecordAppraisalResponse);

                Status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status.ToString(),
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string EmployeeSubmit(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            string status = "";
            string Msg = "";

            List<QuestionResponse> products = new List<QuestionResponse>();

            List<QuestionResponse> products_to_save = new List<QuestionResponse>();

            QuestionResponse fields;

            try
            {

                foreach (NameValue nv in formVars)
                {
                    string QuestionNumber = "";
                    string Comment = "";
                    string ChoiceSelected = "";
                    string WeightScore = "0";

                    if (nv.name.StartsWith("radio_"))
                    {
                        QuestionNumber = nv.name.Substring(6);
                        ChoiceSelected = nv.value;
                        Comment = "";
                        WeightScore = "0";
                    }
                    else

                    if (nv.name.StartsWith("comment_"))
                    {
                        QuestionNumber = nv.name.Substring(8);
                        ChoiceSelected = "";
                        WeightScore = "0";
                        Comment = nv.value;
                    }
                    if (nv.name.StartsWith("text_"))
                    {
                        QuestionNumber = nv.name.Substring(5);
                        ChoiceSelected = "";
                        Comment = "";
                        WeightScore = nv.value;
                    }
                    fields = new QuestionResponse
                    {
                        QuestionNumber = QuestionNumber,
                        ChoiceSelected = ChoiceSelected,
                        WeightedScore = WeightScore,
                        CommentSubmitted = Comment
                    };

                    products.Add(fields);
                }

                string responses = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                dynamic stuff = JsonConvert.DeserializeObject(responses); //.Replace("\"", "'")

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                QuestionResponse field2;

                foreach (var QuestionsFilled in stuff)
                {
                    string questionNumber = QuestionsFilled.QuestionNumber;
                    string Choice = QuestionsFilled.ChoiceSelected;
                    string Description = QuestionsFilled.CommentSubmitted;
                    string WeightedScore = QuestionsFilled.WeightedScore;



                    string choices = "";

                    field2 = new QuestionResponse
                    {
                        ChoiceSelected = Choice,
                        WeightedScore = WeightedScore,
                        CommentSubmitted = Description,
                        QuestionNumber = questionNumber
                    };
                    choices = JsonConvert.SerializeObject(field2);

                    if (!dictionary.ContainsKey(questionNumber))
                    {
                        dictionary.Add(questionNumber, choices);
                    }
                    else
                    {
                        string v = dictionary[questionNumber];

                        dynamic choicecomment = JObject.Parse(v);
                        string myChoice = choicecomment.ChoiceSelected;
                        string myDescription = choicecomment.CommentSubmitted;
                        string myweight = choicecomment.WeightedScore;

                        if (myweight == "")
                        {
                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = myChoice,
                                WeightedScore = WeightedScore,
                                CommentSubmitted = Description,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        else if (myweight != "")
                        {
                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = myChoice,
                                WeightedScore = myweight,
                                CommentSubmitted = Description,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        else if (myChoice == "")
                        {

                            field2 = new QuestionResponse
                            {
                                ChoiceSelected = Choice,
                                WeightedScore = myweight,
                                CommentSubmitted = myDescription,
                                QuestionNumber = questionNumber
                            };
                            choices = JsonConvert.SerializeObject(field2);
                        }
                        dictionary[questionNumber] = choices;
                    }
                }

                //sorted JSON string

                string RecordAppraisalResponse = null;

                foreach (var v in dictionary)
                {
                    string AppraisalTargetNumber = v.Key;
                    string jsoncommentchoices = v.Value;
                    dynamic choicecomment = JObject.Parse(jsoncommentchoices);

                    string _Choice = choicecomment.ChoiceSelected;
                    string _Description = choicecomment.CommentSubmitted;
                    string _WeightedScore = choicecomment.WeightedScore;
                    AppraisalsXMLRequests.UpdateAppraisalResponses(AppraisalHeaderNumber, AppraisalTargetNumber, employeeNumber, _Choice, _Description, _WeightedScore);
                }

                RecordAppraisalResponse = AppraisalsXMLRequests.EmployeeSubmitAppraisal(AppraisalHeaderNumber, EmployeeAppraisalHeaderNo, username);
                dynamic json = JObject.Parse(RecordAppraisalResponse);

                status = json.Status;
                Msg = json.Msg;

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}