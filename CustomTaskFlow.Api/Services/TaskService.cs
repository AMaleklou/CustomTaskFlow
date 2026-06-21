using AutoMapper;
using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Mappings;
using CustomTaskFlow.Api.Models;
using CustomTaskFlow.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;


namespace CustomTaskFlow.Api.Services
{
    public class TaskService : ITaskService
    {
        //private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;

        public TaskService(IMapper mapper,ITaskRepository taskRepository) 
        { 
          //_context = context;
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,

            };
            var createdTask = await _taskRepository.CreateAsync(task);
            var result = _mapper.Map<TaskResponseDto>(createdTask);
            return ApiResponse<TaskResponseDto>.SuccessResponse(result, "Task created  successfully");
        }

        public async Task<ApiResponse<TaskResponseDto>> DeleteAsync(int userId, int id)
        {
            var task = await _taskRepository.GetForUpdateDeleteAsync(userId,id);
            if (task == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse(["Task not found"], "Task not found");
            }
            task.IsDeleted = true;
            await _taskRepository.SaveChangesAsync();
            var response = _mapper.Map<TaskResponseDto>(task);
            return ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task removed successfully");
        }

        public async Task<ApiResponse<PagedResult<TaskResponseDto>>> GetAllAsync(int userId,int pageNumber, int pageSize, bool? isCompleted , string? search)
        {
            if (pageNumber < 1)
            {
                return ApiResponse<PagedResult<TaskResponseDto>>.ErrorResponse([$"Invalid page number {pageNumber}"], "Invalid PageNumber");
            }

            if (pageSize < 1 || pageSize > 50)
            {
                return ApiResponse<PagedResult<TaskResponseDto>>.ErrorResponse([$"Invalid page size {pageSize}"], "Invalid PageSize");
            }
            var taskQuery = await _taskRepository.GetAllAsync(userId,pageNumber,pageSize,isCompleted,search);

            var result = new PagedResult<TaskResponseDto>
            {
                Items = _mapper.Map<List<TaskResponseDto>>(taskQuery.Items),
                PageNumber = taskQuery.PageNumber,
                PageSize = taskQuery.PageSize,
                TotalCount = taskQuery.TotalCount,
                TotalPages = taskQuery.TotalPages
            };
            return ApiResponse<PagedResult<TaskResponseDto>>.SuccessResponse(result, "All Tasks fetched successfully");
        }

        public async Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int userId,int id)
        {
            var RepoTask = await _taskRepository.GetByIdAsync(userId,id);
            if (RepoTask == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse(["Task not found"], "Task not found");
            }
            var task = _mapper.Map<TaskResponseDto>(RepoTask);                        
            return ApiResponse<TaskResponseDto>.SuccessResponse(task, "Task fetched successfully");
        }

        public async Task<ApiResponse<TaskResponseDto>> UpdateAsync(int userId, int id, UpdateTaskDto dto)
        {
            var task = await _taskRepository.GetForUpdateDeleteAsync(userId,id);
            if (task == null)
            {
                return ApiResponse<TaskResponseDto>.ErrorResponse(["Task not found"], "Task not found");
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

             await _taskRepository.SaveChangesAsync();
            var response = _mapper.Map<TaskResponseDto>(task);
            return ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task updated successfully");
        }
    }
}
