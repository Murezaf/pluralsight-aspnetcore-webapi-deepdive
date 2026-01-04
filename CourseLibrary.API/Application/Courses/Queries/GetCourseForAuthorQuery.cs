using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Queries;

public class GetCourseForAuthorQuery : IRequest<CourseDto?>
{
    public Guid AuthorId { get; }
    public Guid CourseId { get; }

    public GetCourseForAuthorQuery(Guid authorId, Guid courseId)
    {
        AuthorId = authorId;
        CourseId = courseId;
    }
}