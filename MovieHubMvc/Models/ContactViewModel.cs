using System.ComponentModel.DataAnnotations;

namespace MovieHubMvc.Models;

/// <summary>
/// Form model for "Suggest a Movie / Contact" — fits MovieHub theme.
/// User can recommend a film or send feedback; message is emailed to admin.
/// </summary>
public class ContactViewModel
{
    [Required(ErrorMessage = "Please enter your name")]
    [StringLength(100)]
    [Display(Name = "Your name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Your email")]
    public string Email { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Movie title (optional)")]
    public string? MovieTitle { get; set; }

    [Required(ErrorMessage = "Please write a message")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be 10–2000 characters")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}
