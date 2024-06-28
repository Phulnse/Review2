using Domain.Enums;

namespace Application.ViewModels.TopicVMs
{
    public class EarlyTermReportProcess
    {
        public string WaitingForCouncilFormation { get; set; }
        public DateTime? CouncilFormationTime { get; set; }
        public string WaitingForUploadMeetingMinutes { get; set; }
        public DateTime? UploadMeetingMinutesTime { get; set; }
        public List<ResubmitProcess> ResubmitProcesses { get; set; }
        public string WaitingForContractSigning { get; set; }
        public DateTime? UploadContractTime { get; set; }

        public EarlyTermReportProcess()
        {
            WaitingForCouncilFormation = ProcessEnum.NotStarted.ToString();
            WaitingForUploadMeetingMinutes = ProcessEnum.NotStarted.ToString();
            WaitingForContractSigning = ProcessEnum.NotStarted.ToString();
        }
    }
}
