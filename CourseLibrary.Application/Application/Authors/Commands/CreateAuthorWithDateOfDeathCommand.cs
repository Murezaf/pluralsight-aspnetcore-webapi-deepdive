using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Commands;

public record CreateAuthorWithDateOfDeathCommand(AuthorForCreationWithDateOfDeathDto AuthorForCreationWithDateOfDeathDto) : IRequest<AuthorDto>;