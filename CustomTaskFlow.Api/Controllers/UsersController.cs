using CustomTaskFlow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomTaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) 
        { 
          _userService = userService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize, string? search)
        {
            var response = await _userService.GetAllAsync(pageNumber, pageSize, search);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
