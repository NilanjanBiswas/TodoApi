using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = default!;


        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = default!;


        // store a salted & hashed password
        [Required]
        public string PasswordHash { get; set; } = default!;


        // Navigation
        public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
    }
}

