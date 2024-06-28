using Application.ViewModels.CouncilVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Validations
{
    public class ChairmanMakeFinalDecisionReqValidator : AbstractValidator<ChairmanMakeFinalDecisionReq>
    {
        public ChairmanMakeFinalDecisionReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForCouncilDecision);
            });

            RuleFor(x => new
            {
                x.FeedbackFileLink,
                x.IsAccepted,
            }).Must(x =>
            {
                if (!x.IsAccepted)
                    return !x.FeedbackFileLink.IsNullOrEmpty();
                return true;
            });
        }
    }
}
