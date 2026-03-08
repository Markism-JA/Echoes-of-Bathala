using Echoes.Application.Auth.Abstractions;
using Echoes.Application.Auth.Models;
using Echoes.Application.Auth.Policies;
using Echoes.Application.Core.Services;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Domain;
using Echoes.Domain.Repository;
using Echoes.Domain.Users;
using Echoes.Shared.Network.DTOs.Auth;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;

namespace Echoes.Application.Auth.RegisterEmail;

public class RegisterEmailCommandHandler(
    IRegistrationPolicy registrationPolicy,
    IIdentityService identityService,
    IUserRepository userRepository,
    ISessionService sessionService,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IOptions<JwtSettings> jwtOptions
) : IRequestHandler<RegisterEmailCommand, ErrorOr<AuthResponseDto>>
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<ErrorOr<AuthResponseDto>> Handle(
        RegisterEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var policyResult = await registrationPolicy.IsAllowedAsync(request, cancellationToken);
        if (policyResult.IsError)
            return policyResult.Errors;

        var userId = Guid.NewGuid();

        var identityResult = await identityService.RegisterUserAsync(
            policyResult.Value.NormalizedUsername,
            policyResult.Value.NormalizedEmail,
            request.Password,
            userId,
            cancellationToken
        );

        if (identityResult.IsError)
            return identityResult.Errors;

        var now = dateTimeProvider.UtcNow;
        var domainUser = User.Create(
            id: userId,
            username: request.Username,
            email: request.Email,
            normalizedUserName: policyResult.Value.NormalizedUsername,
            normalizedEmail: policyResult.Value.NormalizedEmail,
            createdAt: now
        );

        await userRepository.AddAsync(domainUser, cancellationToken);

        var (accessToken, _) = jwtTokenGenerator.GenerateToken(domainUser, now);

        var refreshToken = RefreshToken.Create(
            refreshTokenGenerator.GenerateToken(),
            domainUser.Id,
            now.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            now
        );

        await sessionService.CreateSessionAsync(refreshToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CreateResponse(accessToken, refreshToken, domainUser);
    }

    private static AuthResponseDto CreateResponse(
        string accessToken,
        RefreshToken refreshToken,
        User user
    )
    {
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            Expiration = refreshToken.ExpiryDate,
            User = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            },
        };
    }
}
