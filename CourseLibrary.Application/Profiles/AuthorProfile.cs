using AutoMapper;
using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;

namespace CourseLibrary.Application.Profiles;

public class AuthorsProfile : Profile
{
    public AuthorsProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath)));
                //opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

        CreateMap<AuthorForCreationDto, Author>();
        CreateMap<Author, FullAuthorDto>();
        CreateMap<AuthorForCreationWithDateOfDeathDto, Author>();
    }
}

