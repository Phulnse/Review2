using Application.ViewModels.DepartmentVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class CreateDepartmentReqValidator : AbstractValidator<CreateDepartmentReq>
    {
        public CreateDepartmentReqValidator(IUnitOfWork unitOfWork)
        {
           RuleFor(x => x.DepartmentName).Must(unitOfWork.Department.IsvalidDepartmentName); 
        }
    }
}
