using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HomeService.Data;
using HomeService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[Route("api/[controller]/[action]")]
[ApiController]
public class HomeController : Controller
{
    private readonly IHttpContextAccessor _context;
    private readonly IKafkaProducerService _kafka;
    private readonly AppDbContext _db;


    public HomeController(IHttpContextAccessor context, IKafkaProducerService kafka, AppDbContext db)
    {
        _context = context;
        _kafka = kafka;
        _db = db;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetRecommendedMovies()
    {
        var user = getTokenValue();
        if (string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized("User not found");
        }
        int userId = int.Parse(user.Id);

        // 1. Get user's interaction history
        var interactions = _db.UserInteractions.Where(u => u.UserId == userId).ToList();

        if (!interactions.Any())
        {
            // Fallback: Return top rated or random movies if no history
            return Ok(_db.Data.OrderByDescending(m => m.Rating).Take(10).ToList());
        }

        // 2. Identify top genres
        // Assuming SampleData has a Genre field and we join or fetch to get genres of interacted movies.
        // Since UserInteraction only has MovieId, we need to join with Data (SampleData).
        
        var interactedMovieIds = interactions.Select(i => i.MovieId).Distinct().ToList();
        
        var topGenres = _db.Data
            .Where(m => interactedMovieIds.Contains(m.Id))
            .GroupBy(m => m.Genre)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Take(3)
            .ToList();

        // 3. Return movies from those genres, excluding already interacted ones (optional, but good for discovery)
        var recommendations = _db.Data
            .Where(m => topGenres.Contains(m.Genre) && !interactedMovieIds.Contains(m.Id))
            .OrderByDescending(m => m.Rating)
            .Take(10)
            .ToList();

        if (!recommendations.Any())
        {
             return Ok(_db.Data.OrderByDescending(m => m.Rating).Take(10).ToList());
        }

        return Ok(recommendations);
    }

    [Authorize]
    [HttpGet]
    public IActionResult StreamVideo(string movieId)
    {
        var user = getTokenValue();
        if (string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized("User not found");
        }
        int userId = int.Parse(user.Id);

        var movie = _db.Data.FirstOrDefault(m => m.Id == movieId);
        if (movie == null)
        {
            return NotFound("Movie not found");
        }

        if (movie.Price > 0)
        {
            // Check if user paid
            var hasPurchased = _db.UserPurchases.Any(p => p.UserId == userId && p.MovieId == movieId);
            if (!hasPurchased)
            {
                return StatusCode(402, "Payment Required");
            }
        }

        // Log the view interaction
        var interaction = new UserInteraction
        {
            UserId = userId,
            MovieId = movieId,
            InteractionType = "View",
            Timestamp = DateTime.UtcNow
        };
        _db.UserInteractions.Add(interaction);
        _db.SaveChanges();

        // Return the video link
        return Ok(new { StreamUrl = movie.Link });
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
