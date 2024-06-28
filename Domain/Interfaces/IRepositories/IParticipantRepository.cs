using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IParticipantRepository
    {
        Task AddMembersToTopicAsync(List<Participant> members);
        Task<Topic> GetMeetingReviewOfParticipantAsync(Guid userId, Guid topicId);
        Task<bool> CheckMemberOfTopicAsync(Guid userId, Guid topicId);
        Task<bool> IsValidToAddMemberReviewAsync(Guid topicId, List<Guid> memberReviewId);
    }
}
