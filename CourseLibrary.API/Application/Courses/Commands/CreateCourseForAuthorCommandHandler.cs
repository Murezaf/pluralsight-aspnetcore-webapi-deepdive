using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Commands;

public class CreateCourseForAuthorCommandHandler : IRequestHandler<CreateCourseForAuthorCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CreateCourseForAuthorCommandHandler(
    ICourseRepository courseRepository,
    IMapper mapper)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CourseDto> Handle(CreateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        Course courseEntity = _mapper.Map<Course>(request.CourseForCreationDto);

        _courseRepository.AddCourse(request.AuthorId, courseEntity);
        await _courseRepository.SaveAsync();

        CourseDto courseDtoToReturn = _mapper.Map<CourseDto>(courseEntity);

        return courseDtoToReturn; 
    }
}
