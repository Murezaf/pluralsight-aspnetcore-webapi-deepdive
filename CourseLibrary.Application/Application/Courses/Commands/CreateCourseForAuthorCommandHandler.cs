using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class CreateCourseForAuthorCommandHandler : IRequestHandler<CreateCourseForAuthorCommand, CourseDto>
{
    private readonly ICourseService _courseService;

    public CreateCourseForAuthorCommandHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<CourseDto> Handle(CreateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _courseService.CreateCourseForAuthorAsync(request.AuthorId, request.CourseForCreationDto);
    }
}
