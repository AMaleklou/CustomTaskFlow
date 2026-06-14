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
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository,IMapper mapper,IJwtService jwtService) 
        { 
          _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse([$"Invalid email or password"], "Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);
            var result = new LoginResponseDto
            {
                Token = token,
                UserEmail = user.Email,
                UserName = user.UserName
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(result, "Login successful");
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) ,    
                CreatedAt = DateTime.UtcNow
            };
            var addedUser = await _userRepository.CreateAsync(user);
            var result = _mapper.Map<UserResponseDto>(addedUser);
            return ApiResponse<UserResponseDto>.SuccessResponse(result, "user created successfully");
        }
    }
}
