using Application.ViewModels.RemunerationVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class GetRemunerationReqValidator : AbstractValidator<GetRemunerationReq>
    {
        public GetRemunerationReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForCensorshipRemuneration);
            });
        }
    }
}
