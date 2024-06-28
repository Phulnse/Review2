namespace Application.ViewModels.TopicVMs
{
    public class TopicDetailRes
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public string Progress { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public Guid CreatorId { get; set; }
        public string TopicLeaderName { get; set; }
        public string CategoryName { get; set; }
        public string TopicFileName { get; set; }
        public string TopicFileLink { get; set; }
        public int NumberOfMember { get; set; }
        public int NumberOfTrainingMember { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
