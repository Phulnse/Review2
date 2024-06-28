namespace Application.ViewModels.TopicVMs
{
    public class EarlyTopicForCouncilRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Code { get; set; }
        public DateTime Deadline { get; set; }
        public string Result { get; set; }
        public bool IsChairman { get; set; }
    }
}
