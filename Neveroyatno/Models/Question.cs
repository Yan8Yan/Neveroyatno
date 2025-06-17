namespace Neveroyatno.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        // Тип вопроса: 0 - открытый, 1 - один из вариантов, 2 - много вариантов
        public QuestionType Type { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }

    public enum QuestionType
    {
        OpenText = 0,
        SingleChoice = 1,
        MultipleChoice = 2
    }
}
