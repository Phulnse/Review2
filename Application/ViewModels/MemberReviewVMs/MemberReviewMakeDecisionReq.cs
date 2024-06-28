namespace Application.ViewModels.MemberReviewVMs
{
    public class MemberReviewMakeDecisionReq
    {
        public Guid TopicId { get; set; }
        public Guid MemberReviewId { get; set; }
        public bool IsApproved { get; set; }
        public string? ReasonOfDecision { get; set; }
    }
}
