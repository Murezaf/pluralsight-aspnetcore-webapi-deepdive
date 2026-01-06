using CourseLibrary.Application.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseLibrary.Application.Application.Courses.Commands;

public record PartiallyUpdateCourseForAuthorCommand(Guid AuthorId, Guid CourseId, JsonPatchDocument<CourseForUpdateDto> PatchDocument) : IRequest<CourseDto?>;