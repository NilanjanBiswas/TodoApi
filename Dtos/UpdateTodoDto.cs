using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class UpdateTodoDto
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = default!;


        [MaxLength(2000)]
        public string? Description { get; set; }


        public bool IsCompleted { get; set; }


        public DateTime? DueDate { get; set; }
    }
}
