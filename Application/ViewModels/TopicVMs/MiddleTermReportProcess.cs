using Domain.Enums;

namespace Application.ViewModels.TopicVMs
{
    public class MiddleTermReportProcess
    {
        public int? NumberOfReport { get; set; }
        public string WaitingForMakeReviewSchedule { get; set; }
        public DateTime? MakeReviewScheduleTime { get; set; }
        public string WaitingForDocumentSupplementation { get; set; }
        public DateTime? DocumentSupplementationTime { get; set; }
        public string WaitingForConfigureConference { get; set; }
        public DateTime? ConfigureConferenceTime { get; set; }
        public string WaitingForUploadEvaluate { get; set; }
        public DateTime? UploadEvaluateTime { get; set; }
        public DateTime? DeadlineForDocumentSupplementation { get; set; }

        public MiddleTermReportProcess()
        {
            WaitingForMakeReviewSchedule = ProcessEnum.NotStarted.ToString();
            WaitingForDocumentSupplementation = ProcessEnum.NotStarted.ToString();
            WaitingForConfigureConference = ProcessEnum.NotStarted.ToString();
            WaitingForUploadEvaluate = ProcessEnum.NotStarted.ToString();
            DeadlineForDocumentSupplementation = null;
            NumberOfReport = null;
        }
    }
}
