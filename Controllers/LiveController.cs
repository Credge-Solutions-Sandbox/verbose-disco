using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wellness.Api.Models;
using System.Linq;

namespace Wellness.Api.Controllers;

[Route("api/[controller]")]
public class LiveController : Controller
{
    private readonly ILogger<LiveController> _logger;
    private readonly ApplicationDbContext _context;
    public LiveController(ILogger<LiveController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public List<Live> GetAllAsync()
    {
        return _context.Lives.ToList();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]Live Live)
    {
        await _context.AddAsync(Live);
        _context.SaveChanges();
        return Ok("Success");
        
    }
   
}