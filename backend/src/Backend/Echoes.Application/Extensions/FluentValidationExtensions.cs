using ErrorOr;
using FluentValidation;

namespace Echoes.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithCustomError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule,
            Error error
        )
        {
            return rule.WithErrorCode(error.Code).WithMessage(error.Description);
        }
    }
}
