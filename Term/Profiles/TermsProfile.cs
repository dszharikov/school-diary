using AutoMapper;
using Term.DTOs;

namespace Term.Profiles;

public class TermsProfile : Profile
{
    public TermsProfile()
    {
        // Source -> Target
        CreateMap<CreateTermDto, Models.Term>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.Parse(src.EndDate)));
            
        CreateMap<UpdateTermDto, Models.Term>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.Parse(src.EndDate)));

        CreateMap<Models.Term, TermOutputDto>();
    }
}