using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Repositories.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly CourseLibraryContext _context;
    private readonly IPropertyMappingService _propertyMappingService;

    public AuthorRepository(CourseLibraryContext context, IPropertyMappingService propertyMappingService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
    }

    public async Task<Author> GetAuthorAsync(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        //Repository layer should not make HTTP or business decisions. At the repository level we only deal with data access.
        //If the entity does not exist, return null and let the controller decide how to translate that outcome (e.g., NotFound, BadRequest, etc.).
    }

    public async Task<bool> AuthorExistsAsync(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return await _context.Authors.AnyAsync(a => a.Id == authorId);
    }

    public void AddAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        author.Id = Guid.NewGuid();

        foreach (var course in author.Courses)
        {
            course.Id = Guid.NewGuid();
        }

        _context.Authors.Add(author);
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds)
    {
        if (authorIds == null)
        {
            throw new ArgumentNullException(nameof(authorIds));
        }

        return await _context.Authors.Where(a => authorIds.Contains(a.Id)).ToListAsync();
    }

    public async Task<PagedList<Author>> GetAuthorsAsync(AuthorRecourseParameters authorRecourseParameters)
    {
        if (authorRecourseParameters == null)
            throw new NullReferenceException(nameof(authorRecourseParameters));

        //if (string.IsNullOrWhiteSpace(authorRecourseParameters.MainCategory) && string.IsNullOrWhiteSpace(authorRecourseParameters.SearchQuery))
        //    return await GetAuthorsAsync(); //Paging should happen anyway

        IQueryable<Author> collection = _context.Authors as IQueryable<Author>;

        if (!string.IsNullOrWhiteSpace(authorRecourseParameters.MainCategory))
        {
            string mainCategory = authorRecourseParameters.MainCategory.Trim();
            collection = collection.Where(a => a.MainCategory == mainCategory);
        }

        if (!string.IsNullOrWhiteSpace(authorRecourseParameters.SearchQuery))
        {
            string searchQuery = authorRecourseParameters.SearchQuery.Trim();
            collection = collection.Where(a => a.FirstName.Contains(searchQuery) || a.LastName.Contains(searchQuery) || a.MainCategory.Contains(searchQuery));
        }

        if (!string.IsNullOrWhiteSpace(authorRecourseParameters.OrderBy))
        {
            //if (authorRecourseParameters.OrderBy.ToLowerInvariant() == "name") //Can't write this such code for each parameter and their combination all have asc and desc as an option
            //{
            //    string orderBy = authorRecourseParameters.OrderBy.Trim();
            //    collection = collection.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
            //}

            Dictionary<string, PropertyMappingValue> authorPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<AuthorDto, Author>();

            collection = collection.ApplySort(authorRecourseParameters.OrderBy, authorPropertyMappingDictionary);
        }

        //Add Paging
        //collection = collection.Skip((authorRecourseParameters.PageNumber - 1) * authorRecourseParameters.PageSize)
        //.Take(authorRecourseParameters.PageSize);

        return await PagedList<Author>.CreateAsync(collection, authorRecourseParameters.PageNumber, authorRecourseParameters.PageSize); //paging logic happens inside CreateAsync method. no need to apply here.
    }
}