using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Commands;

public class CreateAuthorWithDateOfDeathCommandHandler : IRequestHandler<CreateAuthorWithDateOfDeathCommand, AuthorDto>
{
    private readonly IAuthorService _authorService;

    public CreateAuthorWithDateOfDeathCommandHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<AuthorDto> Handle(CreateAuthorWithDateOfDeathCommand request, CancellationToken cancellationToken)
    {
        return await _authorService.CreateAuthorWithDateOfDeathAsync(request.AuthorForCreationWithDateOfDeathDto);
    }
}