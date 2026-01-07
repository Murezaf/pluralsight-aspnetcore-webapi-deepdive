using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Queries;

public class GetCoursesForAuthorQueryHandler : IRequestHandler<GetCoursesForAuthorQuery, IEnumerable<CourseDto>>
{
    private readonly ICourseService _courseService;

    public GetCoursesForAuthorQueryHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<IEnumerable<CourseDto>> Handle(GetCoursesForAuthorQuery request, CancellationToken cancellationToken)
    {
        return await _courseService.GetCoursesForAuthorAsync(request.AuthorId);
    }
}