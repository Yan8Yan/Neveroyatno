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

        public async Task<IActionResult> Index(string? fullName, string? lectureTitle, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.TestResults
                .Include(r => r.Test)
                    .ThenInclude(t => t.Lecture)
                .Include(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.Where(r => r.User.FullName.Contains(fullName));
            }

            if (!string.IsNullOrEmpty(lectureTitle))
            {
                query = query.Where(r => r.Test.Lecture.Title.Contains(lectureTitle));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(r => r.PassedAt >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(r => r.PassedAt <= dateTo.Value);
            }

            var results = await query
                .OrderByDescending(r => r.PassedAt)
                .ToListAsync();

            ViewBag.FullNameFilter = fullName;
            ViewBag.LectureTitleFilter = lectureTitle;
            ViewBag.DateFromFilter = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateToFilter = dateTo?.ToString("yyyy-MM-dd");

            return View(results);
        }


    }
}
