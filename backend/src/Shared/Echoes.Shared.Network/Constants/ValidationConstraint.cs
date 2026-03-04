namespace Echoes.Shared.Network.Constants
{
    public static class ValidationConstraint
    {
        public static class Username
        {
            public const string AllowedCharactersRegex = "^[a-zA-Z0-9_]+$";
            public const int MinLength = 3;
            public const int MaxLength = 20;
        }

        public static class Password
        {
            public const int MinLength = 8;
        }
    }
}
