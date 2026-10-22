using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class ArchitectProfile : BaseEntity
{
    public string Initials { get; set; } = "VT";
    public string FullName { get; set; } = "Dr. Vance K. Thorne";
    public string RoleTitle { get; set; } = "Founder & Principal Systems Fellow";
    public string PgpKey { get; set; } = "PGP: 6174:BF90";
    public string Quote { get; set; } = "We reject the idea that distributed systems must be fragile. By building on formal mathematical invariants, we deliver infrastructure that does not fail when load surges.";
    public string CommitsCountText { get; set; } = "14,892 Commits";
    public string PatentsCountText { get; set; } = "12 Patents";
    public string ReleasesCountText { get; set; } = "4 OS Releases";
    public string? AvatarUrl { get; set; }
}
