using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;

namespace CustomTaskFlow.Api.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<TaskResponseDto>> CreateAsync(CreateTaskDto dto);
        Task<ApiResponse<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto);
        Task<ApiResponse<TaskResponseDto>> DeleteAsync(int id);
        Task<ApiResponse<PagedResult<TaskResponseDto>>> GetAllAsync(int pageNumber, int pageSize, bool? isCompleted , string? search );
    }
}
