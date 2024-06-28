namespace Application.ViewModels.TopicVMs
{
    public class TopicProcessRes
    {
        public Guid TopicId { get; set; }
        public Guid CreatorId { get; set; }
        public string TopicName { get; set; }
        public string Progress { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public DateTime? CurrentDeadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public PreliminaryReviewProcess PreliminaryReviewProcess { get; set; }
        public EarlyTermReportProcess EarlyTermReportProcess { get; set; }
        public List<MiddleTermReportProcess> MiddleTermReportProcess { get; set; }
        public FinalTermReportProcess FinalTermReportProcess { get; set; }
    }
}
