namespace MovieHubMvc.Services;

public interface IEmailSender
{
    /// <summary>
    /// Sends an email message.
    /// </summary>
    /// <param name="toEmail">Recipient address.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="htmlBody">HTML body content.</param>
    Task SendEmailAsync(string toEmail, string subject, string htmlBody);
}
