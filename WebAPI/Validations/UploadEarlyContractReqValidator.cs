using Application.ViewModels.ContractVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class UploadEarlyContractReqValidator : AbstractValidator<UploadEarlyContractReq>
    {
        public UploadEarlyContractReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadContract);
            });
        }
    }
}
