using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class DeleteCourseForAuthorCommandHandler : IRequestHandler<DeleteCourseForAuthorCommand, bool>
{
    private readonly ICourseService _courseService;

    public DeleteCourseForAuthorCommandHandler(ICourseService courseService)
    {
        _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
    }

    public async Task<bool> Handle(DeleteCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _courseService.DeleteCourseForAuthorAsync(request.AuthorId, request.CourseId);
    }
}