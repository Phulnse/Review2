using Application.ViewModels.DocumentVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class SupplementationMiddleDocumentReqValidator : AbstractValidator<SupplementationMiddleDocumentReq>
    {
        public SupplementationMiddleDocumentReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForDocumentSupplementation);
            });
        }
    }
}
