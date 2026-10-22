namespace FourDigitCore.Application.Common.Interfaces;

public interface IEmailService
{
    Task<bool> SendAdminInvitationAsync(string recipientEmail, string recipientName, string role, string inviteUrl, CancellationToken cancellationToken = default);
    Task<bool> SendContactNotificationAsync(string senderName, string senderEmail, string intent, string scale, string message, CancellationToken cancellationToken = default);
    Task<bool> SendJobApplicationAlertAsync(string candidateName, string candidateEmail, string jobTitle, string resumeUrl, CancellationToken cancellationToken = default);
    Task<bool> SendCohortApplicationAlertAsync(string applicantName, string applicantEmail, string trackName, CancellationToken cancellationToken = default);
}
