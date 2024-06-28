using Application.ViewModels.DocumentVMs;

namespace Application.ViewModels.ReviewVMs
{
    public class ReviewEarly
    {
        public Guid ReviewId { get; set; }
        public Guid TopicId { get; set; }
        public string State { get; set; }
        public string ResultFileLink { get; set; }
        public string DecisionOfCouncil { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DocumentOfReview> Documents { get; set; }
    }
}
