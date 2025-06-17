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
                        // Например, проверять на точное совпадение, либо оставить ручную проверку
                        // Здесь просто считаем правильным, если есть ответ
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

            ViewBag.Score = correctCount;
            ViewBag.TotalQuestions = totalQuestions;

            return View("Result", test);
        }

    }
}
