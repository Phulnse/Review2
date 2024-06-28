namespace Application.ViewModels.TopicVMs
{
    public class TopicWaitingUploadMeetingMinutesRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
    }
}
