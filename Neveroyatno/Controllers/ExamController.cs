using Microsoft.AspNetCore.Mvc;
using Neveroyatno.Data;
using Neveroyatno.Models;
using Neveroyatno.ViewModels;
using Microsoft.EntityFrameworkCore; 


namespace Neveroyatno.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Exam()
        {
            var tests = await _context.Tests
                .Include(t => t.Lecture)
                .ToListAsync();

            var viewModel = new ExamListViewModel
            {
                Tests = tests
            };

            return View(viewModel);
        }

        // Старт экзамена
        public async Task<IActionResult> Start()
        {
            // Получаем все вопросы из всех тестов
            var allQuestions = await _context.TaskItems
                .Include(t => t.Question)
                    .ThenInclude(q => q.Answers)
                .Select(t => t.Question)
                .Distinct()
                .ToListAsync();

            if (!allQuestions.Any())
                return NotFound("Нет доступных вопросов для экзамена.");

            var model = new ExamViewModel
            {
                TestId = 0, // или Guid.NewGuid().ToString() если нужно
                TestName = "Итоговый экзамен по всем лекциям",
                Questions = allQuestions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Type = q.Type,
                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        AnswerId = a.Id,
                        Text = a.Text
                    }).ToList()
                }).ToList()
            };

            return View("Start", model);
        }


        [HttpPost]
        public async Task<IActionResult> Submit(ExamResultViewModel model)
        {
            int score = 0;

            foreach (var answer in model.Answers)
            {
                var question = await _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

                if (question == null) continue;

                var correctAnswers = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToList();
                var submittedAnswers = answer.SelectedAnswerIds;

                if (question.Type == QuestionType.OpenText)
                {
                    // Можно внедрить ручную проверку текста
                    continue;
                }

                if (correctAnswers.Count == submittedAnswers.Count &&
                    correctAnswers.All(id => submittedAnswers.Contains(id)))
                {
                    score++;
                }
            }

            var percentage = (int)((score / (double)model.Answers.Count) * 100);

            return View("Result", new ExamResultSummaryViewModel
            {
                Score = score,
                Total = model.Answers.Count,
                Percentage = percentage,
                CertificateName = percentage >= 60 ? $"Certificate_{Guid.NewGuid()}.pdf" : null
            });
        }
    }

}
