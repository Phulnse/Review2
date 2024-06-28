using Application.ViewModels.DocumentVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class ResubmitEarlyDocumentReqValidator : AbstractValidator<ResubmitEarlyDocumentReq>
    {
        public ResubmitEarlyDocumentReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForDocumentEditing);
            });
        }
    }
}
