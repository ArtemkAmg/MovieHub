using Microsoft.AspNetCore.Mvc;

namespace MovieHubMvc.Controllers;

public class MovieController : Controller
{
    public IActionResult Details(string? id)
    {
        ViewData["MovieId"] = id;
        return View();
    }
}
