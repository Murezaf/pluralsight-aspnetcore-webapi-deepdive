using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public class GetFullAuthorByIdQueryHandler : IRequestHandler<GetFullAuthorByIdQuery, ExpandoObject>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetFullAuthorByIdQueryHandler(
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ExpandoObject> Handle(GetFullAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        Author author = await _authorRepository.GetAuthorAsync(request.AuthorId);

        if (author == null)
        {
            return null;
        }

        FullAuthorDto authoFullrDto = _mapper.Map<FullAuthorDto>(author);
        ExpandoObject shapedData = authoFullrDto.ShapeData(request.Fields);

        return shapedData;
    }
}
