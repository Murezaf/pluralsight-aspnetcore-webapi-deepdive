using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.AuthorCollections.Commands;

public class CreateAuthorCollectionCommandHandler
        : IRequestHandler<CreateAuthorCollectionCommand, IEnumerable<AuthorDto>>
{
    private readonly IAuthorCollectionService _authorCollectionService;

    public CreateAuthorCollectionCommandHandler(IAuthorCollectionService authorCollectionService)
    {
        _authorCollectionService = authorCollectionService ?? throw new ArgumentNullException(nameof(authorCollectionService));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(CreateAuthorCollectionCommand request, CancellationToken cancellationToken)
    {
        return await _authorCollectionService.CreateAuthorCollectionAsync(request.AuthorsForCreationDto);
    }
}