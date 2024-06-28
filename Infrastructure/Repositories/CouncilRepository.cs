using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CouncilRepository : GenericRepository<Council>, ICouncilRepository
    {
        public CouncilRepository(SRMSContext context) : base(context)
        {
        }

        public async Task AddBulkCouncilAsync(List<Council> councils)
        {
            await AddBulkAsync(councils);
        }

        public async Task<bool> CheckRoleOfCouncil(Guid councilId, Guid topicId)
        {
            var councils = await Find(x => x.UserId.Equals(councilId) && x.IsChairman)
                                .Include(x => x.Review).ToListAsync();
            foreach (var council in councils)
            {
                if (council.Review.TopicId.Equals(topicId))
                    return true;
            }

            return false;
        }

        public async Task<List<Council>> GetCouncilsByReviewIdAsync(Guid reviewId)
        {
            return await Find(x => x.ReviewId.Equals(reviewId)).Include(x => x.User).ToListAsync();
        }

        public async Task<List<Review>> GetOngoingReviewForCouncilAsync(Guid councilId)
        {
            return await Find(x => x.UserId.Equals(councilId))
                            .Include(x => x.Review)
                            .ThenInclude(x => x.Topic)
                            .ThenInclude(x => x.Category)
                            .Select(x => x.Review)
                            .Where(x => x.IsCurrentReview)
                            .Where(x => x.Topic.Status)
                            .ToListAsync();
        }

        public async Task<List<Review>> GetReviewHasMeetingAsync(Guid councilId, Guid topicId)
        {
            return await Find(x => x.UserId.Equals(councilId))
                            .Include(x => x.Review)
                            .ThenInclude(x => x.Topic)
                            .Select(x => x.Review)
                            .Where(x => x.TopicId.Equals(topicId))
                            .Where(x => x.State == ReviewStateEnum.EarlyTermReport || x.State == ReviewStateEnum.FinaltermReport)
                            .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsForCouncilAsync(Guid councilId, Guid topicId)
        {
            return await Find(x => x.UserId.Equals(councilId))
                        .Include(x => x.Review)
                        .Select(x => x.Review)
                        .Where(x => x.TopicId.Equals(topicId))
                        .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsWithDocumentsForCouncilAsync(Guid councilId, Guid topicId)
        {
            return await Find(x => x.UserId.Equals(councilId))
                        .Include(x => x.Review)
                        .ThenInclude(x => x.Documents)
                        .Select(x => x.Review)
                        .Where(x => x.TopicId.Equals(topicId))
                        .ToListAsync();
        }

        public async Task<List<Topic>> GetTopicByStateAndProgressForCouncilAsync(Guid councilId, TopicStateEnum topicState, TopicProgressEnum topicProgress)
        {
            return await Find(x => x.UserId.Equals(councilId))
                                .Include(x => x.Review)
                                .ThenInclude(x => x.Topic)
                                .ThenInclude(x => x.Reviews)
                                .Include(x => x.Review.Topic.Category)
                                .Select(x => x.Review)
                                .Select(x => x.Topic)
                                .Where(x => x.State == topicState
                                        && x.Progress == topicProgress).AsSplitQuery().ToListAsync();
        }

        public async Task<List<Topic>> GetTopicHasBeenResolvedForCouncilAsync(Guid councilId)
        {
            var review = await Find(x => x.UserId.Equals(councilId))
                                .Include(x => x.Review)
                                .ThenInclude(x => x.Topic)
                                .ThenInclude(x => x.Category)
                                .AsSplitQuery()
                                .AsNoTracking()
                                .Select(x => x.Review)
                                .ToListAsync();
            return review.GroupBy(x => x.TopicId)
                        .Where(g => g.All(t => t.IsCurrentReview == false))
                        .SelectMany(g => g)
                        .Select(x => x.Topic).ToList();
        }
    }
}
