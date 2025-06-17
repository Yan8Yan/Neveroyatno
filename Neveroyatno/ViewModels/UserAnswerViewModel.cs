namespace Neveroyatno.ViewModels
{
    public class UserAnswerViewModel
    {
        public int QuestionId { get; set; }
        public List<int> SelectedAnswerIds { get; set; }
        public string? OpenTextAnswer { get; set; }
    }
}
