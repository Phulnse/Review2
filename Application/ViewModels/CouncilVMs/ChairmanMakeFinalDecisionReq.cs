namespace Application.ViewModels.CouncilVMs
{
    public class ChairmanMakeFinalDecisionReq
    {
        public Guid TopicId { get; set; }
        public string? FeedbackFileLink { get; set; }
        public bool IsAccepted { get; set; }
    }
}
