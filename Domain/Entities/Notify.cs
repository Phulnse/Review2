using Domain.Enums;

namespace Domain.Entities
{
    public class Notify : BaseEntity
    {
        public bool HasRead { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsReject { get; set; }
        public TopicStateEnum State { get; set; }
        public TopicProgressEnum Progress { get; set; }
        public Guid TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}
