using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.Application.Application.Authors.Queries;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, PagedList<ExpandoObject>>
{
    private readonly IAuthorService _authorService;

    public GetAuthorsQueryHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<PagedList<ExpandoObject>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authorsDto = await _authorService.GetAuthorsAsync(request.AuthorRecourseParameters);

        var authorsEnumerable = (IEnumerable<AuthorDto>)authorsDto;

        var shapedData = authorsEnumerable
            .ShapeData(request.AuthorRecourseParameters.Fields)
            .ToList();

        return new PagedList<ExpandoObject>(
            shapedData,
            authorsDto.TotalCount,
            authorsDto.CurrentPage,
            authorsDto.PageSize);
    }
}