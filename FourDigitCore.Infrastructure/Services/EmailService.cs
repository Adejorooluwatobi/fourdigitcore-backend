using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using FourDigitCore.Application.Common.Interfaces;

namespace FourDigitCore.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendAdminInvitationAsync(string recipientEmail, string recipientName, string role, string inviteUrl, CancellationToken cancellationToken = default)
    {
        var subject = "[FourDigitCore 6174] Administrator Portal Invitation";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{ font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; background-color: #030712; color: #f8fafc; margin: 0; padding: 24px; }}
    .container {{ max-width: 620px; margin: 0 auto; background-color: #0b1120; border: 1px solid #1e293b; border-radius: 12px; padding: 32px; box-shadow: 0 10px 25px rgba(0,0,0,0.5); }}
    .header {{ border-bottom: 1px solid #1e293b; padding-bottom: 20px; margin-bottom: 24px; }}
    .monogram {{ font-family: 'Courier New', monospace; font-size: 24px; font-weight: 800; color: #38bdf8; letter-spacing: 0.1em; }}
    .badge {{ display: inline-block; background-color: #0369a1; color: #e0f2fe; font-size: 11px; font-weight: 700; padding: 3px 8px; border-radius: 4px; text-transform: uppercase; margin-left: 8px; }}
    h1 {{ color: #f8fafc; font-size: 20px; margin: 0 0 12px 0; }}
    p {{ color: #94a3b8; font-size: 15px; line-height: 1.6; margin: 0 0 18px 0; }}
    .role-box {{ background-color: #020617; border: 1px solid #1e293b; border-left: 4px solid #38bdf8; border-radius: 6px; padding: 16px; margin-bottom: 24px; }}
    .role-label {{ font-size: 11px; text-transform: uppercase; color: #64748b; font-weight: 700; letter-spacing: 0.05em; }}
    .role-value {{ font-size: 16px; font-weight: 600; color: #38bdf8; margin-top: 4px; }}
    .btn {{ display: inline-block; background-color: #38bdf8; color: #020617 !important; font-weight: 700; font-size: 14px; text-decoration: none; padding: 12px 28px; border-radius: 6px; text-transform: uppercase; letter-spacing: 0.05em; margin: 12px 0 24px 0; }}
    .btn:hover {{ background-color: #7dd3fc; }}
    .footer {{ border-top: 1px solid #1e293b; padding-top: 18px; margin-top: 24px; font-size: 12px; color: #475569; }}
    .code-box {{ background-color: #020617; padding: 10px; border-radius: 4px; font-family: monospace; font-size: 12px; color: #94a3b8; word-break: break-all; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <span class='monogram'>6174</span>
      <span class='badge'>Control Plane</span>
    </div>
    <h1>Administrator Access Invitation</h1>
    <p>Greetings {recipientName},</p>
    <p>You have been invited by the System SuperAdmin to join the <strong>FourDigitCore (6174)</strong> Administrative Control Plane.</p>
    
    <div class='role-box'>
      <div class='role-label'>Assigned Governance Role</div>
      <div class='role-value'>{role.ToUpperInvariant()}</div>
    </div>

    <p>To accept this invitation and establish your secure account credentials, click the button below to set your password:</p>

    <a href='{inviteUrl}' class='btn'>Accept Invitation & Set Password</a>

    <p style='font-size: 13px; color: #64748b;'>This single-use cryptographic invitation link will expire in 7 days. If the button does not open, copy and paste this address into your browser:</p>
    <div class='code-box'>{inviteUrl}</div>

    <div class='footer'>
      FourDigitCore 6174 Attractor Architecture • Confidential Administrative Transmission • Automated System
    </div>
  </div>
</body>
</html>";

        return await SendEmailAsync(recipientEmail, recipientName, subject, htmlBody, cancellationToken);
    }

    public async Task<bool> SendContactNotificationAsync(string senderName, string senderEmail, string intent, string scale, string message, CancellationToken cancellationToken = default)
    {
        var subject = $"[6174 Alert] New Enterprise Inquiry from {senderName}";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{ font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background-color: #030712; color: #f8fafc; margin: 0; padding: 24px; }}
    .container {{ max-width: 600px; margin: 0 auto; background-color: #0b1120; border: 1px solid #1e293b; border-radius: 12px; padding: 28px; }}
    .badge {{ display: inline-block; background-color: #0284c7; color: #f0f9ff; font-weight: 700; font-size: 11px; padding: 4px 8px; border-radius: 4px; text-transform: uppercase; margin-bottom: 12px; }}
    h2 {{ margin-top: 0; color: #f8fafc; font-size: 18px; }}
    .field {{ margin-bottom: 14px; }}
    .label {{ font-size: 11px; text-transform: uppercase; color: #64748b; font-weight: 700; margin-bottom: 4px; }}
    .val {{ font-size: 14px; color: #e2e8f0; }}
    .msg {{ background-color: #020617; border-left: 3px solid #38bdf8; padding: 14px; border-radius: 4px; font-size: 14px; line-height: 1.6; white-space: pre-wrap; }}
    .footer {{ margin-top: 24px; padding-top: 14px; border-top: 1px solid #1e293b; font-size: 12px; color: #475569; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='badge'>Inbound Contact Ingestion</div>
    <h2>New Inbound Inquiry</h2>
    <div class='field'><div class='label'>Sender</div><div class='val'>{senderName} &lt;<a href='mailto:{senderEmail}' style='color:#38bdf8;'>{senderEmail}</a>&gt;</div></div>
    <div class='field'><div class='label'>Intent</div><div class='val'>{intent}</div></div>
    <div class='field'><div class='label'>Scale Category</div><div class='val'>{scale}</div></div>
    <div class='field'><div class='label'>Message / Scope Constraints</div><div class='msg'>{message}</div></div>
    <div class='footer'>Received at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC • Reply directly to this email to respond.</div>
  </div>
</body>
</html>";

        var adminRecipient = Environment.GetEnvironmentVariable("EMAIL_RECIPIENT") ?? _configuration["Email:RecipientEmail"] ?? "Adejorotgold1@yahoo.com";
        return await SendEmailAsync(adminRecipient, "6174 Operations", subject, htmlBody, cancellationToken, replyToEmail: senderEmail, replyToName: senderName);
    }

    public async Task<bool> SendJobApplicationAlertAsync(string candidateName, string candidateEmail, string jobTitle, string resumeUrl, CancellationToken cancellationToken = default)
    {
        var subject = $"[6174 ATS] New Candidate Application: {candidateName} for {jobTitle}";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{ font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background-color: #030712; color: #f8fafc; margin: 0; padding: 24px; }}
    .container {{ max-width: 600px; margin: 0 auto; background-color: #0b1120; border: 1px solid #1e293b; border-radius: 12px; padding: 28px; }}
    .badge {{ display: inline-block; background-color: #10b981; color: #022c22; font-weight: 700; font-size: 11px; padding: 4px 8px; border-radius: 4px; text-transform: uppercase; margin-bottom: 12px; }}
    h2 {{ margin-top: 0; color: #f8fafc; font-size: 18px; }}
    .field {{ margin-bottom: 14px; }}
    .label {{ font-size: 11px; text-transform: uppercase; color: #64748b; font-weight: 700; margin-bottom: 4px; }}
    .val {{ font-size: 14px; color: #e2e8f0; }}
    .btn {{ display: inline-block; background-color: #10b981; color: #022c22 !important; font-weight: 700; font-size: 13px; text-decoration: none; padding: 10px 20px; border-radius: 4px; text-transform: uppercase; margin-top: 10px; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='badge'>ATS Ingress</div>
    <h2>New Application Received</h2>
    <div class='field'><div class='label'>Candidate Name</div><div class='val'>{candidateName}</div></div>
    <div class='field'><div class='label'>Email</div><div class='val'><a href='mailto:{candidateEmail}' style='color:#38bdf8;'>{candidateEmail}</a></div></div>
    <div class='field'><div class='label'>Position</div><div class='val'>{jobTitle}</div></div>
    <div class='field'><div class='label'>Resume / CV</div><div class='val'><a href='{resumeUrl}' class='btn' target='_blank'>View Submitted Resume</a></div></div>
  </div>
</body>
</html>";

        var adminRecipient = Environment.GetEnvironmentVariable("EMAIL_RECIPIENT") ?? _configuration["Email:RecipientEmail"] ?? "Adejorotgold1@yahoo.com";
        return await SendEmailAsync(adminRecipient, "6174 Talent Engineering", subject, htmlBody, cancellationToken, replyToEmail: candidateEmail, replyToName: candidateName);
    }

    public async Task<bool> SendCohortApplicationAlertAsync(string applicantName, string applicantEmail, string trackName, CancellationToken cancellationToken = default)
    {
        var subject = $"[6174 Academy] New Fellowship Application: {applicantName}";
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{ font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background-color: #030712; color: #f8fafc; margin: 0; padding: 24px; }}
    .container {{ max-width: 600px; margin: 0 auto; background-color: #0b1120; border: 1px solid #1e293b; border-radius: 12px; padding: 28px; }}
    .badge {{ display: inline-block; background-color: #8b5cf6; color: #2e1065; font-weight: 700; font-size: 11px; padding: 4px 8px; border-radius: 4px; text-transform: uppercase; margin-bottom: 12px; }}
    h2 {{ margin-top: 0; color: #f8fafc; font-size: 18px; }}
    .field {{ margin-bottom: 14px; }}
    .label {{ font-size: 11px; text-transform: uppercase; color: #64748b; font-weight: 700; margin-bottom: 4px; }}
    .val {{ font-size: 14px; color: #e2e8f0; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='badge'>Systems Academy Ingress</div>
    <h2>New Fellowship Cohort Application</h2>
    <div class='field'><div class='label'>Applicant Name</div><div class='val'>{applicantName}</div></div>
    <div class='field'><div class='label'>Email</div><div class='val'><a href='mailto:{applicantEmail}' style='color:#38bdf8;'>{applicantEmail}</a></div></div>
    <div class='field'><div class='label'>Curriculum Track</div><div class='val'>{trackName}</div></div>
  </div>
</body>
</html>";

        var adminRecipient = Environment.GetEnvironmentVariable("EMAIL_RECIPIENT") ?? _configuration["Email:RecipientEmail"] ?? "Adejorotgold1@yahoo.com";
        return await SendEmailAsync(adminRecipient, "6174 Academy Admissions", subject, htmlBody, cancellationToken, replyToEmail: applicantEmail, replyToName: applicantName);
    }

    private async Task<bool> SendEmailAsync(string recipientEmail, string recipientName, string subject, string htmlBody, CancellationToken cancellationToken, string? replyToEmail = null, string? replyToName = null)
    {
        var smtpHost = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
        var smtpPort = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? _configuration["Email:SmtpPort"], out var port) ? port : 587;
        var smtpUser = Environment.GetEnvironmentVariable("EMAIL_USER") ?? _configuration["Email:SmtpUser"] ?? "";
        var smtpPass = Environment.GetEnvironmentVariable("EMAIL_PASS") ?? _configuration["Email:SmtpPassword"] ?? "";
        var fromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM") ?? _configuration["Email:FromEmail"] ?? smtpUser;

        if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
        {
            _logger.LogInformation("SMTP credentials not configured. Email suppressed: To '{Name}' <{Email}>: Subject: {Subject}",
                recipientName, recipientEmail, subject);
            return true;
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FourDigitCore (6174)", fromEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            if (!string.IsNullOrWhiteSpace(replyToEmail))
            {
                message.ReplyTo.Add(new MailboxAddress(replyToName ?? replyToEmail, replyToEmail));
            }
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(smtpUser, smtpPass, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Email successfully dispatched to {Recipient} via SMTP ({Host}:{Port})", recipientEmail, smtpHost, smtpPort);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient} via SMTP.", recipientEmail);
            return false;
        }
    }
}
