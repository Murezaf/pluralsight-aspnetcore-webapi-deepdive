using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.ResourceParameters;

namespace CourseLibrary.API.Repositories.Interfaces;

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