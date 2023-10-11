using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class AdminController : Controller
{
    // GET
    [HttpGet]
    [Authorize( Roles = "Administrator" )]
    public IActionResult Index()
    {
        return View();
    }
}