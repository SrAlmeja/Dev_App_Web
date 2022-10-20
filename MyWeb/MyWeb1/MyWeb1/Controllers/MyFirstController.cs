using Microsoft.AspNetCore.Mvc;

namespace MyWeb1.Controllers;

public class MyFirstController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}