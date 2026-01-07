using CourseLibrary.Application.Models;

namespace CourseLibrary.Application.Contracts.ServiceContracts;

public interface IAuthorCollectionService
{
    Task<IEnumerable<AuthorDto>> CreateAuthorCollectionAsync(IEnumerable<AuthorForCreationDto> authorCollection);

    Task<IEnumerable<AuthorDto>?> GetAuthorCollectionAsync(IEnumerable<Guid> authorIds);
}
