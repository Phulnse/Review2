using Application.ViewModels.DepartmentVMs;
using Domain.Interfaces;
using FluentValidation;
public class UpdateDepartmentReqValidator : AbstractValidator<UpdateDepartmentReq>
{
    public UpdateDepartmentReqValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.DepartmentId).Must(unitOfWork.Department.IsExistDepartmentId);

        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required.")
            .Length(2, 50).WithMessage("Department name must be between 2 and 50 characters.");

    }
}
