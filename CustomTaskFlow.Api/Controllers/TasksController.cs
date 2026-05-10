using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
