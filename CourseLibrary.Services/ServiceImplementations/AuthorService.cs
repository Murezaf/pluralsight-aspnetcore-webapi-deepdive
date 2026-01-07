using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Application.ResourceParameters;
using CourseLibrary.Domain;
using System.Dynamic;

namespace CourseLibrary.Services.ServiceImplementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<bool> AuthorExistsAsync(Guid authorId)
    {
        return await _authorRepository.AuthorExistsAsync(authorId);
    }

    public async Task<AuthorDto> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto)
    {
        var authorEntity = _mapper.Map<Author>(authorForCreationDto);

        _authorRepository.AddAuthor(authorEntity);
        await _authorRepository.SaveAsync();

        return _mapper.Map<AuthorDto>(authorEntity);
    }

    public async Task<AuthorDto> CreateAuthorWithDateOfDeathAsync(AuthorForCreationWithDateOfDeathDto dto)
    {
        var authorEntity = _mapper.Map<Author>(dto);

        _authorRepository.AddAuthor(authorEntity);
        await _authorRepository.SaveAsync();

        return _mapper.Map<AuthorDto>(authorEntity);
    }

    public async Task<ExpandoObject?> GetAuthorAsync(Guid authorId, string? fields)
    {
        var author = await _authorRepository.GetAuthorAsync(authorId);

        if (author == null)
        {
            return null;
        }

        var authorDto = _mapper.Map<AuthorDto>(author);

        return authorDto.ShapeData(fields);
    }

    public async Task<AuthorDto?> GetAuthorAsync(Guid authorId)
    {
        var author = await _authorRepository.GetAuthorAsync(authorId);
        if (author == null) return null;
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<PagedList<AuthorDto>> GetAuthorsAsync(AuthorRecourseParameters parameters)
    {
        var authorsFromRepo = await _authorRepository.GetAuthorsAsync(parameters);

        var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
        
        return new PagedList<AuthorDto>(
            authorsDto.ToList(),
            authorsFromRepo.TotalCount,
            authorsFromRepo.CurrentPage,
            authorsFromRepo.PageSize);
    }

    public async Task<FullAuthorDto?> GetFullAuthorAsync(Guid authorId)
    {
        var author = await _authorRepository.GetAuthorAsync(authorId);
        if (author == null) return null;
        return _mapper.Map<FullAuthorDto>(author);
    }
}
