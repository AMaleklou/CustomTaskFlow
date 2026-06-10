using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;

namespace CustomTaskFlow.Api.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponseDto>> RegisterAsync(RegisterDTO dto);
        Task<ApiResponse<UserResponseDto>> LoginAsync(LoginDto dto);

    }
}
