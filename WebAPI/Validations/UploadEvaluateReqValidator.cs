using Application.ViewModels.ReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class UploadEvaluateReqValidator : AbstractValidator<UploadEvaluateReq>
    {
        public UploadEvaluateReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes);
            });
        }
    }
}
