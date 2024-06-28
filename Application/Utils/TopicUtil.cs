using Application.ViewModels.TopicVMs;
using Domain.Entities;
using Domain.Enums;
using System.Data;

namespace Application.Utils
{
    public static class TopicUtil
    {
        public const string officalCodeFormat = "CS/NĐ2/";
        public static string temporaryCodeFormat = (DateTime.Now.Year % 100).ToString();

        public static Topic SetSateAndProgress(this Topic topic, int selectNumber)
        {
            var progressInState = GetProgressInState(selectNumber);
            topic.Progress = progressInState.progress;
            topic.State = progressInState.state;
            return topic;
        }

        public static string GenerateTemporaryCode(int number)
        {
            var code = (DateTime.Now.Year % 100).ToString() + number.ToString("D4");
            return code;
        }

        public static string GenerateOfficalCode(int number)
        {
            var code = "CS/NĐ2/" + (DateTime.Now.Year % 100).ToString() + "/" + number.ToString("D3");
            return code;
        }

        public static (TopicStateEnum state, TopicProgressEnum progress) GetProgressInState(int selectNumber)
        {
            switch (selectNumber)
            {
                //just create Topic => waiting Dean make decision
                case 1:
                    return (TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForDean);
                //approved by Dean => waiting for staff to create review members
                case 2:                
                    return (TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForCouncilFormation);
                //created review members => waiting for review members make decision
                case 3:
                    return (TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForCouncilDecision);
                //passed review members => waiting for configure conference include council formation and create meeting schedule
                case 4:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForConfigureConference);
                //formed council => waiting for coucil meeting
                case 5:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes);
                //decision of council is false => waiting for edit document
                case 6:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForDocumentEditing);
                //decision of council is true or edit document is accepted => waiting for upload contract
                case 7:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadContract);
                //edit document complete => waiting for chairman decision
                case 8:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForCouncilDecision);
                //upload contract complete => waiting for move to middle term
                case 9:
                    return (TopicStateEnum.EarlyTermReport, TopicProgressEnum.Completed);
                //staff config to done early review => waiting for make review schedule
                case 10:
                    return (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForMakeReviewSchedule);
                // staff made middle review schedule => waiting for user upload document
                case 11:
                    return (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForDocumentSupplementation);
                // user uploaded document => waiting for configure conference include council formation and create meeting schedule
                case 12:
                    return (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForConfigureConference);
                // user created council => waiting for evaluate of coucil
                case 13:
                    return (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes);
                // uploaded evaluate 
                case 14:
                    return (TopicStateEnum.MidtermReport, TopicProgressEnum.Completed);
                // waiting for make schedule final report
                case 15:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForMakeReviewSchedule);
                // made final review schedule => waiting for upload document
                case 16:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForDocumentSupplementation);
                // uploaded document => waiting for config conference
                case 17:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForConfigureConference);
                // config done => waiting for upload meeting minutes
                case 18:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes);
                // uploaded meeting minutes and decision is accept => done
                case 19:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForDocumentEditing);
                // uploaded meeting minutes and decision is edit => waiting for edit document
                case 20:
                    return (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForCouncilDecision);
                // resubmited document => waiting for chairman decision
                case 21:
                    return (TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForSubmitRemuneration);
                // submited remuneration => waiting for staff censorship
                case 22:
                    return (TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForCensorshipRemuneration);
                // staff accept remuneration => waiting for uploadfinal contract
                case 23:
                    return (TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForUploadContract);
                // uploaded contract => done
                case 24:
                    return (TopicStateEnum.EndingPhase, TopicProgressEnum.Completed);
                default:
                    return (TopicStateEnum.EndingPhase, TopicProgressEnum.Completed);
            }
        }

        public static ProcessEnum GetPreTopicWaitingForCouncilFormationStatus(Topic topic)
        {
            var checkMemberReview = topic.MemberReviews.Any();
            if (checkMemberReview)
            {
                return ProcessEnum.Done;
            }

            return ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetPreWaitingForCouncilDecisionStatus(Topic topic)
        {
            var memberReviewList = topic.MemberReviews;
            if (!memberReviewList.Where(x => x.IsApproved == null).Any())
            {
                if (memberReviewList.Where(x => x.IsApproved == true).Count() > memberReviewList.Where(x => x.IsApproved == false).Count())
                    return ProcessEnum.Accept;

                return ProcessEnum.Reject;
            }

            return ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetPreTopicWaitingForDeanStatus(Topic topic)
        {
            var deanDecision = topic.DeanDecision;
            switch (deanDecision)
            {
                case true:
                    return ProcessEnum.Accept;
                case false:
                    return ProcessEnum.Reject;
                default:
                    return ProcessEnum.OnGoing;
            }
        }

        public static ProcessEnum GetEarlyWaitingForConfigureConferenceStatus(Topic topic)
        {
            var council = topic.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).FirstOrDefault();
            if (council == null)
                return ProcessEnum.OnGoing;
            return ProcessEnum.Done;
        }

        public static ProcessEnum GetEarlyWaitingForUploadMeetingMinutesStatus(Topic topic)
        {
            var decisionOfCouncil = topic.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First().DecisionOfCouncil;
            switch (decisionOfCouncil)
            {
                case CouncilDecisionEnum.Accept:
                    return ProcessEnum.Accept;
                case CouncilDecisionEnum.Reject:
                    return ProcessEnum.Reject;
                case CouncilDecisionEnum.Edit:
                    return ProcessEnum.Edit;
                default:
                    return ProcessEnum.OnGoing;
            }
        }

        public static List<ResubmitProcess> GetEarlyResubmitProcessStatus(Topic topic)
        {
            List<ResubmitProcess> resubmitProcessesList = new List<ResubmitProcess>();
            var documents = topic.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First().Documents.OrderBy(x => x.CreatedAt).ToList();
            var numberOfResubmit = 1;
            documents.ForEach(x =>
            {                
                var resubmit = new ResubmitProcess();
                resubmit.NumberOfResubmmit = numberOfResubmit++;
                resubmit.UploadDocumentTime = x.CreatedAt;
                resubmit.UploadFeedbackTime = x.UploadFeedbackTime;
                switch (x.IsAccepted)
                {
                    case true:
                        resubmit.WaitingForDocumentEditing = ProcessEnum.Done.ToString();
                        resubmit.WaitingForCouncilDecision = ProcessEnum.Accept.ToString();
                        break;
                    case false:
                        resubmit.WaitingForDocumentEditing = ProcessEnum.Done.ToString();
                        resubmit.WaitingForCouncilDecision = ProcessEnum.Edit.ToString();
                        break;
                    default:                        
                        resubmit.WaitingForCouncilDecision = ProcessEnum.NotStarted.ToString();
                        if (x.DocumentFileLink != null)
                        {
                            resubmit.WaitingForDocumentEditing = ProcessEnum.Done.ToString();
                            resubmit.WaitingForCouncilDecision = ProcessEnum.OnGoing.ToString();                            
                        }
                        else
                            resubmit.WaitingForDocumentEditing = ProcessEnum.OnGoing.ToString();
                        break;
                }

                resubmitProcessesList.Add(resubmit);
            });

            return resubmitProcessesList;
        }

        public static ProcessEnum GetEarlyWaitingForContractSigningStatus(Topic topic)
        {
            var contracts = topic.Contracts;
            if (contracts.Where(x => x.State == ContractStateEnum.EarlyTermContract).Any())
            {
                return ProcessEnum.Done;
            }

            return ProcessEnum.OnGoing;
        }

        public static DateTime? GetCurrentDeadline(Topic topic)
        {
            if (!topic.Progress.Equals(TopicProgressEnum.WaitingForDocumentEditing))
                return null;
            switch (topic.State)
            {
                case TopicStateEnum.EarlyTermReport:
                    return topic.Reviews.Where(x => x.State.Equals(ReviewStateEnum.EarlyTermReport)).First().ResubmitDeadline;
                case TopicStateEnum.MidtermReport:
                    return topic.Reviews.Where(x => x.State.Equals(ReviewStateEnum.MidtermReport)).First().ResubmitDeadline;
                case TopicStateEnum.FinaltermReport:
                    return topic.Reviews.Where(x => x.State.Equals(ReviewStateEnum.FinaltermReport)).First().ResubmitDeadline;
                default:
                    return null;
            }
        }

        public static ProcessEnum GetMiddleWaitingForDocumentSupplementationStatus(Review review)
        {
            if (review.Documents.Any())
                return ProcessEnum.Done;
            return
                ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetMiddleWaitingForConfigureConferenceStatus(Review x)
        {
            if (x.MeetingTime != null)
                return ProcessEnum.Done;
            return ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetMiddleWaitingForUploadMeetingMinutesStatus(Review review)
        {
            if (review.ResultFileLink != null)
                return ProcessEnum.Done;
            return ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetFinalWaitingForDocumentSupplementationStatus(Review review)
        {
            if (review.Documents.Any())
                return ProcessEnum.Done;
            return
                ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetFinalWaitingForConfigureConferenceStatus(Review review)
        {
            if (review.MeetingTime != null)
                return ProcessEnum.Done;
            return ProcessEnum.OnGoing;
        }

        public static ProcessEnum GetFinalWaitingForUploadMeetingMinutesStatus(Review review)
        {
            if (review.ResultFileLink != null)
                return ProcessEnum.Done;
            return ProcessEnum.OnGoing;
        }

        public static List<ResubmitProcess> GetFinalResubmitProcessStatus(List<Document> documents)
        {
            List<ResubmitProcess> resubmitProcessesList = new List<ResubmitProcess>();
            documents = documents.Skip(1).ToList();
            var numberOfResubmit = 1;

            documents.ForEach(x =>
            {
                var resubmit = new ResubmitProcess();
                resubmit.NumberOfResubmmit = numberOfResubmit++;
                resubmit.UploadDocumentTime = x.CreatedAt;
                resubmit.UploadFeedbackTime = x.UploadFeedbackTime;
                resubmit.WaitingForDocumentEditing = ProcessEnum.Done.ToString();
                switch (x.IsAccepted)
                {
                    case true:
                        resubmit.WaitingForCouncilDecision = ProcessEnum.Accept.ToString();
                        break;
                    case false:
                        resubmit.WaitingForCouncilDecision = ProcessEnum.Reject.ToString();
                        break;
                    default:
                        resubmit.WaitingForCouncilDecision = ProcessEnum.OnGoing.ToString();
                        break;
                }

                resubmitProcessesList.Add(resubmit);
            });

            return resubmitProcessesList;
        }
    }
}
