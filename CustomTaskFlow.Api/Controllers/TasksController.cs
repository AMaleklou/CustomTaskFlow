
using CustomTaskFlow.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using CustomTaskFlow.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace CustomTaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var response = await _taskService.CreateAsync(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize= 10
                                                , bool? isCompleted = null, string? search = null) 
        {
            var response = await _taskService.GetAllAsync(pageNumber, pageSize, isCompleted, search);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _taskService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateTaskDto dto)
        {
           var response = await _taskService.UpdateAsync(id, dto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var response = await _taskService.DeleteAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
