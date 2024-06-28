using Application.IServices;
using Application.Utils;
using Application.ViewModels.MemberReviewVMs;
using Application.ViewModels.UserVMs;
using AutoMapper;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class MemberReviewService : IMemberReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddMemberReviewAsync(AddMemberReviewReq req)
        {
            await _unitOfWork.MemberReview.AddMemberReviewAsync(req.TopicId, req.MemberReviewIds);
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            topic.SetSateAndProgress(3);
            topic.ReviewStartDate = req.StartDate;
            topic.ReviewEndDate = req.EndDate;
            await _unitOfWork.Save();
        }

        public async Task<List<UserInforRes>> GetMemberReviewOfTopicAsync(Guid topicId)
        {
            return _mapper.Map<List<UserInforRes>>(await _unitOfWork.MemberReview.GetMemberReviewOfTopicAsync(topicId));
        }

        public async Task MemberReviewMakeDecisionAsync(MemberReviewMakeDecisionReq req)
        {
            _unitOfWork.MemberReview.MemberMakeDecision(req.TopicId, req.MemberReviewId, req.IsApproved, req.ReasonOfDecision);
            await _unitOfWork.Save();
        }
    }
}
