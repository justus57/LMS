using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.CustomsClasses
{
    public class DynamicsNAVResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
    }
    public class HRPosition
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class OrgUnit
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class RequestResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class LeaveTypes
    {
        public string LeaveCode { get; set; }
        public string LeaveName { get; set; }
    }
    public class LeaveCodeDetails
    {
        public string LeaveCode { get; set; }
        public string Description { get; set; }
        public string EntitledDays { get; set; }
        public string OpeningBalance { get; set; }
        public string LeaveTaken { get; set; }
        public string Accrued { get; set; }
        public string Remaining { get; set; }
        public string RequiresAttachment { get; set; }
        public string AttachmentMandatory { get; set; }
    }
    public class LeaveEndDateAndReturnDate
    {
        public string EndDate { get; set; }
        public string ReturnDate { get; set; }
        public string Message { get; set; }
        public bool Validity { get; set; }
    }
    public class SelectedLeaveEndDateAndReturnDate
    {
        public string EndDate { get; set; }
        public string ReturnDate { get; set; }
        public string StartDate { get; set; }
        public string Quantity { get; set; }
    }
    public class LeaveQuantityAndReturnDate
    {
        public string Quantity { get; set; }
        public string ReturnDate { get; set; }
        public string Message { get; set; }
        public bool Validity { get; set; }
    }
    public class AprrovedLeave
    {
        public string LeaveNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Qty { get; set; }
    }
    public class Employee
    {
      
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }
    class QuestionDetails
    {
        public string QuestionNumber { get; set; }
        public string Description { get; set; }
        public List<QuestionObjective> Objectives { get; set; }
        public string PerformanceMeasurementType { get; set; }
        public string WeightScoreValue { get; set; }
        public string AppraisalSection { get; set; }
    }
    class QuestionObjective
    {
        public string OjectiveCode { get; set; }
        public string ObjectiveDescription { get; set; }
    }
    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class QuestionResponse
    {
        public string QuestionNumber { get; set; }
        public string ChoiceSelected { get; set; }
        public string WeightedScore { get; set; }
        public string CommentSubmitted { get; set; }
        public string SupervisorComent { get; set; }
        public string HRAppraisalSectionComment { get; set; }
        public string HRAppraisalComment { get; set; }

    }
    public class Question
    {
        public string QuestionNumber { get; set; }
        public string QuestionDescription { get; set; }
        public string PerformanceMeasurementType { get; set; }
        public string WeightScoreValue { get; set; }
        public string AppraisalSection { get; set; }
    }
    public class Appraisal
    {
        public string AppraisalHeaderNumber { get; set; }
        public string AppraisalDescription { get; set; }
        public string AppraisalStartDate { get; set; }
        public string AppraisalEndDate { get; set; }
        public string AppraisalType { get; set; }
        public string AppraisalApplicableTo { get; set; }
        public string HRComment { get; set; }
    }
    public class AppraisalSection
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string IsHRDefined { get; set; }
    }
    public class AppraisalPML
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
    }
    public class Training
    {
        public string AbsenceType { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedStartTime { get; set; }
        public string PlannedEndDate { get; set; }
        public string PlannedEndTime { get; set; }
        public string TotalCost { get; set; }
        public string NoSeries { get; set; }
        public string CourseCode { get; set; }
        public string CourseDescription { get; set; }
        public string Trainer { get; set; }
        public string TrainerName { get; set; }
        public string Venue { get; set; }
        public string Room { get; set; }
        public string TrainingInstitution { get; set; }
        public string ScheduledStartDate { get; set; }
        public string ScheduledStartTime { get; set; }
        public string ScheduledEndDate { get; set; }
        public string ScheduledEndTime { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualStartTime { get; set; }
        public string ActualEndDate { get; set; }
        public string ActualEndTime { get; set; }
        public string CancellationCompletionDate { get; set; }
        public string ProgressStatus { get; set; }
        public string LPONo { get; set; }
        public string TrainingPlanNo { get; set; }
        public string Archived { get; set; }
        public string CancellationReason { get; set; }
        public string ActualCost { get; set; }
        public string ApplicableTo { get; set; }
        public string Approver { get; set; }
        public string RequirementOfTrainingRequest { get; set; }
    }
    public class AdvanceRequestHeader
    {
        public string No { get; set; }
        public string DateDue { get; set; }
        public string DateOfRequest { get; set; }
        public string Requester { get; set; }
        public string GlobalDimCode1 { get; set; }
        public string GlobalDimCode2 { get; set; }
        public string ShortcutDimCode1 { get; set; }
        public string ShortcutDimCode2 { get; set; }
        public string ShortcutDimCode3 { get; set; }
        public string ShortcutDimCode4 { get; set; }
        public string ShortcutDimCode5 { get; set; }
        public string ShortcutDimCode6 { get; set; }
        public string ShortcutDimCode7 { get; set; }
        public string ShortcutDimCode8 { get; set; }
        public string PreferredPaymentMethod { get; set; }
        public string Balance { get; set; }
        public string AdvanceRequestHdrNo { get; set; }
        public string MissionSummary { get; set; }
        public string RejectionComment { get; set; }
        public List<AdvanceRequestLines> _AdvanceRequestLines { get; set; }
    }
    public class AdvanceRequestLines
    {
        public string No { get; set; }
        public string Item { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public string NoOfUnits { get; set; }
        public string UnitCost { get; set; }
        public string Currency { get; set; }
        public string Purpose { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentId { get; set; }
        public string AdvancedAmount { get; set; }
        public string AdvancedAmountLCY { get; set; }
        public string ClaimedAmount { get; set; }
        public string ClaimedAmountLCY { get; set; }
        public string SurrenderedAmount { get; set; }
        public string SurrenderedAmountLCY { get; set; }
        public string GlobalDimCode1 { get; set; }
        public string GlobalDimCode2 { get; set; }
        public string ShortcutDimCode1 { get; set; }
        public string ShortcutDimCode2 { get; set; }
        public string ShortcutDimCode3 { get; set; }
        public string ShortcutDimCode4 { get; set; }
        public string ShortcutDimCode5 { get; set; }
        public string ShortcutDimCode6 { get; set; }
        public string ShortcutDimCode7 { get; set; }
        public string ShortcutDimCode8 { get; set; }
        public string Remarks { get; set; }

    }
    public class AdvanceRequestType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UnitCost { get; set; }
        public string Currency { get; set; }
        public string ExchangeRate { get; set; }
    }
    public class AdvanceRequestTypes
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Dimensions
    {
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string ShortcutDimension5Code { get; set; }
        public string ShortcutDimension6Code { get; set; }
        public string ShortcutDimension7Code { get; set; }
        public string ShortcutDimension8Code { get; set; }
    }
    public class DimensionCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class ShortcutDimensionCode3
    {
        public string id { get; set; }
        public string text { get; set; }
    }


    public class TransportRequest
    {
        public string No { get; set; }
        public string Requester { get; set; }
        public string Destination { get; set; }
        public string PurposeOfTrip { get; set; }
        public string VehicleClassRequested { get; set; }
        public string VehicleClassAssigned { get; set; }
        public string VehicleAssigned { get; set; }
        public string EstimatedDistance { get; set; }
        public double EstimatedTripCost { get; set; }
        public string DateVehicleAcquired { get; set; }
        public string DriverAssigned { get; set; }
        public string TransportManagerComment { get; set; }

        public string RequestEndDateTime { get; set; }
        public string RequestStartDateTime { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string ShortcutDimension5Code { get; set; }
        public string ShortcutDimension6Code { get; set; }
        public string ShortcutDimension7Code { get; set; }
        public string ShortcutDimension8Code { get; set; }
        public string RejectionComment { get; set; }
    }

    public class LogSheetLine
    {
        public string No { get; set; }
        public string VehicleRegNo { get; set; }
        public string DriverEmloyeeCode { get; set; }
        public string DestinationRoute { get; set; }
        public string StartTimeOdometerReading { get; set; }
        public string EndTimeOdometerReading { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string TripStartDateTime { get; set; }
        public string TripEndDateTime { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string ShortcutDimension5Code { get; set; }
        public string ShortcutDimension6Code { get; set; }
        public string ShortcutDimension7Code { get; set; }
        public string ShortcutDimension8Code { get; set; }
        public string Notes { get; set; }
    }


    public class LoginResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public string EmployeeName { get; set; }
        public string IsMedicalApprover { get; set; }
        public string IsLeaveApprover { get; set; }
        public string IsStaffAdvanceApprover { get; set; }
        public string IsEmployeeChangesApprover { get; set; }
        public string AppraisalAccessLevel { get; set; }
        public string IsGrievanceApprover { get; set; }
        public string IsDisciplinaryApprover { get; set; }
        public string IsTrainingSupervisor { get; set; }
        public string RequirePasswordChange { get; set; }
        public string IsTransportManager { get; set; }
        public string IsTransportRequestApprover { get; set; }
        public string IsDriver { get; set; }
        public string IsRecruiter { get; set; }
        public string PayrollNo { get; set; }
        public string CanApplyLeaveForAnother { get; set; }
        public string CanApplyBackdatedLeave { get; set; }
        public string SESANo { get; set; }
        public string WindowsUsername { get; set; }
        public string IsHRManager { get; set; }
        public string IsAppraisalSupervisor { get; set; }
    }

}