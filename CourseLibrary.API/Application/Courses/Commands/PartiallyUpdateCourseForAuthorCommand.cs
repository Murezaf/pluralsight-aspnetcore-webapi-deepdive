using CourseLibrary.API.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseLibrary.API.Application.Courses.Commands;

public record PartiallyUpdateCourseForAuthorCommand(Guid AuthorId, Guid CourseId, JsonPatchDocument<CourseForUpdateDto> PatchDocument) : IRequest<CourseDto?>;