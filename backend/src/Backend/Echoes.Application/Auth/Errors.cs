using Echoes.Shared.Network.Common;
using ErrorOr;

namespace Echoes.Application.Auth;

/// <summary>
/// A centralized repository of standardized error objects for the authentication domain.
/// </summary>
/// <remarks>
/// This class utilizes the Result Pattern via <see cref="ErrorOr"/> to provide
/// strongly-typed, descriptive errors that can be easily mapped to HTTP status codes
/// at the API layer.
/// </remarks>
public static class Errors
{
    /// <summary>
    /// Errors related to user identity handles and nicknames.
    /// </summary>
    public static class Username
    {
        /// <summary>Returned when a required username field is null or empty.</summary>
        public static readonly Error Required = Error.Validation(
            code: AuthErrorCodes.Username.Required,
            description: "Username is required."
        );

        /// <summary>Returned when the username contains words blocklisted by the profanity filter.</summary>
        public static readonly Error Profane = Error.Validation(
            code: AuthErrorCodes.Username.Profane,
            description: "Username contains forbidden words."
        );

        /// <summary>Returned when a player tries to use a protected system name.</summary>
        public static readonly Error Reserved = Error.Validation(
            code: AuthErrorCodes.Username.Reserved,
            description: "This username is reserved for system use."
        );

        /// <summary>Returned when the username length is below the system threshold.</summary>
        public static readonly Error TooShort = Error.Validation(
            code: AuthErrorCodes.Username.TooShort,
            description: "Username is too short."
        );

        /// <summary>Returned when the username length exceeds the system threshold.</summary>
        public static readonly Error TooLong = Error.Validation(
            code: AuthErrorCodes.Username.TooLong,
            description: "Username is too long."
        );

        /// <summary>Returned when the username contains non-alphanumeric or disallowed characters.</summary>
        public static readonly Error InvalidCharacters = Error.Validation(
            code: AuthErrorCodes.Username.InvalidCharacters,
            description: "Username contains illegal characters."
        );

        /// <summary>Returned when a user attempts to register with a username that is already in use.</summary>
        public static readonly Error Taken = Error.Conflict(
            code: AuthErrorCodes.Username.Taken,
            description: "Username is already taken."
        );
    }

    /// <summary>
    /// Errors related to email address validation and availability.
    /// </summary>
    public static class Email
    {
        /// <summary>Returned when a required email field is missing.</summary>
        public static readonly Error Required = Error.Validation(
            code: AuthErrorCodes.Email.Required,
            description: "Email is required."
        );

        /// <summary>Returned when the email format does not pass validation rules.</summary>
        public static readonly Error Invalid = Error.Validation(
            code: AuthErrorCodes.Email.Invalid,
            description: "Invalid email format."
        );

        /// <summary>Returned when the email domain belongs to a temporary provider.</summary>
        public static readonly Error Disposable = Error.Validation(
            code: AuthErrorCodes.Email.Disposable,
            description: "Disposable email addresses are not allowed."
        );

        /// <summary>Returned when an account with this email already exists.</summary>
        public static readonly Error Taken = Error.Conflict(
            code: AuthErrorCodes.Email.Taken,
            description: "Email is already taken."
        );
    }

    /// <summary>
    /// Errors related to credential security and complexity.
    /// </summary>
    public static class Password
    {
        /// <summary>Returned when the password field is missing.</summary>
        public static readonly Error Required = Error.Validation(
            code: AuthErrorCodes.Password.Required,
            description: "Password is required."
        );

        /// <summary>Returned when the password length is insufficient.</summary>
        public static readonly Error TooShort = Error.Validation(
            code: AuthErrorCodes.Password.TooShort,
            description: "Password must be at least 8 characters long."
        );

        /// <summary>Returned when the password is insecure or predictable.</summary>
        public static readonly Error TooWeak = Error.Validation(
            code: AuthErrorCodes.Password.TooWeak,
            description: "Password is too common or easy to guess."
        );
    }
}
