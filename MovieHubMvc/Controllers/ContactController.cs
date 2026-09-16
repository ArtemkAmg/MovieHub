using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MovieHubMvc.Models;
using MovieHubMvc.Services;

namespace MovieHubMvc.Controllers;

public class ContactController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        IEmailSender emailSender,
        IOptions<EmailSettings> emailOptions,
        ILogger<ContactController> logger)
    {
        _emailSender = emailSender;
        _emailSettings = emailOptions.Value;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var subject = string.IsNullOrWhiteSpace(model.MovieTitle)
                ? $"[MovieHub] Message from {model.Name}"
                : $"[MovieHub] Movie suggestion: {model.MovieTitle}";

            var htmlBody = $"""
                <div style="font-family:Poppins,Arial,sans-serif;max-width:600px;margin:0 auto;background:#111;color:#fff;padding:24px;border-radius:12px;">
                  <h2 style="color:#E50914;margin-top:0;">🎬 New message from MovieHub</h2>
                  <p><strong>From:</strong> {System.Net.WebUtility.HtmlEncode(model.Name)}</p>
                  <p><strong>Email:</strong> {System.Net.WebUtility.HtmlEncode(model.Email)}</p>
                  {(string.IsNullOrWhiteSpace(model.MovieTitle) ? "" : $"<p><strong>Suggested movie:</strong> {System.Net.WebUtility.HtmlEncode(model.MovieTitle)}</p>")}
                  <hr style="border-color:#333;" />
                  <p style="white-space:pre-wrap;">{System.Net.WebUtility.HtmlEncode(model.Message)}</p>
                  <p style="color:#888;font-size:12px;margin-bottom:0;">Sent via MovieHub contact form</p>
                </div>
                """;

            // Send to site admin (configured in appsettings)
            await _emailSender.SendEmailAsync(
                _emailSettings.AdminEmail,
                subject,
                htmlBody);

            // Optional: confirmation copy to the user
            var confirmHtml = $"""
                <div style="font-family:Poppins,Arial,sans-serif;max-width:600px;margin:0 auto;background:#111;color:#fff;padding:24px;border-radius:12px;">
                  <h2 style="color:#E50914;">Thanks, {System.Net.WebUtility.HtmlEncode(model.Name)}!</h2>
                  <p>We received your message on <strong>MovieHub</strong> and will get back to you soon.</p>
                  <p style="color:#888;font-size:12px;">This is an automated confirmation.</p>
                </div>
                """;

            await _emailSender.SendEmailAsync(
                model.Email,
                "MovieHub — we got your message 🎬",
                confirmHtml);

            TempData["Success"] = "Message sent! Check your inbox for a confirmation.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact email");
            ModelState.AddModelError(string.Empty,
                "Could not send email. Check EmailSettings in appsettings.json (Gmail App Password etc.).");
            return View(model);
        }
    }
}
