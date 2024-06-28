using Application.ViewModels.HolidayVMs;
using FluentValidation;

namespace WebAPI.Validations
{
    public class AddHolidaysReqValidator : AbstractValidator<AddHolidaysReq>
    {
        public AddHolidaysReqValidator()
        {
            var dateTimeNow = DateTime.Now;
            RuleForEach(x => x.Holidays).Must(x =>
            {
                return DateTime.Compare(x, dateTimeNow) > 0;
            });
        }
    }
}
