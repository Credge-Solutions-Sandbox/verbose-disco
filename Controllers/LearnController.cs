using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wellness.Api.Models;
using System.Linq;

namespace Wellness.Api.Controllers;

[Route("api/[controller]")]
public class LearnController : Controller
{
    private readonly ILogger<LearnController> _logger;
    private readonly ApplicationDbContext _context;
    public LearnController(ILogger<LearnController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public List<Learn> GetAllAsync()
    {
        return _context.Learns.ToList();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]Learn learn)
    {
        await _context.AddAsync(learn);
        _context.SaveChanges();
        return Ok("Success");
        
    }
   
}