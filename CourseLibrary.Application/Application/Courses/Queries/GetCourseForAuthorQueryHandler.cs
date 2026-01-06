using AutoMapper;
using CourseLibrary.Application.Contracts;
using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Queries;

public class GetCourseForAuthorQueryHandler : IRequestHandler<GetCourseForAuthorQuery, CourseDto?>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseForAuthorQueryHandler(
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CourseDto?> Handle(GetCourseForAuthorQuery request, CancellationToken cancellationToken)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(request.AuthorId, request.CourseId);

        if (courseEntity == null)
        {
            return null;
        }

        CourseDto courseDto = _mapper.Map<CourseDto>(courseEntity);
        
        return courseDto;
    }
}
