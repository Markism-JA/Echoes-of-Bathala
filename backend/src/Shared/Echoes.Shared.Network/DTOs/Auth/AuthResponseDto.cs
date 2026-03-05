using System;

namespace Echoes.Shared.Network.DTOs.Auth
{
    [Serializable]
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }

        public UserResponseDto User { get; set; } = new UserResponseDto();
    }
}
