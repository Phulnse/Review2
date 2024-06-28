using Domain.Enums;

namespace Domain.Entities
{
    public class Topic : BaseEntity
    {
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public TopicProgressEnum Progress { get; set; }
        public string Code { get; set; }
        public TopicStateEnum State { get; set; }
        public Guid CreatorId { get; set; }
        public Guid CategoryId { get; set; }
        public bool? DeanDecision { get; set; }
        public DateTime? MakeDecisionTime { get; set; }
        public string? ReasonOfDecision { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? ReviewStartDate { get; set; }
        public DateTime? ReviewEndDate { get; set; }
        public Guid? DeciderId { get; set; }
        public DateTime StartTime { get; set; }
        public string TopicFileName { get; set; }
        public string TopicFileLink { get; set; }
        public DateTime? SumarizeResultTime { get; set; }

        public User Creator { get; set; }
        public User? Decider { get; set; }
        public Category Category { get; set; }
        public ICollection<MemberReview> MemberReviews { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Remuneration> Remunerations { get; set; }
        public ICollection<Notify> Notifies { get; set; }
    }
}

