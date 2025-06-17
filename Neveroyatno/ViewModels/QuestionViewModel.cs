using Neveroyatno.Models;

namespace Neveroyatno.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }
}
