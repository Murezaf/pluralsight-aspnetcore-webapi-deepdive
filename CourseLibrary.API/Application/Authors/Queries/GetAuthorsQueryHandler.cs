using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, PagedList<ExpandoObject>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository
            ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<ExpandoObject>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        PagedList<Author> authorsFromRepo = await _authorRepository.GetAuthorsAsync(request.AuthorRecourseParameters);

        IEnumerable<AuthorDto> authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);

        List<ExpandoObject> shapedData = authorsDto.ShapeData(request.AuthorRecourseParameters.Fields).ToList();

        return new PagedList<ExpandoObject>(shapedData, authorsFromRepo.TotalCount, authorsFromRepo.CurrentPage, authorsFromRepo.PageSize);
    }
}