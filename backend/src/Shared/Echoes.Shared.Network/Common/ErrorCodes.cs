namespace Echoes.Shared.Network.Common
{
    public static class AuthErrorCodes
    {
        public static class Username
        {
            public const string Required = "Auth.Username.Required";
            public const string Profane = "Auth.Username.Profane";
            public const string Reserved = "Auth.Username.Reserved";
            public const string TooShort = "Auth.Username.TooShort";
            public const string TooLong = "Auth.Username.TooLong";
            public const string InvalidCharacters = "Auth.Username.InvalidCharacters";
            public const string Taken = "Auth.Username.Taken";
        }

        public static class Email
        {
            public const string Required = "Auth.Email.Required";
            public const string Invalid = "Auth.Email.Invalid";
            public const string Disposable = "Auth.Email.Disposable";
            public const string Taken = "Auth.Email.Taken";
        }

        public static class Password
        {
            public const string Required = "Auth.Password.Required";
            public const string TooShort = "Auth.Password.TooShort";
            public const string TooWeak = "Auth.Password.TooWeak";
        }

        public static class Session
        {
            public const string InvalidCredentials = "Auth.InvalidCredentials";
        }
    }
}
