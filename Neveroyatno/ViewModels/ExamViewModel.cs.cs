using Neveroyatno.Models;

namespace Neveroyatno.ViewModels
{
    public class ExamViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
