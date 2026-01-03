using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.AuthorCollections.Queries;

public class GetAuthorCollectionQueryHandler : IRequestHandler<GetAuthorCollectionQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAuthorCollectionQueryHandler(
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorCollectionQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Author> authorEntities = await _authorRepository.GetAuthorsAsync(request.AuthorIds);

        if (authorEntities.Count() != request.AuthorIds.Count())
            return null;

        IEnumerable<AuthorDto> authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
        
        return authorDtos;
    }
}
