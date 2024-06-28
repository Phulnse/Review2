using Application.ViewModels.TopicVMs;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class CreateTopicReqValidator : AbstractValidator<CreateTopicReq>
    {
        public CreateTopicReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicName).Length(10, 100).WithMessage("Topic length from 10 to 100");
            RuleFor(x => x.Description).Length(100, 1000).WithMessage("Description length from 100 to 1000");
            RuleFor(x => x.CategoryId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Category.IsExistedCategoryAsync(id);
            }).WithMessage("Category id is not exist");
            RuleForEach(x => x.MemberList).ChildRules(member =>
            {
                member.RuleFor(x => x.UserId).MustAsync(async (id, _) =>
                {
                    return await unitOfWork.User.IsExistedUserAsync(id);
                }).WithMessage("User id is not exist");
            });
            RuleFor(x => x.MemberList).Must(x => x.Count() <= 10).WithMessage("Maximum number of member is 10");
            RuleFor(x => x.StartTime).Must(x => DateTime.Compare(x, DateTime.Now) > 0).WithMessage("Invalid start time");
        }
    }
}
