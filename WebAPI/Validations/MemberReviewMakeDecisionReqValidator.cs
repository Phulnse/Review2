using Application.ViewModels.MemberReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Validations
{
    public class MemberReviewMakeDecisionReqValidator : AbstractValidator<MemberReviewMakeDecisionReq>
    {
        public MemberReviewMakeDecisionReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForCouncilDecision);
            });
            RuleFor(x => new
            {
                x.TopicId,
                x.MemberReviewId,
            }).MustAsync(async (id, _) =>
            {
                return await unitOfWork.MemberReview.IsValidToMakeDecisionAsync(id.TopicId, id.MemberReviewId);
            });
            RuleFor(x => new
            {
                x.IsApproved,
                x.ReasonOfDecision,
            }).Must(decision =>
            {
                if (!decision.IsApproved)
                    return !decision.ReasonOfDecision.IsNullOrEmpty();

                return true;
            });
        }
    }
}
