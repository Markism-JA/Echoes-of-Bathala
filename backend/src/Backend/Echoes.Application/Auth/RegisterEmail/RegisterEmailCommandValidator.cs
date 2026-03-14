using Echoes.Application.Extensions;
using Echoes.Shared.Network.Validation;
using FluentValidation;

namespace Echoes.Application.Auth.RegisterEmail;

public class RegisterEmailCommandValidator : AbstractValidator<RegisterEmailCommand>
{
    public RegisterEmailCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithCustomError(Errors.Username.Required)
            .MinimumLength(ValidationConstraint.Username.MinLength)
            .WithCustomError(Errors.Username.TooShort)
            .MaximumLength(ValidationConstraint.Username.MaxLength)
            .WithCustomError(Errors.Username.TooLong)
            .Matches(ValidationConstraint.Username.AllowedCharactersRegex)
            .WithCustomError(Errors.Username.InvalidCharacters);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithCustomError(Errors.Email.Required)
            .EmailAddress()
            .WithCustomError(Errors.Email.Invalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithCustomError(Errors.Password.Required)
            .MinimumLength(ValidationConstraint.Password.MinLength)
            .WithCustomError(Errors.Password.TooShort);
    }
}
