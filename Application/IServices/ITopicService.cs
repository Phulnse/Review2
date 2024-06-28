using Application.ViewModels.CouncilVMs;
using Application.ViewModels.ReviewVMs;
using Application.ViewModels.TopicVMs;
using Domain.Enums;

namespace Application.IServices
{
    public interface ITopicService
    {
        Task CreateTopicAsync(CreateTopicReq createTopicReq);
        Task<List<TopicInfoRes>> GetTopicsForDeanAsync(Guid deanId);
        Task<List<TopicInfoRes>> GetTopicByStateAndProgressForStaffAsync(TopicStateEnum state, TopicProgressEnum progress);
        Task<List<TopicInforForReviewerRes>> GetTopicsForReviewMemberAsync(Guid memberId);
        Task DeanMakeDecisionAsync(DeanMakeDecisionReq req);
        Task<List<GetTopicDecidedByDeanIdRes>> GetTopicDecidedByDeanAsync(Guid deanId);
        Task<TopicDetailRes> GetTopicDetailAsync(Guid topicId);
        Task<List<TopicInforForUser>> GetTopicByUserIdAsync(Guid userId);
        Task<List<EarlyTopicForCouncilRes>> GetTopicWaitingChairmanDecisionAsync(Guid councilId);
        Task<TopicDocumentsRes> GetDocumentOfTopicAsync(Guid topicId);
        Task ChairmanApproveAsync(Guid topicId);
        Task ChairmanRejectAsync(ChairmanRejectReq req);
        Task<TopicProcessRes> GetTopicProcessAsync(Guid topicId);
        Task<TopicDocumentsRes> GetEarlyTopicDetailForCouncilAsync(Guid councilId, Guid topicId);
        Task<List<TopicForCouncilRes>> GetOngoingTopicForCouncilAsync(Guid councilId);
        Task<List<TopicWaitingResubmitRes>> GetEarlyTopicWaitingResubmitAsync();
        Task<List<TopicReviewedForMemberRes>> GetTopicReviewedForMemberAsync(Guid memberId);
        Task<TopicMeetingInforRes> GetTopicMeetingInforResAsync(Guid userId, Guid topicId);
        Task<List<TopicInfoRes>> GetAllActiveTopicAsync();
        Task<ReviewsOfTopicRes> GetDocumentsOfTopicAsync(Guid userid, Guid topicId);
        Task MoveTopicStateToMiddleTermAsync(Guid topicId);
        Task<List<TopicInfoRes>> GetMiddleTopicWaitingForConfigureAsync();
        Task<List<TopicWaitingUploadMeetingMinutesRes>> GetTopicWaitingUploadMeetingMinutesAsync();
        Task<List<TopicInfoRes>> GetTopicHasBeenResolvedForCouncilAsync(Guid councilId);
        Task MoveTopicStateToFinalTermAsync(Guid topicId);
        Task ChairmanMakeFinalDecisionAsync(ChairmanMakeFinalDecisionReq req);
        Task<List<TopicInfoRes>> GetAllTopicAsync();
    }
}
