using AutoMapper;
using User.DTOs.Input;
using User.DTOs.Output;
using User.Models;

namespace User.Profiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        // Source -> Target

        // user
        CreateMap<UserInputDto, Models.User>();
        CreateMap<Models.User, UserOutputDto>();

        // parent
        CreateMap<ParentInputDto, Parent>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Constants.ParentRoleName));
        CreateMap<Parent, ParentOutputDto>();

        // school
        CreateMap<SchoolInputDto, School>();
        CreateMap<School, SchoolOutputDto>();
    }
}