using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Mappings;
using CustomTaskFlow.Api.Models;
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

        public async Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,

            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            var result = task.ToResponseDto();

            return ApiResponse<TaskResponseDto>.SuccessResponse(result, "Task created  successfully");
        }

        public async Task<ApiResponse<TaskResponseDto>> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (task == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse([$"No task exists with id {id}"], "Task not found");
            }
            task.IsDeleted = true;
            await _context.SaveChangesAsync();

            var response = task.ToResponseDto();

            return ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task removed successfully");
        }

        public async Task<ApiResponse<PagedResult<TaskResponseDto>>> GetAllAsync(int pageNumber, int pageSize, bool? isCompleted , string? search)
        {
            if (pageNumber < 1)
            {
                return ApiResponse<PagedResult<TaskResponseDto>>.ErrorResponse([$"Invalid page number {pageNumber}"], "Invalid PageNumber");
            }

            if (pageSize < 1 || pageSize > 50)
            {
                return ApiResponse<PagedResult<TaskResponseDto>>.ErrorResponse([$"Invalid page size {pageSize}"], "Invalid PageSize");
            }
            var taskQuery = _context.Tasks.AsNoTracking();

            taskQuery = taskQuery.Where(q => !q.IsDeleted);
            if (isCompleted.HasValue)
            {
                taskQuery = taskQuery.Where(t => t.IsCompleted == isCompleted.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));
            }

            var totalCount = await taskQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            taskQuery = taskQuery.OrderByDescending(t => t.CreatedAt);
            taskQuery = taskQuery.Skip((pageNumber - 1) * pageSize);
            taskQuery = taskQuery.Take(pageSize);

            var taskList = await taskQuery
                .Select(task => new  TaskResponseDto
                {                  
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt,
                })
                .ToListAsync();

            var result = new PagedResult<TaskResponseDto>  

              {
                Items = taskList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
              } ;

            return ApiResponse<PagedResult<TaskResponseDto>>.SuccessResponse(result, "All Tasks fetched successfully");
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
                return ApiResponse<TaskResponseDto>.ErrorResponse([$"No task exists with id {id}"], "Task not found");
            }                         
            return ApiResponse<TaskResponseDto>.SuccessResponse(task, "Task fetched successfully");
        }

        public async Task<ApiResponse<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (task == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse([$"No task exists with id {id}"], "Task not found");
            }
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            var response = task.ToResponseDto();

            return ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task updated successfully");
        }
    }
}
