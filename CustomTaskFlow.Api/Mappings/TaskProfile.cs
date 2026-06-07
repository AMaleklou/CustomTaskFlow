using AutoMapper;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Mappings
{
    public class TaskProfile : Profile
    {
        
        public TaskProfile()
        {
            CreateMap<TaskItem, TaskResponseDto>();
        }
    }
}
