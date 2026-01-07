using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Commands;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorService _authorService;

    public CreateAuthorCommandHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _authorService.CreateAuthorAsync(request.AuthorForCreationDto);
    }
}
