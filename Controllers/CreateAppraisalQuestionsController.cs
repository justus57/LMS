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
    public class CreateAppraisalQuestionsController : Controller
    {
        static string AppraisalHeaderNo = "";
        static string AppraisalTargetNo = "";
        // GET: CreateAppraisalQuestions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateAppraisalQuestions()
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
                                AppraisalHeaderNo = AppraisalHeader;
                                LoadTable(AppraisalHeader);
                                //AppraisalSection.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                                //AppraisalSection.SelectedIndex = 0;
                            }
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
            //gotten sections 

            foreach (var AppraisalSection in CreateAppraisalXMLREquests.AppraisalSection("CreateAppraisal"))
            {
                html.Append("<ol>");
                html.Append("<b><H5>" + AppraisalSection.Value + "</H5></b>");//

                string quenss = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportAll", AppraisalSection.Key);

                dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

                foreach (var QuestionsToFill in stuff)
                {
                    string QuestnNumber = QuestionsToFill.QuestionNumber;

                    if (QuestionsToFill.QuestionDescription != "")
                    {
                        html.Append("<li><b> " + QuestionsToFill.QuestionDescription + "</b> <a href = 'javascript:void(0)' data-id='" + QuestnNumber + "' class='float-right DeleteQuestion' ><i class='fas fa-fw fa-trash'></i></a> <a href = 'javascript:void(0)' data-id='" + QuestnNumber + "' class='float-right EditQuestion' ><i class='fas fa-fw fa-edit'></i></a>");
                        html.Append("<ul class=''>");
                        //Target Objectives

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
            }

            html.Append("</div>");
            html.Append("</form> ");

            string strText = html.ToString();
            ViewBag.AppraisalBody = strText;
            ////Append the HTML string to Placeholder.
            // placeholder.Controls.Add(new Literal { Text = html.ToString() });
        }

        public JsonResult GetAppraisalSectionList()
        {
            List<AppraisalSection> AppraisalSectionObject = new List<AppraisalSection>();

            foreach (var kvp in CreateAppraisalXMLREquests.AppraisalSection("CreateAppraisal"))
            {
                AppraisalSectionObject.Add(new AppraisalSection { Code = kvp.Key, Description = kvp.Value });
            }
            return Json(JsonConvert.SerializeObject(AppraisalSectionObject), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAppraisalsList()
        {
            //exclude current appraisal
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();

            List<Appraisal> AppraisalObject = new List<Appraisal>();

            foreach (var kvp in CreateAppraisalXMLREquests.AppraisalList(username, AppraisalHeaderNo))
            {
                AppraisalObject.Add(new Appraisal { AppraisalHeaderNumber = kvp.Key, AppraisalDescription = kvp.Value });
            }

            return Json(JsonConvert.SerializeObject(AppraisalObject), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewAppraisalQuestion(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";

            string QuestionNumber = "";
            string QuestionObjectiveNumber = "";
            string objectiveDecription = "";
            string QuestionDescription = "";
            string QuestionType = "";
            string weightScoreValue = "0.0";
            string AppraisalSection = "";


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
                if (nv.name.StartsWith("AppraisalSection"))
                {
                    AppraisalSection = nv.value;
                }
            }

            //save target
            string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTarget");
            dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);
            Status = jsonDocumentNo.Status;

            if (Status == "000")
            {
                string DocumentNo = jsonDocumentNo.DocumentNo;

                string TargetResponse = CreateAppraisalQuestionXMLRequests.CreateNewAppraisalQuestion(DocumentNo, username, AppraisalHeaderNo, QuestionDescription, QuestionType, weightScoreValue, AppraisalSection);
                dynamic json = JObject.Parse(TargetResponse);

                QuestionNumber = DocumentNo;

                Msg = json.Msg;

                foreach (var item in objectivesArray)
                {
                    string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTargetObjective");
                    dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                    Status = jsonObjectiveNo.Status;

                    if (Status == "000")
                    {
                        string ObjectiveNo = jsonObjectiveNo.DocumentNo;
                        QuestionObjectiveNumber = ObjectiveNo;

                        CreateAppraisalQuestionXMLRequests.CreateAppraisalQuestionObjective(username, AppraisalHeaderNo, QuestionObjectiveNumber, QuestionNumber, item.ToString());
                    }

                }
            }
            else
            {
                Msg = jsonDocumentNo.Msg + " - " + AppraisalHeaderNo;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(NameValue[] formVars)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string Status = "000";
            string Msg = "";

            string QuestionNumber = "";
            string QuestionObjectiveNumber = "";
            string objectiveDecription = "";
            string QuestionDescription = "";
            string QuestionType = "";
            string weightScoreValue = "0.0";
            string AppraisalSection = "";


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
                if (nv.name.StartsWith("AppraisalSection"))
                {
                    AppraisalSection = nv.value;
                }
                if (nv.name.StartsWith("AppraisalTargetNo"))
                {
                    QuestionNumber = nv.value;
                }
            }

            //update target
            CreateAppraisalQuestionXMLRequests.DeleteAppraisalTargetObjective(AppraisalHeaderNo, QuestionNumber);

            string TargetResponse = CreateAppraisalQuestionXMLRequests.UpdateAppraisalTarget(QuestionNumber, AppraisalHeaderNo, QuestionDescription, QuestionType, weightScoreValue, AppraisalSection, username);

            //get question number from response
            dynamic json = JObject.Parse(TargetResponse);
            Status = json.Status;
            //Msg = json.Msg ;

            if (Status == "000")
            {
                // 

                foreach (var item in objectivesArray)
                {
                    string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTargetObjective");
                    dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                    Status = jsonObjectiveNo.Status;
                    Msg = jsonObjectiveNo.Msg;
                    if (Status == "000")
                    {
                        string ObjectiveNo = jsonObjectiveNo.DocumentNo;


                        QuestionObjectiveNumber = ObjectiveNo;

                        string CreateAppraisalQuestionObjectiveResponse = CreateAppraisalQuestionXMLRequests.CreateAppraisalQuestionObjective(username, AppraisalHeaderNo, QuestionObjectiveNumber, QuestionNumber, item.ToString());
                        dynamic jsonCreateAppraisalQuestionObjectiveResponse = JObject.Parse(CreateAppraisalQuestionObjectiveResponse);

                        Status = QuestionNumber = jsonCreateAppraisalQuestionObjectiveResponse.Status;
                        Msg = jsonCreateAppraisalQuestionObjectiveResponse.Msg;

                        if (Status == "000")
                        {
                            Msg = "The appraisal target was successfully updated.";
                        }
                    }

                }
            }

            var _RequestResponse = new RequestResponse
            {
                Status = Status,
                Message = Msg
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionDetails(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string QuestnNumber = param1;
            AppraisalTargetNo = QuestnNumber;
            string QuestionDescription = "";
            string Performance_Measurement_Type = "";
            string weightScoreValue = "";
            string AppraisalSection = "";

            string TargetDetails = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNo, QuestnNumber, "ExportSingle", "");

            dynamic stuff = JsonConvert.DeserializeObject(TargetDetails.Replace("\"", "'"));

            foreach (var QuestionsToFill in stuff)
            {
                QuestionDescription = QuestionsToFill.QuestionDescription;
                Performance_Measurement_Type = QuestionsToFill.PerformanceMeasurementType;
                weightScoreValue = QuestionsToFill.WeightScoreValue;
                AppraisalSection = QuestionsToFill.AppraisalSection;
            }

            List<QuestionObjective> QuestionObjectiveObject = new List<QuestionObjective>();

            foreach (var QuestionObjectives in AppraisalsXMLRequests.GetQuestionObjectives(AppraisalHeaderNo, QuestnNumber))
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

            var questionDetails = new QuestionDetails
            {
                Description = QuestionDescription,
                QuestionNumber = "000",
                Objectives = QuestionObjectiveObject,
                PerformanceMeasurementType = Performance_Measurement_Type,
                WeightScoreValue = weightScoreValue,
                AppraisalSection = AppraisalSection
            };

            return Json(JsonConvert.SerializeObject(questionDetails), JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteAppraisalTarget(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string QuestionNumber = param1;
            string response = "";
            string status = "";

            string xmlresponse = CreateAppraisalQuestionXMLRequests.DeleteAppraisalTarget(AppraisalHeaderNo, QuestionNumber);
            CreateAppraisalQuestionXMLRequests.DeleteAppraisalTargetObjective(AppraisalHeaderNo, QuestionNumber);

            dynamic json = JObject.Parse(xmlresponse);

            response = json.Msg;
            status = json.Status;


            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CopyAppraisalTargets(string param1)
        {
            string username = System.Web.HttpContext.Current.Session["Username"].ToString();
            string AppraisalHeaderNumber = param1;
            string response = "";
            string status = "";

            //Get Appraisal Questions from list
            string quenss = AppraisalsXMLRequests.GetQuestionsToFillJson(AppraisalHeaderNumber, "", "ExportCopy", "");

            dynamic stuff = JsonConvert.DeserializeObject(quenss.Replace("\"", "'"));

            foreach (var QuestionsToFill in stuff)
            {
                string QuestnNumber = QuestionsToFill.QuestionNumber;
                string QuestionDescription = QuestionsToFill.QuestionDescription;
                string QuestionType = QuestionsToFill.PerformanceMeasurementType;
                string weightScoreValue = QuestionsToFill.WeightScoreValue;
                string AppraisalSection = QuestionsToFill.AppraisalSection;
                //get new Appraisal Target Number      
                string PMType = "";

                if (QuestionType == "1")
                {
                    PMType = "Description";
                }
                else if (QuestionType == "2")
                {
                    PMType = "Options";
                }
                else if (QuestionType == "3")
                {
                    PMType = "WeightedScore";
                }


                if (QuestionsToFill.QuestionDescription != "")
                {
                    string DocumentNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTarget");
                    dynamic jsonDocumentNo = JObject.Parse(DocumentNoXMLResponse);
                    status = jsonDocumentNo.Status;
                    string QuestionNumber = "";


                    if (status == "000")
                    {
                        QuestionNumber = jsonDocumentNo.DocumentNo;

                        string TargetResponse = CreateAppraisalQuestionXMLRequests.CreateNewAppraisalQuestion(QuestionNumber, username, AppraisalHeaderNo, QuestionDescription, PMType, weightScoreValue, AppraisalSection);
                        dynamic json = JObject.Parse(TargetResponse);

                        response = json.Msg;
                        status = json.Status;

                        if (status == "000")
                        {
                            foreach (var QuestionObjectives in AppraisalsXMLRequests.GetQuestionObjectives(AppraisalHeaderNumber, QuestnNumber))
                            {
                                if (QuestionObjectives.Value.Length > 1)
                                {
                                    string ObjectiveNoXMLResponse = AppraisalsXMLRequests.GetDocumentNo("AppraisalTargetObjective");
                                    dynamic jsonObjectiveNo = JObject.Parse(ObjectiveNoXMLResponse);
                                    status = jsonObjectiveNo.Status;

                                    if (status == "000")
                                    {
                                        string ObjectiveNo = jsonObjectiveNo.DocumentNo;
                                        string QuestionObjectiveNumber = ObjectiveNo;

                                        CreateAppraisalQuestionXMLRequests.CreateAppraisalQuestionObjective(username, AppraisalHeaderNo, QuestionObjectiveNumber, QuestionNumber, QuestionObjectives.Value);
                                    }
                                }
                            }
                        }

                    }
                }
            }

            var _RequestResponse = new RequestResponse
            {
                Message = response,
                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }



    }
}