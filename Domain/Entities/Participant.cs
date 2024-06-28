using Domain.Enums;

namespace Domain.Entities
{
    public class Participant : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public ParticipantRoleEnum Role { get; set; }
        public string TaskDescription { get; set; }

        public User User { get; set; }
        public Topic Topic { get; set; }
    }
}