using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface IReviewRepository
    {
        Task CreateReviewAsync(Review review);
        Task<Review> GetReview(Guid topicId, ReviewStateEnum reviewState, int reportNumber);
        User GetChairmanOfReview(Guid reviewId);
        void UpdateReview(Review review);
        Task<List<Review>> GetAllReviewsOfTopic(Guid topicId);
        Task<Review> GetCurrentReviewByTopicIdAsync(Guid topicId);
        Task<int> GetNumberOfMiddleReviewAsync(Guid topicId);
        Task RemoveCurrentReviewAsync(Guid topicId);
        Task<List<Topic>> GetMiddleTopicsWaitingForConfigureAsync();
        Task<Review> GetLastMiddleReviewAsync(Guid topicId);
    }
}
