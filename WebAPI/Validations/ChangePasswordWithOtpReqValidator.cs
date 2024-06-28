using Application.ViewModels.AccountVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class ChangePasswordWithOtpReqValidator : AbstractValidator<ChangePasswordWithOtpReq>
    {
        public ChangePasswordWithOtpReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Email).Must(unitOfWork.Account.IsExistedEmail);
        }
    }
}
