namespace Neveroyatno.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neveroyatno.Data;
using Neveroyatno.Models;
using Neveroyatno.ViewModels;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var results = await _context.TestResults
            .Include(r => r.Test)
                .ThenInclude(t => t.Lecture)
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.PassedAt)
            .ToListAsync();

        var totalTests = await _context.Tests.CountAsync();
        var completedTests = results.Select(r => r.TestId).Distinct().Count();

        var model = new UserProfileViewModel
        {
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            CompletedTests = completedTests,
            TotalTests = totalTests,
            Results = results
        };

        return View(model);
    }
}
