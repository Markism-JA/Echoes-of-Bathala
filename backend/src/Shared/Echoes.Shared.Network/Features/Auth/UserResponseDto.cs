using System;

namespace Echoes.Shared.Network.Features.Auth;

/// <summary>
/// An immutable DTO representing a user account profile returned by the server.
/// </summary>
public record UserResponseDto(
    Guid Id,
    string UserName,
    string Email,
    DateTime CreatedAt
);
