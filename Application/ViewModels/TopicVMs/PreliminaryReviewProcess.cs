using Domain.Enums;

namespace Application.ViewModels.TopicVMs
{
    public class PreliminaryReviewProcess
    {
        public string WaitingForDean { get; set; }
        public DateTime? DeanMakeDecisionTime { get; set; }
        public string WaitingForCouncilFormation { get; set; }
        public DateTime? CouncilFormationTime { get; set; }
        public string WaitingForCouncilDecision { get; set; }
        public DateTime? CouncilMakeDecisionTime { get; set; }

        public PreliminaryReviewProcess()
        {
            WaitingForDean = ProcessEnum.NotStarted.ToString();
            WaitingForCouncilFormation = ProcessEnum.NotStarted.ToString();
            WaitingForCouncilDecision = ProcessEnum.NotStarted.ToString();
        }
    }
}
