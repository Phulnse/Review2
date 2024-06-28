using Application.ViewModels.CouncilVMs;

namespace Application.ViewModels.ReviewVMs
{
    public abstract class ConfigReview
    {
        public Guid TopicId { get; set; }
        public DateTime MeetingTime { get; set; }
        public List<AddCouncil> Councils { get; set; }
        public string MeetingDetail { get; set; }
    }
}
