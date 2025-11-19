
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
[ApiController]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        new  GetDashBoardValues("name",2);
        return Ok();
    }
}