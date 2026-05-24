using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;

namespace CustomTaskFlow.Api.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponseDto>> GetByIdAsync(int id);
    }
}
