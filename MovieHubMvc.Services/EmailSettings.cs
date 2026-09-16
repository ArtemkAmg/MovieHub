namespace MovieHubMvc.Services;

/// <summary>
/// Configuration for SMTP email sending. Bound from appsettings.json section "EmailSettings".
/// </summary>
public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SenderName { get; set; } = "MovieHub";
    public string SenderEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// Where contact / feedback messages are delivered (site admin).
    /// </summary>
    public string AdminEmail { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}
