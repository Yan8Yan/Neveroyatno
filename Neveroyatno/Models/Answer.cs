using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Neveroyatno.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int? QuestionId { get; set; }

        [ValidateNever]
        public virtual Question? Question { get; set; }
    }
}
