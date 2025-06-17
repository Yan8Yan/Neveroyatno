using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Neveroyatno.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LectureId { get; set; }

        [ValidateNever]
        public virtual Lecture? Lecture { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }


}
