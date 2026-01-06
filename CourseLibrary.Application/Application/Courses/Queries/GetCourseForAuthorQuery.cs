using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Queries;

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