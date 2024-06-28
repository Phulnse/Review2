using Application.ViewModels.MemberReviewVMs;
using Application.ViewModels.UserVMs;

namespace Application.IServices
{
    public interface IMemberReviewService
    {
        Task AddMemberReviewAsync(AddMemberReviewReq req);
        Task MemberReviewMakeDecisionAsync(MemberReviewMakeDecisionReq req);
        Task<List<UserInforRes>> GetMemberReviewOfTopicAsync(Guid topicId);
    }
}
