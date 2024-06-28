namespace Application.ViewModels.ReviewVMs
{
    public abstract class UploadMeetingMinutes
    {
        public Guid TopicId { get; set; }
        public string ResultFileLink { get; set; }
        public int DecisionOfCouncil { get; set; }
        public DateTime? ResubmitDeadline { get; set; }
    }
}
