using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public record UpdateCourseForAuthorCommand(Guid AuthorId, Guid CourseId, CourseForUpdateDto CourseForUpdateDto) : IRequest<CourseDto?>;
