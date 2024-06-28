using Application.ViewModels.ReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class UploadFinalMeetingMinutesReqValidator : AbstractValidator<UploadFinalMeetingMinutesReq>
    {
        public UploadFinalMeetingMinutesReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes);
            });
            RuleFor(x => x.DecisionOfCouncil).Must(decision => decision >= 1 && decision <= 3);
            RuleFor(x => new
            {
                x.DecisionOfCouncil,
                x.ResubmitDeadline,
            }).Must(decision =>
            {
                switch (decision.DecisionOfCouncil)
                {
                    case 2:
                        return decision.ResubmitDeadline != null && DateTime.Compare(decision.ResubmitDeadline.Value, DateTime.Now) > 0;
                    default:
                        return decision.ResubmitDeadline == null;
                }
            });
        }
    }
}
