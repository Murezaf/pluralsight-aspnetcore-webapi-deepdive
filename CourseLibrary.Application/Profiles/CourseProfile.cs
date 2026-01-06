using AutoMapper;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;

namespace CourseLibrary.Application.Profiles;

public class CoursesProfile : Profile
{
    public CoursesProfile()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<Models.CourseForCreationDto, Course>();
        CreateMap<CourseForUpdateDto, Course>().ReverseMap();
    }
}