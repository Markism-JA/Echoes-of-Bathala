using Echoes.Application.Extensions;
using Echoes.Shared.Network.Validation;
using FluentValidation;

namespace Echoes.Application.Auth.RegisterEmail;

/// <summary>
/// Defines the structural validation rules for the <see cref="RegisterEmailCommand"/>.
/// </summary>
/// <remarks>
/// This validator is automatically picked up by the <see cref="ValidationBehavior{TRequest, TResponse}"/>
/// to enforce constraints before the command reaches the handler. It uses centralized constants
/// for length and format to ensure consistency across the ecosystem.
/// </remarks>
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
