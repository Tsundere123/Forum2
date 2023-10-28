using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class MemberController : Controller
{
    public IActionResult List()
    {
        return View();
    }
    public IActionResult Grid()
    {
        return View();
    }
}