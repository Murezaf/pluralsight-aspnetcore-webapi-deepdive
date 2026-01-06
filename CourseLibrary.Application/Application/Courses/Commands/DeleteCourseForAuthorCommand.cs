using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public record DeleteCourseForAuthorCommand(Guid AuthorId, Guid CourseId) : IRequest<bool>;