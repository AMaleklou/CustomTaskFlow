using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomTaskFlow.Api.Common;
using System.Diagnostics;
using System.Net.WebSockets;

namespace CustomTaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
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
            return Ok(task);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize= 10
                                                , bool? isCompleted = null, string? search = null) 
        {
            if (pageNumber < 1)
            {
               return BadRequest("Invalid page number");
            }

            if (pageSize < 1 || pageSize > 50) 
            {
                return BadRequest("Invalid paging");
            }
            var taskQuery = _context.Tasks.AsNoTracking();

            taskQuery = taskQuery.Where(q => !q.IsDeleted);
            if (isCompleted.HasValue)
            {
                taskQuery = taskQuery.Where(t=> t.IsCompleted == isCompleted.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));
            }

            taskQuery = taskQuery.Skip((pageNumber - 1) * pageSize);
            taskQuery = taskQuery.Take(pageSize);
          
            var taskList = await taskQuery
                .Select(task => new TaskResponseDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt,
                })             
                 .ToListAsync();


            //return Ok(taskList);

            //return Ok(new ApiResponse <List<TaskResponseDto>>
            //{
            //    Success = true,
            //    Message = "Tasks fetched successfully",
            //    Data = taskList
            //});

            return Ok( ApiResponse<List<TaskResponseDto>>.SuccessResponse(taskList, "Tasks fetched successfully"));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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
            //if (task == null) return NotFound();
            //return Ok(task);
            if (task == null) return NotFound(ApiResponse<string>.ErrorResponse(["No task exists with id " + id], "Task not found"));
            return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(task, "Task fetched successfully"));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateTaskDto dto)
        {
           var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (task == null) return NotFound(ApiResponse<string>.ErrorResponse(["No task exists with id " + id], "Task not found"));

            task.Title = dto.Title; 
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
           
            return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (task == null) return NotFound(ApiResponse<string>.ErrorResponse(["No task exists with id " + id], "Task not found"));

            task.IsDeleted = true;
            await _context.SaveChangesAsync();
            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
            return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(response, "Task deleted successfully"));
        }

    }
}
