namespace Application.ViewModels.TopicVMs
{
    public class TopicWaitingResubmitRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Code { get; set; }
        public DateTime Deadline { get; set; }
    }
}
