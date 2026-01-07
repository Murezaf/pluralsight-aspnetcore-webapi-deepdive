using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Queries;

public class GetCourseForAuthorQueryHandler : IRequestHandler<GetCourseForAuthorQuery, CourseDto?>
{
    private readonly ICourseService _courseService;

    public GetCourseForAuthorQueryHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<CourseDto?> Handle(GetCourseForAuthorQuery request, CancellationToken cancellationToken)
    {
        return await _courseService.GetCourseForAuthorAsync(request.AuthorId, request.CourseId);
    }
}
