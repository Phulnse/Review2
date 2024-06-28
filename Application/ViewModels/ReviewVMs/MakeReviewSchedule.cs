namespace Application.ViewModels.ReviewVMs
{
    public class MakeReviewSchedule
    {
        public Guid TopicId { get; set; }
        public DateTime DocumentSupplementationDeadline { get; set; }
        public string DirectiveMinutes { get; set; }
    }
}
