using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Neveroyatno.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AuthorId { get; set; }

        [ValidateNever]
        public virtual ApplicationUser Author { get; set; }

        //virtrual для ленивой загрузки
        public virtual Test? Test { get; set; }
    }
}
