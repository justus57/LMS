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
    public class AppraisalApplicationController : Controller
    {
        static string _AppraisalHeaderNumber = "";
        // GET: AppraisalApplication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AppraisalApplication()
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

            var log = System.Web.HttpContext.Current.Session["logged"] = "yes";
            var passRequired = System.Web.HttpContext.Current.Session["RequirePasswordChange"] = true || false;
            //check if user is logged
            if ((string)log == "No")
            {
                Response.Redirect("/Account/login");
            }
            else if ((string)log == "yes")
            {
                if ((object)passRequired == "true")
                {
                    Response.Redirect("/Account/OneTimePassword");
                }
                else
                {
                    string s = Request.QueryString["id"].Trim();

                    if (s == "")
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        if (s == "")
                        {
                            Response.Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            string AppraisalHeader = AppFunctions.Base64Decode(s);
                            _AppraisalHeaderNumber = AppraisalHeader;
                            LoadTable(AppraisalHeader);
                        }
                    }
                }
            }
            return View();
        }

        private void LoadTable(string AppraisalHeaderNumber)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            StringBuilder html = new StringBuilder();

            html.Append("<form id='needs-validation' novalidate autocomplete='off' method ='post'>");
            html.Append("<div class='card-body'>");

            foreach (var AppraisalSection in CreateAppraisalXMLREquests.AppraisalSection("ViewAppraisal"))
            {
                html.Append("<ol>");

                html.Append("<H5> " + AppraisalSection.Value + "</H5>");//

                string quenss = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportAll", AppraisalSection.Key);

                dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

                foreach (var QuestionsToFill in stuff)
                {
                    string QuestnNumber = QuestionsToFill.QuestionNumber;

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

                            foreach (var PML in AppraisalsXMLRequests.GetPerformanceMeasurementLevels())
                            {
                                string RadioText = PML.Key;
                                string PMLNumber = PML.Value;
                                html.Append("<div class='custom-control custom-radio'>");
                                html.Append("<input type='radio' class='custom-control-input' id='MVC1_OptionNumber_" + QuestnNumber + "_" + PMLNumber + "' name='radio_" + QuestnNumber + "' value ='" + RadioText + "' required>");
                                html.Append("<label class='custom-control-label' for='MVC1_OptionNumber_" + QuestnNumber + "_" + PMLNumber + "'>" + RadioText + "</label>");
                                html.Append("<div class='invalid-feedback'>Choose the performance measurement level</div> ");
                                html.Append("</div>");
                                html.Append("<br>");
                            }

                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250></textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                        }
                        else if (QuestionsToFill.PerformanceMeasurementType == "3")
                        {
                            string WeightedScore = QuestionsToFill.WeightScoreValue;
                            //Weighted Score
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Weighted score value</label>");
                            html.Append("<input type ='text' class='form-control' id='text_" + QuestnNumber + "' name='weighttxt_" + QuestnNumber + "' value ='" + Convert.ToInt16(Math.Floor(Convert.ToDouble(WeightedScore))) + "' disabled ='true'> ");
                            html.Append("<div class='invalid-feedback'>You must provide a score for this target</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                            // user input
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Your score value</label>");
                            html.Append("<input type ='number' class='form-control' id='numb_" + QuestnNumber + "' name='text_" + QuestnNumber + "' min='1' max='" + Convert.ToInt16(Math.Floor(Convert.ToDouble(WeightedScore))) + "' required> ");
                            html.Append("<div class='invalid-feedback'>You must provide a score for this target. Weight values must fall between 0 and " + Convert.ToInt16(Math.Floor(Convert.ToDouble(WeightedScore))) + "</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                            //desc
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250></textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                        }
                        else
                        {
                            html.Append("<div>");
                            html.Append("<label for='basic-url'>Employee comments</label>");
                            html.Append("<textarea class='form-control MultiLineLimit' id='comment_" + QuestnNumber + "' name='comment_" + QuestnNumber + "' rows='5' required data-limit=250></textarea>");
                            html.Append("<p class='pull-right text-muted small MultiLineLimitComment' id='count_message_" + QuestnNumber + "'></p>");
                            html.Append("<div class='invalid-feedback'>Describe the level of performance achieved against each performance objective targets citing verifiable performance indicators</div>");
                            html.Append("</div>");
                            html.Append("<br>");
                            html.Append("</li>");
                        }
                    }
                }

                html.Append("</ol>");
            }

            html.Append("</div>");
            html.Append("<div class='card-footer bg-light text-right'>");
            html.Append("<button id='Save' type='submit' class='btn btn-primary'>Save Changes</button> ");
            html.Append("<button id='Submit' type='submit' class='btn btn-primary'>Submit Appraisal</button> ");
            html.Append("</div>");
            html.Append("</form> ");

            string strText = html.ToString();
            ViewBag.LoadTable = strText;
            ////Append the HTML string to Placeholder.
            // placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }
       
        public static string Save(NameValue[] formVars)
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
                    _Description = AppFunctions.EscapeInvalidXMLCharacters(_Description);
                    string _WeightedScore = choicecomment.WeightedScore;

                    RecordAppraisalResponse = AppraisalsXMLRequests.RecordResponses("", username, _AppraisalHeaderNumber, AppraisalTargetNumber, _Choice, _Description, _WeightedScore);
                }

                string AppraisalResponsesDocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalResponses");
                dynamic jsonDocumentNo = JObject.Parse(AppraisalResponsesDocumentNoXMLResponse);

                string employeeAppraisalHeaderNo = jsonDocumentNo.DocumentNo;
                string CreateEmployeeAppraisalHeaderResponse = AppraisalsXMLRequests.CreateEmployeeAppraisalHeader(_AppraisalHeaderNumber, username, employeeAppraisalHeaderNo);

                Msg = "The appraisal responses were successfully saved";

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
        
        public static string Submit(NameValue[] formVars)
        {
            string username =System.Web.HttpContext.Current.Session["Username"].ToString();

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
                    _Description = AppFunctions.EscapeInvalidXMLCharacters(_Description);
                    string _WeightedScore = choicecomment.WeightedScore;

                    RecordAppraisalResponse = AppraisalsXMLRequests.RecordResponses("", username, _AppraisalHeaderNumber, AppraisalTargetNumber, _Choice, _Description, _WeightedScore);
                }

                string AppraisalResponsesDocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalResponses");
                dynamic jsonDocumentNo = JObject.Parse(AppraisalResponsesDocumentNoXMLResponse);

                string EmployeeAppraisalHeaderNo = jsonDocumentNo.DocumentNo;
                string CreateEmployeeAppraisalHeaderResponse = AppraisalsXMLRequests.CreateEmployeeAppraisalHeader(_AppraisalHeaderNumber, username, EmployeeAppraisalHeaderNo);

                string SubmitAppraisalResponse = AppraisalsXMLRequests.EmployeeSubmitAppraisal(_AppraisalHeaderNumber, EmployeeAppraisalHeaderNo, username);

                dynamic json = JObject.Parse(SubmitAppraisalResponse);

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
    }
}