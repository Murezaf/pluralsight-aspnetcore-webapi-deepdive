using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.AuthorCollections.Commands;

public class CreateAuthorCollectionCommand : IRequest<IEnumerable<AuthorDto>>
{
    public IEnumerable<AuthorForCreationDto> AuthorsForCreationDto { get; }

    public CreateAuthorCollectionCommand(IEnumerable<AuthorForCreationDto> authorsForCreationDto)
    {
        AuthorsForCreationDto = authorsForCreationDto;
    }
}