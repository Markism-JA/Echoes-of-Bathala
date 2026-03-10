using Echoes.Shared.Network.Common;
using ErrorOr;

namespace Echoes.Application.Auth;

/// <summary>
/// Contains standardized error definitions for the entire GameBackend ecosystem.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Errors related to authentication, registration, and identity management.
    /// </summary>
    #region Username Errors

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

        /// <summary>Returned when a player tries to use a protected system name (e.g., 'Admin', 'GameMaster').</summary>
        public static readonly Error Reserved = Error.Validation(
            code: AuthErrorCodes.Username.Reserved,
            description: "This username is reserved for system use."
        );

        public static readonly Error TooShort = Error.Validation(
            code: AuthErrorCodes.Username.TooShort,
            description: "Username is too short."
        );
        public static readonly Error TooLong = Error.Validation(
            code: AuthErrorCodes.Username.TooLong,
            description: "Username is too long."
        );
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

    #endregion

    #region Email Errors

    public static class Email
    {
        /// <summary>Returned when a required email field is missing.</summary>
        public static readonly Error Required = Error.Validation(
            code: AuthErrorCodes.Email.Required,
            description: "Email is required."
        );

        /// <summary>Returned when the email format does not pass basic regex validation.</summary>
        public static readonly Error Invalid = Error.Validation(
            code: AuthErrorCodes.Email.Invalid,
            description: "Invalid email format."
        );

        /// <summary>Returned when the email domain belongs to a known temporary/disposable provider.</summary>
        public static readonly Error Disposable = Error.Validation(
            code: AuthErrorCodes.Email.Disposable,
            description: "Disposable email addresses are not allowed."
        );

        /// <summary>Returned when an account with the canonical version of this email already exists.</summary>
        public static readonly Error Taken = Error.Conflict(
            code: AuthErrorCodes.Email.Taken,
            description: "Email is already taken."
        );
    }

    #endregion

    #region Password Errors

    public static class Password
    {
        public static readonly Error Required = Error.Validation(
            code: AuthErrorCodes.Password.Required,
            description: "Password is required."
        );

        /// <summary>Returned when the password does not meet the minimum length requirements.</summary>
        public static readonly Error TooShort = Error.Validation(
            code: AuthErrorCodes.Password.TooShort,
            description: "Password must be at least 8 characters long."
        );

        /// <summary>Returned when the password is found in a list of common passwords or is too predictable.</summary>
        public static readonly Error TooWeak = Error.Validation(
            code: AuthErrorCodes.Password.TooWeak,
            description: "Password is too common or easy to guess."
        );
    }

    #endregion
}
