using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using MediatR;

namespace CourseLibrary.Application.Application.Authors.Queries;

public class CheckAuthorExistsQueryHandler : IRequestHandler<CheckAuthorExistsQuery, bool>
{
    private readonly IAuthorService _authorService;

    public CheckAuthorExistsQueryHandler(IAuthorService authorService)
    {
        _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
    }

    public async Task<bool> Handle(CheckAuthorExistsQuery request, CancellationToken cancellationToken)
    {
        return await _authorService.AuthorExistsAsync(request.AuthorId);
    }
}
