using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(AdminUser user, IEnumerable<string> roles);
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
