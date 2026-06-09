using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomTaskFlow.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _appDbContext;

        public TaskRepository(AppDbContext appDbContext) 
        {         
          _appDbContext = appDbContext;
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
               _appDbContext.Tasks.Add(task);
              await _appDbContext.SaveChangesAsync();
            return task;
        }

        public async Task<PagedResult<TaskItem>> GetAllAsync(int pageNumber, int pageSize, bool? isCompleted, string? search)
        {
            var taskQuery =  _appDbContext.Tasks.AsNoTracking();

            taskQuery =  taskQuery.Where(q => !q.IsDeleted);
            if (isCompleted.HasValue)
            {
                taskQuery =  taskQuery.Where(t => t.IsCompleted == isCompleted.Value);
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

            var taskList = await taskQuery.ToListAsync();

            var result = new PagedResult<TaskItem>
            {
                Items = taskList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return result;
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _appDbContext.Tasks
                                     .AsNoTracking()
                                     .Where(t => t.Id == id && !t.IsDeleted)
                                     .FirstOrDefaultAsync();
        }

        public async Task<TaskItem?> GetForUpdateDeleteAsync(int id)
        {
            return await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

    }
}
