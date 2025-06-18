namespace Neveroyatno.Models
{
    public class UserAnswerDto
    {
        public int TaskId { get; set; }
        public List<int> SelectedAnswerIds { get; set; } = new List<int>(); 
        public string? OpenTextAnswer { get; set; }
    }

}
