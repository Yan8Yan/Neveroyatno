using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveroyatno.Data;
using Neveroyatno.Models;
using System.Linq;
using System.Threading.Tasks;
using Neveroyatno.ViewModels;

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

        [HttpPost]
        public async Task<IActionResult> SubmitTest(int testId, List<UserAnswerDto> userAnswers)
        {
            var test = await _context.Tests
                .Include(t => t.Tasks)
                .ThenInclude(t => t.Question)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
                return NotFound();

            int totalQuestions = test.Tasks.Count;
            int correctCount = 0;

            foreach (var task in test.Tasks)
            {
                var userAnswer = userAnswers.FirstOrDefault(ua => ua.TaskId == task.Id);
                if (userAnswer == null)
                    continue;

                var question = task.Question;

                switch (question.Type)
                {
                    case QuestionType.OpenText:
                        if (!string.IsNullOrEmpty(userAnswer.OpenTextAnswer))
                            correctCount++;
                        break;

                    case QuestionType.SingleChoice:
                        var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                        if (correctAnswer != null && userAnswer.SelectedAnswerIds.Contains(correctAnswer.Id))
                            correctCount++;
                        break;

                    case QuestionType.MultipleChoice:
                        var correctAnswersIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToList();

                        if (userAnswer.SelectedAnswerIds.Count == correctAnswersIds.Count &&
                            !userAnswer.SelectedAnswerIds.Except(correctAnswersIds).Any())
                        {
                            correctCount++;
                        }
                        break;
                }
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var testResult = new TestResult
            {
                TestId = testId,
                UserId = userId,
                Score = correctCount,
                Total = totalQuestions,
                Percentage = (int)((correctCount / (double)totalQuestions) * 100),
                PassedAt = DateTime.UtcNow
            };

            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();

            ViewBag.Score = correctCount;
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.Percentage = testResult.Percentage;

            return View("Result", testResult);
        }


    }
}
