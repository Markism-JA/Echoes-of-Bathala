using ErrorOr;

namespace GameBackend.Shared.Errors;

public static class GameErrors
{
    public static class Auth
    {
        public const string ProfaneUsernameCode = "Auth.ProfaneReservedUsername";
        public static Error ProfaneUsername =>
            Error.Conflict(
                code: ProfaneUsernameCode,
                description: "Username contains forbidden words or is reserved."
            );
        public const string EmailTakenCode = "Auth.EmailTaken";
        public static Error EmailTaken =>
            Error.Conflict(code: EmailTakenCode, description: "Email is already taken.");
        public const string UsernameTakenCode = "Auth.UsernameTaken";
        public static Error UsernameTaken =>
            Error.Conflict(code: UsernameTakenCode, description: "Username is already taken.");
    }
}
