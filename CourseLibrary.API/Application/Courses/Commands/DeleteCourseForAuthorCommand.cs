using MediatR;

namespace CourseLibrary.API.Application.Courses.Commands;

public record DeleteCourseForAuthorCommand(Guid AuthorId, Guid CourseId) : IRequest<bool>;