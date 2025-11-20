using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HomeService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]/[action]")]
[ApiController]
public class DataController : Controller
{
    private readonly IHttpContextAccessor _context;
    private readonly AppDbContext _db;


    public DataController(IHttpContextAccessor context,AppDbContext db)
    {
        _context = context;
        _db = db;
    }
    [HttpPost]
    public IActionResult SaveData(CreateData req)
    {
        var newData = new SampleData()
        {
            Genre = req.Genre,
            Name = req.Name,
            Description = req.Description,
            Link = req.Link
        };
        _db.Data.Add(newData);
        _db.SaveChanges();
        return Ok();
    }

    #region  private helpers
    #endregion
}
