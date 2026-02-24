using System;

namespace GameBackend.Shared.DTOs.Identity;

public record UserResponseDto(Guid Id, string Username, string Email, DateTime CreatedAt);
