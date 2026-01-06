using AutoMapper;
using CourseLibrary.Application.Contracts;
using MediatR;

namespace CourseLibrary.Application.Application.Courses.Commands;

public class DeleteCourseForAuthorCommandHandler : IRequestHandler<DeleteCourseForAuthorCommand, bool>
{
    private readonly ICourseRepository _courseRepository;

    public DeleteCourseForAuthorCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    public async Task<bool> Handle(DeleteCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(request.AuthorId, request.CourseId);

        if (courseEntity == null)
            return false;

        _courseRepository.DeleteCourse(courseEntity);
        await _courseRepository.SaveAsync();

        return true;
    }
}