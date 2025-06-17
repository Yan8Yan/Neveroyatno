using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Neveroyatno.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int TestId { get; set; }

        [ValidateNever]
        public virtual Test? Test { get; set; }

        public int QuestionId { get; set; }

        [ValidateNever]
        public virtual Question? Question { get; set; }
    }

}
