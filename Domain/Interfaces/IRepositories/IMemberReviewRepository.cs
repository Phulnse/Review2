using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IMemberReviewRepository
    {
        Task AddMemberReviewAsync(Guid topicId, List<Guid> memberReviewIds);
        void MemberMakeDecision(Guid topicId, Guid memberReviewId, bool decision, string? reason);
        Task SetDefaultValueForMemberNotResponse(DateTime time);
        Task<List<Topic>> GetTopicForMemberReviewAsync(Guid memberId);
        Task<List<Guid>> GetTopicDecidedByAllMembersAsync();
        Task<List<MemberReview>> GetTopicReviewedForMemberAsync(Guid memberId);
        Task<List<User>> GetMemberReviewOfTopicAsync(Guid topicId);
        Task<bool> IsValidToMakeDecisionAsync(Guid topicId, Guid userId);
    }
}
