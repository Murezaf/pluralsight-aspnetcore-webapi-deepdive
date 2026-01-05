using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public record GetFullAuthorByIdQuery(Guid AuthorId, string? Fields) : IRequest<ExpandoObject>;