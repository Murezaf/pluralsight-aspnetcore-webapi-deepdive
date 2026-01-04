using CourseLibrary.API.Repositories.Interfaces;
using MediatR;

namespace CourseLibrary.API.Application.Authors.Queries;

public class CheckAuthorExistsQueryHandler : IRequestHandler<CheckAuthorExistsQuery, bool>
{
    private readonly IAuthorRepository _authorRepository;

    public CheckAuthorExistsQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
    }

    public async Task<bool> Handle(CheckAuthorExistsQuery request, CancellationToken cancellationToken)
    {
        return await _authorRepository.AuthorExistsAsync(request.AuthorId);
    }
}
