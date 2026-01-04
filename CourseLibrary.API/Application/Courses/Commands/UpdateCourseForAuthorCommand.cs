using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Commands;

public record UpdateCourseForAuthorCommand(Guid AuthorId, Guid CourseId, CourseForUpdateDto CourseForUpdateDto) : IRequest<CourseDto?>;
