using CourseLibrary.API.Models;
using MediatR;

namespace CourseLibrary.API.Application.Courses.Commands;

public record CreateCourseForAuthorCommand(Guid AuthorId, CourseForCreationDto CourseForCreationDto): IRequest<CourseDto>;