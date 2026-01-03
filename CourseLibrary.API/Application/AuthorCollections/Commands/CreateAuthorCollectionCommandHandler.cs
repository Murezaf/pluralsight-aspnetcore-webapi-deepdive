using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.AuthorCollections.Commands;

public class CreateAuthorCollectionCommandHandler
        : IRequestHandler<CreateAuthorCollectionCommand, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public CreateAuthorCollectionCommandHandler(
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(CreateAuthorCollectionCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Author> authorEntities = _mapper.Map<IEnumerable<Author>>(request.AuthorsForCreationDto);

        foreach (var author in authorEntities)
        {
            _authorRepository.AddAuthor(author);
        }

        await _authorRepository.SaveAsync();

        IEnumerable<AuthorDto> authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
        
        return authorDtos;
    }
}