using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public class GetAuthorByIdQuery : IRequest<ExpandoObject>
{
    public Guid AuthorId { get; }
    public string? Fields { get; }

    public GetAuthorByIdQuery(Guid authorId, string? fields)
    {
        AuthorId = authorId;
        Fields = fields;
    }
}
