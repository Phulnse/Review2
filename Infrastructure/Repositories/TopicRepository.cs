using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(SRMSContext context) : base(context)
        {
        }

        public int CountNumberOfFormatCode(string codeFormat)
        {
            return Find(x => x.Code.StartsWith(codeFormat)).Count();
        }

        public async Task CreateTopicAsync(Topic topic)
        {
            await AddAsync(topic);
        }

        public async Task<Document> GetCurrentDocumentOfTopic(Guid topicId)
        {
            var topic = await Find(x => x.Id.Equals(topicId)).Include(x => x.Reviews.Where(r => r.IsCurrentReview)).FirstAsync();
            var review = topic.Reviews.First();
            return review.Documents.OrderByDescending(x => x.CreatedAt).First();
        }

        public async Task<Topic> GetTopicAsync(Guid topicId)
        {
            return await GetByIdAsync(topicId);
        }

        public async Task<List<Topic>> GetTopicByStateAndProgressForStaffAsync(TopicStateEnum state, TopicProgressEnum progress)
        {
            return await Find(x => x.State == state 
                            && x.Progress == progress 
                            && x.Status).Include(x => x.Category).ToListAsync();                     
        }

        public async Task<List<Topic>> GetTopicDecidedByDeanIdAsync(Guid deanId)
        {
            return await Find(x => x.DeciderId.Equals(deanId)).Include(x => x.Category).ToListAsync();
        }

        public async Task<Topic> GetTopicDetailAsync(Guid topicId)
        {
            return await Find(x => x.Id.Equals(topicId))
                        .Include(x => x.Category)
                        .Include(x => x.Creator)
                        .Include(x => x.Participants)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstAsync();
        }

        public async Task<Topic> GetTopicProcessAsync(Guid topicId)
        {
            return await Find(x => x.Id.Equals(topicId))
                        .Include(x => x.Contracts)
                        .Include(x => x.MemberReviews)
                        .Include(x => x.Reviews)
                        .ThenInclude(x => x.Documents)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstAsync();
        }

        public async Task<Topic> GetTopicDocumentAsync(Guid topicId)
        {
            return await Find(x => x.Id.Equals(topicId))
                        .Include(x => x.Category)
                        .Include(x => x.Reviews)                        
                        .ThenInclude(x => x.Documents)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstAsync();
        }

        public void SummarizeTheResultsAsync(List<Guid> topicIds)
        {
            if (topicIds.Any())
                topicIds.ForEach(id =>
                {
                    var topic = Find(x => x.Id.Equals(id)).Include(x => x.MemberReviews).First();
                    if (topic.MemberReviews.Where(x => x.IsApproved == true).Count() > topic.MemberReviews.Where(x => x.IsApproved == false).Count())
                    {
                        topic.SetSateAndProgress(4);
                        topic.SumarizeResultTime = DateTime.Now;
                    }                    
                    else
                        topic.Status = false;
                    Update(topic);
                });
        }

        public void UpdateTopic(Topic topic)
        {
            Update(topic);
        }

        public async Task<List<Topic>> GetTopicByStateAndProgressIncludeForStaffAsync(TopicStateEnum state, TopicProgressEnum progress)
        {
            return await Find(x => x.State == state
                            && x.Progress == progress
                            && x.Status)
                        .Include(x => x.Category)
                        .Include(x => x.Reviews)
                        .ToListAsync();
        }

        public Task<List<Topic>> GetAllActiveTopicAsync()
        {
            return Find(x => x.Status)
                    .Include(x => x.Category)
                    .ToListAsync();
        }

        public async Task<bool> CheckTopicOwner(Guid userId, Guid topicId)
        {
            return await Find(x => x.CreatorId.Equals(userId) && topicId.Equals(topicId)).AnyAsync();
        }

        public async Task<Topic?> GetMeetingReviewForLeader(Guid userId, Guid topicId)
        {
            return await Find(x => x.CreatorId.Equals(userId) && x.Id.Equals(topicId))
                        .Include(x => x.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport || x.State == ReviewStateEnum.FinaltermReport))
                        .FirstOrDefaultAsync();
        }

        public async Task<bool> IsValidTopicAsync(Guid topicId, TopicStateEnum stateEnum, TopicProgressEnum progressEnum)
        {
            return await Find(x => x.Id.Equals(topicId) && x.State == stateEnum && x.Progress == progressEnum && x.Status).AnyAsync();
        }

        public async Task<List<Topic>> GetTopicWaitingForUploadMeetingMinutesAsync()
        {
            return await Find(x => x.Progress == TopicProgressEnum.WaitingForUploadMeetingMinutes
                                && x.Status)
                        .Include(x => x.Category)
                        .ToListAsync();
        }

        public async Task<List<Guid>> GetMemberIdOfTopicAsync(Guid topicId)
        {
            var topic = await Find(x => x.Id.Equals(topicId)).Include(x => x.Participants).FirstAsync();
            var memberIdList = topic.Participants.Select(x => x.UserId).ToList();
            memberIdList.Add(topic.CreatorId);

            return memberIdList;
        }

        public async Task<Guid> GetDepartmentIdOfTopicCreatorAsync(Guid topicId)
        {
            return await Find(x => x.Id.Equals(topicId)).Include(x => x.Creator).Select(x => x.Creator.DepartmentId).FirstAsync();
        }

        public async Task<bool> IsValidToAddCouncilAsync(Guid topicId, List<Guid> councilIdList)
        {
            var topic = await Find(x => x.Id.Equals(topicId))
                                .Include(x => x.Participants)
                                .Include(x => x.MemberReviews)
                                .AsSplitQuery()
                                .FirstAsync();
            var isParticipant = topic.Participants.Select(x => x.UserId).All(councilIdList.Contains);
            var numberOfReviewMember = 0;
            topic.MemberReviews.Select(x => x.UserId).ToList().ForEach(id =>
            {
                if (councilIdList.Contains(id))
                    numberOfReviewMember++;
            });

            return !isParticipant && numberOfReviewMember == 1;
        }

        public async Task<List<Topic>> GetTopicListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public IEnumerable<Notify> GetNotifiesOfTopicForOwner(Guid userId)
        {
            return Find(x => x.CreatorId.Equals(userId))
                    .Include(x => x.Notifies)
                    .ThenInclude(x => x.Topic)
                    .SelectMany(x => x.Notifies)
                    .AsNoTracking()
                    .AsEnumerable();
        }
    }
}
