using Microsoft.AspNetCore.Mvc;

namespace MovieHubMvc.Controllers;

public class MoviesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
