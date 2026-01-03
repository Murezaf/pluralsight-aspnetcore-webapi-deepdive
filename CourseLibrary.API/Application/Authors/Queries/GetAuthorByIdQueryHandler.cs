using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using CourseLibrary.API.Services;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, ExpandoObject>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ExpandoObject> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        Author author = await _authorRepository.GetAuthorAsync(request.AuthorId);
        
        if (author == null)
        {
            return null;
        }

        AuthorDto authorDto = _mapper.Map<AuthorDto>(author);
        ExpandoObject shapedData = authorDto.ShapeData(request.Fields);
        
        return shapedData;
    }
}
