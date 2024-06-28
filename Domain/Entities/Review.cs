using Domain.Enums;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid TopicId { get; set; }
        public int ReportNumber { get; set; }
        public ReviewStateEnum State { get; set; }
        public string? ResultFileLink { get; set; }
        public CouncilDecisionEnum? DecisionOfCouncil { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string? MeetingDetail { get; set; }
        public DateTime? ResubmitDeadline { get; set; }
        public DateTime? UploadMeetingMinutiesTime { get; set; }
        public bool IsCurrentReview { get; set; }
        public DateTime? ConfigureConferenceTime { get; set; }
        public string? DirectiveMinutes { get; set; }
        public DateTime? DocumentSupplementationDeadline { get; set; }

        public Topic Topic { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Council> Councils { get; set; }
    }
}
