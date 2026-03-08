namespace Echoes.Application.Auth.Abstractions
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateToken();
    }
}
