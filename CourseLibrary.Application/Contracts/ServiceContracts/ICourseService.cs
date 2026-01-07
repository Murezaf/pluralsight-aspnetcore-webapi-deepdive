using CourseLibrary.Application.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseLibrary.Application.Contracts.ServiceContracts;

public interface ICourseService
{
    Task<CourseDto> CreateCourseForAuthorAsync(Guid authorId, CourseForCreationDto courseForCreationDto);

    Task<IEnumerable<CourseDto>> GetCoursesForAuthorAsync(Guid authorId);

    Task<CourseDto?> GetCourseForAuthorAsync(Guid authorId, Guid courseId);

    Task<CourseDto?> UpdateCourseForAuthorAsync(Guid authorId, Guid courseId, CourseForUpdateDto courseForUpdateDto);

    Task<CourseDto?> PartiallyUpdateCourseForAuthorAsync(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument);

    Task<bool> DeleteCourseForAuthorAsync(Guid authorId, Guid courseId);
}
