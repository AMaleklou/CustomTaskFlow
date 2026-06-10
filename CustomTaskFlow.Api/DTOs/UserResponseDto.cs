namespace CustomTaskFlow.Api.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
