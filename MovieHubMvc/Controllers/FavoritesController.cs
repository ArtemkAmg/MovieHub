using Microsoft.AspNetCore.Mvc;

namespace MovieHubMvc.Controllers;

public class FavoritesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
