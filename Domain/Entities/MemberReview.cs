namespace Domain.Entities
{
    public class MemberReview : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public bool? IsApproved { get; set; }
        public string? ReasonOfDecision { get; set; }

        public User User { get; set; }
        public Topic Topic { get; set; }
    }
}
