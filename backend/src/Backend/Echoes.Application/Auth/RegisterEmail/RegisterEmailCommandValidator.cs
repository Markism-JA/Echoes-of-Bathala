using Echoes.Application.Common;
using Echoes.Application.Extensions;
using Echoes.Shared.Network.Constants;
using FluentValidation;

namespace Echoes.Application.Auth.RegisterEmail;

public class RegisterEmailCommandValidator : AbstractValidator<RegisterEmailCommand>
{
    public RegisterEmailCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithCustomError(Errors.Auth.Username.Required)
            .MinimumLength(ValidationConstraint.Username.MinLength)
            .WithCustomError(Errors.Auth.Username.TooShort)
            .MaximumLength(ValidationConstraint.Username.MaxLength)
            .WithCustomError(Errors.Auth.Username.TooLong)
            .Matches(ValidationConstraint.Username.AllowedCharactersRegex)
            .WithCustomError(Errors.Auth.Username.InvalidCharacters);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithCustomError(Errors.Auth.Email.Required)
            .EmailAddress()
            .WithCustomError(Errors.Auth.Email.Invalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithCustomError(Errors.Auth.Password.Required)
            .MinimumLength(ValidationConstraint.Password.MinLength)
            .WithCustomError(Errors.Auth.Password.TooShort);
    }
}
