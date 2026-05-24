using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Mappings
{
    public static class TaskMappings
    {

        public static TaskResponseDto ToResponseDto(this TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }
    }
}
