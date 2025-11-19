using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]/[action]")]
[ApiController]
public class HomeController : Controller
{
    private readonly IHttpContextAccessor _context;

    public HomeController(IHttpContextAccessor context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        var name = getTokenValue();
        Console.WriteLine("name" + name);
        return Ok(name);
    }

    #region  private helpers
    private GetTokenValue getTokenValue()
    {
        string email = _context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        string userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string role = _context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        return new GetTokenValue(email, userId, role);
    }
    #endregion
}
