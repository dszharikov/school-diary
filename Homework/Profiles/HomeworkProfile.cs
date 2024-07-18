using AutoMapper;
using Homework.DTOs;

namespace Homework.Profiles;

public class HomeworkProfile : Profile
{
    public HomeworkProfile()
    {
        // Source -> Target
        CreateMap<CreateHomeworkDTO, Models.Homework>();
        CreateMap<UpdateHomeworkDTO, Models.Homework>();
    }
}