using AutoMapper;
using CourseLibrary.Application.Contracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

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
