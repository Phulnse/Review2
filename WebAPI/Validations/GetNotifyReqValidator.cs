using Application.ViewModels.NotifyVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class GetNotifyReqValidator : AbstractValidator<GetNotifyReq>
    {
        public GetNotifyReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.UserId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.User.IsExistedUserAsync(id);
            });
    }
}
}
