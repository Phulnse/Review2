using Application.ViewModels.CouncilVMs;
using Application.ViewModels.ReviewVMs;
using Domain.Enums;

namespace Application.IServices
{
    public interface IReviewService
    {
        Task<List<CouncilInforRes>> ConfigEarlyReviewAsync(ConfigEarlyReviewReq req);
        Task UpdateEarlyMeetingResultAsync(UploadEarlyMeetingMinutesReq req);
        Task UpdateFinalMeetingResultAsync(UploadFinalMeetingMinutesReq req);
        Task EditDeadlineAsync(Guid topicId, ReviewStateEnum reviewState, DateTime deadline);
        Task MakeMiddleReviewScheduleAsync(MakeMiddleReviewScheduleReq req);
        Task<List<CouncilInforRes>> ConfigMiddleReviewAsync(ConfigMiddleReviewReq req);
        Task UploadEvaluateAsync(UploadEvaluateReq req);
        Task MakeFinalReviewScheduleAsync(MakeFinalReviewScheduleReq req);
        Task<List<CouncilInforRes>> ConfigFinalReviewAsync(ConfigFinalReviewReq req);
    }
}
