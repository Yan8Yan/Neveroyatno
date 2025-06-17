namespace Neveroyatno.Models
{
    public class UserAnswerDto
    {
        public int TaskId { get; set; }
        public List<int> SelectedAnswerIds { get; set; } = new List<int>(); // для SingleChoice и MultipleChoice
        public string? OpenTextAnswer { get; set; } // для OpenText
    }

}
