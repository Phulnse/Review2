namespace Domain.Entities
{
    public class Document : BaseEntity
    {
        public bool? IsAccepted { get; set; }
        public string? FeedbackFileLink { get; set; }
        public DateTime? UploadFeedbackTime { get; set; }
        public Guid ReviewId { get; set; }
        public string DocumentFileLink { get; set; }

        public Review Review { get; set; }
    }
}
