using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int id);
        Task<PagedResult<TaskItem>> GetAllAsync(int pageNumber,int pageSize,bool? isCompleted,string? search);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem?> GetForUpdateDeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
