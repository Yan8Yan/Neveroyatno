using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveroyatno.Data;
using Neveroyatno.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Neveroyatno.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateTest(int lectureId)
        {
            var model = new Test
            {
                LectureId = lectureId,
                Tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Question = new Question
                {
                    Answers = new List<Answer>
                    {
                        new Answer(),
                        new Answer(),
                        new Answer(),
                        new Answer()
                    }
                }
            }
        }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest(Test model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _context.Tests.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Lecture");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
                return View(model);
            }
        }

        // Страница прохождения теста
        public IActionResult TakeTest(int testId)
        {
            var test = _context.Tests
                .Include(t => t.Tasks)
                .ThenInclude(t => t.Question)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == testId);

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // Обработка ответа
        [HttpPost]
        public async Task<IActionResult> SubmitTest(int testId, List<Answer> answers)
        {
            var test = await _context.Tests
                .Include(t => t.Tasks)
                .ThenInclude(t => t.Question)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
            {
                return NotFound();
            }

            // Логика проверки ответов
            var correctAnswers = answers.Where(a => a.IsCorrect).ToList();
            var userAnswers = _context.Answers.Where(a => a.QuestionId == correctAnswers.First().QuestionId).ToList();

            // Пример оценки
            var score = userAnswers.Count(a => a.IsCorrect == true);
            ViewBag.Score = score;

            return View("Result", new { Score = score });
        }
    }
}
