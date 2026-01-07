using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.ResourceParameters;
using CourseLibrary.Domain;

namespace CourseLibrary.Application.Contracts.RepositoryContracts;

public interface IAuthorRepository : IRepository
{
    Task<Author> GetAuthorAsync(Guid authorId);
    Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds);
    void AddAuthor(Author author);
    Task<bool> AuthorExistsAsync(Guid authorId);

    //Task<IEnumerable<Author>> GetAuthorsAsync(string? mainCategory = "");
    //Task<IEnumerable<Author>> GetAuthorsAsync(string? mainCategory = "", string? searchQuery = "");
    //Task<IEnumerable<Author>> GetAuthorsAsync(AuthorRecourseParameters authorRecourseParameters);
    Task<PagedList<Author>> GetAuthorsAsync(AuthorRecourseParameters authorRecourseParameters);
}