using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = default!;


        [MaxLength(2000)]
        public string? Description { get; set; }


        public bool IsCompleted { get; set; } = false;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public DateTime? DueDate { get; set; }


        // Foreign Key
        [Required]
        public Guid UserId { get; set; }


        // Navigation
        public User? User { get; set; }
    }
}
