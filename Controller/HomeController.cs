using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class HomeController : Controller
{
    private readonly IHttpContextAccessor _context;
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        new  GetDashBoardValues("name",2);
        var name = _context.HttpContext.User.FindFirst("name")?.Value;
        return Ok(name);
    }
}