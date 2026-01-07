using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;

namespace CourseLibrary.Services.ServiceImplementations;

public class AuthorCollectionsService : IAuthorCollectionService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorCollectionsService(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AuthorDto>> CreateAuthorCollectionAsync(IEnumerable<AuthorForCreationDto> authorCollection)
    {
        var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);

        foreach (var author in authorEntities)
        {
            _authorRepository.AddAuthor(author);
        }

        await _authorRepository.SaveAsync();

        return _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
    }

    public async Task<IEnumerable<AuthorDto>?> GetAuthorCollectionAsync(IEnumerable<Guid> authorIds)
    {
        if (authorIds == null)
        {
            throw new ArgumentNullException(nameof(authorIds));
        }

        var authorEntities = await _authorRepository.GetAuthorsAsync(authorIds);

        if (authorIds.Count() != authorEntities.Count())
        {
            return null;
        }

        return _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
    }
}
