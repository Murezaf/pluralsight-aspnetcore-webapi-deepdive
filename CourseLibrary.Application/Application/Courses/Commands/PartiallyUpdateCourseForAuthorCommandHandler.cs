using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class PartiallyUpdateCourseForAuthorCommandHandler : IRequestHandler<PartiallyUpdateCourseForAuthorCommand, CourseDto?>
{
    private readonly ICourseService _courseService;

    public PartiallyUpdateCourseForAuthorCommandHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<CourseDto?> Handle(PartiallyUpdateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _courseService.PartiallyUpdateCourseForAuthorAsync(
            request.AuthorId,
            request.CourseId,
            request.PatchDocument);
    }
}
