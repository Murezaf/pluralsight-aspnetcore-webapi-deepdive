using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.AuthorCollections.Queries;

public class GetAuthorCollectionQuery : IRequest<IEnumerable<AuthorDto>>
{
    public IEnumerable<Guid> AuthorIds { get; }

    public GetAuthorCollectionQuery(IEnumerable<Guid> authorIds)
    {
        AuthorIds = authorIds;
    }
}