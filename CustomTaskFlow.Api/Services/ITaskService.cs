using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;

namespace CustomTaskFlow.Api.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int userId,int id);
        Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto,int userId);
        Task<ApiResponse<TaskResponseDto>> UpdateAsync(int userId, int id, UpdateTaskDto dto);
        Task<ApiResponse<TaskResponseDto>> DeleteAsync(int userId, int id);
        Task<ApiResponse<PagedResult<TaskResponseDto>>> GetAllAsync(int userId,int pageNumber, int pageSize, bool? isCompleted , string? search);
    }
}
