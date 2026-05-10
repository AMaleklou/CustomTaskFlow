using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize= 10) 
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
            taskQuery = taskQuery.Skip((pageNumber - 1) * pageSize);
            taskQuery = taskQuery.Take(pageSize);

            var taskList = await taskQuery.ToListAsync();

            return Ok(taskList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(t=> t.Id == id && !t.IsDeleted);
            if (task == null) return NotFound();
            return Ok(task);                                   
        }

    }
}
