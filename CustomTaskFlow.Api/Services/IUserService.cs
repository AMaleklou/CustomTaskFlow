using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;

namespace CustomTaskFlow.Api.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponseDto>> RegisterAsync(RegisterDTO dto);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<ApiResponse<PagedResult<UserResponseDto>>> GetAllAsync(int pageNumber, int pageSize, string? search);


    }
}
