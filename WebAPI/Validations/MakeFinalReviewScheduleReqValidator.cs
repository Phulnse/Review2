using Application.ViewModels.ReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class MakeFinalReviewScheduleReqValidator : AbstractValidator<MakeFinalReviewScheduleReq>
    {
        public MakeFinalReviewScheduleReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForMakeReviewSchedule);
            });
            RuleFor(x => x.DocumentSupplementationDeadline).Must(x => DateTime.Compare(x, DateTime.Now) > 0);
        }
    }
}
