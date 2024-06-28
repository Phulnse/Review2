using Application.ViewModels.DocumentVMs;

namespace Application.ViewModels.ReviewVMs
{
    public class ReviewEarlyDocument
    {
        public DateTime Deadline { get; set; }
        public string ResultFileLink { get; set; }
        public string DecisionOfCouncil { get; set; }
        public List<DocumentOfEarlyReview> Documents { get; set; }
    }
}
