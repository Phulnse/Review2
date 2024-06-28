using Application.IServices;
using Application.Utils;
using Application.ViewModels.CouncilVMs;
using Application.ViewModels.ReviewVMs;
using Application.ViewModels.TopicVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Services
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateTopicAsync(CreateTopicReq createTopicReq)
        {
            var newTopic = _mapper.Map<Topic>(createTopicReq);
            newTopic.Id = Guid.NewGuid();
            newTopic.Code = TopicUtil.GenerateTemporaryCode(_unitOfWork.Topic.CountNumberOfFormatCode(TopicUtil.temporaryCodeFormat) + 1);

            newTopic.SetSateAndProgress(1);
            await _unitOfWork.Topic.CreateTopicAsync(newTopic);

            var participants = _mapper.Map<List<Participant>>(createTopicReq.MemberList);
            participants.ForEach(x =>
            {
                x.TopicId = newTopic.Id;
                x.Id = Guid.NewGuid();
            });
            await _unitOfWork.Participant.AddMembersToTopicAsync(participants);

            int numberOfRegenerateCode = 5;
            while (numberOfRegenerateCode > 0)
            {
                try
                {
                    await _unitOfWork.Save();
                    break;
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.InnerException as SqlException;

                    // re-generate topic code if it already exist
                    if (sqlException != null
                        && (sqlException.Number == 2627 || sqlException.Number == 2601))
                    {
                        newTopic.Code = TopicUtil.GenerateTemporaryCode(_unitOfWork.Topic.CountNumberOfFormatCode(TopicUtil.temporaryCodeFormat) + 1);
                        numberOfRegenerateCode--;
                    }

                    else
                        throw;
                }
            }
        }

        public async Task DeanMakeDecisionAsync(DeanMakeDecisionReq req)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            _mapper.Map(req, topic);
            if (req.DeanDecision)
                topic.SetSateAndProgress(2);
            else
                topic.Status = req.DeanDecision;

            topic.MakeDecisionTime = DateTime.Now;
            _unitOfWork.Topic.UpdateTopic(topic);
            await _unitOfWork.Save();
        }

        public async Task<List<TopicInforForReviewerRes>> GetTopicsForReviewMemberAsync(Guid memberId)
        {
            return _mapper.Map<List<TopicInforForReviewerRes>>(await _unitOfWork.MemberReview.GetTopicForMemberReviewAsync(memberId));
        }

        public async Task<List<TopicInfoRes>> GetTopicByStateAndProgressForStaffAsync(TopicStateEnum state, TopicProgressEnum progress)
        {
            return _mapper.Map<List<TopicInfoRes>>(await _unitOfWork.Topic.GetTopicByStateAndProgressForStaffAsync(state, progress));
        }

        public async Task<List<TopicInfoRes>> GetTopicsForDeanAsync(Guid deanId)
        {
            var topics = await _unitOfWork.User.GetTopicForDeanAsync(deanId);
            var topicsForDean = _mapper.Map<List<TopicInfoRes>>(topics);

            return topicsForDean;
        }

        public async Task<List<GetTopicDecidedByDeanIdRes>> GetTopicDecidedByDeanAsync(Guid deanId)
        {
            return _mapper.Map<List<GetTopicDecidedByDeanIdRes>>(await _unitOfWork.Topic.GetTopicDecidedByDeanIdAsync(deanId));
        }

        public async Task<TopicDetailRes> GetTopicDetailAsync(Guid topicId)
        {
            return _mapper.Map<TopicDetailRes>(await _unitOfWork.Topic.GetTopicDetailAsync(topicId));
        }

        public async Task<List<TopicInforForUser>> GetTopicByUserIdAsync(Guid userId)
        {
            var reuslt = _mapper.Map<List<TopicInforForUser>>(await _unitOfWork.User.GetTopicForUserAsync(userId)).OrderByDescending(x => x.CreatedAt).ToList();
            reuslt.ForEach(x => x.IsOwner = x.CreatorId.Equals(userId));

            return reuslt;
        }

        public async Task<List<EarlyTopicForCouncilRes>> GetTopicWaitingChairmanDecisionAsync(Guid councilId)
        {
            var progressInState = TopicUtil.GetProgressInState(8);
            return _mapper.Map<List<EarlyTopicForCouncilRes>>(await GetTopicForCouncilByStateAndProgress(councilId, progressInState.state, progressInState.progress));
        }

        public async Task<TopicDocumentsRes> GetDocumentOfTopicAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topic.GetTopicDocumentAsync(topicId);
            var result = _mapper.Map<TopicDocumentsRes>(topic);
            return result;
        }

        public async Task ChairmanApproveAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(topicId);
            var document = await _unitOfWork.Topic.GetCurrentDocumentOfTopic(topicId);
            document.IsAccepted = true;
            topic.SetSateAndProgress(7);
            await _unitOfWork.Save();
        }

        public async Task ChairmanRejectAsync(ChairmanRejectReq req)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            var document = await _unitOfWork.Topic.GetCurrentDocumentOfTopic(req.TopicId);
            document.IsAccepted = false;
            document.FeedbackFileLink = req.FeedbackFileLink;
            document.UploadFeedbackTime = DateTime.Now;

            topic.SetSateAndProgress(6);
            await _unitOfWork.Save();
        }

        public async Task ChairmanMakeFinalDecisionAsync(ChairmanMakeFinalDecisionReq req)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            var document = await _unitOfWork.Topic.GetCurrentDocumentOfTopic(req.TopicId);
            document.IsAccepted = req.IsAccepted;
            
            if (req.FeedbackFileLink != null)
            {
                document.FeedbackFileLink = req.FeedbackFileLink;
                document.UploadFeedbackTime = DateTime.Now;
            }

            topic.SetSateAndProgress(19);

            if (req.IsAccepted)
                topic.SetSateAndProgress(21);
            await _unitOfWork.Save();
        }

        public async Task<TopicProcessRes> GetTopicProcessAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topic.GetTopicProcessAsync(topicId);
            var topicProcessRes = _mapper.Map<TopicProcessRes>(topic);
            topicProcessRes.PreliminaryReviewProcess = GetPreliminaryReviewProcess(topic);
            if (topic.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).Any())
            {
                topicProcessRes.EarlyTermReportProcess = GetEarlyTermReportProcess(topic);
                topicProcessRes.CurrentDeadline = TopicUtil.GetCurrentDeadline(topic);
            }

            if (topic.Reviews.Where(x => x.State == ReviewStateEnum.MidtermReport).Any())
            {
                topicProcessRes.MiddleTermReportProcess = GetMiddleTermReportProcess(topic);
            }

            if (topic.Reviews.Where(x => x.State == ReviewStateEnum.FinaltermReport).Any())
            {
                topicProcessRes.FinalTermReportProcess = GetFinalTermReportProcess(topic);
            }

            return topicProcessRes;
        }

        public async Task<TopicDocumentsRes> GetEarlyTopicDetailForCouncilAsync(Guid councilId, Guid topicId)
        {
            var topics = await _unitOfWork.Topic.GetTopicDocumentAsync(topicId);
            var reviews = await _unitOfWork.Council.GetReviewsForCouncilAsync(councilId, topicId);
            topics.Reviews.Except(reviews);
            return _mapper.Map<TopicDocumentsRes>(topics);
        }

        public async Task<List<TopicForCouncilRes>> GetOngoingTopicForCouncilAsync(Guid councilId)
        {
            List<TopicForCouncilRes> topicForCouncilRes = new List<TopicForCouncilRes>();
            var reviews = await _unitOfWork.Council.GetOngoingReviewForCouncilAsync(councilId);
            reviews.ForEach(x =>
            {
                var topicForCouncil = _mapper.Map<TopicForCouncilRes>(x.Topic);
                topicForCouncil.ChairmanName = _unitOfWork.Review.GetChairmanOfReview(x.Id).FullName;
                topicForCouncil.HasResultFile = !x.ResultFileLink.IsNullOrEmpty();
                topicForCouncilRes.Add(topicForCouncil);
            });
            return topicForCouncilRes;
        }

        public async Task<List<TopicWaitingResubmitRes>> GetEarlyTopicWaitingResubmitAsync()
        {
            var progressInState = TopicUtil.GetProgressInState(6);
            return _mapper.Map<List<TopicWaitingResubmitRes>>(await _unitOfWork.Topic.GetTopicByStateAndProgressIncludeForStaffAsync(progressInState.state, progressInState.progress));
        }

        public async Task<TopicMeetingInforRes> GetTopicMeetingInforResAsync(Guid userId, Guid topicId)
        {
            TopicMeetingInforRes topicMeetingInfor = new TopicMeetingInforRes();
            var reviewsOfCouncil = await _unitOfWork.Council.GetReviewHasMeetingAsync(userId, topicId);
            if (reviewsOfCouncil.Any())
            {
                topicMeetingInfor.TopicId = topicId;
                topicMeetingInfor.TopicName = reviewsOfCouncil.First().Topic.TopicName;
                topicMeetingInfor.ReviewMeetingInfors = _mapper.Map<List<ReviewMeetingInfor>>(reviewsOfCouncil);
                topicMeetingInfor.ReviewMeetingInfors.ForEach(x =>
                {
                    x.ChairmanName = _unitOfWork.Review.GetChairmanOfReview(x.ReviewId).FullName;
                });

                return topicMeetingInfor;
            }

            var topicOfLeader = await _unitOfWork.Topic.GetMeetingReviewForLeader(userId, topicId);
            if (topicOfLeader != null)
            {
                topicMeetingInfor.TopicId = topicId;
                topicMeetingInfor.TopicName = topicOfLeader.TopicName;
                topicMeetingInfor.ReviewMeetingInfors = _mapper.Map<List<ReviewMeetingInfor>>(topicOfLeader.Reviews);
                topicMeetingInfor.ReviewMeetingInfors.ForEach(x =>
                {
                    x.ChairmanName = _unitOfWork.Review.GetChairmanOfReview(x.ReviewId).FullName;
                });

                return topicMeetingInfor;
            }

            var topicOfMember = await _unitOfWork.Participant.GetMeetingReviewOfParticipantAsync(userId, topicId);
            topicMeetingInfor.TopicId = topicId;
            topicMeetingInfor.TopicName = topicOfMember.TopicName;
            topicMeetingInfor.ReviewMeetingInfors = _mapper.Map<List<ReviewMeetingInfor>>(topicOfMember.Reviews);
            topicMeetingInfor.ReviewMeetingInfors.ForEach(x =>
            {
                x.ChairmanName = _unitOfWork.Review.GetChairmanOfReview(x.ReviewId).FullName;
            });

            return topicMeetingInfor;
        }

        public async Task<List<TopicInfoRes>> GetAllActiveTopicAsync()
        {
            return _mapper.Map<List<TopicInfoRes>>(await _unitOfWork.Topic.GetAllActiveTopicAsync());
        }

        public async Task<ReviewsOfTopicRes> GetDocumentsOfTopicAsync(Guid userId, Guid topicId)
        {
            ReviewsOfTopicRes result = new ReviewsOfTopicRes();
            if (await _unitOfWork.Participant.CheckMemberOfTopicAsync(userId, topicId) || await _unitOfWork.Topic.CheckTopicOwner(userId, topicId))
            {
                if (await _unitOfWork.Topic.CheckTopicOwner(userId, topicId))
                    result.Role = "Leader";
                else
                    result.Role = "Member";
                var reviews = await _unitOfWork.Review.GetAllReviewsOfTopic(topicId);
                var earlyReveiw = reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First();
                result.ReviewEarlyDocument = _mapper.Map<ReviewEarlyDocument>(earlyReveiw);
            }

            var reviewOfCouncil = await _unitOfWork.Council.GetReviewsWithDocumentsForCouncilAsync(userId, topicId);
            if (reviewOfCouncil.Any())
            {
                var earlyReveiw = reviewOfCouncil.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First();
                result.ReviewEarlyDocument = _mapper.Map<ReviewEarlyDocument>(earlyReveiw);
                if (await _unitOfWork.Council.CheckRoleOfCouncil(userId, topicId))
                    result.Role = "Chairman";
                else
                    result.Role = "Council";
            }

            return result;
        }

        public async Task MoveTopicStateToMiddleTermAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(topicId);
            topic.SetSateAndProgress(10);

            await _unitOfWork.Save();
        }

        public async Task<List<TopicInfoRes>> GetMiddleTopicWaitingForConfigureAsync()
        {
            var topics = await _unitOfWork.Review.GetMiddleTopicsWaitingForConfigureAsync();
            return _mapper.Map<List<TopicInfoRes>>(topics);
        }

        public async Task<List<TopicReviewedForMemberRes>> GetTopicReviewedForMemberAsync(Guid memberId)
        {
            List<TopicReviewedForMemberRes> result = new List<TopicReviewedForMemberRes>();
            var memberReviews = await _unitOfWork.MemberReview.GetTopicReviewedForMemberAsync(memberId);
            memberReviews.ForEach(x =>
            {
                var topicReviewedForMemberRes = _mapper.Map<TopicReviewedForMemberRes>(x.Topic);
                topicReviewedForMemberRes.MemberDecision = x.IsApproved!.Value;
                result.Add(topicReviewedForMemberRes);
            });

            return result;
        }

        public async Task<List<TopicWaitingUploadMeetingMinutesRes>> GetTopicWaitingUploadMeetingMinutesAsync()
        {
            var topics = await _unitOfWork.Topic.GetTopicWaitingForUploadMeetingMinutesAsync();
            var result = _mapper.Map<List<TopicWaitingUploadMeetingMinutesRes>>(topics);
            return result;
        }

        public async Task<List<TopicInfoRes>> GetTopicHasBeenResolvedForCouncilAsync(Guid councilId)
        {
            var topic = await _unitOfWork.Council.GetTopicHasBeenResolvedForCouncilAsync(councilId);
            var result = _mapper.Map<List<TopicInfoRes>>(topic);
            return result;
        }

        public async Task MoveTopicStateToFinalTermAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topic.GetTopicAsync(topicId);
            topic.SetSateAndProgress(15);

            await _unitOfWork.Save();
        }

        public async Task<List<TopicInfoRes>> GetAllTopicAsync()
        {
            return _mapper.Map<List<TopicInfoRes>>(await _unitOfWork.Topic.GetTopicListAsync());
        }

        #region
        private PreliminaryReviewProcess GetPreliminaryReviewProcess(Topic topic)
        {
            PreliminaryReviewProcess preliminaryReview = new PreliminaryReviewProcess();
            var waitingForDean = TopicUtil.GetPreTopicWaitingForDeanStatus(topic);
            preliminaryReview.DeanMakeDecisionTime = topic.MakeDecisionTime;
            preliminaryReview.WaitingForDean = waitingForDean.ToString();
            if (waitingForDean != ProcessEnum.Accept)
            {
                return preliminaryReview;
            }

            var waitingForCouncilFormation = TopicUtil.GetPreTopicWaitingForCouncilFormationStatus(topic);
            preliminaryReview.WaitingForCouncilFormation = waitingForCouncilFormation.ToString();
            if (waitingForCouncilFormation != ProcessEnum.Done)
            {
                return preliminaryReview;
            }

            preliminaryReview.CouncilFormationTime = topic.MemberReviews.First().CreatedAt;
            preliminaryReview.WaitingForCouncilDecision = TopicUtil.GetPreWaitingForCouncilDecisionStatus(topic).ToString();
            preliminaryReview.CouncilMakeDecisionTime = topic.SumarizeResultTime;

            return preliminaryReview;
        }

        private EarlyTermReportProcess GetEarlyTermReportProcess(Topic topic)
        {
            EarlyTermReportProcess earlyTermReport = new EarlyTermReportProcess();
            var waitingForCouncilFormation = TopicUtil.GetEarlyWaitingForConfigureConferenceStatus(topic);
            earlyTermReport.WaitingForCouncilFormation = waitingForCouncilFormation.ToString();
            if (waitingForCouncilFormation == ProcessEnum.OnGoing)
                return earlyTermReport;

            var earlyReview = topic.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First();
            earlyTermReport.CouncilFormationTime = earlyReview.CreatedAt;
            var waitingForUploadMeetingMinutes = TopicUtil.GetEarlyWaitingForUploadMeetingMinutesStatus(topic);
            earlyTermReport.WaitingForUploadMeetingMinutes = waitingForUploadMeetingMinutes.ToString();
            if (waitingForUploadMeetingMinutes == ProcessEnum.OnGoing)
                return earlyTermReport;

            earlyTermReport.UploadMeetingMinutesTime = earlyReview.UploadMeetingMinutiesTime;
            var resubmitProcesses = TopicUtil.GetEarlyResubmitProcessStatus(topic);
            earlyTermReport.ResubmitProcesses = resubmitProcesses;
            if (!resubmitProcesses.Any())
                return earlyTermReport;

            var waitingForContractSigning = TopicUtil.GetEarlyWaitingForContractSigningStatus(topic);
            earlyTermReport.WaitingForContractSigning = waitingForContractSigning.ToString();
            return earlyTermReport;
        }

        private List<MiddleTermReportProcess> GetMiddleTermReportProcess(Topic topic)
        {
            var middleReviews = topic.Reviews.Where(x => x.State == ReviewStateEnum.MidtermReport).OrderBy(x => x.ReportNumber).ToList();
            List<MiddleTermReportProcess> middleTermReports = new List<MiddleTermReportProcess>();

            middleReviews.ForEach(x =>
            {
                var middleTermReport = new MiddleTermReportProcess();
                middleTermReports.Add(middleTermReport);
                middleTermReport.NumberOfReport = x.ReportNumber;
                middleTermReport.WaitingForMakeReviewSchedule = ProcessEnum.Done.ToString();
                middleTermReport.MakeReviewScheduleTime = x.CreatedAt;
                var waitingForDocumentSupplementation = TopicUtil.GetMiddleWaitingForDocumentSupplementationStatus(x);
                middleTermReport.WaitingForDocumentSupplementation = waitingForDocumentSupplementation.ToString();

                if (waitingForDocumentSupplementation == ProcessEnum.OnGoing)
                {
                    middleTermReport.DeadlineForDocumentSupplementation = x.DocumentSupplementationDeadline;
                    return;
                }

                middleTermReport.DocumentSupplementationTime = x.Documents.First().CreatedAt;
                var waitingForConfigureConference = TopicUtil.GetMiddleWaitingForConfigureConferenceStatus(x);
                middleTermReport.WaitingForConfigureConference = waitingForConfigureConference.ToString();
                if (waitingForConfigureConference == ProcessEnum.OnGoing)
                    return;

                middleTermReport.ConfigureConferenceTime = x.ConfigureConferenceTime;
                var waitingForUploadEvaluate = TopicUtil.GetMiddleWaitingForUploadMeetingMinutesStatus(x);
                middleTermReport.WaitingForUploadEvaluate = waitingForUploadEvaluate.ToString();
                if (waitingForUploadEvaluate == ProcessEnum.OnGoing)
                {
                    return;
                }
                middleTermReport.UploadEvaluateTime = x.UploadMeetingMinutiesTime;
            });

            return middleTermReports;
        }

        private FinalTermReportProcess GetFinalTermReportProcess(Topic topic)
        {
            var finalReview = topic.Reviews.Where(x => x.State == ReviewStateEnum.FinaltermReport).First();
            var finalTermReport = new FinalTermReportProcess();

            finalTermReport.WaitingForMakeReviewSchedule = ProcessEnum.Done.ToString();
            finalTermReport.MakeReviewScheduleTime = finalReview.CreatedAt;
            var waitingForDocumentSupplementation = TopicUtil.GetFinalWaitingForDocumentSupplementationStatus(finalReview);
            finalTermReport.WaitingForDocumentSupplementation = waitingForDocumentSupplementation.ToString();

            if (waitingForDocumentSupplementation == ProcessEnum.OnGoing)
            {
                finalTermReport.DeadlineForDocumentSupplementation = finalReview.DocumentSupplementationDeadline;
                return finalTermReport;
            }

            finalTermReport.DocumentSupplementationTime = finalReview.Documents.First().CreatedAt;
            var waitingForConfigureConference = TopicUtil.GetFinalWaitingForConfigureConferenceStatus(finalReview);
            finalTermReport.WaitingForConfigureConference = waitingForConfigureConference.ToString();
            if (waitingForConfigureConference == ProcessEnum.OnGoing)
                return finalTermReport;

            finalTermReport.ConfigureConferenceTime = finalReview.ConfigureConferenceTime;
            var waitingForUploadMeetingMinutes = TopicUtil.GetFinalWaitingForUploadMeetingMinutesStatus(finalReview);
            finalTermReport.WaitingForUploadMeetingMinutes = waitingForUploadMeetingMinutes.ToString();
            if (waitingForUploadMeetingMinutes == ProcessEnum.OnGoing)
                return finalTermReport;

            finalTermReport.UploadMeetingMinutesTime = finalReview.UploadMeetingMinutiesTime;
            var resubmitDocument = topic.Reviews.Where(x => x.State == ReviewStateEnum.FinaltermReport).First().Documents.OrderBy(x => x.CreatedAt).ToList();
            if (resubmitDocument.Count() > 1)
            {
                var resubmitProcesses = TopicUtil.GetFinalResubmitProcessStatus(resubmitDocument);
                finalTermReport.ResubmitProcesses = resubmitProcesses;
            }            
            
            return finalTermReport;
        }

        private async Task<List<Topic>> GetTopicForCouncilByStateAndProgress(Guid councilId, TopicStateEnum topicState, TopicProgressEnum topicProgress)
        {
            return await _unitOfWork.Council.GetTopicByStateAndProgressForCouncilAsync(councilId, topicState, topicProgress);
        }
        #endregion
    }
}
