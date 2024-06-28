using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MemberReviewRepository : GenericRepository<MemberReview>, IMemberReviewRepository
    {
        public MemberReviewRepository(SRMSContext context) : base(context)
        {
        }

        public async Task AddMemberReviewAsync(Guid topicId, List<Guid> memberReviewIds)
        {
            List<MemberReview> memberList = new List<MemberReview>();
            foreach (var memberReviewId in memberReviewIds)
            {
                var memberReview = new MemberReview()
                {
                    Id = Guid.NewGuid(),
                    TopicId = topicId,
                    UserId = memberReviewId,
                };

                memberList.Add(memberReview);
            }

            await AddBulkAsync(memberList);
        }

        public async Task<List<Topic>> GetTopicForMemberReviewAsync(Guid memberId)
        {
            return await Find(x => x.UserId == memberId && x.IsApproved == null)
                            .Include(x => x.Topic.Category)
                            .Select(x => x.Topic)
                            .ToListAsync();
        }

        public void MemberMakeDecision(Guid topicId, Guid memberReviewId, bool decision, string? reason)
        {
            var memberReview = Find(x => x.TopicId.Equals(topicId) && x.UserId.Equals(memberReviewId)).First();
            memberReview.IsApproved = decision;
            if (!decision)
            {
                memberReview.ReasonOfDecision = reason;
            }
            Update(memberReview);
        }

        public async Task SetDefaultValueForMemberNotResponse(DateTime time)
        {
            var memberReviews = await Find(x => x.IsApproved == null)
                                        .Include(x => x.Topic)
                                        .Where(x => x.Topic.ReviewEndDate != null && DateTime.Compare(time, x.Topic.ReviewEndDate.Value) > 0)
                                        .ToListAsync();
            if (memberReviews.Any())
            {
                memberReviews.ForEach(x => x.IsApproved = true);
                UpdateBulk(memberReviews);
            }
        }

        public async Task<List<Guid>> GetTopicDecidedByAllMembersAsync()
        {
            return await GetAll().Include(x => x.Topic)
                                    .Where(x => x.Topic.State == TopicStateEnum.PreliminaryReview 
                                            && x.Topic.Progress == TopicProgressEnum.WaitingForCouncilDecision 
                                            && x.Topic.Status)
                                                .GroupBy(x => x.TopicId)
                                                .Select(members => new
                                                    {
                                                        TopicId = members.Key,
                                                        NumberOfReviewer = members.Count(),
                                                        NumberOfNonNullDecision = members.Where(x => x.IsApproved != null).Count(),
                                                    }).Where(x => x.NumberOfReviewer == x.NumberOfNonNullDecision).Select(x => x.TopicId).ToListAsync();
        }

        public async Task<List<MemberReview>> GetTopicReviewedForMemberAsync(Guid memberId)
        {
            return await Find(x => x.UserId.Equals(memberId))
                        .Include(x => x.Topic)
                        .ThenInclude(x => x.Category)
                        .Where(x => x.IsApproved != null)
                        .ToListAsync();
        }

        public async Task<List<User>> GetMemberReviewOfTopicAsync(Guid topicId)
        {
            return await Find(x => x.TopicId.Equals(topicId))
                        .Include(x => x.User)
                        .Select(x => x.User)
                        .ToListAsync();
        }

        public async Task<bool> IsValidToMakeDecisionAsync(Guid topicId, Guid userId)
        {
            var currentTime = DateTime.Now;
            var memberReview = await Find(x => x.TopicId.Equals(topicId) 
                                            && x.UserId.Equals(userId) && x.IsApproved == null)
                                    .Include(x => x.Topic)
                                    .FirstOrDefaultAsync();
            if (memberReview != null 
                && memberReview.IsApproved == null 
                && DateTime.Compare(currentTime, memberReview.Topic.ReviewStartDate!.Value) > 0
                && DateTime.Compare(currentTime, memberReview.Topic.ReviewEndDate!.Value) < 0)
            {
                return true;
            }

            return false;
        }
    }
}
