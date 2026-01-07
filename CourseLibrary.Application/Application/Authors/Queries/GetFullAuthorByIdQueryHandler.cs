using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.Application.Application.Authors.Queries;

public class GetFullAuthorByIdQueryHandler : IRequestHandler<GetFullAuthorByIdQuery, ExpandoObject>
{
    private readonly IAuthorService _authorService;

    public GetFullAuthorByIdQueryHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<ExpandoObject> Handle(GetFullAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var fullAuthorDto = await _authorService.GetFullAuthorAsync(request.AuthorId);

        if (fullAuthorDto == null)
        {
            return null;
        }

        return fullAuthorDto.ShapeData(request.Fields);
    }
}
