using Echoes.Application.Auth.Abstractions;
using Echoes.Application.Auth.Models;
using Echoes.Application.Auth.Policies;
using Echoes.Application.Core.Services;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Domain;
using Echoes.Domain.Repository;
using Echoes.Domain.Users;
using Echoes.Shared.Network.Auth;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;

namespace Echoes.Application.Auth.RegisterEmail;

/// <summary>
/// Orchestrates the end-to-end user registration process via email and password.
/// </summary>
/// <remarks>
/// This handler coordinates multiple concerns including:
/// <list type="bullet">
/// <item><description>Registration policy enforcement (uniqueness and formatting).</description></item>
/// <item><description>Identity provider registration (credential security).</description></item>
/// <item><description>Domain user entity creation.</description></item>
/// <item><description>Initial session issuance (JWT and Refresh Tokens).</description></item>
/// </list>
/// </remarks>
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

    /// <summary>
    /// Processes the registration request and returns authentication tokens upon success.
    /// </summary>
    /// <param name="request">The <see cref="RegisterEmailCommand"/> containing user credentials.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>
    /// An <see cref="ErrorOr{T}"/> containing the <see cref="AuthResponseDto"/> on success,
    /// or a collection of errors if policy or identity registration fails.
    /// </returns>
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

        var (accessToken, accessExpiry) = jwtTokenGenerator.GenerateToken(domainUser, now);

        var refreshToken = RefreshToken.Create(
            refreshTokenGenerator.GenerateToken(),
            domainUser.Id,
            now.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            now
        );

        await sessionService.CreateSessionAsync(refreshToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CreateResponse(accessToken, refreshToken, domainUser, accessExpiry);
    }

    /// <summary>
    /// Maps the internal entities and tokens to a unified authentication response DTO.
    /// </summary>
    private static AuthResponseDto CreateResponse(
        string accessToken,
        RefreshToken refreshToken,
        User user,
        DateTime accessTokenExpiration
    )
    {
        return new AuthResponseDto(
            accessToken,
            refreshToken.Token,
            accessTokenExpiration,
            refreshToken.ExpiryDate,
            new UserResponseDto(user.Id, user.UserName, user.Email, user.CreatedAt)
        );
    }
}
