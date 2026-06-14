namespace CustomTaskFlow.Api.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
    }
}
