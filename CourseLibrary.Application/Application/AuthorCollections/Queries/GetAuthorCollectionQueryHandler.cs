using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.AuthorCollections.Queries;

public class GetAuthorCollectionQueryHandler : IRequestHandler<GetAuthorCollectionQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorCollectionService _authorCollectionService;

    public GetAuthorCollectionQueryHandler(IAuthorCollectionService authorCollectionService)
    {
        _authorCollectionService = authorCollectionService ?? throw new ArgumentNullException(nameof(authorCollectionService));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorCollectionQuery request, CancellationToken cancellationToken)
    {
        return await _authorCollectionService.GetAuthorCollectionAsync(request.AuthorIds);
    }
}
