using Application.ViewModels.MemberReviewVMs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace WebAPI.Validations
{
    public class AddMemberReviewReqValidator : AbstractValidator<AddMemberReviewReq>
    {
        public AddMemberReviewReqValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TopicId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Topic.IsValidTopicAsync(id, TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForCouncilFormation);
            }).WithMessage("Topic id invalid");
            RuleFor(x => new
            {
                x.TopicId,
                x.MemberReviewIds,
            }).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Participant.IsValidToAddMemberReviewAsync(id.TopicId, id.MemberReviewIds);
            }).WithMessage("Member review id invalid");
            RuleFor(x => x.MemberReviewIds).Must(x => x.Count % 2 != 0);
            RuleFor(x => new
            {
                x.StartDate,
                x.EndDate,
            }).Must(x =>
            {
                return DateTime.Compare(x.StartDate, x.EndDate) < 0;
            });
        }
    }
}
