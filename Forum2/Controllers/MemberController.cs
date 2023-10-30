using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class MemberController : Controller
{
    [HttpGet]
    public IActionResult List()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Grid()
    {
        return View();
    }
}