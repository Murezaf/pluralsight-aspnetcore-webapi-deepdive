using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Queries;

public class GetCoursesForAuthorQueryHandler : IRequestHandler<GetCoursesForAuthorQuery, IEnumerable<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCoursesForAuthorQueryHandler(
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> Handle(
        GetCoursesForAuthorQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetCoursesAsync(request.AuthorId);
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }
}