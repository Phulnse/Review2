using Domain.Enums;

namespace Application.ViewModels.UserVMs
{
    public class AddMemberToTopic
    {
        public Guid UserId { get; set; }
        public ParticipantRoleEnum Role { get; set; }
        public string TaskDescription { get; set; }
    }
}
