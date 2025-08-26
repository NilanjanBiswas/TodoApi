using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class CreateTodoDto
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = default!;


        [MaxLength(2000)]
        public string? Description { get; set; }


        public DateTime? DueDate { get; set; }
    }
}
