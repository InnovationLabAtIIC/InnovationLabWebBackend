using InnovationLab.Shared.Interfaces;
using InnovationLab.Shared.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InnovationLab.Shared.Services;

public class EmailService(IOptions<SmtpOptions> smtpOptions) : IEmailService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_smtpOptions.FromName, _smtpOptions.FromEmail));
        msg.To.Add(MailboxAddress.Parse(toEmail));
        msg.Subject = subject;
        var body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };
        msg.Body = body;

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);
        if (!string.IsNullOrEmpty(_smtpOptions.User))
            await smtp.AuthenticateAsync(_smtpOptions.User, _smtpOptions.Password);
        await smtp.SendAsync(msg);
        await smtp.DisconnectAsync(true);
    }
}