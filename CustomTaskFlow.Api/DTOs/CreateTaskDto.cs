namespace CustomTaskFlow.Api.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
