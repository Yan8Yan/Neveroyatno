using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveroyatno.Data;
using Neveroyatno.Models;

namespace Neveroyatno.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _context.TestResults
                .Include(r => r.Test)
                    .ThenInclude(t => t.Lecture)
                .Include(r => r.User)
                .OrderByDescending(r => r.PassedAt)
                .ToListAsync();

            return View(results);
        }
    }
}
