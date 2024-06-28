namespace Application.ViewModels.NotifyVMs
{
    public class NotifyVM
    {
        public bool HasRead { get; set; }
        public bool IsReject { get; set; }
        public string State { get; set; }
        public string Progress { get; set; }
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
