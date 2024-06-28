using Application.ViewModels.AccountVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class ChangePasswordReqValidator : AbstractValidator<ChangePasswordReq>
    {
        public ChangePasswordReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Email).Must(unitOfWork.Account.IsExistedEmail);
        }
    }
}
