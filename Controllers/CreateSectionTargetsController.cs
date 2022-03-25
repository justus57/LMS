using LMS.CustomsClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class CreateSectionTargetsController : Controller
    {static string _AppraisalSection = "";
        // GET: CreateSectionTargets
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateSectionTargets()
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
                    //PerformanceMeasurementType.Items.Clear();

                    //PerformanceMeasurementType.Items.Insert(0, new ListItem("Options", "Options"));
                    //PerformanceMeasurementType.Items.Insert(0, new ListItem("Weighted Score", "WeightedScore"));
                    //PerformanceMeasurementType.Items.Insert(0, new ListItem("Description", "Description"));
                    //PerformanceMeasurementType.Items.Insert(0, new ListItem(" ", ""));

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
                            string AppraisalSection = AppFunctions.Base64Decode(s);
                            _AppraisalSection = AppraisalSection;
                            LoadTable(_AppraisalSection);
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
            string quenss = DefineAppraisalSectionsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportAll", _AppraisalSection);

            dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

            html.Append("<ol>");

            foreach (var QuestionsToFill in stuff)
            {
                string QuestnNumber = QuestionsToFill.QuestionNumber;


                if (QuestionsToFill.QuestionDescription != "")
                {
                    html.Append("<li><b> " + QuestionsToFill.QuestionDescription + "</b> <a href = 'javascript:void(0)' data-id='" + QuestnNumber + "' class='float-right DeleteQuestion' ><i class='fas fa-fw fa-trash'></i></a> <a href = 'javascript:void(0)' data-id='" + QuestnNumber + "' class='float-right EditQuestion' ><i class='fas fa-fw fa-edit'></i></a>");
                    html.Append("<ul class=''>");
                    //Target Objectives

                    foreach (var QuestionObjectives in DefineAppraisalSectionsXMLRequests.GetQuestionObjectives(AppraisalHeaderNumber, QuestnNumber))
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
                    }
                    else if (QuestionsToFill.PerformanceMeasurementType == "3")
                    {
                        //Weighted Score
                        html.Append("<div>");
                        html.Append("<label for='basic-url'>Weighted score value</label>");
                        html.Append("<input type ='text' class='form-control' id='text_" + QuestnNumber + "' name='text_" + QuestnNumber + "' value ='" + QuestionsToFill.WeightScoreValue + "' disabled ='true'> ");
                        html.Append("<div class='invalid-feedback'>You must provide a score for this target</div>");
                        html.Append("</div>");
                        html.Append("<br>");
                        html.Append("</li>");

                    }
                    else
                    {
                        html.Append("<div>");
                        html.Append("<label for='basic-url'>Target description</label>");
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

            html.Append("</div>");
            html.Append("</form> ");

            string strText = html.ToString();
            ////Append the HTML string to Placeholder.
            //placeholder.Controls.Add(new Literal { Text = html.ToString() });
            ViewBag.AppraisalBody = strText;
        }
        
        public static string GetAppraisalSectionList()
        {
            List<AppraisalSection> AppraisalSectionObject = new List<AppraisalSection>();

            foreach (var kvp in CreateAppraisalXMLREquests.AppraisalSection("DefineAppraisalSection"))
            {
                AppraisalSectionObject.Add(new AppraisalSection { Code = kvp.Key, Description = kvp.Value });
            }

            return JsonConvert.SerializeObject(AppraisalSectionObject);
        }
        
        public static string CreateNewAppraisalQuestion(NameValue formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";

            string QuestionObjectiveNumber = "";
            string objectiveDecription = "";
            string QuestionDescription = "";
            string QuestionType = "";
            string weightScoreValue = "0.0";
            // string AppraisalSection = "";


            ArrayList objectivesArray = new ArrayList();


            foreach (NameValue nv in formVars)
            {
                if (nv.name.StartsWith("new_objective"))
                {
                    objectiveDecription = nv.value;

                    //trim
                    if (objectiveDecription.Length > 250)
                    {
                        var truncated = objectiveDecription.Substring(0, 250);

                        objectiveDecription = truncated.ToString();
                    }

                    objectiveDecription = AppFunctions.EscapeInvalidXMLCharacters(objectiveDecription);

                    objectivesArray.Add(objectiveDecription);
                }
                if (nv.name.StartsWith("QuestionDescription"))
                {
                    QuestionDescription = nv.value;
                    QuestionDescription = AppFunctions.EscapeInvalidXMLCharacters(QuestionDescription);
                }
                if (nv.name.StartsWith("PerformanceMeasurementType"))
                {
                    QuestionType = nv.value;
                }
                if (nv.name.StartsWith("WeightedScoreValue"))
                {
                    weightScoreValue = nv.value;

                    if (weightScoreValue == "")
                    {
                        weightScoreValue = "0.0";
                    }
                }

            }

            //save target

            string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("HRAppraisalTarget");
            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);
            Status = jsonDocumentNo.Status;
            string HRAppraisalTargetNo = "";

            if (Status == "000")
            {
                HRAppraisalTargetNo = jsonDocumentNo.DocumentNo;

                string TargetResponse = DefineAppraisalSectionsXMLRequests.CreateNewAppraisalQuestion(HRAppraisalTargetNo, username, _AppraisalSection, AppFunctions.EscapeInvalidXMLCharacters(QuestionDescription), QuestionType, weightScoreValue, _AppraisalSection);
                Msg = "The appraisal section target was created successfully";
                foreach (var item in objectivesArray)
                {
                    //get objectives nos
                    string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("HRAppraisalTargetObjective");
                    dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                    Status = jsonObjectiveNo.Status;

                    if (Status == "000")
                    {
                        string ObjectiveNo = jsonObjectiveNo.DocumentNo;
                        QuestionObjectiveNumber = ObjectiveNo;

                        DefineAppraisalSectionsXMLRequests.CreateAppraisalQuestionObjective(username, _AppraisalSection, QuestionObjectiveNumber, HRAppraisalTargetNo, AppFunctions.EscapeInvalidXMLCharacters(item.ToString()));

                    }

                }

            }
            else
            {
                Msg = jsonDocumentNo.Status;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string Update(NameValue formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";

            string HRAppraisalTargetNo = "";
            string QuestionObjectiveNumber = "";
            string objectiveDecription = "";
            string QuestionDescription = "";
            string QuestionType = "";
            string weightScoreValue = "0.0";

            ArrayList objectivesArray = new ArrayList();


            foreach (NameValue nv in formVars)
            {
                if (nv.name.StartsWith("new_objective"))
                {
                    objectiveDecription = nv.value;

                    //trim
                    if (objectiveDecription.Length > 250)
                    {
                        var truncated = objectiveDecription.Substring(0, 250);

                        objectiveDecription = truncated.ToString();
                    }

                    objectiveDecription = AppFunctions.EscapeInvalidXMLCharacters(objectiveDecription);

                    objectivesArray.Add(objectiveDecription);
                }
                if (nv.name.StartsWith("QuestionDescription"))
                {
                    QuestionDescription = nv.value;
                    QuestionDescription = AppFunctions.EscapeInvalidXMLCharacters(QuestionDescription);
                }
                if (nv.name.StartsWith("PerformanceMeasurementType"))
                {
                    QuestionType = nv.value;
                }
                if (nv.name.StartsWith("WeightedScoreValue"))
                {
                    weightScoreValue = nv.value;

                    if (weightScoreValue == "")
                    {
                        weightScoreValue = "0.0";
                    }
                }

                if (nv.name.StartsWith("AppraisalTargetNo"))
                {
                    HRAppraisalTargetNo = nv.value;
                }
            }

            //save target changes

            string TargetResponse = DefineAppraisalSectionsXMLRequests.CreateNewAppraisalQuestion(HRAppraisalTargetNo, username, _AppraisalSection, AppFunctions.EscapeInvalidXMLCharacters(QuestionDescription), QuestionType, weightScoreValue, _AppraisalSection);

            Msg = "The appraisal section target was updated successfully";

            DefineAppraisalSectionsXMLRequests.DeleteAppraisalTargetObjectives(_AppraisalSection, HRAppraisalTargetNo);

            foreach (var item in objectivesArray)
            {
                //get objectives nos
                string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("HRAppraisalTargetObjective");
                dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                Status = jsonObjectiveNo.Status;

                if (Status == "000")
                {
                    string ObjectiveNo = jsonObjectiveNo.DocumentNo;
                    QuestionObjectiveNumber = ObjectiveNo;

                    DefineAppraisalSectionsXMLRequests.CreateAppraisalQuestionObjective(username, _AppraisalSection, QuestionObjectiveNumber, HRAppraisalTargetNo, AppFunctions.EscapeInvalidXMLCharacters(item.ToString()));
                }
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
        
        public static string GetQuestionDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string QuestnNumber = param1;
            string QuestionDescription = "";
            string Performance_Measurement_Type = "";
            string weightScoreValue = "";

            string TargetDetails = DefineAppraisalSectionsXMLRequests.GetQuestionsToFillJson(_AppraisalSection, QuestnNumber, "ExportSingle", "");

            dynamic stuff = JsonConvert.DeserializeObject(TargetDetails.Replace("\"", "'"));

            foreach (var QuestionsToFill in stuff)
            {
                QuestionDescription = QuestionsToFill.QuestionDescription;
                Performance_Measurement_Type = QuestionsToFill.PerformanceMeasurementType;
                weightScoreValue = QuestionsToFill.WeightScoreValue;
            }

            List<QuestionObjective> QuestionObjectiveObject = new List<QuestionObjective>();

            foreach (var QuestionObjectives in DefineAppraisalSectionsXMLRequests.GetQuestionObjectives(_AppraisalSection, QuestnNumber))
            {
                if (QuestionObjectives.Value.Length > 1)
                {
                    QuestionObjectiveObject.Add(new QuestionObjective { OjectiveCode = QuestionObjectives.Key, ObjectiveDescription = QuestionObjectives.Value });
                }
            }
            //set options

            if (Performance_Measurement_Type == "1")
            {
                Performance_Measurement_Type = "Description";

            }
            else if (Performance_Measurement_Type == "2")
            {
                Performance_Measurement_Type = "Options";
            }
            else if (Performance_Measurement_Type == "3")
            {
                Performance_Measurement_Type = "WeightedScore";
            }

            var _QuestionDetails = new QuestionDetails
            {
                Description = QuestionDescription,
                QuestionNumber = "000",
                Objectives = QuestionObjectiveObject,
                PerformanceMeasurementType = Performance_Measurement_Type,
                WeightScoreValue = weightScoreValue
            };

            return JsonConvert.SerializeObject(_QuestionDetails);

        }
        
        public static string DeleteAppraisalTarget(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string QuestnNumber = param1;
            string response = "";
            string status = "";

            string xmlresponse = DefineAppraisalSectionsXMLRequests.DeleteAppraisalTarget(_AppraisalSection, QuestnNumber);
            DefineAppraisalSectionsXMLRequests.DeleteAppraisalTargetObjectives(_AppraisalSection, QuestnNumber);

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return JsonConvert.SerializeObject(_RequestResponse);
        }
    }
}
