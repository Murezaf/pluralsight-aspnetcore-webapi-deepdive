using AutoMapper;
using CourseLibrary.Application.Contracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Commands;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author authorEntity = _mapper.Map<Author>(request.AuthorForCreationDto);

        _authorRepository.AddAuthor(authorEntity);
        await _authorRepository.SaveAsync();

        AuthorDto authorDto = _mapper.Map<AuthorDto>(authorEntity);
        
        return authorDto;
    }
}
