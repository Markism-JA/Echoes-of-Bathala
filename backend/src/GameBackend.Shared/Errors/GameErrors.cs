using ErrorOr;

namespace GameBackend.Shared.Errors;

/// <summary>
/// Contains standardized error definitions for the entire GameBackend ecosystem.
/// </summary>
public static class GameErrors
{
    /// <summary>
    /// Errors related to authentication, registration, and identity management.
    /// </summary>
    public static class Auth
    {
        #region Username Errors

        /// <summary>Returned when a required username field is null or empty.</summary>
        public static readonly Error UsernameRequired = Error.Validation(
            code: "Auth.Username.Required",
            description: "Username is required."
        );

        /// <summary>Returned when the username contains words blocklisted by the profanity filter.</summary>
        public static readonly Error UsernameProfane = Error.Validation(
            code: "Auth.Username.Profane",
            description: "Username contains forbidden words."
        );

        /// <summary>Returned when a player tries to use a protected system name (e.g., 'Admin', 'GameMaster').</summary>
        public static readonly Error UsernameReserved = Error.Validation(
            code: "Auth.Username.Reserved",
            description: "This username is reserved for system use."
        );

        /// <summary>Returned when the username format is invalid (e.g., illegal symbols or too long).</summary>
        public static readonly Error UsernameInvalid = Error.Validation(
            code: "Auth.Username.Invalid",
            description: "The username provided is invalid."
        );

        /// <summary>Returned when a user attempts to register with a username that is already in use.</summary>
        public static readonly Error UsernameTaken = Error.Conflict(
            code: "Auth.Username.Taken",
            description: "Username is already taken."
        );

        #endregion

        #region Email Errors

        /// <summary>Returned when a required email field is missing.</summary>
        public static readonly Error EmailRequired = Error.Validation(
            code: "Auth.Email.Required",
            description: "Email is required."
        );

        /// <summary>Returned when the email format does not pass basic regex validation.</summary>
        public static readonly Error EmailInvalid = Error.Validation(
            code: "Auth.Email.Invalid",
            description: "Invalid email format."
        );

        /// <summary>Returned when the email domain belongs to a known temporary/disposable provider.</summary>
        public static readonly Error EmailDisposable = Error.Validation(
            code: "Auth.Email.Disposable",
            description: "Disposable email addresses are not allowed."
        );

        /// <summary>Returned when an account with the canonical version of this email already exists.</summary>
        public static readonly Error EmailTaken = Error.Conflict(
            code: "Auth.Email.Taken",
            description: "Email is already taken."
        );

        #endregion

        #region Password Errors

        /// <summary>Returned when the password field is missing.</summary>
        public static readonly Error PasswordRequired = Error.Validation(
            code: "Auth.Password.Required",
            description: "Password is required."
        );

        /// <summary>Returned when the password does not meet the minimum length requirements.</summary>
        public static readonly Error PasswordTooShort = Error.Validation(
            code: "Auth.Password.TooShort",
            description: "Password must be at least 8 characters long."
        );

        /// <summary>Returned when the password is found in a list of common passwords or is too predictable.</summary>
        public static readonly Error PasswordTooWeak = Error.Validation(
            code: "Auth.Password.TooWeak",
            description: "Password is too common or easy to guess."
        );

        #endregion

        #region Login & Session Errors

        /// <summary>
        /// Returned when the email/password combination is incorrect.
        /// Maps to 401 Unauthorized in the API layer.
        /// </summary>
        /// <remarks>
        /// This is a generic error used to prevent account enumeration.
        /// </remarks>
        public static Error InvalidCredentials =>
            Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "The email or password provided is incorrect."
            );

        #endregion
    }
}
