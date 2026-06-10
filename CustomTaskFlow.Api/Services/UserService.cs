using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Repositories;
using CustomTaskFlow.Api.Models;
using System.Threading.Tasks;
using AutoMapper;

namespace CustomTaskFlow.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IMapper mapper) 
        { 
          _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserResponseDto>> RegisterAsync(RegisterDTO dto)
        {
            var userNameCheck = await _userRepository.GetByUserNameAsync(dto.UserName);
            var userEmailCheck = await _userRepository.GetByEmailAsync(dto.Email);
            if (userEmailCheck != null )
            {
                return ApiResponse<UserResponseDto>.ErrorResponse([$"User With Email {dto.Email} Already Exists"] , "User Already Exists");
            }

            if ( userNameCheck != null)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse([$"User With Username {dto.UserName} Already Exists"], "User Already Exists");
            }

            var user = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = dto.Password,    
                CreatedAt = DateTime.UtcNow
            };
            var addedUser = await _userRepository.CreateAsync(user);
            var result = _mapper.Map<UserResponseDto>(addedUser);
            return ApiResponse<UserResponseDto>.SuccessResponse(result, "user created successfully");
        }
    }
}
