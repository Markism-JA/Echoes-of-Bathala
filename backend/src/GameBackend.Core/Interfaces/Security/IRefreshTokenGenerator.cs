namespace GameBackend.Core.Interfaces.Security
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateToken();
    }
}
