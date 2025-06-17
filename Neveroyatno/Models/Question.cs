using System.ComponentModel.DataAnnotations;

namespace Neveroyatno.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }


    public enum QuestionType
    {
        OpenText = 0,
        SingleChoice = 1,
        MultipleChoice = 2
    }
}