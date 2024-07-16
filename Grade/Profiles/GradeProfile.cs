using AutoMapper;
using Grade.DTOs.Input.AssessmentTypes;
using Grade.DTOs.Input.Grades;
using Grade.DTOs.Input.QuarterlyGrades;
using Grade.DTOs.Input.TermAssessments;
using Grade.DTOs.Output;
using Grade.Models;

namespace Grade.Profiles;

public class GradeProfile : Profile
{
    public GradeProfile()
    {
        CreateMap<CreateGradeDto, Models.Grade>();
        CreateMap<UpdateGradeDto, Models.Grade>();

        CreateMap<CreateQuarterlyGradeDTO, QuarterlyGrade>();
        CreateMap<UpdateQuarterlyGradeDTO, QuarterlyGrade>();

        CreateMap<CreateAssessmentTypeDTO, AssessmentType>();
        CreateMap<UpdateAssessmentTypeDTO, AssessmentType>();

        CreateMap<CreateTermAssessmentDTO, TermAssessment>();
        CreateMap<UpdateTermAssessmentDTO, TermAssessment>();
        CreateMap<TermAssessment, TermAssessmentOutputDTO>();
    }
}