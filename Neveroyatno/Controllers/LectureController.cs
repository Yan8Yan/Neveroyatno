using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveroyatno.Data;
using Neveroyatno.Models;

namespace Neveroyatno.Controllers
{
    [Authorize(Roles = "Преподаватель")]
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LectureController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var lectures = await _context.Lectures
                .Where(l => l.AuthorId == user.Id) 
                .ToListAsync();

            return View(lectures);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecture lecture)
        {
            var user = await _userManager.GetUserAsync(User);
            lecture.AuthorId = user.Id; 

            if (ModelState.IsValid)
            {
                _context.Lectures.Add(lecture);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(lecture);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lecture == null)
                return NotFound();

            return View(lecture);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
                return NotFound();

            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lecture lecture)
        {
            if (id != lecture.Id)
                return NotFound();

            ModelState.Remove("AuthorId");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                    if (existingLecture == null)
                        return NotFound();

                    lecture.AuthorId = existingLecture.AuthorId;

                    _context.Update(lecture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lecture);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
                return NotFound();

            return View(lecture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.Id == id);
        }

    }

}
