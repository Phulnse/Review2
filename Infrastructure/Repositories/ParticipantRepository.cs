using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ParticipantRepository : GenericRepository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(SRMSContext context) : base(context)
        {
        }

        public async Task AddMembersToTopicAsync(List<Participant> members)
        {
            await AddBulkAsync(members);
        }

        public async Task<bool> CheckMemberOfTopicAsync(Guid userId, Guid topicId)
        {
            return await Find(x => x.UserId.Equals(userId) && x.TopicId.Equals(topicId)).AnyAsync();
        }

        public async Task<Topic> GetMeetingReviewOfParticipantAsync(Guid userId, Guid topicId)
        {
            return await Find(x => x.UserId.Equals(userId) && x.TopicId.Equals(topicId))
                        .Include(x => x.Topic)
                        .ThenInclude(x => x.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport || x.State == ReviewStateEnum.FinaltermReport))
                        .Select(x => x.Topic)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstAsync();
        }

        public async Task<bool> IsValidToAddMemberReviewAsync(Guid topicId, List<Guid> memberReviewId)
        {
            var participantIdList = await Find(x => x.TopicId.Equals(topicId)).Select(x => x.UserId).ToListAsync();
            return !participantIdList.All(memberReviewId.Contains);
        }
    }
}
