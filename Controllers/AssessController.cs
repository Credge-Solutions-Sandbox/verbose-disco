using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wellness.Api.Models;
using System.Linq;

namespace Wellness.Api.Controllers;

[Route("api/[controller]")]
public class AssessController : Controller
{
    private readonly ILogger<AssessController> _logger;
    private readonly ApplicationDbContext _context;
    public AssessController(ILogger<AssessController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public List<Assess> GetAllAsync()
    {
        return _context.Assesses.ToList();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]Assess assess)
    {
        await _context.AddAsync(assess);
        _context.SaveChanges();
        return Ok("Success");
        
    }
   
}