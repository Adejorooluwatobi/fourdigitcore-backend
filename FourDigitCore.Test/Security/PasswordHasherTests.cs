using FluentAssertions;
using FourDigitCore.Infrastructure.Services;
using Xunit;

namespace FourDigitCore.Test.Security;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher;

    public PasswordHasherTests()
    {
        _hasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedString_NotMatchingPlaintext()
    {
        // Arrange
        const string plainPassword = "SuperAdminPassword2026!";

        // Act
        var hash = _hasher.HashPassword(plainPassword);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(plainPassword);
        hash.Should().StartWith("$2"); // BCrypt prefix
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        const string plainPassword = "SuperAdminPassword2026!";
        var hash = _hasher.HashPassword(plainPassword);

        // Act
        var isValid = _hasher.VerifyPassword(plainPassword, hash);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        const string plainPassword = "SuperAdminPassword2026!";
        const string wrongPassword = "WrongPassword123!";
        var hash = _hasher.HashPassword(plainPassword);

        // Act
        var isValid = _hasher.VerifyPassword(wrongPassword, hash);

        // Assert
        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-valid-bcrypt-hash")]
    [InlineData(null)]
    public void VerifyPassword_WithInvalidHash_ShouldReturnFalse_WithoutThrowing(string? invalidHash)
    {
        // Arrange & Act
        var isValid = _hasher.VerifyPassword("AnyPassword", invalidHash!);

        // Assert
        isValid.Should().BeFalse();
    }
}
