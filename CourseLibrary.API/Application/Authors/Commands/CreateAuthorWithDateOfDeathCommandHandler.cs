using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.Authors.Commands;

public class CreateAuthorWithDateOfDeathCommandHandler : IRequestHandler<CreateAuthorWithDateOfDeathCommand, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public CreateAuthorWithDateOfDeathCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AuthorDto> Handle(CreateAuthorWithDateOfDeathCommand request, CancellationToken cancellationToken)
    {
        Author authorEntity = _mapper.Map<Author>(request.AuthorForCreationWithDateOfDeathDto);

        _authorRepository.AddAuthor(authorEntity);
        await _authorRepository.SaveAsync();

        AuthorDto authorDto = _mapper.Map<AuthorDto>(authorEntity);

        return authorDto;
    }
}