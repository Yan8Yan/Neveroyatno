namespace Neveroyatno.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }
    }

}
