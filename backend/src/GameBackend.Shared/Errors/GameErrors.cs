using ErrorOr;

namespace GameBackend.Shared.Errors;

public static class GameErrors
{
    public static class Auth
    {
        // Username

        public static readonly Error UsernameRequired = Error.Validation(
            code: "Auth.Username.Required",
            description: "Username is required."
        );

        public static readonly Error UsernameProfane = Error.Validation(
            code: "Auth.Username.Profane",
            description: "Username contains forbidden words."
        );

        public static readonly Error UsernameReserved = Error.Validation(
            code: "Auth.Username.Reserved",
            description: "This username is reserved for system use."
        );

        public static readonly Error UsernameInvalid = Error.Validation(
            code: "Auth.Username.Invalid",
            description: "The username provided is invalid."
        );

        public static readonly Error UsernameTaken = Error.Conflict(
            code: "Auth.Username.Taken",
            description: "Username is already taken."
        );

        // Email

        public static readonly Error EmailRequired = Error.Validation(
            code: "Auth.Email.Required",
            description: "Email is required."
        );

        public static readonly Error EmailInvalid = Error.Validation(
            code: "Auth.Email.Invalid",
            description: "Invalid email format."
        );

        public static readonly Error EmailDisposable = Error.Validation(
            code: "Auth.Email.Disposable",
            description: "Disposable email addresses are not allowed."
        );

        public static readonly Error EmailTaken = Error.Conflict(
            code: "Auth.Email.Taken",
            description: "Email is already taken."
        );

        // Password

        public static readonly Error PasswordRequired = Error.Validation(
            code: "Auth.Password.Required",
            description: "Password is required."
        );

        public static readonly Error PasswordTooShort = Error.Validation(
            code: "Auth.Password.TooShort",
            description: "Password must be at least 8 characters long."
        );

        public static readonly Error PasswordTooWeak = Error.Validation(
            code: "Auth.Password.TooWeak",
            description: "Password is too common or easy to guess."
        );

        /// <summary>
        /// Returned when the email/password combination is incorrect.
        /// Maps to 401 Unauthorized.
        /// </summary>
        public static Error InvalidCredentials =>
            Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "The email or password provided is incorrect."
            );
    }
}
