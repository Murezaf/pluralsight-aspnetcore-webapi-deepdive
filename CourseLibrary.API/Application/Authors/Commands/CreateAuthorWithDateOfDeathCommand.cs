using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Authors.Commands;

public record CreateAuthorWithDateOfDeathCommand(AuthorForCreationWithDateOfDeathDto AuthorForCreationWithDateOfDeathDto) : IRequest<AuthorDto>;