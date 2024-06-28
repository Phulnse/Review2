using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface ICouncilRepository
    {
        Task AddBulkCouncilAsync(List<Council> councils);
        Task<List<Council>> GetCouncilsByReviewIdAsync(Guid reviewId);
        Task<List<Topic>> GetTopicByStateAndProgressForCouncilAsync(Guid councilId, TopicStateEnum topicState, TopicProgressEnum topicProgress);
        Task<List<Review>> GetReviewsForCouncilAsync(Guid councilId, Guid topicId);
        Task<List<Review>> GetOngoingReviewForCouncilAsync(Guid councilId);
        Task<List<Review>> GetReviewHasMeetingAsync(Guid councilId, Guid topicId);
        Task<List<Review>> GetReviewsWithDocumentsForCouncilAsync(Guid councilId, Guid topicId);
        Task<bool> CheckRoleOfCouncil(Guid councilId, Guid topicId);
        Task<List<Topic>> GetTopicHasBeenResolvedForCouncilAsync(Guid councilId);
    }
}
