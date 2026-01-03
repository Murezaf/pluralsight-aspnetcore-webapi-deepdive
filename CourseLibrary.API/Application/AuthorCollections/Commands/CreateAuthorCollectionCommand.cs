using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.AuthorCollections.Commands;

public class CreateAuthorCollectionCommand : IRequest<IEnumerable<AuthorDto>>
{
    public IEnumerable<AuthorForCreationDto> AuthorsForCreationDto { get; }

    public CreateAuthorCollectionCommand(IEnumerable<AuthorForCreationDto> authorsForCreationDto)
    {
        AuthorsForCreationDto = authorsForCreationDto;
    }
}