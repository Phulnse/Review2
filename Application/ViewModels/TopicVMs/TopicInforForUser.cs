namespace Application.ViewModels.TopicVMs
{
    public class TopicInforForUser
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public string Progress { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public Guid CreatorId { get; set; }
    }
}
