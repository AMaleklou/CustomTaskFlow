namespace CustomTaskFlow.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        public DateTime InsertDate { get; set; }
    }
}
