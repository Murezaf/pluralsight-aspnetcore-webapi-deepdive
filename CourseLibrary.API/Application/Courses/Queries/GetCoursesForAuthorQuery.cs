using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Queries;

public class GetCoursesForAuthorQuery : IRequest<IEnumerable<CourseDto>>
{
    public Guid AuthorId { get; }

    public GetCoursesForAuthorQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
}
