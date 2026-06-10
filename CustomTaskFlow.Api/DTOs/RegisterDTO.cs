using System.ComponentModel.DataAnnotations;

namespace CustomTaskFlow.Api.DTOs
{
    public class RegisterDTO
    {
            [Required]
            [MinLength(3)]
            [MaxLength(50)]
            public string UserName { get; set; } = null!;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [Required]
            [MinLength(6)]
            public string Password { get; set; } = null!;
    }
}
