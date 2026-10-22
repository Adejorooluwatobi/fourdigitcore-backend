using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.DTOs.Auth;

namespace FourDigitCore.Application.Common.Interfaces;

public interface IAdminUserManagementService
{
    // SuperAdmin User Governance
    Task<List<SystemAdminDetailDto>> GetAllAdminsAsync(CancellationToken cancellationToken = default);
    Task<AdminInvitationDto> InviteAdminAsync(InviteAdminDto dto, Guid currentAdminId, string currentAdminName, CancellationToken cancellationToken = default);
    Task<List<AdminInvitationDto>> GetInvitationsAsync(CancellationToken cancellationToken = default);
    Task<bool> ResendInvitationAsync(Guid invitationId, CancellationToken cancellationToken = default);
    Task<bool> RevokeInvitationAsync(Guid invitationId, CancellationToken cancellationToken = default);
    Task<bool> SuspendAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default);
    Task<bool> ReactivateAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default);

    // Public Onboarding Flow
    Task<InvitationInfoResponseDto> GetInvitationInfoAsync(string token, CancellationToken cancellationToken = default);
    Task<LoginResponseDto> AcceptInvitationAsync(AcceptInvitationRequestDto dto, string? clientIp = null, CancellationToken cancellationToken = default);
}
