namespace Application.ViewModels.DocumentVMs
{
    public class DocumentOfReview
    {
        public Guid DocumentId { get; set; }
        public DateTime Deadline { get; set; }
        public bool? IsAccepted { get; set; }
        public string? FeedbackFileLink { get; set; }
        public Guid ReviewId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DocumentFileLink { get; set; }
    }
}
