using System.ComponentModel.DataAnnotations;

namespace CustomTaskFlow.Api.DTOs
{
    public class UpdateTaskDto
    {
        [Required,MaxLength(50),MinLength(3)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
