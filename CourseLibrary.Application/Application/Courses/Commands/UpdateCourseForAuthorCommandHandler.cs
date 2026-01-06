using AutoMapper;
using CourseLibrary.Application.Contracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class UpdateCourseForAuthorCommandHandler : IRequestHandler<UpdateCourseForAuthorCommand, CourseDto?>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public UpdateCourseForAuthorCommandHandler(
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CourseDto?> Handle(UpdateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(request.AuthorId, request.CourseId);

        if (courseEntity == null)
        {
            Course newCourse = _mapper.Map<Course>(request.CourseForUpdateDto);
            newCourse.Id = request.CourseId;

            _courseRepository.AddCourse(request.AuthorId, newCourse);
            await _courseRepository.SaveAsync();

            return _mapper.Map<CourseDto>(newCourse);
        }

        _mapper.Map(request.CourseForUpdateDto, courseEntity);
        await _courseRepository.SaveAsync();

        return null;
    }
}