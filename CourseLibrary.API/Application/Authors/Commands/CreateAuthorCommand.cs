using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Authors.Commands;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public AuthorForCreationDto AuthorForCreationDto { get; }

    public CreateAuthorCommand(AuthorForCreationDto authorForCreationDto)
    {
        AuthorForCreationDto = authorForCreationDto ?? throw new ArgumentNullException(nameof(authorForCreationDto));
    }
}
