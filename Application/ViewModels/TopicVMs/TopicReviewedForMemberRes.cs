namespace Application.ViewModels.TopicVMs
{
    public class TopicReviewedForMemberRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Code { get; set; }
        public bool MemberDecision { get; set; }
    }
}
