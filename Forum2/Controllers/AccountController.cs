using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class AccountController : Controller
{
    public IActionResult Table()
    {
        return View();
    }
    public IActionResult Grid()
    {
        return View();
    }
}