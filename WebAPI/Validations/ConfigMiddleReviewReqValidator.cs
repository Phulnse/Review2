using Application.ViewModels.ReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class ConfigMiddleReviewReqValidator : AbstractValidator<ConfigMiddleReviewReq>
    {
        public ConfigMiddleReviewReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForConfigureConference);
            });
            RuleFor(x => x.MeetingTime).Must(x => DateTime.Compare(x, DateTime.Now) > 0);
            RuleFor(x => new
            {
                x.TopicId,
                x.Councils,
            }).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidToAddCouncilAsync(id.TopicId, id.Councils.Select(x => x.CouncilId).ToList());
            });
            RuleFor(x => x.Councils).Must(councils =>
            {
                return councils.Where(x => x.IsChairman).Count() == 1 && councils.Count % 2 != 0;
            });
        }
    }
}
