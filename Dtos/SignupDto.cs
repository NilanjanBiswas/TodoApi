using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class SignupDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; } = default!;


        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = default!;


        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;
    }
}
