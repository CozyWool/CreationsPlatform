using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Controllers;

public class CreationsPlatformController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}