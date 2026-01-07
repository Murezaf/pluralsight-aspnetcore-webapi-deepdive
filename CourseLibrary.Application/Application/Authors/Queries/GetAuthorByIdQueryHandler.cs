using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.Application.Application.Authors.Queries;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, ExpandoObject>
{
    private readonly IAuthorService _authorService;

    public GetAuthorByIdQueryHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<ExpandoObject> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var authorDto = await _authorService.GetAuthorAsync(request.AuthorId);

        if (authorDto == null)
        {
            return null;
        }

        return authorDto.ShapeData(request.Fields);
    }
}
