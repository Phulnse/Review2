using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(SRMSContext context) : base(context)
        {
        }

        public async Task CreateReviewAsync(Review review)
        {;
            await AddAsync(review);
        }

        public async Task<List<Review>> GetAllReviewsOfTopic(Guid topicId)
        {
            var reuslt = await Find(x => x.TopicId.Equals(topicId))
                        .Include(x => x.Documents)
                        .ToListAsync();
            return reuslt;
        }

        public User GetChairmanOfReview(Guid reviewId)
        {
            return  Find(x => x.Id.Equals(reviewId))
                        .Include(x => x.Councils)
                        .ThenInclude(x => x.User)
                        .SelectMany(x => x.Councils)
                        .Where(x => x.IsChairman)
                        .Select(x => x.User).First();
        }

        public async Task<Review> GetCurrentReviewByTopicIdAsync(Guid topicId)
        {
            return await Find(x => x.TopicId.Equals(topicId) && x.IsCurrentReview).Include(x => x.Documents).FirstAsync();
        }

        public async Task<Review> GetLastMiddleReviewAsync(Guid topicId)
        {
            return await Find(x => x.TopicId.Equals(topicId) && x.State == ReviewStateEnum.MidtermReport && x.IsCurrentReview).FirstAsync();
        }

        public async Task<List<Topic>> GetMiddleTopicsWaitingForConfigureAsync()
        {
            return await Find(x => x.State == ReviewStateEnum.MidtermReport && x.IsCurrentReview && x.MeetingTime == null)
                        .Include(x => x.Documents)
                        .Include(x => x.Topic)
                        .ThenInclude(x => x.Category)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Select(x => x.Topic)
                        .ToListAsync();
        }

        public async Task<int> GetNumberOfMiddleReviewAsync(Guid topicId)
        {
            return await Find(x => x.TopicId.Equals(topicId) 
                                && x.State == ReviewStateEnum.MidtermReport).CountAsync();
        }

        public async Task<Review> GetReview(Guid topicId, ReviewStateEnum reviewState, int reportNumber)
        {
            return await Find(x => x.TopicId.Equals(topicId) && x.State.Equals(reviewState) && x.ReportNumber == reportNumber).FirstAsync();
        }

        public async Task RemoveCurrentReviewAsync(Guid topicId)
        {
            var review = await Find(x => x.TopicId.Equals(topicId)).Where(x => x.IsCurrentReview).FirstOrDefaultAsync();
            if (review != null)
                review.IsCurrentReview = false;
        }

        public void UpdateReview(Review review)
        {
            Update(review);
        }
    }
}
