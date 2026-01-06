using CourseLibrary.Application.Models;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public record CreateCourseForAuthorCommand(Guid AuthorId, CourseForCreationDto CourseForCreationDto): IRequest<CourseDto>;