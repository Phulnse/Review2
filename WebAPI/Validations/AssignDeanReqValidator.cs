using Application.ViewModels.UserVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class AssignDeanReqValidator : AbstractValidator<AssignDeanReq>
    {
        public AssignDeanReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Email).Must(unitOfWork.User.IsValidToAssignDean);
        }
    }
}
