using Application.ViewModels.UserVMs;

namespace Application.ViewModels.TopicVMs
{
    public class CreateTopicReq
    {
        public Guid CategoryId { get; set; }
        public Guid CreatorId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public List<AddMemberToTopic> MemberList { get; set; }
        public string TopicFileName { get; set; }
        public string TopicFileLink { get; set; }
        public DateTime StartTime { get; set; }
    }
}
