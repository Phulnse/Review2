using Application.ViewModels.ContractVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class UploadContractForEndingPhaseReqValidator : AbstractValidator<UploadContractForEndingPhaseReq>
    {
        public UploadContractForEndingPhaseReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForUploadContract);
            });
        }
    }
}
