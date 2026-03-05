namespace Echoes.Application.Common.Identity
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateToken();
    }
}
