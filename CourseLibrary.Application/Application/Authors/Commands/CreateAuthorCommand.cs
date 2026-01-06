using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Commands;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public AuthorForCreationDto AuthorForCreationDto { get; }

    public CreateAuthorCommand(AuthorForCreationDto authorForCreationDto)
    {
        AuthorForCreationDto = authorForCreationDto ?? throw new ArgumentNullException(nameof(authorForCreationDto));
    }
}
