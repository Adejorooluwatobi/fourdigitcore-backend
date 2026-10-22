using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FourDigitCore.Test.Security;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _tokenService;
    private readonly IConfiguration _configuration;

    public JwtTokenServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Secret", "UnitTestingSecretKeyForFourDigitCore6174PlatformSecurity2026!" },
            { "Jwt:Issuer", "FourDigitCoreTestIssuer" },
            { "Jwt:Audience", "FourDigitCoreTestAudience" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _tokenService = new JwtTokenService(_configuration);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt_WithExpectedClaims()
    {
        // Arrange
        var user = new AdminUser
        {
            Id = Guid.NewGuid(),
            Email = "admin@6174.io",
            FullName = "Lead Systems Architect"
        };
        var roles = new List<string> { "SuperAdmin", "SecurityAuditor" };

        // Act
        var tokenString = _tokenService.GenerateToken(user, roles);

        // Assert
        tokenString.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        handler.CanReadToken(tokenString).Should().BeTrue();

        var token = handler.ReadJwtToken(tokenString);
        token.Issuer.Should().Be("FourDigitCoreTestIssuer");
        token.Audiences.Should().Contain("FourDigitCoreTestAudience");

        var subClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        subClaim.Should().Be(user.Id.ToString());

        var emailClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        emailClaim.Should().Be(user.Email);

        var nameClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        nameClaim.Should().Be(user.FullName);

        var roleClaims = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        roleClaims.Should().Contain("SuperAdmin");
        roleClaims.Should().Contain("SecurityAuditor");

        token.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
