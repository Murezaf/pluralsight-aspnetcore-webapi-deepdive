using MediatR;

namespace CourseLibrary.API.Application.Authors.Queries;

public class CheckAuthorExistsQuery : IRequest<bool>
{
    public Guid AuthorId { get; }

    public CheckAuthorExistsQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
}