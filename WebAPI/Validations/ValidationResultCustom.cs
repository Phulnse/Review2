using FluentValidation.Results;

namespace WebAPI.Validations
{
    public static class ValidationResultCustom
    {
        public static object SelectNecessaryAttributes(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(x => new
            {
                x.PropertyName,
                x.AttemptedValue,
                x.ErrorMessage,
            });
        }
    }
}
