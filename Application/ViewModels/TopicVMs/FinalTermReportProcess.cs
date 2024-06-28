using Domain.Enums;

namespace Application.ViewModels.TopicVMs
{
    public class FinalTermReportProcess
    {
        public string WaitingForMakeReviewSchedule { get; set; }
        public DateTime? MakeReviewScheduleTime { get; set; }
        public string WaitingForDocumentSupplementation { get; set; }
        public DateTime? DocumentSupplementationTime { get; set; }
        public string WaitingForConfigureConference { get; set; }
        public DateTime? ConfigureConferenceTime { get; set; }
        public string WaitingForUploadMeetingMinutes { get; set; }
        public DateTime? UploadMeetingMinutesTime { get; set; }
        public List<ResubmitProcess> ResubmitProcesses { get; set; }
        public DateTime? DeadlineForDocumentSupplementation { get; set; }

        public FinalTermReportProcess()
        {
            WaitingForMakeReviewSchedule = ProcessEnum.NotStarted.ToString();
            WaitingForDocumentSupplementation = ProcessEnum.NotStarted.ToString();
            WaitingForConfigureConference = ProcessEnum.NotStarted.ToString();
            WaitingForUploadMeetingMinutes = ProcessEnum.NotStarted.ToString();
        }
    }
}
