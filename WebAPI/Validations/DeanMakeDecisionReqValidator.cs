using Application.ViewModels.TopicVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Validations
{
    public class DeanMakeDecisionReqValidator : AbstractValidator<DeanMakeDecisionReq>
    {
        public DeanMakeDecisionReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForDean);
            });
            RuleFor(x => x.DiciderId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.User.IsExistedUserAsync(id);
            });
            RuleFor(x => new
            {
                x.DiciderId,
                x.TopicId,
            }).MustAsync(async (id, _) =>
            {
                var departmentId = await unitOfWork.Topic.GetDepartmentIdOfTopicCreatorAsync(id.TopicId);
                return await unitOfWork.User.IsDeanOfDepartmentAsync(id.DiciderId, departmentId);
            });
            RuleFor(x => new
            {
                x.DeanDecision,
                x.ReasonOfDecision,
            }).Must(decision =>
            {
                if (!decision.DeanDecision)
                    return !decision.ReasonOfDecision.IsNullOrEmpty();
                return true;
            });
        }
    }
}
