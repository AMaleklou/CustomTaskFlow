using AutoMapper;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
