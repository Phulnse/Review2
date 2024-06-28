namespace Application.ViewModels.ReviewVMs
{
    public class TopicMeetingInforRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public List<ReviewMeetingInfor> ReviewMeetingInfors { get; set; }
    }
}
