using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.Models;
using CourseLibrary.Application.ResourceParameters;
using System.Dynamic;

namespace CourseLibrary.Application.Contracts.ServiceContracts;

public interface IAuthorService
{
    Task<AuthorDto> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto);

    Task<AuthorDto> CreateAuthorWithDateOfDeathAsync(AuthorForCreationWithDateOfDeathDto authorDto);

    Task<bool> AuthorExistsAsync(Guid authorId);

    Task<AuthorDto?> GetAuthorAsync(Guid authorId);

    Task<FullAuthorDto?> GetFullAuthorAsync(Guid authorId);

    Task<PagedList<AuthorDto>> GetAuthorsAsync(AuthorRecourseParameters parameters);
}