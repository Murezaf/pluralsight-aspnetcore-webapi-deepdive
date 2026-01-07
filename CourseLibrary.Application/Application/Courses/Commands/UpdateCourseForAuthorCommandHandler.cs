using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class UpdateCourseForAuthorCommandHandler : IRequestHandler<UpdateCourseForAuthorCommand, CourseDto?>
{
    private readonly ICourseService _courseService;

    public UpdateCourseForAuthorCommandHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<CourseDto?> Handle(UpdateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _courseService.UpdateCourseForAuthorAsync(
            request.AuthorId,
            request.CourseId,
            request.CourseForUpdateDto);
    }
}