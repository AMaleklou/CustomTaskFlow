
using CustomTaskFlow.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using CustomTaskFlow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CustomTaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            return int.Parse(userIdClaim.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _taskService.CreateAsync(dto,userId);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize= 10
                                                , bool? isCompleted = null, string? search = null) 
        {
            var userId = GetCurrentUserId();
            var response = await _taskService.GetAllAsync(userId,pageNumber, pageSize, isCompleted, search);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();
            var response = await _taskService.GetByIdAsync(userId,id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateTaskDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _taskService.UpdateAsync(userId,id, dto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var userId = GetCurrentUserId();
            var response = await _taskService.DeleteAsync(userId, id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
