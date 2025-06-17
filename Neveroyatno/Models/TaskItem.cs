namespace Neveroyatno.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }

        public virtual Question Question { get; set; }
    }

}
