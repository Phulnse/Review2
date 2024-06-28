using Application.IServices;
using Application.Utils;
using Application.ViewModels.CouncilVMs;
using Application.ViewModels.ReviewVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CouncilInforRes>> ConfigEarlyReviewAsync(ConfigEarlyReviewReq req)
        {
            var review = _mapper.Map<Review>(req);
            await SetStateAndNumberOfReviewAsync(review, ReviewStateEnum.EarlyTermReport, 1);
            await CreateCouncilAndSetTopicStateAsync(review, 5);
            await _unitOfWork.Review.CreateReviewAsync(review);
            await _unitOfWork.Save();

            return _mapper.Map<List<CouncilInforRes>>(await _unitOfWork.Council.GetCouncilsByReviewIdAsync(review.Id));
        }

        public async Task<List<CouncilInforRes>> ConfigFinalReviewAsync(ConfigFinalReviewReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            _mapper.Map(req, review);
            await CreateCouncilAndSetTopicStateAsync(review, 18);
            review.ConfigureConferenceTime = DateTime.Now;
            _unitOfWork.Review.UpdateReview(review);
            await _unitOfWork.Save();

            return _mapper.Map<List<CouncilInforRes>>(await _unitOfWork.Council.GetCouncilsByReviewIdAsync(review.Id));
        }

        public async Task<List<CouncilInforRes>> ConfigMiddleReviewAsync(ConfigMiddleReviewReq req)
        {
            var review = await _unitOfWork.Review.GetLastMiddleReviewAsync(req.TopicId);
            _mapper.Map(req, review);
            await CreateCouncilAndSetTopicStateAsync(review, 13);
            review.ConfigureConferenceTime = DateTime.Now;
            _unitOfWork.Review.UpdateReview(review);
            await _unitOfWork.Save();

            return _mapper.Map<List<CouncilInforRes>>(await _unitOfWork.Council.GetCouncilsByReviewIdAsync(review.Id));
        }

        public async Task EditDeadlineAsync(Guid topicId, ReviewStateEnum reviewState, DateTime deadline)
        {
            var review = await _unitOfWork.Review.GetReview(topicId, reviewState, 1);
            review.ResubmitDeadline = deadline;
            await _unitOfWork.Save();
        }

        public async Task MakeFinalReviewScheduleAsync(MakeFinalReviewScheduleReq req)
        {
            var finalReview = _mapper.Map<Review>(req);
            await SetStateAndNumberOfReviewAsync(finalReview, ReviewStateEnum.FinaltermReport, 1);
            await _unitOfWork.Review.CreateReviewAsync(finalReview);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(16);

            await _unitOfWork.Save();
        }

        public async Task MakeMiddleReviewScheduleAsync(MakeMiddleReviewScheduleReq req)
        {
            var middleReview = _mapper.Map<Review>(req);
            var numberOfReport = (await _unitOfWork.Review.GetNumberOfMiddleReviewAsync(req.TopicId)) + 1;
            await SetStateAndNumberOfReviewAsync(middleReview, ReviewStateEnum.MidtermReport, numberOfReport);
            await _unitOfWork.Review.CreateReviewAsync(middleReview);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(11);

            await _unitOfWork.Save();
        }

        public async Task UpdateEarlyMeetingResultAsync(UploadEarlyMeetingMinutesReq req)
        {
            var review = await _unitOfWork.Review.GetReview(req.TopicId, ReviewStateEnum.EarlyTermReport, 1);
            review.ResultFileLink = req.ResultFileLink;
            review.UploadMeetingMinutiesTime = DateTime.Now;
            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);

            if (req.DecisionOfCouncil == 2)
            {
                review.ResubmitDeadline = req.ResubmitDeadline!.Value;
                updateTopic.SetSateAndProgress(6);
                review.DecisionOfCouncil = CouncilDecisionEnum.Edit;
            }
            else if (req.DecisionOfCouncil == 1)
            {
                updateTopic.SetSateAndProgress(7);
                review.DecisionOfCouncil = CouncilDecisionEnum.Accept;
            }
            else if (req.DecisionOfCouncil == 0)
            {
                review.DecisionOfCouncil = CouncilDecisionEnum.Reject;
                updateTopic.Status = false;
            }

            await _unitOfWork.Save();
        }

        public async Task UpdateFinalMeetingResultAsync(UploadFinalMeetingMinutesReq req)
        {
            var review = await _unitOfWork.Review.GetReview(req.TopicId, ReviewStateEnum.FinaltermReport, 1);
            review.ResultFileLink = req.ResultFileLink;
            review.UploadMeetingMinutiesTime = DateTime.Now;
            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);

            if (req.DecisionOfCouncil == 2)
            {
                review.ResubmitDeadline = req.ResubmitDeadline!.Value;
                updateTopic.SetSateAndProgress(19);
                review.DecisionOfCouncil = CouncilDecisionEnum.Edit;
            }
            else if (req.DecisionOfCouncil == 1)
            {
                updateTopic.SetSateAndProgress(21);
                review.DecisionOfCouncil = CouncilDecisionEnum.Accept;
            }
            else if (req.DecisionOfCouncil == 0)
            {
                review.DecisionOfCouncil = CouncilDecisionEnum.Reject;
                updateTopic.Status = false;
            }

            await _unitOfWork.Save();
        }

        public async Task UploadEvaluateAsync(UploadEvaluateReq req)
        {
            var review = await _unitOfWork.Review.GetCurrentReviewByTopicIdAsync(req.TopicId);
            review.ResultFileLink = req.NewFile.FileLink;
            review.UploadMeetingMinutiesTime = DateTime.Now;

            _unitOfWork.Review.UpdateReview(review);

            var updateTopic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            updateTopic.SetSateAndProgress(14);

            await _unitOfWork.Save();
        }

        private async Task CreateCouncilAndSetTopicStateAsync(Review review, int topicProcess)
        {
            review.Councils.ToList().ForEach(x =>
            {
                x.ReviewId = review.Id;
                x.Id = Guid.NewGuid();
            });

            var updateTopic = (await _unitOfWork.Topic.GetTopicAsync(review.TopicId))
                                .SetSateAndProgress(topicProcess);
            _unitOfWork.Topic.UpdateTopic(updateTopic);
        }

        private async Task SetStateAndNumberOfReviewAsync(Review review, ReviewStateEnum reviewState, int reportNumber)
        {
            review.IsCurrentReview = true;
            review.ReportNumber = reportNumber;
            review.Id = Guid.NewGuid();
            review.State = reviewState;
            await _unitOfWork.Review.RemoveCurrentReviewAsync(review.TopicId);
        }
    }
}
