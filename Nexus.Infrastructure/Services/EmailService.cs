using Microsoft.Extensions.Logging;
using Nexus.Application.Interfaces;

namespace Nexus.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        _logger.LogInformation($"[MOCK EMAIL] To: {to} | Subject: {subject} | Body: {body}");
        return Task.CompletedTask;
    }
}
