using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CustomTaskFlow.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) 
        { 
          _context = context;
        }

        public async Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int id)
        {
            var task = await _context.Tasks
                                     .AsNoTracking()
                                     .Where(t => t.Id == id && !t.IsDeleted)
                                     .Select(t => new TaskResponseDto
                                     {
                                         Id = t.Id,
                                         Title = t.Title,
                                         Description = t.Description,
                                         IsCompleted = t.IsCompleted,
                                         CreatedAt = t.CreatedAt,
                                     })
                                     .FirstOrDefaultAsync();
            if (task == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse(["No task exists with id " + id], "Task not found");
            }                         
            return ApiResponse<TaskResponseDto>.SuccessResponse(task, "Task fetched successfully");
        }
    }
}
