using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index() => RedirectToAction(nameof(Dashboard));


    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return View();
    }
}
